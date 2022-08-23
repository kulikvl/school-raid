using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	public class D2dDestructible_Editor<T> : D2dEditor<T>
		where T : D2dDestructible
	{
		protected override void OnInspector()
		{
			var rebuild = false;

			if (Any(t => t.GetComponent<PolygonCollider2D>() != null))
			{
				EditorGUILayout.HelpBox("D2dDestructible isn't compatible with PolygonCollider2D, use D2dPolygonCollider instead.", MessageType.Warning);
			}

			if (Any(t => t.GetComponent<EdgeCollider2D>() != null))
			{
				EditorGUILayout.HelpBox("D2dDestructible isn't compatible with EdgeCollider2D, use D2dEdgeCollider instead.", MessageType.Warning);
			}

			DrawDefault("healSnapshot", "If you want to be able to heal this destructible sprite, then set a snapshot of the healed state here.");

			if (Any(t => t.HealSnapshot != null && t.CanHeal == false))
			{
				EditorGUILayout.HelpBox("This healSnapshot is incompatible with this destructible sprite state.", MessageType.Warning);
			}

			DrawDefault("overrideSharpness", "This allows you to manually control the sharpness of the alpha gradient:\nZero = AlphaSharpness\nPositive = OverrideSharpness\nNegative = AlphaSharpness * -OverrideSharpness");
			DrawDefault("paintMultiplier", "This allows you to control how easily this object can be painted.\n\n1 = Default.\n2 = Twice as much damage.\n0.5 = Half as much damage.");
			DrawDefault("pixels", "This allows you to control how the alphaTex pixels are handled.");
			DrawDefault("indestructible", "This keeps your destructible sprite active, but prevents it from taking visual damage.");

			Separator();

			EditorGUILayout.BeginHorizontal();
				if (Any(t => t.InvalidMaterial))
				{
					if (GUILayout.Button("Change Material") == true)
					{
						Each(t => { if (t.InvalidMaterial == true) t.ChangeMaterial(); });
					}
				}

				if (GUILayout.Button("Optimize") == true)
				{
					DirtyEach(t => t.Optimize());
				}

				if (GUILayout.Button("Trim") == true)
				{
					DirtyEach(t => t.Trim());
				}

				if (GUILayout.Button("Clear") == true)
				{
					DirtyEach(t => t.Clear());
				}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
				if (Any(t => t.GetComponent<D2dCollider>() == null))
				{
					if (GUILayout.Button("+ Polygon Collider") == true)
					{
						Each(t => t.gameObject.AddComponent<D2dPolygonCollider>());
					}

					if (GUILayout.Button("+ Edge Collider") == true)
					{
						Each(t => t.gameObject.AddComponent<D2dEdgeCollider>());
					}
				}

				if (Any(t => t.GetComponent<D2dSplitter>() == null) && GUILayout.Button("+ Splitter") == true)
				{
					Each(t => t.gameObject.AddComponent<D2dSplitter>());
				}
			EditorGUILayout.EndHorizontal();

			Separator();

			if (Targets.Length == 1)
			{
				BeginDisabled();
					EditorGUI.ObjectField(D2dHelper.Reserve(), "Alpha Tex", Target.AlphaTex, typeof(Texture2D), true);
					EditorGUILayout.IntField("Alpha Width", Target.AlphaWidth);
					EditorGUILayout.IntField("Alpha Height", Target.AlphaHeight);
					EditorGUILayout.FloatField("Alpha Sharpness", Target.AlphaSharpness);
					EditorGUILayout.IntField("Alpha Count", Target.AlphaCount);
					EditorGUILayout.IntField("Original Alpha Count", Target.OriginalAlphaCount);
					EditorGUI.ProgressBar(D2dHelper.Reserve(), Target.AlphaRatio, "Alpha Ratio");
				EndDisabled();
			}

			if (rebuild == true)
			{
				DirtyEach(t => t.RebuildAlphaTex());
			}
		}
	}
}
#endif

namespace Destructible2D
{
	/// <summary>This is the base class for all destructible objects.</summary>
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[HelpURL(D2dHelper.HelpUrlPrefix + "D2dDestructible")]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Destructible")]
	public abstract partial class D2dDestructible : D2dLinkedBehaviour<D2dDestructible>
	{
		public enum PixelsType
		{
			Smooth,
			Pixelated,
			PixelatedBinary
		}

		/// <summary>This is invoked when the whole destruction state changes.</summary>
		public event System.Action OnRebuilt;

