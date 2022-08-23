using UnityEngine;

namespace Destructible2D
{
	/// <summary>This class handles the various ways to modify the destruction state of a D2dDestructible.</summary>
	public partial class D2dDestructible
	{
		public enum PaintType
		{
			Cut,
			Heal,
			Subtract
		}

		/// <summary>This paints the current destructible with the specified paint type, at the specified matrix, with the specified shape.</summary>
		public void Paint(PaintType paint, Matrix4x4 matrix, Texture2D shape, Color color)
		{
			switch (paint)
			{
				case PaintType.Cut: Cut(matrix, shape, color); break;
				case PaintType.Heal: Heal(matrix, shape, color); break;
				case PaintType.Subtract: Subtract(matrix, shape, color); break;
			}
		}

		private static Matrix4x4 matrix;
		private static Matrix4x4 inverse;

		private static Texture2D shape;
		private static int       shapeW;
		private static int       shapeH;
		private static Color     shapeColor;
		private static Vector3   shapeCoordA;
		private static Vector3   shapeCoordB;

		public void Cut(Matrix4x4 newMatrix, Texture2D newShape, Color newColor)
		{
			if (ready == true && indestructible == false && newShape != null)
			{
				var rect = default(D2dRect);

				matrix     = WorldToAlphaMatrix * newMatrix;
				alphaCount = -1;

				if (D2dHelper.CalculateRect(matrix, ref rect, alphaWidth, alphaHeight) == true)
				{
					inverse    = matrix.inverse;
					shape      = newShape;
					shapeW     = newShape.width;
					shapeH     = newShape.height;
					shapeColor = newColor * paintMultiplier;

					rect.MinX = Mathf.Clamp(rect.MinX, 0, alphaWidth );
					rect.MaxX = Mathf.Clamp(rect.MaxX, 0, alphaWidth );
					rect.MinY = Mathf.Clamp(rect.MinY, 0, alphaHeight);
					rect.MaxY = Mathf.Clamp(rect.MaxY, 0, alphaHeight);

					var alphaPixelX     = D2dHelper.Reciprocal(alphaWidth );
					var alphaPixelY     = D2dHelper.Reciprocal(alphaHeight);
					var alphaHalfPixelX = alphaPixelX * 0.5f;
					var alphaHalfPixelY = alphaPixelY * 0.5f;

					for (var y = rect.MinY; y < rect.MaxY; y++)
					{
						var v      = y * alphaPixelY + alphaHalfPixelY;
						var offset = y * alphaWidth;

						for (var x = rect.MinX; x < rect.MaxX; x++)
						{
							var u = x * alphaPixelX + alphaHalfPixelX;

							if (CalculateShapeCoord(u, v) == true)
							{
								var index      = offset + x;
								var alphaPixel = alphaData[index];
								var shapePixel = SampleShapeA();

								alphaPixel.a = (byte)System.Math.Max(alphaPixel.a - shapePixel, 0);

								if (pixels == PixelsType.PixelatedBinary)
								{
									alphaPixel.a = alphaPixel.a > 127 ? (byte)255 : (byte)0;
								}

								alphaData[index] = alphaPixel;
							}
						}
					}

					AlphaModified.Add(rect);
				}
			}
		}

		public void Heal(Matrix4x4 newMatrix, Texture2D newShape, Color newColor)
		{
			if (ready == true && newShape != null && CanHeal == true)
			{
				var healData = healSnapshot.Data;
				var rect = default(D2dRect);

				matrix     = WorldToAlphaMatrix * newMatrix;
				alphaCount = -1;

				if (D2dHelper.CalculateRect(matrix, ref rect, alphaWidth, alphaHeight) == true)
				{
					inverse    = matrix.inverse;
					shape      = newShape;
					shapeW     = newShape.width;
					shapeH     = newShape.height;
					shapeColor = newColor * paintMultiplier;

					rect.MinX = Mathf.Clamp(rect.MinX, 0, alphaWidth );
					rect.MaxX = Mathf.Clamp(rect.MaxX, 0, alphaWidth );
					rect.MinY = Mathf.Clamp(rect.MinY, 0, alphaHeight);
					rect.MaxY = Mathf.Clamp(rect.MaxY, 0, alphaHeight);

					var alphaPixelX     = D2dHelper.Reciprocal(alphaWidth );
					var alphaPixelY     = D2dHelper.Reciprocal(alphaHeight);
					var alphaHalfPixelX = alphaPixelX * 0.5f;
					var alphaHalfPixelY = alphaPixelY * 0.5f;

					for (var y = rect.MinY; y < rect.MaxY; y++)
					{
						var v      = y * alphaPixelY + alphaHalfPixelY;
						var offset = y * alphaWidth;

						for (var x = rect.MinX; x < rect.MaxX; x++)
						{
							var u = x * alphaPixelX + alphaHalfPixelX;

							if (CalculateShapeCoord(u, v) == true)
							{
								var index      = offset + x;
								var alphaPixel = alphaData[index];
								var shapePixel = SampleShapeA();

								alphaPixel.a = (byte)System.Math.Min(alphaPixel.a + shapePixel, healData.AlphaData[index].a);

								if (pixels == PixelsType.PixelatedBinary)
								{
									alphaPixel.a = alphaPixel.a > 127 ? (byte)255 : (byte)0;
								}

								alphaData[index] = alphaPixel;
							}
						}
					}

					AlphaModified.Add(rect);
				}
			}
		}
		
