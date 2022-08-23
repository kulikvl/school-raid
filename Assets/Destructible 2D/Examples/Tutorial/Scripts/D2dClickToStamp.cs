using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dClickToStamp))]
	public class D2dClickToStamp_Editor : D2dEditor<D2dClickToStamp>
	{
		protected override void OnInspector()
		{
			DrawDefault("Requires");
			DrawDefault("Intercept");
			BeginError(Any(t => t.Layers == 0));
				DrawDefault("Layers");
			EndError();
			BeginError(Any(t => t.IndicatorPrefab == null));
				DrawDefault("IndicatorPrefab");
			EndError();

			Separator();

			DrawDefault("Paint");
			BeginError(Any(t => t.Shape == null));
				DrawDefault("Shape");
			EndError();
			BeginError(Any(t => t.Size.x == 0.0f || t.Size.y == 0.0f));
				DrawDefault("Size");
			EndError();
			DrawDefault("Angle");
		}
	}
}
#endif

namespace Destructible2D
{
	/// <summary>This component allows you to stamp all destructible sprites under the mouse.</summary>
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Click To Stamp")]
	public class D2dClickToStamp : MonoBehaviour
	{
		[Tooltip("The key you must hold down to perform stamping.")]
		public KeyCode Requires = KeyCode.Mouse0;

		[Tooltip("The z position in world space the indicator should spawn at.")]
		public float Intercept;

		[Tooltip("The destructible sprite layers we want to stamp.")]
		public LayerMask Layers = -1;

		[Tooltip("The prefab used to show what the stamp will look like.")]
		public GameObject IndicatorPrefab;

		[Tooltip("This allows you to change the painting type.")]
		public D2dDestructible.PaintType Paint;

		[Tooltip("The shape of the stamp.")]
		public Texture2D Shape;

		[Tooltip("The stamp shape will be multiplied by this.\nSolid White = No Change")]
		public Color Color = Color.white;

		[Tooltip("The size of the stamp in world space.")]
		public Vector2 Size = Vector2.one;

		[Tooltip("The angle of the stamp in degrees.")]
		public float Angle;

		// Currently dragging?
		[SerializeField]
		private bool down;

		// Instance of the indicator
		[SerializeField]
		private GameObject indicatorInstance;

		protected virtual void Update()
		{
			// Main camera exists?
			var mainCamera = Camera.main;

			if (mainCamera != null)
			{
				// World position of the mouse
				var position = D2dHelper.ScreenToWorldPosition(Input.mousePosition, Intercept, mainCamera);

				// Begin dragging
				if (Input.GetKey(Requires) == true && down == false)
				{
					down = true;
				}

				// End dragging
				if (Input.GetKey(Requires) == false && down == true)
				{
					down = false;

					// Stamp at that point
					D2dStamp.All(Paint, position, Size, Angle, Shape, Color, Layers);
				}

				// Update indicator?
				if (down == true && IndicatorPrefab != null)
				{
					if (indicatorInstance == null)
					{
						indicatorInstance = Instantiate(IndicatorPrefab);
					}

					indicatorInstance.transform.position = position;
				}
				// Destroy indicator?
				else if (indicatorInstance != null)
				{
					Destroy(indicatorInstance.gameObject);
				}
			}
		}
	}
}