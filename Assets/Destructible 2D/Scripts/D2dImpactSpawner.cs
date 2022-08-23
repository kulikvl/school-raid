using UnityEngine;
using UnityEngine.Events;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dImpactSpawner))]
	public class D2dImpactSpawner_Editor : D2dEditor<D2dImpactSpawner>
	{
		protected override void OnInspector()
		{
			BeginError(Any(t => t.Mask == 0));
				DrawDefault("mask", "The collision layers you want to listen to.");
			EndError();
			DrawDefault("threshold", "The impact force required.");
			DrawDefault("delay", "This allows you to control the minimum amount of time between fissure creation in seconds.");

			Separator();
			
			BeginError(Any(t => t.Prefab == null));
				DrawDefault("prefab", "If you want a prefab to spawn at the impact point, set it here.");
			EndError();
			DrawDefault("offset", "This allows you to move the start point of the fissure back a bit.");
            DrawDefault("explosionRadius", "Radius of a Bomb explosion");

			DrawDefault("explosionForceY", "forceOfExplosion");
			DrawDefault("fireParticle", "fire");
			DrawDefault("toDelay", "delay?");

			DrawDefault("rotateTo", "How should the spawned prefab be rotated?");
			Separator();
			DrawDefault("onImpact");
		}
	}
}
#endif

namespace Destructible2D
{
	/// <summary>This component spawns a prefab at the impact point when an object hits the current object.</summary>
	[RequireComponent(typeof(D2dCollisionHandler))]
	[HelpURL(D2dHelper.HelpUrlPrefix + "D2dImpactSpawner")]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Impact Spawner")]
	public class D2dImpactSpawner : MonoBehaviour
	{
		public enum RotationType
		{
			RandomDirection,
			ImpactDirection,
			SurfaceNormal
		}

		public LayerMask Mask { set { mask = value; } get { return mask; } } [SerializeField] private LayerMask mask = -1;

		public float Threshold { set { threshold = value; } get { return threshold; } } [SerializeField] private float threshold = 1.0f;

		public float Delay { set { delay = value; } get { return delay; } } [SerializeField] private float delay = 1.0f;

		public GameObject Prefab { set { prefab = value; } get { return prefab; } } [SerializeField] private GameObject prefab;

		public float Offset { set { offset = value; } get { return offset; } } [SerializeField] private float offset = 0.1f;

		public float ExplosionForceY { set { explosionForceY = value; } get { return explosionForceY; } }
		[SerializeField] private float explosionForceY = 8000f;

		public float ExplosionRadius { set { explosionRadius = value; } get { return explosionRadius; } } [SerializeField] private float explosionRadius = 0.1f;

		public GameObject FireOnExplosion { set { fireParticle = value; } get { return fireParticle; } }
		[SerializeField] private GameObject fireParticle;

		public bool ToDelay { set { toDelay = value; } get { return toDelay; } }
		[SerializeField] private bool toDelay = false;

		public bool ToThrowTnts = false;

		public RotationType RotateTo { set { rotateTo = value; } get { return rotateTo; } } [SerializeField] private RotationType rotateTo;

		public UnityEvent OnImpact { get { if (onImpact == null) onImpact = new UnityEvent(); return onImpact; } } [SerializeField] private UnityEvent onImpact;

		[System.NonSerialized]
		private D2dCollisionHandler cachedCollisionHandler;

		[SerializeField]
		private float cooldown;

