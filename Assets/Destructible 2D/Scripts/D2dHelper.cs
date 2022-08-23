using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Destructible2D
{
	/// <summary>This class implements various helper methods used by many different code.</summary>
	public static partial class D2dHelper
	{
		public const string HelpUrlPrefix = "https://bitbucket.org/Darkcoder/destructible-2d/wiki/";

		public const string ComponentMenuPrefix = "Destructible 2D/D2D ";

		private static Texture2D tempTexture2D;

		public static Color32[] tempAlphaData;

		// This gives you the time-independent 't' value for lerp when used for dampening
		public static float DampenFactor(float dampening, float deltaTime)
		{
			if (dampening < 0.0f)
			{
				return 1.0f;
			}
#if UNITY_EDITOR
			if (Application.isPlaying == false)
			{
				return 1.0f;
			}
#endif
			return 1.0f - Mathf.Exp(-dampening * deltaTime);
		}

		public static float DampenFactor(float dampening, float deltaTime, float linear)
		{
			var factor = DampenFactor(dampening, deltaTime);

			return factor + linear * deltaTime;
		}

		public static float Divide(float a, float b)
		{
			return b != 0.0f ? a / b : 0.0f;
		}

		public static float Reciprocal(float v)
		{
			return v != 0.0f ? 1.0f / v : 0.0f;
		}

		public static float Atan2(Vector2 xy)
		{
			return Mathf.Atan2(xy.x, xy.y);
		}

		public static float InverseLerp(float a, float b, float value)
		{
			return a != b ? (value - a) / (b - a) : 0.0f;
		}

		public static T Destroy<T>(T o)
			where T : Object
		{
			if (o != null)
			{
#if UNITY_EDITOR
				if (Application.isPlaying == false)
				{
					Object.DestroyImmediate(o, true); return null;
				}
#endif
				Object.Destroy(o);
			}
			
			return null;
		}

#if UNITY_EDITOR
		public static Rect Reserve(float height = 16.0f)
		{
			var rect = default(Rect);

			rect = EditorGUILayout.BeginVertical();
			{
				EditorGUILayout.LabelField(string.Empty, GUILayout.Height(height));
			}
			EditorGUILayout.EndVertical();

			return rect;
		}
#endif

		private static void PrepareTempTexture(int w, int h)
		{
			if (tempTexture2D == null)
			{
				tempTexture2D = new Texture2D(w, h, TextureFormat.ARGB32, false);
			}
			else if (tempTexture2D.width != w || tempTexture2D.height != h)
			{
				tempTexture2D.Resize(w, h, TextureFormat.ARGB32, false);
			}
		}

		public static Color32[] ReadPixels(Texture texture, int x, int y, int w, int h)
		{
			var oldActive     = RenderTexture.active;
			var renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.ARGB32); // TODO: Only blit the rect

			Graphics.Blit(texture, renderTexture);

			RenderTexture.active = renderTexture;

			PrepareTempTexture(w, h);

			y = texture.height - y - h;

			tempTexture2D.ReadPixels(new Rect(x, y, w, h), 0, 0);

			RenderTexture.active = oldActive;

			RenderTexture.ReleaseTemporary(renderTexture);

			return tempTexture2D.GetPixels32();
		}

		public static Color32[] ReadPixelsDirect(Texture texture, int x, int y, int w, int h)
		{
			var pixels    = new Color32[w * h];
			var texture2D = texture as Texture2D;

			if (texture2D != null)
			{
				for (var sampleY = 0; sampleY < h; sampleY++)
				{
					for (var sampleX = 0; sampleX < w; sampleX++)
					{
						pixels[sampleX + sampleY * w] = texture2D.GetPixel(sampleX + x, sampleY + y);
					}
				}
			}

			return pixels;
		}

		public static void ReserveTempAlphaData(int width, int height)
		{
			if (width <= 0 || height <= 0)
			{
				throw new System.ArgumentOutOfRangeException("Invalid width or height");
			}

			var total = width * height;

			// Replace alpha data array?
			if (tempAlphaData == null || tempAlphaData.Length < total)
			{
				tempAlphaData = new Color32[total];
			}
		}

		public static void ReserveTempAlphaDataClear(int width, int height)
		{
			if (width <= 0 || height <= 0)
			{
				throw new System.ArgumentOutOfRangeException("Invalid width or height");
			}

			var total = width * height;

			// Replace alpha data array?
			if (tempAlphaData == null || tempAlphaData.Length < total)
			{
				tempAlphaData = new Color32[total];
			}
			else
			{
				for (var i = 0; i < total; i++)
				{
					tempAlphaData[i] = default(Color32);
                }
			}
		}

		public static void PasteAlpha(Color32[] src, int srcWidth, int srcXMin, int srcXMax, int srcYMin, int srcYMax, int dstXMin, int dstYMin, int dstWidth)
		{
			for (var srcY = srcYMin; srcY < srcYMax; srcY++)
			{
				var dstOffset = (srcY - srcYMin + dstYMin) * dstWidth - srcXMin + dstXMin;
				var srcOffset = srcY * srcWidth;

				for (var srcX = srcXMin; srcX < srcXMax; srcX++)
				{
					var dstI = dstOffset + srcX;
					var srcI = srcOffset + srcX;

					tempAlphaData[dstI] = src[srcI];
				}
			}
		}

		public static Vector3 ScreenToWorldPosition(Vector2 screenPosition, float intercept, Camera camera = null)
		{
			if (camera == null) camera = Camera.main;
			if (camera == null) return screenPosition;

			// Get ray of screen position
			var ray = camera.ScreenPointToRay(screenPosition);

			// Find point along this ray that intersects with Z = 0
			var distance = Divide(ray.origin.z - intercept, ray.direction.z);

			return ray.origin - ray.direction * distance;
		}

		/// <summary>This will return true if the specified layer index (0..31) is inside the specified layer mask (e.g. from LayerMask).</summary>
		public static bool IndexInMask(int index, int mask)
		{
			return ((1 << index) & mask) != 0;
		}

		public static bool CalculateRect(Matrix4x4 matrix, ref D2dRect rect, int sizeX, int sizeY)
		{
			// Grab transformed corners
			var a = matrix.MultiplyPoint(new Vector3(0.0f, 0.0f, 0.0f));
			var b = matrix.MultiplyPoint(new Vector3(1.0f, 0.0f, 0.0f));
			var c = matrix.MultiplyPoint(new Vector3(0.0f, 1.0f, 0.0f));
			var d = matrix.MultiplyPoint(new Vector3(1.0f, 1.0f, 0.0f));
			
			// Find min/max x/y
			var minX = Mathf.Min(Mathf.Min(a.x, b.x), Mathf.Min(c.x, d.x));
			var maxX = Mathf.Max(Mathf.Max(a.x, b.x), Mathf.Max(c.x, d.x));
			var minY = Mathf.Min(Mathf.Min(a.y, b.y), Mathf.Min(c.y, d.y));
			var maxY = Mathf.Max(Mathf.Max(a.y, b.y), Mathf.Max(c.y, d.y));
			
			// Has volume?
			if (minX < maxX && minY < maxY)
			{
				rect.MinX = Mathf.FloorToInt(minX * sizeX);
				rect.MaxX = Mathf. CeilToInt(maxX * sizeX);
				rect.MinY = Mathf.FloorToInt(minY * sizeY);
				rect.MaxY = Mathf. CeilToInt(maxY * sizeY);
				
				return true;
			}

			return false;
		}
	}
}