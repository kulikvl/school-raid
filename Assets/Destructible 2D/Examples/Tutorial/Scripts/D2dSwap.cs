using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dSwap))]
	public class D2dSwap_Editor : D2dEditor<D2dSwap>
	{
		protected override void OnInspector()
		{
			BeginError(Any(t => t.VisualSprite == null));
				DrawDefault("VisualSprite", "The visual sprite you want to swap in.");
			EndError();
		}
	}
}
#endif

namespace Destructible2D
{
	// This component causes the current GameObject to follow the target Transform
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(SpriteRenderer))]
	[HelpURL(D2dHelper.HelpUrlPrefix + "D2dSwap")]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Swap")]
	public class D2dSwap : MonoBehaviour
	{
		[Tooltip("The visual sprite you want to swap in.")]
		public Sprite VisualSprite;

		[System.NonSerialized]
		private SpriteRenderer cachedSpriteRenderer;

		public void Swap()
		{
			if (cachedSpriteRenderer == null) cachedSpriteRenderer = GetComponent<SpriteRenderer>();

			cachedSpriteRenderer.sprite = VisualSprite;
		}
	}
}