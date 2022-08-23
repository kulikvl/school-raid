using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dBackground))]
	public class D2dBackground_Editor : D2dEditor<D2dBackground>
	{
		protected override void OnInspector()
		{
			DrawDefault("Size", "The width of the original sprite in world space (width pixels / pixels per unit).");
			DrawDefault("ParallaxMultiplier", "The distance this layer is from the maain scene focal point, where positive values are in the distance, and negative values are closer to the camera.");
		}
	}
}
#endif

namespace Destructible2D
{
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Background")]
	public class D2dBackground : MonoBehaviour
	{
		[Tooltip("The width of the original sprite in world space (width pixels / pixels per unit).")]
		public Vector2 Size;

		[Tooltip("The distance this layer is from the maain scene focal point, where positive values are in the distance, and negative values are closer to the camera.")]
		public float ParallaxMultiplier;

		protected virtual void Update()
		{
			UpdatePosition(Camera.main);
		}

		private void UpdatePosition(Camera camera)
		{
			if (camera != null)
			{
				// Grab current values
				var currentPosition = transform.position;
				var cameraPosition  = camera.transform.position;

				// Calculate parallax on all axes
				var parallax = cameraPosition * ParallaxMultiplier;

				// Calculate snap while factoring in horizontal parallax
				var snap = default(Vector2);

				if (Size.x > 0.0f)
				{
					snap.x = Mathf.RoundToInt((cameraPosition.x - parallax.x) / Size.x) * Size.x;
				}

				if (Size.y > 0.0f)
				{
					snap.y = Mathf.RoundToInt((cameraPosition.y - parallax.y) / Size.y) * Size.y;
				}

				// Write and update positions
				currentPosition.x = parallax.x + snap.x;
				currentPosition.y = parallax.y + snap.y;

				transform.position = currentPosition;
			}
		}
	}
}