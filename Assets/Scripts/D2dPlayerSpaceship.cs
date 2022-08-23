using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;

namespace Destructible2D
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(D2dPlayerSpaceship))]
    public class D2dPlayerSpaceship_Editor : D2dEditor<D2dPlayerSpaceship>
    {
        protected override void OnInspector()
        {
            BeginError(Any(t => t.ShootDelayBlasters < 0.0f));
            DrawDefault("ShootDelayBlasters");
            DrawDefault("ShootDelayBomb");
            DrawDefault("ShootDelayToygun");
            EndError();
            DrawDefault("LeftGun");
            DrawDefault("RightGun");
            DrawDefault("Characters");
            DrawDefault("KindOfGun");
            DrawDefault("Bomb");
            DrawDefault("toyGun");
            DrawDefault("laser");

            DrawDefault("HP_slider");

            DrawDefault("KILLEFFECT");

        }
    }
}
#endif

namespace Destructible2D
{
    public class D2dPlayerSpaceship : MonoBehaviour
    {
        [Space]

        public string KindOfGun;

        [Tooltip("Minimum time between each shot in seconds for BLASTERS")]
        public float ShootDelayBlasters = 0.1f;

        [Tooltip("Minimum time between each shot in seconds for BOMB")]
        public float ShootDelayBomb = 2f;

        [Tooltip("toy")]
        public float ShootDelayToygun = 3f;

        [Tooltip("The left gun")]
        public D2dGun LeftGun;

        [Tooltip("The right gun")]
        public D2dGun RightGun;

        [Tooltip("Bomb")]
        public D2dBomb Bomb;

        public ToyGun toyGun;
        public Laser laser;

        [Tooltip("Animator Character")]
        public GameObject Characters;

        public GameObject KILLEFFECT;
        private GameObject PLAYER;

        [System.NonSerialized]
        private Rigidbody2D body;

        [SerializeField]
        private float cooldown;

        public Slider HP_slider;
        public static float HP;

        private Animator anim;
        private GameObject cameraFollow;

        private void Awake()
        {
            GameObject hp = GameObject.FindGameObjectWithTag("HP");
            if (hp != null)
                HP_slider = hp.GetComponent<Slider>();
            else
                Debug.LogError("CAN NOT FIND HP! ");

            cameraFollow = GameObject.FindGameObjectWithTag("cameraFollow");
            if (cameraFollow == null)
                Debug.LogError("CAN NOT FIND CAMERA FOLLOW! ");

            anim = Characters.GetComponent<Animator>();
            PLAYER = GameObject.FindGameObjectWithTag("PLAYER");
            HP = 1f;

        }

        public void BlastersMenuShoot()
        {
            if (reloaded == true)
            StartCoroutine(Go());
        }

        private bool go = false;
        private bool reloaded = true;

        public void takeDamage(float amount)
        {
            HP -= amount;

            if (HP <= 0)
            {
                PlayerController.Lost = true;
                DeathActions();
            }
        }

        private void DeathActions()
        {
            ///// set win animation for everyone /////
            GameObject[] gos = GameObject.FindGameObjectsWithTag("logic");

            if (gos != null)
            {
                foreach (GameObject go in gos)
                {
                    Enemy enemy = go.GetComponent<Enemy>();
                    enemy.winAnimation();
                }
            }
            /////

            Instantiate(KILLEFFECT, transform.position, Quaternion.identity);
            PLAYER.SetActive(false);
            cameraFollow.GetComponent<D2dFollow>().ifPlayerLost();
        }

        IEnumerator Go()
        {
            go = true;
            reloaded = false;
            sputnikCount.count -= 1;

            yield return new WaitForSeconds(2f);
            go = false;
            anim.SetBool("IsFiring", false);
            yield return new WaitForSeconds(2f);
            reloaded = true;
        }

        IEnumerator ToyGunGo()
        {
            yield return new WaitForSeconds(0.5f);
            toyGun.Shoot();
        }

        IEnumerator LasersGo()
        {
            yield return new WaitForSeconds(0.5f);
            laser.Shoot();
        }

        public virtual void Update()
        {
            HP_slider.value = HP;

            cooldown -= Time.deltaTime;

            /////////// BLASTERS ///////////
            if (FireButton.Pressed == true && KindOfGun == "Blasters" && go == true) //todo
            {

                BlastersMenuShoot();

                if (go == true)
                {
                    if (cooldown <= 0.0f)
                    {
                        anim.SetBool("IsFiring", true);

                        cooldown = ShootDelayBlasters;

                        if (LeftGun != null && LeftGun.CanShoot == true)
                        {
                            LeftGun.Shoot();
                        }

                        else if (RightGun != null && RightGun.CanShoot == true)
                        {
                            RightGun.Shoot();
                        }
                    }

                }
                
            }

            /////////// BOMB ///////////
            else if (FireButton.Pressed == true && KindOfGun == "Bomb" && (sputnikCount.count > 0 || PlayerPrefs.GetString("currentMode") == "Deathmatch"))
            {
                if (cooldown <= 0.0f)
                {
                    anim.SetTrigger("IsFiringBomb");

                    cooldown = ShootDelayBomb;

                    if (Bomb != null)
                    {
                        sputnikCount.count -= 1;

                        //
                       // LVL.AddLevel(0.01f);
                        //

                        Bomb.Shoot();
                    }
                }
            }

            /////////// LASERS ///////////
            else if (FireButton.Pressed == true && KindOfGun == "Lasers" && (sputnikCount.count > 0 || PlayerPrefs.GetString("currentMode") == "Deathmatch"))
            {
                if (cooldown <= 0.0f)
                {
                    anim.SetTrigger("IsFiringLasers");

                    cooldown = ShootDelayToygun;

                    if (laser != null)
                    {
                        sputnikCount.count -= 1;
                        //LVL.AddLevel(0.01f);

                        laser.Shoot();
                    }
                }
            }

            /////////// TOYGUN ///////////
            else if (FireButton.Pressed == true && KindOfGun == "Toygun" && (sputnikCount.count > 0 || PlayerPrefs.GetString("currentMode") == "Deathmatch"))
            {
                if (cooldown <= 0.0f)
                {
                    anim.SetTrigger("IsFiringToygun");

                    cooldown = ShootDelayToygun;

                    if (toyGun != null)
                    {
                        sputnikCount.count -= 1;
                       // LVL.AddLevel(0.01f);

                        StartCoroutine(ToyGunGo());
                    }
                }
            }

            else
            {
                switch (KindOfGun)
                {
                    case "Blasters":
                        anim.SetBool("IsFiring", false);
                        break;

                    case "Bomb":
                        anim.SetBool("IsFiringBomb", false);
                        break;

                    case "Toygun":
                        anim.SetBool("IsFiringToygun", false);
                        break;

                    case "Lasers":
                        anim.SetBool("IsFiringLasers", false);
                        break;
                }
            }
        }
    }
}