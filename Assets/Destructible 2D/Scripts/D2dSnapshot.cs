using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dSnapshot))]
	public class D2dSnapshot_Editor : D2dEditor<D2dSnapshot>
	{
		protected override void OnInspector()
		{
			var destructible = (D2dDestructible)EditorGUILayout.ObjectField("Save", default(D2dDestructible), typeof(D2dDestructible), true);

			if (destructible != null)
			{
				DirtyEach(t => t.Data.Save(destructible));
			}

			Separator();

			if (Button("Clear") == true)
			{
				DirtyEach(t => t.Clear());
			}

			Separator();

			if (Target.Data.Ready == true)
			{
				BeginDisabled();
					EditorGUILayout.IntField("Alpha Width", Target.Data.AlphaWidth);
					EditorGUILayout.IntField("Alpha Height", Target.Data.AlphaHeight);
				EndDisabled();
			}
		}
	}
}
#endif

namespace Destructible2D
{
	/// <summary>This class stores a snapshot of a D2dSprite's current state of destruction.</summary>
	[RequireComponent(typeof(D2dDestructible))]
	[HelpURL(D2dHelper.HelpUrlPrefix + "D2dSnapshot")]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Snapshot")]
	public class D2dSnapshot : MonoBehaviour
	{
		/// <summary>This gives you the snapshot data in a normal class.</summary>
		public D2dSnapshotData Data { set { data = value; } get { if (data == null) data = new D2dSnapshotData(); return data; } } [SerializeField] private D2dSnapshotData data;

		/// <summary>This allows you to get the data value without causing it to automatically initialize.</summary>
		public D2dSnapshotData DataRaw { set { data = value; } get { return data; } }

		/// <summary>This will clear all snapshot data.</summary>
		[ContextMenu("Clear")]
		public void Clear()
		{
			if (data != null)
			{
				data.Clear();
			}
		}
	}
}