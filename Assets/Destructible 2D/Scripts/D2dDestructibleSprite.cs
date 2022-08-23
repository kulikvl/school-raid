using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dDestructibleSprite))]
	public class D2dDestructibleSprite_Editor : D2dDestructible_Editor<D2dDestructibleSprite>
	{
		protected override void OnInspector()
		{
			var rebuild      = false;
			var updateSprite = false;

			if (Any(t => t.CachedSpriteRenderer.flipX == true || t.CachedSpriteRenderer.flipY == true))
			{
				EditorGUILayout.HelpBox("D2dDestructible isn't compatible with flipped sprites, use Transform.localRotation instead.", MessageType.Error);
			}

			if (Any(t => t.CachedSpriteRenderer.drawMode != SpriteDrawMode.Simple))
			{
				EditorGUILayout.HelpBox("D2dDestructible is only compatible with Simple sprites.", MessageType.Error);
			}

			if (Any(t => t.Shape != null && t.Shape.packed == true && t.Shape.packingMode != SpritePackingMode.Tight))
			{
				EditorGUILayout.HelpBox("D2dDestructible is only compatible with sprites packed with the import setting: Mesh Type = Tight", MessageType.Error);
			}

			BeginError(Any(t => t.Shape == null));
				DrawDefault("shape", ref rebuild, "This allows you to set the shape of the destructible sprite.\nNOTE: This should match the settings of your visual sprite.");
			EndError();
			DrawDefault("overrideTexture", "This allows you to override the sprite texture with any Texture.");
			DrawDefault("channels", ref rebuild, "This allows you to set which color channels you want the destructible texture to use.");
			DrawDefault("cropSprite", ref updateSprite, "Enable this if you want the attached SpriteRenderer.sprite to automatically crop to the AlphaTex boundary, reducing the fillrate requirements for large splitting sprites.");
			DrawDefault("rebuildInGame", "To save scene file size you can Clear your destructible, and allow it to Rebuilt on Start.");

			if (Any(t => t.RebuildInGame == true))
			{
				if (Application.isPlaying == false && Any(t => t.Ready == true))
				{
					EditorGUILayout.HelpBox("If you want your destructible sprite to be rebuilt in game, then you should click the Clear button in edit mode.", MessageType.Warning);
				}
				BeginIndent();
					DrawDefault("rebuildOptimizeCount", "This allows you to set how many times the rebuilt alpha data will be optimzied when rebuilt on Start.");
				EndIndent();
			}

			if (Any(t => t.Ready == false && t.RebuildInGame == false))
			{
				EditorGUILayout.HelpBox("If you want your destructible sprite to be rebuilt on Play, then you should Clear it in edit mode.", MessageType.Warning);
			}

			if (GUILayout.Button("Rebuild") == true)
			{
				DirtyEach(t => t.Rebuild());
			}

			Separator();

			base.OnInspector();

			if (rebuild == true)
			{
				DirtyEach(t => t.Rebuild());
			}

			if (updateSprite == true)
			{
				DirtyEach(t => t.UpdateSprite());
			}
		}
	}
}
#endif

