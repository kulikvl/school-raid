using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dFixture))]
	public class D2dFixture_Editor : D2dEditor<D2dFixture>
	{
		protected override void OnInspector()
		{
			DrawDefault("offset", "This allows you to set the local offset of the fixture point.");

			if (Any(t => t.GetComponentInParent<D2dDestructible>()) == false)
			{
				EditorGUILayout.HelpBox("This fixture isn't a child of any D2dSprite", MessageType.Error);
			}

			if (Any(t => {var d = t.GetComponentInParent<D2dDestructible>(); return d == null || d.gameObject != t.gameObject; }) == false)
			{
				EditorGUILayout.HelpBox("Fixtures shouldn't be attached to the same GameObject as the D2dSprite", MessageType.Error);
			}
		}
	}
}
#endif

namespace Destructible2D
{
	/// <summary>This component can be attached to a child GameObject of a D2dDestructible, and when split this will automatically follow the correct split part. If the pixel underneath this fixture is destroyed then this fixture will also be destroyed.</summary>
	[DisallowMultipleComponent]
	[HelpURL(D2dHelper.HelpUrlPrefix + "D2dFixture")]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Fixture")]
	public class D2dFixture : MonoBehaviour
	{
		/// <summary>This allows you to set the local offset of the fixture point.</summary>
		public Vector3 Offset { set { offset = value; } get { return offset; } } [SerializeField] private Vector3 offset;

		[System.NonSerialized]
		private D2dDestructible cachedDestructible;

		protected virtual void OnEnable()
		{
			Hook();
		}

		protected virtual void OnDisable()
		{
			Unhook();
		}

		protected virtual void Update()
		{
			UpdateFixture();
		}

#if UNITY_EDITOR
		protected virtual void OnDrawGizmosSelected()
		{
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.color  = Color.red;

			Gizmos.DrawLine(Offset - Vector3.left, Offset + Vector3.left);
			Gizmos.DrawLine(Offset - Vector3.up  , Offset + Vector3.up  );
		}
#endif

		private void UpdateFixture()
		{
			if (cachedDestructible == null) cachedDestructible = GetComponentInParent<D2dDestructible>();

			if (cachedDestructible == null)
			{
				DestroyFixture();
			}
			else
			{
				var worldPosition = transform.TransformPoint(Offset);

				if (cachedDestructible.SampleAlpha(worldPosition).a < 128)
				{
					DestroyFixture();
				}
			}
		}

		private void DestroyFixture()
		{
			D2dHelper.Destroy(gameObject);
		}

		private void OnStartSplit()
		{
			transform.SetParent(null, false);
		}

		private void OnEndSplit(List<D2dDestructible> destructibles)
		{
			for (var i = destructibles.Count - 1; i >= 0; i--)
			{
				var destructible = destructibles[i];

				if (TryFixTo(destructible) == true)
				{
					return;
				}
			}

			DestroyFixture();
		}

		private bool TryFixTo(D2dDestructible destructible)
		{
			var isDifferent = cachedDestructible != destructible;

			// Temporarily change parent
			transform.SetParent(destructible.transform, false);

			// Find world position of fixture if it were attached to tempDestructible
			var worldPosition = transform.TransformPoint(Offset);

			// Can fix to new point?
			if (destructible.SampleAlpha(worldPosition).a >= 128)
			{
				if (isDifferent == true)
				{
					Unhook();

					cachedDestructible = destructible;

					Hook();
				}

				return true;
			}

			// Change back to old parent
			transform.SetParent(cachedDestructible.transform, false);

			return false;
		}

		private void Hook()
		{
			if (cachedDestructible == null) cachedDestructible = GetComponentInParent<D2dDestructible>();

			cachedDestructible.OnSplitStart += OnStartSplit;
			cachedDestructible.OnSplitEnd   += OnEndSplit;
		}

		private void Unhook()
		{
			cachedDestructible.OnSplitStart -= OnStartSplit;
			cachedDestructible.OnSplitEnd   -= OnEndSplit;
		}
	}
}