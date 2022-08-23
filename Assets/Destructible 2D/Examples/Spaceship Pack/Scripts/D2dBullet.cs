using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dBullet))]
	public class D2dBullet_Editor : D2dEditor<D2dBullet>
	{
		protected override void OnInspector()
		{
			DrawDefault("IgnoreTag");
			DrawDefault("RaycastMask");
			DrawDefault("ExplosionPrefab");
			DrawDefault("Speed");
			DrawDefault("MaxLength");
			DrawDefault("MaxScale");
            DrawDefault("ExplosionForce");
            DrawDefault("ExplosionRadius");
        }
	}
}
#endif

namespace Destructible2D
{
	[ExecuteInEditMode]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Bullet")]
	public class D2dBullet : MonoBehaviour
	{
		[Tooltip("The tag this bullet cannot hit")]
		public string IgnoreTag;
		
		[Tooltip("The layers this bullet can hit")]
		public LayerMask RaycastMask = -1;
		
		[Tooltip("The prefab that gets spawned when this bullet hits something")]
		public GameObject ExplosionPrefab;
		
		[Tooltip("The distance this bullet moves each second")]
		public float Speed;
		
		[Tooltip("The maximum length of the bullet trail")]
		public float MaxLength;
		
		[Tooltip("The scale of the bullet after it's scaled up")]
		public Vector3 MaxScale;
		
		private Vector3 oldPosition;

        [Tooltip("Force of the Explosion")]
        public float ExplosionForce;

        [Tooltip("Radius of the Explosion")]
        public float ExplosionRadius;
		
		protected virtual void Start()
		{
			oldPosition = transform.position;
		}
		
		protected virtual void FixedUpdate()
		{
			var newPosition  = transform.position;
			var rayLength    = (newPosition - oldPosition).magnitude;
			var rayDirection = (newPosition - oldPosition).normalized;
			var hit          = Physics2D.Raycast(oldPosition, rayDirection, rayLength, RaycastMask);
			
			// Update old position to trail behind 
			if (rayLength > MaxLength)
			{
				rayLength   = MaxLength;
				oldPosition = newPosition - rayDirection * rayLength;
			}
			
			transform.localScale = MaxScale * D2dHelper.Divide(rayLength, MaxLength);
			
			if (hit.collider != null)
			{
				if (string.IsNullOrEmpty(IgnoreTag) == true || hit.collider.tag != IgnoreTag)
				{
					if (ExplosionPrefab != null)
					{

                        Explode(hit.point);


                        Instantiate(ExplosionPrefab, hit.point, Quaternion.identity);
                        
                    }
					
					Destroy(gameObject);
				}
			}
		}


        public void Explode(Vector3 place)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(place, ExplosionRadius);

            foreach (Collider2D nb in colliders)
            {
                Rigidbody2D rbs = nb.GetComponent<Rigidbody2D>();

                //if (nb.name.Contains("Enemy"))
                //{
                //    if (nb.name.Contains("Big"))
                //    {
                //        nb.gameObject.GetComponent<Enemy0Die>().hips.GetComponent<ShootEnemy1>().enabled = false;
                //    }

                //    nb.gameObject.GetComponent<Enemy0Die>().Die();

                //}

                if (nb.tag == "BUS")
                {
                    nb.gameObject.GetComponent<BusController>().takeDamage(5f);
                }


                //if (nb.name.Contains("TANK"))
                //{
                //    if (nb.name.Contains("DESTROY"))
                //    {
                //        nb.gameObject.GetComponent<checkTank>().Die(1f);
                //    }

                //    nb.gameObject.GetComponent<checkTank>().Die(Random.Range(0.2f, 0.25f));
                //}

                //if (nb.name == "checkBoy")
                //{
                //    nb.gameObject.GetComponent<checkSchoolBoy>().Die();
                //}

                //if (nb.name == "checkBoyFat")
                //{
                //    nb.gameObject.GetComponent<checkSchoolFatBoy>().Die();
                //}

                //if (nb.gameObject.tag == "bird")
                //{
                //    nb.gameObject.GetComponent<Bird>().Die();
                //}

                //if (nb.tag == "chicken")
                //{
                //    nb.gameObject.GetComponent<chicken>().Die();
                //}

                //if (nb.name.Contains("dead"))
                //{
                //    //if (Random.Range(0, 2) == 0)
                //    nb.gameObject.GetComponent<BloodPart>().Release();
                //}

                //if (rbs != null && nb.name.Contains("TANK") == false) //  && nb.name.Contains("BUS") == false TODO!
                //{
                //    rbs.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);

                //    if (nb.name.Contains("Enemy") || nb.name.Contains("Body") || nb.name.Contains("body"))
                //    {
                //        rbs.gravityScale = 1;
                //    }
                //    rbs.AddForce(new Vector2(Random.Range(-100f, 100f), 100f));
                //}
            }
        }


        protected virtual void Update()
		{
			transform.Translate(0.0f, Speed * Time.deltaTime, 0.0f);
		}
		
#if UNITY_EDITOR
		protected virtual void OnDrawGizmos()
		{
			Gizmos.DrawLine(transform.position, transform.TransformPoint(0.0f, -MaxLength, 0.0f));
		}
#endif
	}
}
