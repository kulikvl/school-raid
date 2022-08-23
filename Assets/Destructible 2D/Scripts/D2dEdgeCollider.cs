using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dEdgeCollider))]
	public class D2dEdgeCollider_Editor : D2dCollider_Editor<D2dEdgeCollider>
	{
		protected override void OnInspector()
		{
			base.OnInspector();

			var refresh = false;
			var rebuild = false;

			DrawDefault("edgeRadius", ref rebuild, "This allows you to set the edgeRadius setting on each generated collider.");
			DrawDefault("cellSize", ref rebuild, "This allows you to change the pixel width & height of each collider cell to improve performance. The pixel size you choose should be in relation to the typical size of destruction in your scene.");
			DrawDefault("straighten", ref rebuild, "This allows you to control how easily the edges can merge together. A higher value gives better performance, but less accurate colliders.");

			Separator();

			if (Button("Rebuild") == true)
			{
				DirtyEach(t => t.Rebuild());
			}

			if (refresh == true) DirtyEach(t => t.Refresh());
			if (rebuild == true) DirtyEach(t => t.Rebuild());
		}
	}
}
#endif

namespace Destructible2D
{
	/// <summary>This component allows you to generate edge colliders for a destructible sprite. Edge colliders should only be used for non-moving objects.</summary>
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(D2dDestructible))]
	[HelpURL(D2dHelper.HelpUrlPrefix + "D2dEdgeCollider")]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Edge Collider")]
	public class D2dEdgeCollider : D2dCollider
	{
		public enum CellSizes
		{
			Square8  = 8,
			Square16 = 16,
			Square32 = 32,
			Square64 = 64
		}

		[System.Serializable]
		public class Cell
		{
			public List<EdgeCollider2D> Colliders = new List<EdgeCollider2D>();

			public void Clear()
			{
				for (var i = Colliders.Count - 1; i >= 0; i--)
				{
					var collider = Colliders[i];

					if (collider != null)
					{
						colliderPool.Push(collider);
					}
				}

				Colliders.Clear();
			}
		}

		/// <summary>This allows you to set the edgeRadius setting on each generated collider.</summary>
		public float EdgeRadius { set { edgeRadius = value; Refresh(); } get { return edgeRadius; } } [SerializeField] protected float edgeRadius;

		/// <summary>This allows you to set the edgeRadius value without causing Refresh to be called.</summary>
		public float EdgeRadiusRaw { set { edgeRadius = value; } get { return edgeRadius; } }

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
		private static Stack<EdgeCollider2D> colliderPool = new Stack<EdgeCollider2D>();

		[System.NonSerialized]
		private static Stack<Cell> cellPool = new Stack<Cell>();

		[ContextMenu("Refresh")]
		public override void Refresh()
		{
			for (var i = cells.Count - 1; i >= 0; i--)
			{
				var cell = cells[i];

				for (var j = cell.Colliders.Count - 1; j >= 0; j--)
				{
					var collider = cell.Colliders[j];

					if (collider != null)
					{
						collider.sharedMaterial = material;
						collider.isTrigger      = isTrigger;
						collider.edgeRadius     = edgeRadius;
					}
				}
			}
		}

		public EdgeCollider2D AddCollider()
		{
			var collider = colliderPool.Count > 0 ? colliderPool.Pop() : child.AddComponent<EdgeCollider2D>();

			collider.sharedMaterial = material;
			collider.isTrigger      = isTrigger;
			collider.edgeRadius     = edgeRadius;

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
					cells[cellX + offset].Clear();
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
				var edgeCollider      = splitDestructible.GetComponent<D2dEdgeCollider>();

				if (edgeCollider != null)
				{
					edgeCollider.Rebuild();
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
					cell.Clear();

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
			var xMax = Mathf.Min(x + cellSiz, cachedDestructible.AlphaWidth );
			var yMax = Mathf.Min(y + cellSiz, cachedDestructible.AlphaHeight);
			
			D2dEdgeSquares.AlphaD = cachedDestructible.AlphaData;
			D2dEdgeSquares.AlphaW = cachedDestructible.AlphaWidth;
			D2dEdgeSquares.AlphaH = cachedDestructible.AlphaHeight;
			D2dEdgeSquares.MinX   = xMin;
			D2dEdgeSquares.MinY   = yMin;
			D2dEdgeSquares.MaxX   = xMax;
			D2dEdgeSquares.MaxY   = yMax;

			D2dEdgeSquares.CalculateCells();
			D2dEdgeSquares.Build(cell, this);
		}
	}
}