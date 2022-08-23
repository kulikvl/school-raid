using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour, IDieable
{
    public bool isMenuScene = false;
    public int rewardForKilling;
    public float damage;
    public bool ableToRun;
   [HideInInspector] public bool ableToDamage;

    public Transform CenterOfEnemy { get { return mainGameObject.GetComponentInChildren<CheckCollision>().transform; } }
    protected GameObject mainGameObject { get { return transform.parent.gameObject; } }
    
    [Range(0f, 3f)]
    [Tooltip("СКОРОСТРЕЛЬНОСТЬ")]
    public float cooldownTime;

    private GameObject shake;
    public float initialSpeed { get; set; }
    protected float cooldown;
    
    public GameObject targetToDamage;
    public virtual GameObject TargetToDamage { get { return targetToDamage; } set { if (value == null) { stopDamaging(); } targetToDamage = value;  } }

    [SerializeField] public float speed;
    public float Speed { get { return speed; } set { speed = value; } }

    #region Freezing
    public bool IsFreezed { get; set; }
    public virtual void DecreaseSpeed(float value)
    {
        if ((!IsDamaging && Speed == initialSpeed) || (isDamaging && Speed == 0))
            StartCoroutine(IDecreaseSpeed(value));
    }
    IEnumerator IDecreaseSpeed(float value)
    {
        Debug.Log("decreased: " + gameObject.transform.parent.name);

        SpriteRenderer[] sprites = transform.parent.gameObject.GetComponentsInChildren<SpriteRenderer>();
        List<Color> InitialColors = new List<Color>();

        for (int i = 0; i < sprites.Length; ++i)
        {
            InitialColors.Add(sprites[i].color);
            sprites[i].color = new Color(0f, 1f, 1f);
        }

        IsFreezed = true;
        bool WasDamaging = isDamaging;

        if (isDamaging)
        {
            ableToDamage = false;
            IsDamaging = false;

            ableToRun = false;
            mainGameObject.GetComponent<Animator>().speed = 0f;

            Debug.Log("Stop damaging");
            mainGameObject.GetComponent<Animator>().SetBool("damage", false);
        }
        else
        {
            Speed -= value;
        }

        yield return new WaitForSeconds(5f);

        IsFreezed = false;

        if (WasDamaging)
        {
            ableToDamage = true;
            IsDamaging = true;

            ableToRun = true;
            mainGameObject.GetComponent<Animator>().speed = 1f;

            mainGameObject.GetComponent<Animator>().SetBool("damage", true);
        }
        else
        {
            Speed = initialSpeed;
        }

        for (int i = 0; i < sprites.Length; ++i)
        {
            sprites[i].color = InitialColors[i];
        }
        //foreach (SpriteRenderer sp in sprites) sp.color = new Color(1f, 1f, 1f);
    }
    #endregion

    private bool isDead;
    public bool IsDead
    {
        get
        {
            return isDead;
        }
        set
        {
            if (value == true)
            {
                if (!isMenuScene)
                {
                    if (GetComponent<DialogSystem>() != null)
                        GetComponent<DialogSystem>().DestroyDialogWindow();

                    PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + rewardForKilling);
                    FindObjectOfType<Coins>().PlayAnimation();

                    if (PlayerPrefs.GetInt("currentModel") == 2 && Abilities2.HealthForKilling)
                    {
                        GameObject bus = GameObject.FindGameObjectWithTag("BUS");
                        bus.GetComponent<BusController>().Health += 50f;
                    }
                    if (PlayerPrefs.GetInt("currentModel") == 2 && Abilities2.AdditionalRewardOn)
                    {
                        sputnikCount.count += 2;
                    }

                    PlayerPrefs.SetInt("enemiesKilled", PlayerPrefs.GetInt("enemiesKilled") + 1);
                    PlayerPrefs.SetInt("MissionFirstValue", PlayerPrefs.GetInt("MissionFirstValue") + 1);

                    PlayGamesManager.PostToLeaderboard(PlayerPrefs.GetInt("enemiesKilled"), PlayGamesManager.LeaderboardID.TOTALENEMIESKILLED);
                    GameCenterManager.PostToLeaderboard(PlayerPrefs.GetInt("enemiesKilled"), GameCenterManager.LeaderboardID.TOTALENEMIESKILLED);

                    if (PlayerPrefs.GetInt("enemiesKilled") == 1000)
                    {
                        GameCenterManager.UnlockAchievement(GameCenterManager.AchievementID.Killer);
                        PlayGamesManager.UnlockAchievement(PlayGamesManager.AchievementID.Killer);
                    }

                    if (PlayerPrefs.GetString("currentMode") == "Deathmatch") FindObjectOfType<DeathmatchLevelController>().EnemiesAlive--;

                    RankManager.instance.AddXP(5);
                }
                

                string nameOfSound = "BloodSplash" + Random.Range(1, 7).ToString();
                AudioManager.instance.Play(nameOfSound);

                SpriteRenderer[] sprites = transform.parent.gameObject.GetComponentsInChildren<SpriteRenderer>();
                if (IsFreezed) foreach (SpriteRenderer sp in sprites) sp.color = new Color(1f, 1f, 1f);
            }
               
            isDead = value;
        }
    }

    public bool isDamaging;
    public bool IsDamaging
    {
        get
        {
            return isDamaging;
        }
        set
        {
            isDamaging = value;
        }
    }

    public virtual void OnTriggerStay2D(Collider2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        if (damageable != null && damageable.CanBeDamaged && ableToDamage)
        {
            IsDamaging = true;

            speed = 0f;
            mainGameObject.GetComponent<Animator>().SetBool("damage", true);
            TargetToDamage = collision.gameObject;
        }
    }

    public virtual void Awake()
    {
        if (!isMenuScene && PlayerPrefs.GetString("currentMode") == "Deathmatch" && FindObjectOfType<DeathmatchLevelController>().EnemiesAlive >= 8)
        {
            FindObjectOfType<DeathmatchLevelController>().EnemiesAlive--;
            Destroy(mainGameObject);
        }

        IsDead = false;

        ableToDamage = true;

        speed = Random.Range(speed - 0.05f, speed + 0.05f);
        initialSpeed = speed;

        IsDamaging = false;
        cooldown = cooldownTime;

        ///// FIND SHAKE GAMEOBJECT /////
        Transform[] trs2 = gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform _shake in trs2)
            if (_shake.gameObject.name == "shake")
                shake = _shake.gameObject;

        if (shake == null)
            Debug.LogError("SHAKE OBJECT WAS NOT FOUND!");

        //if (Random.Range(0, 3) == 0)
        //StartCoroutine(SpawnDialogWindow());
    }

    public virtual void Update()
    {
        if (ableToRun) Run(speed);

        cooldown -= Time.deltaTime;

        if (cooldown <= 0.0f && IsDamaging && !IsDead && !IsFreezed && TargetToDamage != null && !PlayerController.Lost && !PlayerController.Won)
        {
            cooldown = cooldownTime;

            IDamageable damageable = TargetToDamage.GetComponent<IDamageable>();
            damageable.takeDamage(damage);

            shakeEffect();
        }
        else if (TargetToDamage == null && IsDamaging)
        {
            stopDamaging();
        }
    }

    protected virtual void Run(float _speed)
    {
        mainGameObject.transform.Translate(Vector2.right * Time.deltaTime * _speed);
    }

    protected void shakeEffect()
    {
        var prefab = Instantiate(shake);
        Destroy(prefab);
    }

    public virtual void winAnimation()
    {
        ableToDamage = false;
        speed = 0f;
        IsDamaging = false;
        mainGameObject.GetComponent<Animator>().SetTrigger("win");  
    }
     
    public virtual void stopDamaging()
    {
        IsDamaging = false;
        speed = initialSpeed;
        mainGameObject.GetComponent<Animator>().SetBool("damage", false);
    }

    public virtual void Die(bool Impact, bool Blood, bool Stamp)
    {
        if (!IsDead)
        {
            mainGameObject.GetComponent<PartsOfEnemy>().Die();

            if (Impact) mainGameObject.GetComponent<PartsOfEnemy>().ImpactAdd();
            if (Blood) mainGameObject.GetComponent<PartsOfEnemy>().RandomBloodCome();
            if (Stamp) mainGameObject.GetComponent<PartsOfEnemy>().MakeStamp(); 
        }    
    }



    /////////// DAMAGE INCREASE STUFF ///////////

    private bool isDamageIncreased;
    public bool IsDamageIncreased
    {
        get { return isDamageIncreased; }
        set
        {
            if (value == true) StartCoroutine(IncreaseDamage());

            isDamageIncreased = value;
        }
    }
    IEnumerator IncreaseDamage()
    {
        float initialDamage = damage;

        damage *= 2f;
        yield return new WaitForSeconds(6f);

        damage = initialDamage;
        IsDamageIncreased = false;
    }
}