		/// <summary>This is invoked when a subset of the destruction state changes.</summary>
		public event System.Action<D2dRect> OnModified;

		/// <summary>This is invoked before the destructible is about to be split into separate parts.</summary>
		public event System.Action OnSplitStart;

		/// <summary>This is invoked after the destructible is split into separate parts, with a list of all the parts.\nLast Element = This Destructible)</summary>
		public event System.Action<List<D2dDestructible>> OnSplitEnd;

		/// <summary>If you want to be able to heal this destructible sprite, then set a snapshot of the healed state here.</summary>
		public D2dSnapshot HealSnapshot { set { healSnapshot = value; } get { return healSnapshot; } } [SerializeField] private D2dSnapshot healSnapshot;

		/// <summary>This allows you to manually control the sharpness of the alpha gradient (0 = AlphaSharpness) (+ = OverrideSharpness) (- = AlphaSharpness * -OverrideSharpness).</summary>
		public float OverrideSharpness { set { overrideSharpness = value; } get { return overrideSharpness; } } [SerializeField] private float overrideSharpness;

		/// <summary>This allows you to control how easily this object can be painted.
		/// 1 = Default.
		/// 2 = Twice as much damage.
		/// 0.5 = Half as much damage.</summary>
		public float PaintMultiplier { set { paintMultiplier = value; } get { return paintMultiplier; } } [SerializeField] private float paintMultiplier = 1.0f;

		/// <summary>This allows you to control how the alphaTex pixels are handled.</summary>
		public PixelsType Pixels { set { pixels = value; } get { return pixels; } } [SerializeField] private PixelsType pixels;

		/// <summary>This keeps your destructible sprite active, but prevents it from taking visual damage.</summary>
		public bool Indestructible { set { indestructible = value; } get { return indestructible; } } [SerializeField] private bool indestructible;

		/// <summary>If this destructible has been generated correctly, this will be set.</summary>
		public bool Ready { get { return ready; } } [SerializeField] protected bool ready;

		/// <summary>This stores the current visual damage state of the destructible.</summary>
		public Color32[] AlphaData { set { alphaData = value; } get { return alphaData; } } [SerializeField] protected Color32[] alphaData;

		/// <summary>This stores the current width of the visual damage data.</summary>
		public int AlphaWidth { set { alphaWidth = value; } get { return alphaWidth; } } [SerializeField] protected int alphaWidth;

		/// <summary>This stores the current height of the visual damage data.</summary>
		public int AlphaHeight { set { alphaHeight = value; } get { return alphaHeight; } } [SerializeField] protected int alphaHeight;

		/// <summary>This tells you how many pixels in the alphaData/alphaTex are solid (above 127).</summary>
		public int AlphaCount { set { alphaCount = value; } get { if (alphaCount == -1) CalculateAlphaCount(); return alphaCount; } } [SerializeField] private int alphaCount;

		/// <summary>This allows you to read the alphaCount value directly without causing it to be recalculated.</summary>
		public int AlphaCountRaw { set { alphaCount = value; } get { return alphaCount; } }

		/// <summary>This tells you the original AlphaCount value, if it was set.</summary>
		public int OriginalAlphaCount { set { originalAlphaCount = value; } get { if (originalAlphaCount == -1) CalculateAlphaCount(); return originalAlphaCount; } } [SerializeField] protected int originalAlphaCount;

		/// <summary>This allows you to read the originalAlphaCount value directly without causing it to be recalculated.</summary>
		public int OriginalAlphaCountRaw { set { originalAlphaCount = value; } get { return originalAlphaCount; } }

		/// <summary>This will return the ratio of remaining alpha (0 = no pixels remain, 1 = all pixels remain).</summary>
		public float AlphaRatio { get { return D2dHelper.Divide(AlphaCount, OriginalAlphaCount); } }

		/// <summary>This tells you offset of the alpha data in local space.</summary>
		public Vector2 AlphaOffset { set { alphaOffset = value; } get { return alphaOffset; } } [SerializeField] protected Vector2 alphaOffset;

		/// <summary>This tells you scale of the alpha data in local space.</summary>
		public Vector2 AlphaScale { set { alphaScale = value; } get { return alphaScale; } } [SerializeField] protected Vector2 alphaScale;

		/// <summary>Each time you optimize/halve this sprite, this value will double.</summary>
		public float AlphaSharpness { get { return alphaSharpness; } } [SerializeField] protected float alphaSharpness;

