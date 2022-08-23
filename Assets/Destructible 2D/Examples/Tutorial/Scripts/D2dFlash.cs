using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dFlash))]
	public class D2dFlash_Editor : D2dEditor<D2dFlash>
	{
		protected override void OnInspector()
		{
			BeginError(Any(t => t.Flash <= 0.0f));
				DrawDefault("Flash");
			EndError();
		}
	}
}
#endif

namespace Destructible2D
{
	// This component automatically adds flash to the D2dCameraFlash component
	[HelpURL(D2dHelper.HelpUrlPrefix + "D2dFlash")]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Flash")]
	public class D2dFlash : MonoBehaviour
	{
		[Tooltip("The amount of flash this applies to the D2dCameraFlash component")]
		public float Flash;

		protected virtual void Awake()
		{
			for (var i = D2dCameraFlash.Instances.Count - 1; i >= 0; i--)
			{
				D2dCameraFlash.Instances[i].Flash += Flash;
			}
		}
	}
}