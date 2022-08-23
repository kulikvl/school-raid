using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dRetainVelocity))]
	public class D2dRetainVelocity_Editor : D2dEditor<D2dRetainVelocity>
	{
		protected override void OnInspector()
		{
			EditorGUILayout.HelpBox("This component allows a sprite to maintain its velocity after being split.", MessageType.Info);
		}
	}
}
#endif

namespace Destructible2D
{
	/// <summary>This component allows a sprite to maintain its velocity after being split.</summary>
	[DisallowMultipleComponent]
	[RequireComponent(typeof(D2dDestructible))]
	[RequireComponent(typeof(Rigidbody2D))]
	[HelpURL(D2dHelper.HelpUrlPrefix + "D2dRetainVelocity")]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Retain Velocity")]
	public class D2dRetainVelocity : MonoBehaviour
	{
		[System.NonSerialized]
		private Rigidbody2D cachedBody;

		[System.NonSerialized]
		private D2dDestructible cachedDestructible;

		[System.NonSerialized]
		private Vector2 velocity;

		[System.NonSerialized]
		private float angularVelocity;

		protected virtual void OnEnable()
		{
			if (cachedDestructible == null) cachedDestructible = GetComponent<D2dDestructible>();

			cachedDestructible.OnSplitStart += SplitStart;
			cachedDestructible.OnSplitEnd   += SplitEnd;
		}

		protected virtual void OnDisable()
		{
			cachedDestructible.OnSplitStart -= SplitStart;
			cachedDestructible.OnSplitEnd   -= SplitEnd;
		}

		protected virtual void SplitStart()
		{
			if (cachedBody == null) cachedBody = GetComponent<Rigidbody2D>();

			velocity        = cachedBody.velocity;
			angularVelocity = cachedBody.angularVelocity;
		}

		protected virtual void SplitEnd(List<D2dDestructible> splitDestructibles)
		{
			for (var i = splitDestructibles.Count - 1; i >= 0; i--)
			{
				var splitDestructible = splitDestructibles[i];

				if (splitDestructible.gameObject != gameObject)
				{
					var splitRigidbody2D = splitDestructible.GetComponent<Rigidbody2D>();

					if (splitRigidbody2D != null)
					{
						splitRigidbody2D.velocity        = velocity;
						splitRigidbody2D.angularVelocity = angularVelocity;
					}
				}
			}
		}
	}
}