		/// <summary>This stores the current texture of the visual destruction state.</summary>
		public Texture2D AlphaTex { get { return alphaTex; } } [System.NonSerialized] private Texture2D alphaTex;

		/// <summary>This stores the pixel region of the alphaData that hasn't been copied to the texture yet. These pixels will be copied in LateUpdate.</summary>
		[System.NonSerialized]
		public D2dRect AlphaModified;

		// Set while Split is being invoked, to prevent infinite cycles and such
		[System.NonSerialized]
		public bool IsSplitting;

		// Is OnStartSplit currently being invoked? (used to prevent collider generation issues)
		[System.NonSerialized]
		public bool IsOnStartSplit;

		[System.NonSerialized]
		private static MaterialPropertyBlock propertyBlock;

		[System.NonSerialized]
		private static bool propertyBlockSet;

		/// <summary>This returns true if the healSnapshot is in a valid state for healing this destructible sprite.</summary>
		public bool CanHeal
		{
			get
			{
				if (healSnapshot != null)
				{
					var data = healSnapshot.DataRaw;

					if (data != null && data.Ready == true && data.AlphaWidth == alphaWidth && data.AlphaHeight == alphaHeight)
					{
						return true;
					}
				}

				return false;
			}
		}

		/// <summary>This tells you the format the alphaTex should have based on your settings.</summary>
		public abstract TextureFormat FinalFormat
		{
			get;
		}

		public abstract bool InvalidMaterial
		{
			get;
		}

		public abstract void ChangeMaterial();

		/// <summary>This allows you to cut and smooth the edges of your destructible. This is automatically done in many cases, but when making a new destructible it isn't, so you can control how the edges look yourself.</summary>
		[ContextMenu("Trim")]
		public void Trim()
		{
			if (ready == true)
			{
				D2dTrim.Trim(this);

				NotifyRebuilt();
			}
		}

		/// <summary>This allows you to blur the pixels in your current destruction state. This can be used for certain effects, or to smooth the edges.</summary>
		[ContextMenu("Blur")]
		public void Blur()
		{
			if (ready == true)
			{
				D2dBlur.Blur(this);

				NotifyRebuilt();
			}
		}

		/// <summary>This allows you to threshold all the pixels in your current destruction state. This will set them to full opacity if they are above half opacity, otheriwse they will be set to zero opacity.</summary>
		[ContextMenu("Threshold")]
		public void Threshold()
		{
			if (ready == true)
			{
				for (var i = alphaWidth * alphaHeight - 1; i >= 0; i--)
				{
					var pixel = alphaData[i];

					pixel.a = pixel.a > 127 ? (byte)255 : (byte)0;

					alphaData[i] = pixel;
				}

				NotifyRebuilt();
			}
		}

		/// <summary>This allows you to halve the width & height of your destruction pixels.</summary>
		[ContextMenu("Halve")]
		public void Halve()
		{
			if (ready == true && alphaWidth > 2 && alphaHeight > 2)
			{
				D2dHalve.Halve(ref alphaData, ref alphaWidth, ref alphaHeight, ref alphaOffset, ref alphaScale);

				alphaSharpness    *= 2;
				originalAlphaCount = CalculateAlphaCount();

				NotifyRebuilt();
			}
		}

		/// <summary>This allows you to reduce the amount of pixels used to store the destruction state of your sprite. Each time you do this you will increase performance 4x, but there will be some visual quality loss.</summary>
		[ContextMenu("Optimize")]
		public void Optimize()
		{
			if (ready == true && alphaWidth > 2 && alphaHeight > 2)
			{
				D2dTrim.Trim(this);
				D2dBlur.Blur(this);
				D2dHalve.Halve(ref alphaData, ref alphaWidth, ref alphaHeight, ref alphaOffset, ref alphaScale);
				D2dTrim.Trim(this);

				alphaSharpness    *= 2;
				originalAlphaCount = CalculateAlphaCount();

				NotifyRebuilt();
			}
		}

		/// <summary>This allows you to clear all destruction data from the sprite, reverting it to a normal non-destructible sprite. NOTE: You will need to manually revert the material to completely revert the sprite state.</summary>
		[ContextMenu("Clear")]
		public void Clear()
		{
			ready              = false;
			alphaSharpness     = 0.0f;
			alphaTex           = D2dHelper.Destroy(alphaTex);
			alphaData          = null;
			alphaWidth         = 0;
			alphaHeight        = 0;
			alphaCount         = 0;
			originalAlphaCount = 0;

			AlphaModified.Clear();

			NotifyRebuilt();
		}

