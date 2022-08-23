using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dDamage))]
	public class D2dDamage_Editor : D2dEditor<D2dDamage>
	{
		protected override void OnInspector()
		{
			var damageChanged = false;

			DrawDefault("damage", ref damageChanged, "This tells you how much numerical damage this sprite has taken. This is automatically increased by nearby explosions and such. NOTE: This is separate to the visual damage.");
			DrawDefault("multiplier", "This allows you to reduce or increase the rate at which damage changes.");

			Separator();

			DrawDefault("states", "This allows you to modify the damage value directly without invoking NotifyDamageChanged/OnDamageChanged.");

			if (damageChanged == true)
			{
				DirtyEach(t => t.InvokeDamageChanged());
			}
		}
	}
}
#endif

namespace Destructible2D
{
	/// <summary>This component stores numerical damage for the current GameObject. This damage can then be used to swap the sprite to show different damage states.</summary>
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(SpriteRenderer))]
	[HelpURL(D2dHelper.HelpUrlPrefix + "D2dDamage")]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Damage")]
	public class D2dDamage : MonoBehaviour
	{
		[System.Serializable]
		public class State
		{
			public float  DamageMin;
			public Sprite Sprite;
			public float  DamageMax;
		}

		/// <summary>This is invoked when the damage field is modified.</summary>
		public event System.Action OnDamageChanged;

		/// <summary>This tells you how much numerical damage this sprite has taken. This is automatically increased by nearby explosions and such.</summary>
		public float Damage { set { if (damage != value) { damage += (value - damage) * multiplier; InvokeDamageChanged(); } } get { return damage; } } [SerializeField] private float damage;

		/// <summary>This allows you to modify the damage value directly without invoking NotifyDamageChanged/OnDamageChanged.</summary>
		public float DamageRaw { set { damage = value; } get { return damage; } }

		/// <summary>This allows you to reduce or increase the rate at which damage changes.</summary>
		public float Multiplier { set { multiplier = value; } get { return multiplier; } } [SerializeField] private float multiplier = 1.0f;

		/// <summary>This allows you to modify the damage value directly without invoking NotifyDamageChanged/OnDamageChanged.</summary>
		public List<State> States { set { states = value; } get { if (states == null) states = new List<State>(); return states; } } [SerializeField] private List<State> states;

		[System.NonSerialized]
		private SpriteRenderer cachedSpriteRenderer;

		/// <summary>Call this if you manually modified the damage value.</summary>
		public void InvokeDamageChanged()
		{
			if (OnDamageChanged != null)
			{
				OnDamageChanged();
			}
		}

		protected virtual void OnEnable()
		{
			if (cachedSpriteRenderer == null) cachedSpriteRenderer = GetComponent<SpriteRenderer>();
		}

		protected virtual void Update()
		{
			var bestSprite = default(Sprite);

			if (states != null)
			{
				for (var i = states.Count - 1; i >= 0; i--)
				{
					var state = states[i];

					if (state != null && Damage >= state.DamageMin && Damage < state.DamageMax)
					{
						bestSprite = state.Sprite;
					}
				}
			}

			if (bestSprite != null)
			{
				cachedSpriteRenderer.sprite = bestSprite;
			}
		}
	}
}