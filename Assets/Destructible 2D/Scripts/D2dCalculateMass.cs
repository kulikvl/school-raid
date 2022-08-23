using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dCalculateMass))]
	public class D2dCalculateMass_Editor : D2dEditor<D2dCalculateMass>
	{
		protected override void OnInspector()
		{
			DrawDefault("massPerSolidPixel", "The amount of mass added to the Rigidbody2D by each solid pixel in the attached D2dDestructible.");
			DrawDefault("factorInSharpness", "Automatically multiply the mass by the D2dDestructible.AlphaSharpness value to account for optimizations?");
		}
	}
}
#endif

namespace Destructible2D
{
	/// <summary>This component automatically sets the Rigidbody2D.mass based on the D2dDestructible.AlphaCount.</summary>
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(D2dDestructible))]
	[RequireComponent(typeof(Rigidbody2D))]
	[HelpURL(D2dHelper.HelpUrlPrefix + "D2dCalculateMass")]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Calculate Mass")]
	public class D2dCalculateMass : MonoBehaviour
	{
		/// <summary>The amount of mass added to the Rigidbody2D by each solid pixel in the attached D2dDestructible.</summary>
		public float MassPerSolidPixel { set { massPerSolidPixel = value; } get { return massPerSolidPixel; } } [SerializeField] private float massPerSolidPixel = 0.01f;

		/// <summary>Automatically multiply the mass by the D2dDestructible.AlphaSharpness value to account for optimizations?</summary>
		public bool FactorInSharpness { set { factorInSharpness = value; } get { return factorInSharpness; } } [SerializeField] private bool factorInSharpness = true;

		[System.NonSerialized]
		private Rigidbody2D cachedRigidbody2D;

		[System.NonSerialized]
		private D2dDestructible cachedDestructible;

		[System.NonSerialized]
		private float lastSetMass = -1.0f;

		protected virtual void OnEnable()
		{
			if (cachedRigidbody2D  == null) cachedRigidbody2D  = GetComponent<Rigidbody2D>();
			if (cachedDestructible == null) cachedDestructible = GetComponent<D2dDestructible>();
		}

		protected virtual void Update()
		{
			var newMass = cachedDestructible.AlphaCount * MassPerSolidPixel;

			if (factorInSharpness == true)
			{
				newMass *= cachedDestructible.AlphaSharpness * cachedDestructible.AlphaSharpness;
			}

			if (newMass != lastSetMass)
			{
				cachedRigidbody2D.mass = lastSetMass = newMass;
			}
		}
	}
}