		public void RebuildAlphaTex()
		{
			if (alphaTex != null)
			{
				var format = FinalFormat;

				if (alphaTex.format != format)
				{
					alphaTex.Resize(alphaWidth, alphaHeight, format, false);
				}
			}
		}

		/// <summary>Call this if you manually modified the whole destruction state.</summary>
		public void NotifyRebuilt()
		{
			if (ready == true && alphaTex != null)
			{
				var format = FinalFormat;

				if (alphaTex.width != alphaWidth || alphaTex.height != alphaHeight || alphaTex.format != format)
				{
					alphaTex.Resize(alphaWidth, alphaHeight, format, false);
				}

				alphaTex.SetPixels32(alphaData);
				alphaTex.Apply();

				AlphaModified.Clear();
			}

			if (OnRebuilt != null)
			{
				OnRebuilt();
			}
		}

		private void NotifyModified(D2dRect rect)
		{
			AlphaModified.Clear();

			if (OnModified != null)
			{
				OnModified(rect);
			}
		}

		protected int CalculateAlphaCount()
		{
			alphaCount = 0;

			if (ready == true)
			{
				var total = alphaWidth * alphaHeight;

				for (var i = 0; i < total; i++)
				{
					if (alphaData[i].a >= 128)
					{
						alphaCount++;
					}
				}
			}

			return alphaCount;
		}

		public Matrix4x4 WorldToAlphaMatrix
		{
			get
			{
				if (ready == true)
				{
					var matrix1 = Matrix4x4.identity;
					var matrix2 = Matrix4x4.identity;

					matrix1.m00 = D2dHelper.Reciprocal(alphaScale.x);
					matrix1.m11 = D2dHelper.Reciprocal(alphaScale.y);

					matrix2.m03 = -alphaOffset.x;
					matrix2.m13 = -alphaOffset.y;

					return matrix1 * matrix2 * transform.worldToLocalMatrix;
				}

				return Matrix4x4.identity;
			}
		}

		public void SubsetAlphaWith(Color32[] subData, D2dRect subRect, int newAlphaCount = -1)
		{
			var stepX = D2dHelper.Divide(alphaScale.x, alphaWidth );
			var stepY = D2dHelper.Divide(alphaScale.y, alphaHeight);

			alphaOffset.x += stepX * subRect.MinX;
			alphaOffset.y += stepY * subRect.MinY;
			alphaScale.x  += stepX * (subRect.SizeX - alphaWidth );
			alphaScale.y  += stepY * (subRect.SizeY - alphaHeight);

			FastCopyAlphaData(subData, subRect.SizeX, subRect.SizeY, newAlphaCount);

			NotifyRebuilt();
		}

		private void FastCopyAlphaData(Color32[] newAlphaData, int newAlphaWidth, int newAlphaHeight, int newAlphaCount = -1)
		{
			var newAlphaTotal = newAlphaWidth * newAlphaHeight;

			if (alphaData == null || alphaData.Length != newAlphaTotal)
			{
				alphaData = new Color32[newAlphaTotal];
			}

			for (var i = newAlphaTotal - 1; i >= 0; i--)
			{
				alphaData[i] = newAlphaData[i];
			}

			alphaWidth  = newAlphaWidth;
			alphaHeight = newAlphaHeight;
			alphaCount  = newAlphaCount;
		}

		private static List<D2dDestructible> splitDestructibles = new List<D2dDestructible>();

		public void SplitBegin()
		{
			splitDestructibles.Clear();

			alphaData = null;

			if (OnSplitStart != null)
			{
				OnSplitStart();
			}
		}

		public D2dDestructible SplitNext(bool isLast)
		{
			var splitDestructible = default(D2dDestructible);

			if (isLast == true)
			{
				splitDestructible = this;
			}
			else
			{
				splitDestructible = Instantiate(this, transform.position, transform.rotation);
			}

			splitDestructibles.Add(splitDestructible);

			return splitDestructible;
		}

		public void SplitEnd()
		{
			if (OnSplitEnd != null)
			{
				OnSplitEnd(splitDestructibles);
			}
		}