namespace Destructible2D
{
	/// <summary>This component allows you to turn a normal SpriteRenderer into a destructible one. The destruction is stored using a copy of the alpha/opacity of the original sprite, and you have many options to reduce/optimize the amount of destruction pixels used, as well as cut holes in the data.</summary>
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(SpriteRenderer))]
	[HelpURL(D2dHelper.HelpUrlPrefix + "D2dDestructibleSprite")]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Destructible Sprite")]
	public class D2dDestructibleSprite : D2dDestructible
	{
		public enum ChannelType
		{
			AlphaOnly,
			AlphaWithWhiteRGB,
			FullRGBA
		}

		/// <summary>This allows you to set the shape of the destructible sprite.\nNOTE: This should match the settings of your visual sprite.</summary>
		public Sprite Shape { set { shape = value; } get { return shape; } } [FormerlySerializedAs("alphaSprite")] [SerializeField] private Sprite shape;

		/// <summary>This allows you to override the sprite texture with any Texture.</summary>
		public Texture OverrideTexture { set { overrideTexture = value; } get { return overrideTexture; } } [SerializeField] private Texture overrideTexture;

		/// <summary>This allows you to set which color channels you want the destructible texture to use.</summary>
		public ChannelType Channels { set { if (value != channels) { channels = value; RebuildAlphaTex(); } } get { return channels; } } [SerializeField] private ChannelType channels;

		/// <summary>This allows you to set channels without triggering a rebuild.</summary>
		public ChannelType ChannelsRaw { set { channels = value; } get { return channels; } }

		/// <summary>Enable this if you want the attached SpriteRenderer.sprite to automatically crop to the AlphaTex boundary, reducing the fillrate requirements for large splitting sprites.</summary>
		public bool CropSprite { set { if (value != cropSprite) { cropSprite = value; UpdateSprite(); } } get { return cropSprite; } } [SerializeField] private bool cropSprite = true;

		/// <summary>To save scene file size you can Clear your destructible, and allow it to Rebuilt on Start.</summary>
		public bool RebuildInGame { set { rebuildInGame = value; } get { return rebuildInGame; } } [FormerlySerializedAs("rebuildWhenPlaying")] [SerializeField] private bool rebuildInGame;

		/// <summary>This allows you to set how many times the rebuilt alpha data will be optimzied when rebuilt on Start.</summary>
		public int RebuildOptimizeCount { set { rebuildOptimizeCount = value; } get { return rebuildOptimizeCount; } } [SerializeField] private int rebuildOptimizeCount;

		[SerializeField]
		private Sprite originalSprite;

		[SerializeField]
		private Sprite clonedSprite;

		[System.NonSerialized]
		protected SpriteRenderer cachedSpriteRenderer;

		[System.NonSerialized]
		private bool cachedSpriteRendererSet;

		[System.NonSerialized]
		private static Vector2[] vertices = new Vector2[4];

		[System.NonSerialized]
		private static readonly ushort[] triangles = { 0, 1, 2, 3, 2, 1 };

		/// <summary>This gives you the attached SpriteRenderer.</summary>
		public SpriteRenderer CachedSpriteRenderer
		{
			get
			{
				if (cachedSpriteRendererSet == false)
				{
					cachedSpriteRenderer    = GetComponent<SpriteRenderer>();
					cachedSpriteRendererSet = true;
				}

				return cachedSpriteRenderer;
			}
		}

		/// <summary>This tells you if the attached SpriteRenderer's sharedMaterial uses the default Unity sprite material, which isn't compatible with destructible objects.</summary>
		public override bool InvalidMaterial
		{
			get
			{
				var material = CachedSpriteRenderer.sharedMaterial;

				return material == null || material.shader == null || material.shader.name == "Sprites/Default";
			}
		}

		public override TextureFormat FinalFormat
		{
			get
			{
				return channels == ChannelType.AlphaOnly ? TextureFormat.Alpha8 : TextureFormat.ARGB32;
			}
		}

		public override bool IsAbove(D2dDestructible other)
		{
			var otherDestructibleSprite = other as D2dDestructibleSprite;

			if (otherDestructibleSprite != null)
			{
				var thisSr  = CachedSpriteRenderer;
				var otherSr = otherDestructibleSprite.CachedSpriteRenderer;

				return thisSr.sortingLayerID >= otherSr.sortingLayerID && thisSr.sortingOrder >= otherSr.sortingOrder;
			}

			return false;
		}

		/// <summary>If you're using the normal Unity sprite material, then this swaps it to the Destructible 2D supported equivalent.</summary>
		public override void ChangeMaterial()
		{
			CachedSpriteRenderer.sharedMaterial = Resources.Load<Material>("Destructible 2D/Default");
		}

		public void UpdateSprite()
		{
			if (cropSprite == true)
			{
				if (CachedSpriteRenderer.sprite == null)
				{
					cachedSpriteRenderer.sprite = originalSprite;
				}

				if (cachedSpriteRenderer.sprite != clonedSprite)
				{
					originalSprite = cachedSpriteRenderer.sprite;
					clonedSprite   = D2dHelper.Destroy(clonedSprite);
					clonedSprite   = cachedSpriteRenderer.sprite = Instantiate(originalSprite);
				}
			}
			else
			{
				if (CachedSpriteRenderer.sprite == clonedSprite)
				{
					CachedSpriteRenderer.sprite = originalSprite;

					clonedSprite = D2dHelper.Destroy(clonedSprite);
				}
			}
		}

		/// <summary>This allows you to rebuild the destruction state using the current sprite settings.</summary>
		[ContextMenu("Rebuild")]
		public void Rebuild()
		{
			Rebuild(0);
		}

		/// <summary>This allows you to rebuild the destruction state using the specified sprites.</summary>
		public void Rebuild(int optimizeCount)
		{
			if (shape == null)
			{
				shape = CachedSpriteRenderer.sprite;
			}

			if (shape != null)
			{
				var x   = Mathf.FloorToInt(shape.textureRect.xMin);
				var y   = Mathf.FloorToInt(shape.textureRect.yMin);
				var w   = Mathf.CeilToInt(shape.textureRect.xMax) - x;
				var h   = Mathf.CeilToInt(shape.textureRect.yMax) - y;
				var ppu = shape.pixelsPerUnit;

				ready              = true;
				alphaData          = D2dHelper.ReadPixels(shape.texture, x, y, w, h);
				alphaWidth         = w;
				alphaHeight        = h;
				alphaOffset.x      = (shape.textureRectOffset.x - shape.pivot.x) / ppu;
				alphaOffset.y      = (shape.textureRectOffset.y - shape.pivot.y) / ppu;
				alphaScale.x       = shape.textureRect.width  / ppu;
				alphaScale.y       = shape.textureRect.height / ppu;
				alphaSharpness     = 1.0f;
				originalAlphaCount = CalculateAlphaCount();

				if (channels == ChannelType.AlphaWithWhiteRGB)
				{
					var t = w * h;

					for (var i = 0; i < t; i++)
					{
						var alphaPixel = alphaData[i]; alphaData[i] = new Color32(255, 255, 255, alphaPixel.a);
					}
				}

				for (var i = 0; i < optimizeCount; i++)
				{
					Optimize();
				}

				NotifyRebuilt();
			}
			else
			{
				Clear();
			}
		}

		protected virtual void Start()
		{
			// Auto upgrade data?
#if UNITY_EDITOR
			if (ready == true && alphaScale.x == 0.0f && alphaScale.y == 0.0f && Application.isPlaying == false)
			{
				Rebuild(Mathf.RoundToInt(Mathf.Log(alphaSharpness, 2.0f)));

				EditorUtility.SetDirty(this);
				EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
			}
#endif
#if UNITY_EDITOR
			if (Application.isPlaying == false)
			{
				return;
			}
#endif
			if (rebuildInGame == true)
			{
				Rebuild(rebuildOptimizeCount);
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			if (cropSprite == true)
			{
				if (shape == null)
				{
					shape = CachedSpriteRenderer.sprite;
				}

				if (originalSprite != null)
				{
					clonedSprite = CachedSpriteRenderer.sprite = Instantiate(originalSprite);
				}
				else
				{
					originalSprite = CachedSpriteRenderer.sprite;
					clonedSprite   = cachedSpriteRenderer.sprite = Instantiate(originalSprite);
				}
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			if (CachedSpriteRenderer.sprite == clonedSprite)
			{
				CachedSpriteRenderer.sprite = originalSprite;
			}

			clonedSprite   = D2dHelper.Destroy(clonedSprite);
			originalSprite = null;
		}

		protected override void LateUpdate()
		{
			base.LateUpdate();

			UpdateSprite();

			if (cropSprite == true && clonedSprite != null)
			{
				var ppu = clonedSprite.pixelsPerUnit;
				var siz = clonedSprite.rect.size;
				var alphaOffsetX      = (clonedSprite.textureRectOffset.x - clonedSprite.pivot.x) / ppu;
				var alphaOffsetY      = (clonedSprite.textureRectOffset.y - clonedSprite.pivot.y) / ppu;
				var alphaScaleX       = clonedSprite.textureRect.width  / ppu;
				var alphaScaleY       = clonedSprite.textureRect.height / ppu;

				var l = siz.x * Mathf.InverseLerp(alphaOffsetX, alphaOffsetX + alphaScaleX, alphaOffset.x);
				var b = siz.y * Mathf.InverseLerp(alphaOffsetY, alphaOffsetY + alphaScaleY, alphaOffset.y);
				var r = siz.x * Mathf.InverseLerp(alphaOffsetX, alphaOffsetX + alphaScaleX, alphaOffset.x + alphaScale.x);
				var t = siz.y * Mathf.InverseLerp(alphaOffsetY, alphaOffsetY + alphaScaleY, alphaOffset.y + alphaScale.y);

				vertices[0] = new Vector2(l, b);
				vertices[1] = new Vector2(r, b);
				vertices[2] = new Vector2(l, t);
				vertices[3] = new Vector2(r, t);

				clonedSprite.OverrideGeometry(vertices, triangles);
			}
		}

		protected virtual void OnDestroy()
		{
			D2dHelper.Destroy(clonedSprite);
		}

		[System.NonSerialized]
		private static MaterialPropertyBlock propertyBlock;

		[System.NonSerialized]
		private static bool propertyBlockSet;

		protected virtual void OnWillRenderObject()
		{
			var renderer = CachedSpriteRenderer;

			OnWillRenderObject(renderer);

			if (overrideTexture != null)
			{
				if (ready == true)
				{
					if (propertyBlockSet == false)
					{
						propertyBlock    = new MaterialPropertyBlock();
						propertyBlockSet = true;
					}

					renderer.GetPropertyBlock(propertyBlock);

					propertyBlock.SetTexture("_MainTex", overrideTexture);

					renderer.SetPropertyBlock(propertyBlock);
				}
			}
		}
	}
}