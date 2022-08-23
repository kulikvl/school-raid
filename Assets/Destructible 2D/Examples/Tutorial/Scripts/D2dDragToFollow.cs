using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dDragToFollow))]
	public class D2dDragToFollow_Editor : D2dEditor<D2dDragToFollow>
	{
		protected override void OnInspector()
		{
			DrawDefault("Requires");
			DrawDefault("Intercept");
			BeginError(Any(t => t.Target == null));
				DrawDefault("Target");
			EndError();
			DrawDefault("Dampening");
		}
	}
}
#endif

namespace Destructible2D
{
	/// <summary>This component allows you to drag the mouse and have the target object follow it.</summary>
	[HelpURL(D2dHelper.HelpUrlPrefix + "D2dDragToFollow")]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Drag To Follow")]
	public class D2dDragToFollow : MonoBehaviour
	{
		[Tooltip("The key you must hold down to perform slicing.")]
		public KeyCode Requires = KeyCode.Mouse0;

		[Tooltip("The z position in world space the indicator should spawn at.")]
		public float Intercept;

		[Tooltip("The object that will be dragged.")]
		public Rigidbody2D Target;

		[Tooltip("The speed the object will follow.")]
		public float Dampening = 10.0f;

		protected virtual void FixedUpdate()
		{
			if (Target != null)
			{
				// Get the main camera
				var mainCamera = Camera.main;

				// Begin dragging
				if (Input.GetKey(Requires) == true)
				{
					var position = D2dHelper.ScreenToWorldPosition(Input.mousePosition, Intercept, mainCamera);
					var factor   = D2dHelper.DampenFactor(Dampening, Time.fixedDeltaTime);

					Target.velocity += (Vector2)(position - Target.transform.position) * factor;
				}
			}
		}
	}
}