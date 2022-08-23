using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dCollisionHandler))]
	public class D2dCollisionHandler_Editor : D2dEditor<D2dCollisionHandler>
	{
		protected override void OnInspector()
		{
			EditorGUILayout.HelpBox("This component can be used to detect collisions.", MessageType.Info);
		}
	}
}
#endif

namespace Destructible2D
{
	/// <summary>This component listens for collision events and forwards them using the OnCollision event.</summary>
	[HelpURL(D2dHelper.HelpUrlPrefix + "D2dCollisionHandler")]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Collision Handler")]
	public class D2dCollisionHandler : MonoBehaviour
	{
		/// <summary>This is invoked once for each collision.</summary>
		public event System.Action<Collision2D> OnCollision;

		/// <summary>This is invoked once for each collision.</summary>
		public event System.Action<Collider2D> OnOverlap;

		// Show enable/disable toggle
		protected virtual void OnEnable()
		{
		}

		protected virtual void OnCollisionEnter2D(Collision2D collision)
		{
			if (enabled == true)
			{
				if (OnCollision != null)
				{
					OnCollision(collision);
				}
			}
		}

		protected virtual void OnTriggerStay2D(Collider2D collider)
		{
			if (enabled == true)
			{
				if (OnOverlap != null)
				{
					OnOverlap(collider);
				}
			}
		}
	}
}