		public void Subtract(Matrix4x4 newMatrix, Texture2D newShape, Color newColor)
		{
			if (ready == true && indestructible == false && newShape != null)
			{
				var rect = default(D2dRect);

				matrix     = WorldToAlphaMatrix * newMatrix;
				alphaCount = -1;

				if (D2dHelper.CalculateRect(matrix, ref rect, alphaWidth, alphaHeight) == true)
				{
					inverse    = matrix.inverse;
					shape      = newShape;
					shapeW     = newShape.width;
					shapeH     = newShape.height;
					shapeColor = newColor * paintMultiplier;

					rect.MinX = Mathf.Clamp(rect.MinX, 0, alphaWidth );
					rect.MaxX = Mathf.Clamp(rect.MaxX, 0, alphaWidth );
					rect.MinY = Mathf.Clamp(rect.MinY, 0, alphaHeight);
					rect.MaxY = Mathf.Clamp(rect.MaxY, 0, alphaHeight);

					var alphaPixelX     = D2dHelper.Reciprocal(alphaWidth );
					var alphaPixelY     = D2dHelper.Reciprocal(alphaHeight);
					var alphaHalfPixelX = alphaPixelX * 0.5f;
					var alphaHalfPixelY = alphaPixelY * 0.5f;

					for (var y = rect.MinY; y < rect.MaxY; y++)
					{
						var v      = y * alphaPixelY + alphaHalfPixelY;
						var offset = y * alphaWidth;

						for (var x = rect.MinX; x < rect.MaxX; x++)
						{
							var u = x * alphaPixelX + alphaHalfPixelX;

							if (CalculateShapeCoord(u, v) == true)
							{
								var index      = offset + x;
								var alphaPixel = alphaData[index];
								var shapePixel = SampleShape();

								alphaPixel.r = (byte)System.Math.Max(alphaPixel.r - shapePixel.r, 0);
								alphaPixel.g = (byte)System.Math.Max(alphaPixel.g - shapePixel.g, 0);
								alphaPixel.b = (byte)System.Math.Max(alphaPixel.b - shapePixel.b, 0);
								alphaPixel.a = (byte)System.Math.Max(alphaPixel.a - shapePixel.a, 0);

								if (pixels == PixelsType.PixelatedBinary)
								{
									alphaPixel.a = alphaPixel.a > 127 ? (byte)255 : (byte)0;
								}

								alphaData[index] = alphaPixel;
							}
						}
					}

					AlphaModified.Add(rect);
				}
			}
		}

		private Color32 SampleShape()
		{
			var x = (int)(shapeCoordB.x * shapeW);
			var y = (int)(shapeCoordB.y * shapeH);

			return shape.GetPixel(x, y) * shapeColor;
		}

		private byte SampleShapeA()
		{
			var x = (int)(shapeCoordB.x * shapeW);
			var y = (int)(shapeCoordB.y * shapeH);

			return (byte)(shape.GetPixel(x, y).a * shapeColor.a * 255.0f);
		}

		private static bool CalculateShapeCoord(float u, float v)
		{
			shapeCoordA.x = u;
			shapeCoordA.y = v;

			shapeCoordB = inverse.MultiplyPoint(shapeCoordA);

			return shapeCoordB.x >= 0.0f && shapeCoordB.x < 1.0f && shapeCoordB.y >= 0.0f && shapeCoordB.y < 1.0f;
		}
	}
}