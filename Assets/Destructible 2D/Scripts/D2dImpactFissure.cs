using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dImpactFissure))]
	public class D2dImpactFissure_Editor : D2dEditor<D2dImpactFissure>
	{
		protected override void OnInspector()
		{
			BeginError(Any(t => t.Mask == 0));
				DrawDefault("mask", "The collision layers you want to listen to.");
			EndError();
			BeginError(Any(t => t.Threshold < 0.0f));
				DrawDefault("threshold", "The impact force required.");
			EndError();
			DrawDefault("delay", "This allows you to control the minimum amount of time between fissure creation in seconds.");

			Separator();

			DrawDefault("paint", "The paint type.");
			BeginError(Any(t => t.Shape == null));
				DrawDefault("shape", "This allows you to set the shape of the fissure.");
			EndError();
			DrawDefault("color", "The stamp shape will be multiplied by this.\nSolid White = No Change");
			DrawDefault("prefab", "If you want a prefab to spawn at the impact point, set it here.");
			DrawDefault("thickness", "This allows you to control the width of the fissure.");
			DrawDefault("depth", "This allows you to control how deep into the impact point the fissure will go.");
			DrawDefault("offset", "This allows you to move the start point of the fissure back a bit.");
			DrawDefault("useSurfaceNormal", "Use the surface normal instead of the impact velocity normal?");

			Separator();

			DrawDefault("onImpact");
		}
	}
}
#endif

namespace Destructible2D
{
	/// <summary>This component slices a shape at the collision impact point when another object hits this destructible object.</summary>
	[RequireComponent(typeof(D2dDestructible))]
	[RequireComponent(typeof(D2dCollisionHandler))]
	[HelpURL(D2dHelper.HelpUrlPrefix + "D2dImpactFissure")]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Impact Fissure")]
	public class D2dImpactFissure : MonoBehaviour
	{
		/// <summary>The collision layers you want to listen to.</summary>
		public LayerMask Mask { set { mask = value; } get { return mask; } } [SerializeField] private LayerMask mask = -1;

		/// <summary>The impact force required.</summary>
		public float Threshold { set { threshold = value; } get { return threshold; } } [SerializeField] private float threshold = 10.0f;

		/// <summary>This allows you to control the minimum amount of time between fissure creation in seconds.</summary>
		public float Delay { set { delay = value; } get { return delay; } } [SerializeField] private float delay = 0.1f;

		/// <summary>If you want a prefab to spawn at the impact point, set it here.</summary>
		public GameObject Prefab { set { prefab = value; } get { return prefab; } } [SerializeField] private GameObject prefab;

		/// <summary>The paint type.</summary>
		public D2dDestructible.PaintType Paint { set { paint = value; } get { return paint; } } [SerializeField] private D2dDestructible.PaintType paint;

		/// <summary>This allows you to set the shape of the fissure.</summary>
		public Texture2D Shape { set { shape = value; } get { return shape; } } [SerializeField] private Texture2D shape;

		/// <summary>The stamp shape will be multiplied by this. Solid White = No Change</summary>
		public Color Color { set { color = value; } get { return color; } } [SerializeField] private Color color;

		/// <summary>This allows you to control the width of the fissure.</summary>
		public float Thickness { set { thickness = value; } get { return thickness; } } [SerializeField] private float thickness = 0.25f;

		/// <summary>This allows you to control how deep into the impact point the fissure will go.</summary>
		public float Depth { set { depth = value; } get { return depth; } } [SerializeField] private float depth = 5.0f;

		/// <summary>This allows you to move the start point of the fissure back a bit.</summary>
		public float Offset { set { offset = value; } get { return offset; } } [SerializeField] private float offset = 1.0f;

		/// <summary>Use the surface normal instead of the impact velocity normal?</summary>
		public bool UseSurfaceNormal { set { useSurfaceNormal = value; } get { return useSurfaceNormal; } } [SerializeField] private bool useSurfaceNormal;

		/// <summary>This gets called when the prefab was spawned.</summary>
		public UnityEvent OnImpact { get { if (onImpact == null) onImpact = new UnityEvent(); return onImpact; } } [SerializeField] private UnityEvent onImpact;

		[System.NonSerialized]
		private D2dCollisionHandler cachedCollisionHandler;

		[System.NonSerialized]
		private D2dDestructible cachedDestructible;

		[SerializeField]
		private float cooldown;

		protected virtual void OnEnable()
		{
			if (cachedCollisionHandler == null) cachedCollisionHandler = GetComponent<D2dCollisionHandler>();
			if (cachedDestructible     == null) cachedDestructible     = GetComponent<D2dDestructible>();

			cachedCollisionHandler.OnCollision += Collision;
		}

		protected virtual void Update()
		{
			cooldown -= Time.deltaTime;
		}

		protected virtual void OnDisable()
		{
			cachedCollisionHandler.OnCollision -= Collision;
		}

		private void Collision(Collision2D collision)
		{
			if (cooldown <= 0.0f)
			{
				if (D2dHelper.IndexInMask(collision.gameObject.layer, mask) == true)
				{
					var contacts = collision.contacts;

					for (var i = contacts.Length - 1; i >= 0; i--)
					{
						var contact = contacts[i];
						var normal  = collision.relativeVelocity;
						var force   = normal.magnitude;

						if (force >= threshold)
						{
							if (useSurfaceNormal == true)
							{
								normal = contact.normal;
							}
							else
							{
								normal /= force;
							}

							var point  = contact.point;
							var pointA = point - normal * offset;
							var pointB = point + normal * depth;
							var matrix = D2dSlice.CalculateMatrix(pointA, pointB, thickness);

							cooldown = delay;

							cachedDestructible.Paint(paint, matrix, shape, color);

							if (prefab != null)
							{
								Instantiate(prefab, point, Quaternion.identity);
							}

							if (onImpact != null)
							{
								onImpact.Invoke();
							}

							if (delay > 0.0f)
							{
								break;
							}
						}
					}
				}
			}
		}
	}
}