using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dSplitSpawner))]
	public class D2dSplitSpawner_Editor : D2dEditor<D2dSplitSpawner>
	{
		protected override void OnInspector()
		{
			BeginError(Any(t => t.Prefab == null));
				DrawDefault("Prefab");
			EndError();
		}
	}
}
#endif

namespace Destructible2D
{
	// This component will automatically destroy the current GameObject after a certain amount of time
	[RequireComponent(typeof(D2dDestructible))]
	[RequireComponent(typeof(D2dSplitter))]
	[HelpURL(D2dHelper.HelpUrlPrefix + "D2dSplitSpawner")]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Split Spawner")]
	public class D2dSplitSpawner : MonoBehaviour
	{
		[Tooltip("The prefab spawned on split")]
		public GameObject Prefab;

		[System.NonSerialized]
		private D2dDestructible cachedDestructible;

		protected virtual void OnEnable()
		{
			if (cachedDestructible == null) cachedDestructible = GetComponent<D2dDestructible>();

			cachedDestructible.OnSplitEnd += SplitEnd;
		}

		protected virtual void OnDisable()
		{
			cachedDestructible.OnSplitEnd -= SplitEnd;
		}

		private void SplitEnd(List<D2dDestructible> splitDestructibles)
		{
			if (Prefab != null)
			{
				Instantiate(Prefab, transform.position, Quaternion.identity);
			}
		}
	}
}
