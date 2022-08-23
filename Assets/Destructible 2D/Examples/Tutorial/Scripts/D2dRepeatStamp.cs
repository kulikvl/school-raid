using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace Destructible2D
{
	[UnityEditor.CanEditMultipleObjects]
	[UnityEditor.CustomEditor(typeof(D2dRepeatStamp))]
	public class D2dRepeatStamp_Editor : D2dEditor<D2dRepeatStamp>
	{
		protected override void OnInspector()
		{
			DrawDefault("Layers");

			Separator();
			
			DrawDefault("Paint");
			BeginError(Any(t => t.Shape == null));
				DrawDefault("Shape");
			EndError();
			DrawDefault("Size");
			DrawDefault("Delay");
		}
	}
}
#endif

namespace Destructible2D
{
	/// <summary>This component constantly stamps the current position, allowing you to make effects like melting.</summary>
	[HelpURL(D2dHelper.HelpUrlPrefix + "D2dRepeatStamp")]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Repeat Stamp")]
	public class D2dRepeatStamp : MonoBehaviour
	{
		[Tooltip("The layers the stamp works on.")]
		public LayerMask Layers = -1;

		[Tooltip("This allows you to change the painting type.")]
		public D2dDestructible.PaintType Paint;

		[Tooltip("The shape of the stamp.")]
		public Texture2D Shape;

		[Tooltip("The stamp shape will be multiplied by this.\nSolid White = No Change")]
		public Color Color = Color.white;

		[Tooltip("The size of the stamp in world space.")]
		public Vector2 Size = Vector2.one;

		[Tooltip("The delay between each repeat stamp.")]
		public float Delay = 0.25f;

		private float cooldown;

		protected virtual void Update()
		{
			cooldown -= Time.deltaTime;

			if (cooldown <= 0.0f)
			{
				cooldown = Delay;

				var angle = Random.Range(0.0f, 360.0f);

				D2dStamp.All(Paint, transform.position, Size, angle, Shape, Color, Layers);
			}
		}
	}
}