		protected virtual void OnEnable()
		{
			if (cachedCollisionHandler == null) cachedCollisionHandler = GetComponent<D2dCollisionHandler>();

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

        public void Explode(Vector3 place)
        {
			if (gameObject.name.Contains("BombOrdinary"))
            {
				AudioManager.instance.Play("Explosion11");
				iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactMedium);
			}
			else if (gameObject.name.Contains("BigDynamite"))
			{
				AudioManager.instance.Play("Explosion11");
				iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactMedium);
			}
			else if (gameObject.name.Contains("BombFire"))
            {
				AudioManager.instance.Play("FireExplosion");
				iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactMedium);
			}
            else if (gameObject.name.Contains("ToyGun"))
            {
				AudioManager.instance.Play("Explosion4");
				iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactMedium);
			}
			else if (gameObject.name.Contains("DYNAMITE"))
            {
				AudioManager.instance.Play("Explosion3");
				iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactMedium);
			}
			else if (gameObject.name.Contains("DYNLITTLE"))
            {
				iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);
				AudioManager.instance.Play("Explosion" + Random.Range(6, 8).ToString());
			}
			else if (gameObject.name.Contains("GRENADE"))
            {
				AudioManager.instance.Play("Explosion5");
				iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);
			}
			else if (gameObject.name.Contains("GRENWILD"))
            {
				AudioManager.instance.Play("Explosion2");
				iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);
			}

			Collider2D[] collidersBlood = Physics2D.OverlapCircleAll(place, ExplosionRadius + 0.3f);
			foreach (Collider2D nb in collidersBlood)
			{
				// Blood
				BloodPart bloodPart = nb.gameObject.GetComponent<BloodPart>();
				if (bloodPart != null)
				{
					nb.gameObject.GetComponent<BloodPart>().Release();
				}
			}

			BusController.currentMultiKillCount = 0;
			Collider2D[] colliders = Physics2D.OverlapCircleAll(place, ExplosionRadius);

            foreach (Collider2D nb in colliders)
            {
				IDieable dieable = nb.gameObject.GetComponent<IDieable>();

                if (dieable != null && !dieable.IsDead && !nb.isTrigger)
                {
					BusController.currentMultiKillCount++;
					dieable.Die(false, false, false);
                }

				// Impact
				if (nb.name.Contains("Body")) 
				{
                    if (nb.transform.parent.parent.GetChild(0).gameObject.GetComponent<Enemy>().IsDead)
                    {
						Rigidbody2D RB = nb.GetComponent<Rigidbody2D>();

						if (Random.Range(0, 2) == 0)
						{
							RB.AddForce(new Vector2(Random.Range(2500f, 3500f), ExplosionForceY));
						}
						else
						{
							RB.AddForce(new Vector2(Random.Range(-3500f, -2500f), ExplosionForceY));
						}

						Debug.Log("IMPACT FROM SPAWNER!");
					}
                    else
                    {
						BusController.currentMultiKillCount++;
						nb.transform.parent.parent.GetChild(0).gameObject.GetComponent<Enemy>().Die(true, false, false);
					}
				}

				// STAMP AUTOMATICALLY
			}

			if (BusController.currentMultiKillCount >= 2)
			{
				LocalAchievementsManager.instance.ActivateLocalAchievementMULTIKILL(BusController.currentMultiKillCount, true);

				if (BusController.currentMultiKillCount > BusController.MaxMultiKillCount) BusController.MaxMultiKillCount = BusController.currentMultiKillCount;
			}
			else
				BusController.currentMultiKillCount = 0;

			// ABILITY

			if (PlayerPrefs.GetInt("0currentAlteration") == 1 && PlayerPrefs.GetInt("currentModel") == 0)
			{
				Collider2D[] colliders2 = Physics2D.OverlapCircleAll(place, 0.2f);
				foreach (Collider2D nb in colliders2)
				{
					if (nb.name.Contains("line"))
					{ 
					    Instantiate(FireOnExplosion, place, FireOnExplosion.transform.rotation);
						break;
					}
				}
			}

			if (PlayerPrefs.GetInt("3currentAlteration") == 1 && PlayerPrefs.GetInt("currentModel") == 3 && ToThrowTnts)
			{
				GameObject bus = GameObject.FindGameObjectWithTag("BUS");
				Collider2D[] colliders2 = Physics2D.OverlapCircleAll(place, 0.2f);

				foreach (Collider2D nb in colliders2)
				{
					if (nb.name.Contains("line"))
					{
						if (bus != null)
						{
							BusController busController = bus.GetComponent<BusController>();
							busController.dynamiteManager.Throw3LittleTnts(place);
						}
						break;
					}
				}
			}

			if (gameObject.name.Contains("BigDynamite"))
			{
				Collider2D[] colliders2 = Physics2D.OverlapCircleAll(place, 0.2f);

				foreach (Collider2D nb in colliders2)
				{
					if (nb.name.Contains("line"))
					{
						gameObject.GetComponent<BigDynamite>().Throw3LittleTnts(place);
						break;
					}
				}
			}

			if (PlayerPrefs.GetInt("4currentAlteration") == 2 && PlayerPrefs.GetInt("currentModel") == 4 && gameObject.name != "MORTIRA GRENADE" && !gameObject.name.Contains("GRENADE"))
			{
				Collider2D[] colliders2 = Physics2D.OverlapCircleAll(place, 0.2f);
				foreach (Collider2D nb in colliders2)
				{
					if (nb.name.Contains("line"))
					{
						Instantiate(FireOnExplosion, place, FireOnExplosion.transform.rotation);
						break;
					}
				}
			}
		}

		private bool startedDelayAnimation = false;

		private void DelayExplosion(Vector2 point)
		{
			Collider2D[] colliders = Physics2D.OverlapCircleAll(point, ExplosionRadius);

			foreach (Collider2D nb in colliders)
			{
				IDieable dieable = nb.gameObject.GetComponent<IDieable>();

				if (dieable != null && !dieable.IsDead)
				{
					dieable.Die(true, true, false);
					Instantiate(prefab, point, Quaternion.identity);
					if (onImpact != null)
					{
						onImpact.Invoke();
					}

					Debug.Log("ENEMY collision");
					return;
				}

                else if (nb.gameObject.layer == 15)
                {
					Instantiate(prefab, point, Quaternion.identity);
					if (onImpact != null)
					{
						onImpact.Invoke();
					}
					Debug.Log("something else collision");
					return;
				}
			}
		
			StartCoroutine(IDelayExplosion(point));
		}

        private void FixedUpdate()
        {
            if (startedDelayAnimation)
            {
				transform.Translate(new Vector3(0f, 0.15f, 0f) * Time.fixedDeltaTime);
			}
        }

        IEnumerator IDelayExplosion(Vector2 point)
        {
			Debug.Log("todelay");

			ToDelay = false;
			startedDelayAnimation = true;

			if (gameObject.name.Contains("BlueBird"))
            {
				GetComponent<Animation>().Stop();
				GetComponent<Animation>().Play("BlueBirdIsGrowing");
			}
            else
            {
				GetComponent<Animation>().Stop();
				GetComponent<Animation>().Play("ChickenIsGrowing");
			}

			gameObject.GetComponent<BoxCollider2D>().enabled = false;

			GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

			yield return new WaitForSeconds(2f);

			ExplosionRadius += 2f;

			Explode(transform.position);
			Instantiate(prefab, point, Quaternion.identity);
			if (onImpact != null)
			{
				onImpact.Invoke();
			}
		}

        private void Collision(Collision2D collision)
		{
			if (cooldown <= 0.0f && prefab != null)
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
							switch (rotateTo)
							{
								case RotationType.RandomDirection:
								{
									var angle = Random.Range(-Mathf.PI, Mathf.PI);

									normal.x = Mathf.Sin(angle);
									normal.y = Mathf.Cos(angle);
								}
								break;

								case RotationType.ImpactDirection:
								{
									normal /= force;
								}
								break;

								case RotationType.SurfaceNormal:
								{
									normal = contact.normal;
								}
								break;
							}

							var point = contact.point - contact.normal * offset;

                            if (!startedDelayAnimation)
                            {
								if (!ToDelay)
								{
									Explode(point);
									Instantiate(prefab, point, Quaternion.identity);
									if (onImpact != null)
									{
										onImpact.Invoke();
									}
								}
								else
								{
									DelayExplosion(point);
								}
							}
                            
                            cooldown = delay;

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