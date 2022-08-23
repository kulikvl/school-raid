using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dRequirements))]
	public class D2dRequirements_Editor : D2dEditor<D2dRequirements>
	{
		protected override void OnInspector()
		{
			DrawDefault("damage", "If you enable this then the D2dDamage.Damage value will be checked.");
			if (Any(t => t.Damage == true))
			{
				if (Any(t => t.GetComponent<D2dDamage>() == null))
				{
					EditorGUILayout.HelpBox("There is no D2dDamage on this GameObject, so you cannot require Damage.", MessageType.Warning);
				}
				BeginIndent();
					DrawDefault("damageMin", "The D2dDamage.Damage value must be at least this value.");
					DrawDefault("damageMax", "The D2dDamage.Damage value must be below this value.");
				EndIndent();
			}

			Separator();

			DrawDefault("alphaCount", "If you enable this then the D2dDestructible.AlphaCount value will be checked.");
			if (Any(t => t.AlphaCount == true))
			{
				if (Any(t => t.GetComponent<D2dDestructible>() == null))
				{
					EditorGUILayout.HelpBox("There is no D2dDestructible on this GameObject, so you cannot require AlphaCount.", MessageType.Warning);
				}
				BeginIndent();
					DrawDefault("alphaCountMin", "The D2dDestructible.AlphaCount value must be at or above this value.");
					DrawDefault("alphaCountMax", "The D2dDestructible.AlphaCount value must be below this value.");
				EndIndent();
			}

			Separator();

			DrawDefault("alphaRatio", "If you enable this then the D2dDestructible.AlphaRatio value will be checked.");
			if (Any(t => t.AlphaRatio == true))
			{
				if (Any(t => t.GetComponent<D2dDestructible>() == null))
				{
					EditorGUILayout.HelpBox("There is no D2dDestructible on this GameObject, so you cannot require AlphaRatio.", MessageType.Warning);
				}
				BeginIndent();
					DrawDefault("alphaRatioMin", "The D2dDestructible.AlphaRatio value must be at or above this value.");
					DrawDefault("alphaRatioMax", "The D2dDestructible.AlphaRatio value must be below this value.");
				EndIndent();
			}

			Separator();

			DrawDefault("onRequirementsMet");
		}
	}
}
#endif

namespace Destructible2D
{
	/// <summary>This component allows you to trigger an event when the attached D2dDestructible or D2dDamage component settings meet the specified threshold.</summary>
	[HelpURL(D2dHelper.HelpUrlPrefix + "D2dRequirements")]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Requirements")]
	public class D2dRequirements : MonoBehaviour
	{
		/// <summary>If you enable this then the D2dDamage.Damage value will be checked.</summary>
		public bool Damage { set { damage = value; } get { return damage; } } [SerializeField] private bool damage;

		/// <summary>The D2dDamage.Damage value must be at or above this value.</summary>
		public float DamageMin { set { damageMin = value; } get { return damageMin; } } [SerializeField] private float damageMin;

		/// <summary>The D2dDamage.Damage value must be below this value.</summary>
		public float DamageMax { set { damageMax = value; } get { return damageMax; } } [SerializeField] private float damageMax;

		/// <summary>If you enable this then the D2dDestructible.AlphaCount value will be checked.</summary>
		public bool AlphaCount { set { alphaCount = value; } get { return alphaCount; } } [SerializeField] private bool alphaCount;

		/// <summary>The D2dDestructible.AlphaCount value must be at or above this value.</summary>
		public float AlphaCountMin { set { alphaCountMin = value; } get { return alphaCountMin; } } [SerializeField] private float alphaCountMin;

		/// <summary>The D2dDestructible.AlphaCount value must be below this value.</summary>
		public float AlphaCountMax { set { alphaCountMax = value; } get { return alphaCountMax; } } [SerializeField] private float alphaCountMax;

		/// <summary>If you enable this then the D2dDestructible.AlphaRatio value will be checked.</summary>
		public bool AlphaRatio { set { alphaRatio = value; } get { return alphaRatio; } } [SerializeField] private bool alphaRatio;

		/// <summary>The D2dDestructible.AlphaRatio value must be at or above this value.</summary>
		public float AlphaRatioMin { set { alphaRatioMin = value; } get { return alphaRatioMin; } } [SerializeField] private float alphaRatioMin;

		/// <summary>The D2dDestructible.AlphaRatio value must be below this value.</summary>
		public float AlphaRatioMax { set { alphaRatioMax = value; } get { return alphaRatioMax; } } [SerializeField] private float alphaRatioMax;

		/// <summary>When all requirements have been met, this method will be invoked.</summary>
		public UnityEvent OnRequirementsMet { get { if (onRequirementsMet == null) onRequirementsMet = new UnityEvent(); return onRequirementsMet; } } [SerializeField] private UnityEvent onRequirementsMet;

		[SerializeField]
		private bool met;

		[System.NonSerialized]
		private D2dDestructible cachedDestructible;

		[System.NonSerialized]
		private D2dDamage cachedDamage;

		public void UpdateMet()
		{
			if (met != CheckMet())
			{
				if (met == true)
				{
					met = false;
				}
				else
				{
					met = true;

					if (onRequirementsMet != null)
					{
						onRequirementsMet.Invoke();
					}
				}
			}
		}

		protected virtual void Update()
		{
			UpdateMet();
		}

		private bool CheckMet()
		{
			if (damage == true)
			{
				if (cachedDamage == null) cachedDamage = GetComponent<D2dDamage>();

				if (cachedDamage != null)
				{
					if (cachedDamage.Damage < damageMin || cachedDamage.Damage >= damageMax)
					{
						return false;
					}
				}
			}

			if (alphaCount == true)
			{
				if (cachedDestructible == null) cachedDestructible = GetComponent<D2dDestructible>();

				if (cachedDestructible != null)
				{
					if (cachedDestructible.AlphaCount < alphaCountMin || cachedDestructible.AlphaCount >= alphaCountMax)
					{
						return false;
					}
				}
			}

			if (alphaRatio == true)
			{
				if (cachedDestructible == null) cachedDestructible = GetComponent<D2dDestructible>();

				if (cachedDestructible != null)
				{
					if (cachedDestructible.AlphaRatio < alphaRatioMin || cachedDestructible.AlphaRatio >= alphaRatioMax)
					{
						return false;
					}
				}
			}

			return true;
		}
	}
}