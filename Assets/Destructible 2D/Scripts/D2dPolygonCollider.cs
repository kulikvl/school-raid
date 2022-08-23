using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dPolygonCollider))]
	public class D2dPolygonCollider_Editor : D2dCollider_Editor<D2dPolygonCollider>
	{
		protected override void OnInspector()
		{
			base.OnInspector();

			var rebuild = false;

			DrawDefault("cellSize", ref rebuild, "This allows you to change the pixel width & height of each collider cell to improve performance. The pixel size you choose should be in relation to the typical size of destruction in your scene.");
			DrawDefault("straighten", ref rebuild, "This allows you to control how easily the edges can merge together. A higher value gives better performance, but less accurate colliders.");

			Separator();

			if (Button("Rebuild") == true)
			{
				DirtyEach(t => t.Rebuild());
			}

			if (rebuild == true) DirtyEach(t => t.Rebuild());
		}
	}
}
#endif

namespace Destructible2D
{
	/// <summary>This component allows you to generate polygon colliders for a destructible sprite. Polygon colliders should be used for moving objects.</summary>
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(D2dDestructible))]
	[HelpURL(D2dHelper.HelpUrlPrefix + "D2dPolygonCollider")]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Polygon Collider")]
	public class D2dPolygonCollider : D2dCollider
	{
		public enum CellSizes
		{
			Square8  = 8,
			Square16 = 16,
			Square32 = 32,
			Square64 = 64
		}

		[System.Serializable]
		public class Path : IComparer<Path>
		{
			public Vector2[] Points;
			public Vector2   Left;

			public void CalculateLeft()
			{
				Left.x = float.PositiveInfinity;

				for (var i = Points.Length - 1; i >= 0; i--)
				{
					var point = Points[i];

					if (point.x < Left.x)
					{
						Left = point;
					}
				}
			}

			public static float LineSide(Vector2 a, Vector2 b, Vector2 p)
			{
				return (b.y - a.y) * (p.x - a.x) - (b.x - a.x) * (p.y - a.y);
			}

			public bool Contains(Vector2 point)
			{
				var total  = 0;
				var pointA = Points[0];

				for (var j = Points.Length - 1; j >= 0; j--)
				{
					var pointB = Points[j];

					if (pointA.y <= point.y)
					{
						if (pointB.y > point.y && LineSide(pointA, pointB, point) > 0.0f) total += 1;
					}
					else
					{
						if (pointB.y <= point.y && LineSide(pointA, pointB, point) < 0.0f) total -= 1;
					}

					pointA = pointB;
				}

				return total != 0;
			}

			public int Compare(Path a, Path b)
			{
				return a.Left.x.CompareTo(b.Left.x);
			}
		}

		[System.Serializable]
		public class Shape
		{
			public PolygonCollider2D Collider;

			public Path Outside;

			public List<Path> Holes = new List<Path>();

			public bool Contains(Vector2 point)
			{
				if (Outside.Contains(point) == true)
				{
					for (var i = 0; i < Holes.Count; i++)
					{
						if (Holes[i].Contains(point) == true)
						{
							return false;
						}
					}

					return true;
				}

				return false;
			}
		}

		[System.Serializable]
		public class Cell
		{
			public List<Shape> Shapes = new List<Shape>();
		}

		/// <summary>This allows you to change the pixel width & height of each collider cell to improve performance. The pixel size you choose should be in relation to the typical size of destruction in your scene.</summary>
		public CellSizes CellSize { set { cellSize = value; Rebuild(); } get { return cellSize; } } [SerializeField] private CellSizes cellSize = CellSizes.Square16;

		/// <summary>This allows you to control how easily the edges can merge together. A higher value gives better performance, but less accurate colliders.</summary>
		public float Straighten { set { straighten = value; Rebuild(); } get { return straighten; } } [SerializeField] [Range(0.0f, 0.5f)] private float straighten = 0.01f;

		[SerializeField]
		private int cellRow;

		[SerializeField]
		private int cellCol;

		[SerializeField]
		private int cellSiz;

		[SerializeField]
		private List<Cell> cells;

		[System.NonSerialized]
		private static Stack<PolygonCollider2D> colliderPool = new Stack<PolygonCollider2D>();

		[System.NonSerialized]
		private static Stack<Cell> cellPool = new Stack<Cell>();

		[System.NonSerialized]
		private static Stack<Shape> shapePool = new Stack<Shape>();

		[ContextMenu("Refresh")]
		public override void Refresh()
		{
			for (var i = cells.Count - 1; i >= 0; i--)
			{
				var cell = cells[i];

				for (var j = cell.Shapes.Count - 1; j >= 0; j--)
				{
					var shape = cell.Shapes[j];

					if (shape.Collider != null)
					{
						shape.Collider.sharedMaterial = material;
						shape.Collider.isTrigger      = isTrigger;
					}
				}
			}
		}

		public PolygonCollider2D AddCollider()
		{
			var collider = colliderPool.Count > 0 ? colliderPool.Pop() : child.AddComponent<PolygonCollider2D>();

			collider.sharedMaterial = material;
			collider.isTrigger      = isTrigger;

			return collider;
		}

		protected override void DoModified(D2dRect rect)
		{
			var cellXMin = rect.MinX / cellSiz;
			var cellYMin = rect.MinY / cellSiz;
			var cellXMax = (rect.MaxX + 1) / cellSiz;
			var cellYMax = (rect.MaxY + 1) / cellSiz;

			cellXMin = Mathf.Clamp(cellXMin, 0, cellCol - 1);
			cellXMax = Mathf.Clamp(cellXMax, 0, cellCol - 1);
			cellYMin = Mathf.Clamp(cellYMin, 0, cellRow - 1);
			cellYMax = Mathf.Clamp(cellYMax, 0, cellRow - 1);

			for (var cellY = cellYMin; cellY <= cellYMax; cellY++)
			{
				var offset = cellY * cellCol;

				for (var cellX = cellXMin; cellX <= cellXMax; cellX++)
				{
					ClearCell(cells[cellX + offset]);
				}
			}

			for (var cellY = cellYMin; cellY <= cellYMax; cellY++)
			{
				var offset = cellY * cellCol;

				for (var cellX = cellXMin; cellX <= cellXMax; cellX++)
				{
					var cell = cells[cellX + offset];

					RebuildCell(cell, cellX, cellY);
				}
			}

			SweepColliders();
		}

		protected override void SplitStart()
		{
			base.SplitStart();

			ClearCells();
			SweepColliders();
		}

		protected override void SplitEnd(List<D2dDestructible> splitDestructibles)
		{
			base.SplitEnd(splitDestructibles);

			for (var i = splitDestructibles.Count - 1; i >= 0; i--)
			{
				var splitDestructible = splitDestructibles[i];
				var polygonCollider   = splitDestructible.GetComponent<D2dPolygonCollider>();

				if (polygonCollider != null)
				{
					polygonCollider.Rebuild();
				}
			}
		}

		protected override void DoRebuild()
		{
			if (cells == null)
			{
				cells = new List<Cell>();
			}

			ClearCells();

			var sprite = CachedDestructible;

			cellSiz = (int)cellSize;
			cellCol = (sprite.AlphaWidth + cellSiz - 1) / cellSiz;
			cellRow = (sprite.AlphaHeight + cellSiz - 1) / cellSiz;

			for (var cellY = 0; cellY < cellRow; cellY++)
			{
				for (var cellX = 0; cellX < cellCol; cellX++)
				{
					var cell = cellPool.Count > 0 ? cellPool.Pop() : new Cell();

					cells.Add(cell);

					RebuildCell(cell, cellX, cellY);
				}
			}

			SweepColliders();
		}

		private void ClearCells()
		{
			for (var i = cells.Count - 1; i >= 0; i--)
			{
				var cell = cells[i];
				
				if (cell != null)
				{
					ClearCell(cell);

					cellPool.Push(cell);
				}
			}

			cells.Clear();
		}

		private void SweepColliders()
		{
			while (colliderPool.Count > 0)
			{
				D2dHelper.Destroy(colliderPool.Pop());
			}
		}

		private void RebuildCell(Cell cell, int cellX, int cellY)
		{
			var x    = cellSiz * cellX;
			var y    = cellSiz * cellY;
			var xMin = Mathf.Max(x - 1, 0);
			var yMin = Mathf.Max(y - 1, 0);
			var xMax = Mathf.Min(x + cellSiz, CachedDestructible.AlphaWidth );
			var yMax = Mathf.Min(y + cellSiz, cachedDestructible.AlphaHeight);
			
			D2dPolygonSquares.AlphaD = cachedDestructible.AlphaData;
			D2dPolygonSquares.AlphaW = cachedDestructible.AlphaWidth;
			D2dPolygonSquares.AlphaH = cachedDestructible.AlphaHeight;
			D2dPolygonSquares.MinX   = xMin;
			D2dPolygonSquares.MinY   = yMin;
			D2dPolygonSquares.MaxX   = xMax;
			D2dPolygonSquares.MaxY   = yMax;

			D2dPolygonSquares.CalculateCells();
			D2dPolygonSquares.Build(cell, shapePool, this);
		}

		private void ClearCell(Cell cell)
		{
			if (cell.Shapes != null)
			{
				for (var j = cell.Shapes.Count - 1; j >= 0; j--)
				{
					var ring = cell.Shapes[j];

					if (ring.Collider != null)
					{
						ring.Collider.pathCount = 0;

						colliderPool.Push(ring.Collider);
					}

					shapePool.Push(ring);
				}

				cell.Shapes.Clear();
			}
		}
	}
}