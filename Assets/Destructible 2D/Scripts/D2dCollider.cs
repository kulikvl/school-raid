using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
namespace Destructible2D
{
	public class D2dCollider_Editor<T> : D2dEditor<T>
		where T : D2dCollider
	{
		protected override void OnInspector()
		{
			var refresh = false;

			DrawDefault("material", ref refresh, "This allows you to set the sharedMaterial setting on each generated collider.");
			DrawDefault("isTrigger", ref refresh, "This allows you to set the isTrigger setting on each generated collider.");

			if (refresh == true) DirtyEach(t => t.Refresh());
		}
	}
}
#endif

namespace Destructible2D
{
	/// <summary>This is the base class for all collider types.</summary>
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(D2dDestructible))]
	public abstract class D2dCollider : MonoBehaviour
	{
		/// <summary>This allows you to set the isTrigger setting on each generated collider.</summary>
		public bool IsTrigger { set { isTrigger = value; Refresh(); } get { return isTrigger; } } [SerializeField] protected bool isTrigger;

		/// <summary>This allows you to set the isTrigger value without causing Refresh to be called.</summary>
		public bool IsTriggerRaw { set { isTrigger = value; } get { return isTrigger; } }

		/// <summary>This allows you to set the material setting on each generated collider.</summary>
		public PhysicsMaterial2D Material { set { material = value; Refresh(); } get { return material; } } [SerializeField] protected PhysicsMaterial2D material;

		/// <summary>This allows you to set the material value without causing Refresh to be called.</summary>
		public PhysicsMaterial2D MaterialRaw { set { material = value; } get { return material; } }

		[SerializeField]
		protected GameObject child;

		[System.NonSerialized]
		protected D2dDestructible cachedDestructible;

		[System.NonSerialized]
		protected bool cachedDestructibleSet;

		[SerializeField]
		protected bool awoken;

		[System.NonSerialized]
		private GameObject tempChild;

		public D2dDestructible CachedDestructible
		{
			get
			{
				if (cachedDestructibleSet == false)
				{
					cachedDestructible    = GetComponent<D2dDestructible>();
					cachedDestructibleSet = true;
				}

				return cachedDestructible;
			}
		}

		public abstract void Refresh();

		[ContextMenu("Rebuild")]
		public void Rebuild()
		{
			if (CachedDestructible.Ready == true)
			{
				UpdateBeforeBuild();

				DoRebuild();
			}
			else
			{
				child = D2dHelper.Destroy(child);
			}
		}

		public void DestroyChild()
		{
			if (child != null)
			{
				child = D2dHelper.Destroy(child);
			}
		}

		protected abstract void DoModified(D2dRect rect);

		protected abstract void DoRebuild();

		protected virtual void OnEnable()
		{
			if (cachedDestructible == null) cachedDestructible = GetComponent<D2dDestructible>();

			cachedDestructible.OnRebuilt    += Rebuilt;
			cachedDestructible.OnModified   += Modified;
			cachedDestructible.OnSplitStart += SplitStart;
			cachedDestructible.OnSplitEnd   += SplitEnd;

			if (child != null)
			{
				child.SetActive(true);
			}
		}

		protected virtual void OnDisable()
		{
			cachedDestructible.OnRebuilt    -= Rebuilt;
			cachedDestructible.OnModified   -= Modified;
			cachedDestructible.OnSplitStart -= SplitStart;
			cachedDestructible.OnSplitEnd   -= SplitEnd;

			if (child != null)
			{
				child.SetActive(false);
			}

			// If the collider was disabled while splitting then run this special code to destroy the children
			if (cachedDestructible.IsOnStartSplit == true)
			{
				if (child != null)
				{
					child.transform.SetParent(null, false);

					child = D2dHelper.Destroy(child);
				}

				if (tempChild != null)
				{
					tempChild = D2dHelper.Destroy(tempChild);
				}
			}
		}

		protected virtual void Awake()
		{
			// Auto destroy all default collider2Ds
			if (GetComponent<Collider2D>() != null)
			{
				var collider2Ds = GetComponents<Collider2D>();

				for (var i = collider2Ds.Length - 1; i >= 0; i--)
				{
					D2dHelper.Destroy(collider2Ds[i]);
				}
			}
		}

		protected virtual void Start()
		{
			if (awoken == false)
			{
				awoken = true;

				Rebuilt();
			}
		}

		protected virtual void Update()
		{
			if (child == null)
			{
				Rebuilt();
			}
		}

		protected virtual void OnDestroy()
		{
			DestroyChild();
		}

		private void Rebuilt()
		{
			UpdateBeforeBuild();

			DoRebuild();
		}

		private void Modified(D2dRect rect)
		{
			UpdateBeforeBuild();

			DoModified(rect);
		}

		protected virtual void SplitStart()
		{
			if (child != null)
			{
				child.transform.SetParent(null, false);

				tempChild = child;
				child     = null;
			}
		}

		protected virtual void SplitEnd(List<D2dDestructible> splitDestructibles)
		{
			ReconnectChild();
		}

		private void UpdateBeforeBuild()
		{
			if (cachedDestructible == null) cachedDestructible = GetComponent<D2dDestructible>();

			if (child == null)
			{
				ReconnectChild();

				if (child == null)
				{
					child = new GameObject("Collider");

					child.layer = transform.gameObject.layer;

					child.transform.SetParent(transform, false);
				}
			}

			if (cachedDestructible.Ready == true)
			{
				var w = cachedDestructible.AlphaScale.x / cachedDestructible.AlphaWidth;
				var h = cachedDestructible.AlphaScale.y / cachedDestructible.AlphaHeight;

				var offsetX = cachedDestructible.AlphaOffset.x + w * 0.5f;
				var offsetY = cachedDestructible.AlphaOffset.y + h * 0.5f;
				var scaleX  = w / 255.0f;
				var scaleY  = h / 255.0f;

				child.transform.localPosition = new Vector3(offsetX, offsetY, 0.0f);
				child.transform.localScale    = new Vector3(scaleX, scaleY, 1.0f);
			}
		}

		private void ReconnectChild()
		{
			if (tempChild != null)
			{
				child = tempChild;

				child.transform.SetParent(transform, false);

				tempChild = null;
			}
		}
	}
}