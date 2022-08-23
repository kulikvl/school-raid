using UnityEngine;

namespace Destructible2D
{
	/// <summary>This component will horizontally snap the current Transform based on the position of the camera, and also allows for a parallax offset.</summary>
	[ExecuteInEditMode]
	public class D2dHorizontalBackground : MonoBehaviour
	{
		[Tooltip("The width of the original sprite in world space (width pixels / pixels per unit).")]
		public float Width = 10.0f;

		[Tooltip("If the background needs to be moved up or down, specify the world space offset here.")]
		public float VerticalOffset;

		[Tooltip("The distance this layer is from the main scene focal point, where positive values are in the distance, and negative values are closer to the camera.")]
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
				var snap = Mathf.RoundToInt((cameraPosition.x - parallax.x) / Width) * Width;

				// Write and update positions
				currentPosition.x = parallax.x + snap;
				currentPosition.y = parallax.y + VerticalOffset;

				transform.position = currentPosition;
			}
		}
	}
}