		public Color32 SampleAlpha(Vector3 worldPosition)
		{
			var uv = (Vector2)WorldToAlphaMatrix.MultiplyPoint(worldPosition);

			if (uv.x >= 0.0f && uv.y >= 0.0f && uv.x < 1.0f && uv.y < 1.0f)
			{
				var x = Mathf.FloorToInt(uv.x * alphaWidth );
				var y = Mathf.FloorToInt(uv.y * alphaHeight);

				return alphaData[x + y * alphaWidth];
			}

			return default(Color32);
		}

		public static Color32 TrySampleAlphaAll(Vector3 worldPosition)
		{
			var destructible = default(D2dDestructible);
			var alpha        = default(Color32);

			TrySampleAlphaAll(worldPosition, ref destructible, ref alpha);

			return alpha;
		}

		public static bool TrySampleThrough(Vector3 worldPosition, ref D2dDestructible hitDestructible, byte threshold = 127)
		{
			var hit          = false;
			var destructible = FirstInstance;

			for (var i = 0; i < InstanceCount; i++)
			{
				var alpha = destructible.SampleAlpha(worldPosition);

				if (alpha.a > threshold)
				{
					if (hit == false || destructible.IsAbove(hitDestructible) == true)
					{
						hit             = true;
						hitDestructible = destructible;
					}
				}

				destructible = destructible.NextInstance;
			}

			return hit;
		}

		public abstract bool IsAbove(D2dDestructible other);

		public static bool TrySampleAlphaAll(Vector3 worldPosition, ref D2dDestructible hitDestructible, ref Color32 hitAlpha)
		{
			var hit          = false;
			var destructible = FirstInstance;

			for (var i = 0; i < InstanceCount; i++)
			{
				var alpha = destructible.SampleAlpha(worldPosition);

				if (alpha.a > 0.0f)
				{
					if (hit == false || destructible.IsAbove(hitDestructible) == true)
					{
						hit             = true;
						hitDestructible = destructible;
						hitAlpha        = alpha;
					}
				}

				destructible = destructible.NextInstance;
			}

			return hit;
		}

		protected virtual void LateUpdate()
		{
			if (ready == true && AlphaModified.IsSet == true && alphaTex != null)
			{
				var w = AlphaModified.SizeX;
				var h = AlphaModified.SizeY;

				// Replace all pixels?
				if (w * h > 1000)
				{
					alphaTex.SetPixels32(alphaData);
					alphaTex.Apply();
				}
				// Replace updated pixels?
				else
				{
					var i = 0;

					D2dHelper.ReserveTempAlphaData(w, h);

					for (var y = AlphaModified.MinY; y < AlphaModified.MaxY; y++)
					{
						var o = y * alphaWidth;

						for (var x = AlphaModified.MinX; x < AlphaModified.MaxX; x++)
						{
							D2dHelper.tempAlphaData[i++] = alphaData[o + x];
						}
					}

					alphaTex.SetPixels32(AlphaModified.MinX, AlphaModified.MinY, w, h, D2dHelper.tempAlphaData);
					alphaTex.Apply();
				}

				NotifyModified(AlphaModified);
			}
		}

		protected virtual void Destroy()
		{
			D2dHelper.Destroy(alphaTex);
		}

		protected virtual void OnWillRenderObject(Renderer renderer)
		{
			if (ready == true)
			{
				if (propertyBlockSet == false)
				{
					propertyBlock    = new MaterialPropertyBlock();
					propertyBlockSet = true;
				}

				renderer.GetPropertyBlock(propertyBlock);

				if (alphaTex == null)
				{
					alphaTex = new Texture2D(alphaWidth, alphaHeight, FinalFormat, false);

					alphaTex.wrapMode = TextureWrapMode.Clamp;

					alphaTex.SetPixels32(alphaData);
					alphaTex.Apply();

					AlphaModified.Clear();
				}

				alphaTex.filterMode = pixels == PixelsType.Smooth ? FilterMode.Bilinear : FilterMode.Point;

				propertyBlock.SetTexture("_AlphaTex", alphaTex);
				propertyBlock.SetVector("_AlphaScale", alphaScale);
				propertyBlock.SetVector("_AlphaOffset", alphaOffset);

				if (overrideSharpness == 0.0f)
				{
					propertyBlock.SetFloat("_AlphaSharpness", alphaSharpness);
				}
				else if (overrideSharpness > 0.0f)
				{
					propertyBlock.SetFloat("_AlphaSharpness", overrideSharpness);
				}
				else
				{
					propertyBlock.SetFloat("_AlphaSharpness", alphaSharpness * -overrideSharpness);
				}

				renderer.SetPropertyBlock(propertyBlock);
			}
		}
	}
}