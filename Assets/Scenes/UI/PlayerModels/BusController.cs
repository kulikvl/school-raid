using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BusController : MonoBehaviour
{
    public enum Weapons
    {
        Bomb = 0,
        Lasers = 1,
        ToyGun = 2,
        Dynamite = 3,
        Blasters = 4
    }

    public Weapons CurrentWeapon;

    public Bomb bombManager;
    public Laser lasersManager;
    public ToyGun toyGunManager;
    public Dynamite dynamiteManager;
    public Blasters blastersManager;

    public GameObject Characters;
    public GameObject particleOnDeath;

    private GameObject player;
    private Image sliderHP;

    private float cooldown;
    private Animator animatorOfCharacters;
    private GameObject cameraFollow;
    private TextMeshProUGUI BusHP;

    [Header("Health 500 -> 1300 HP")]
    public float Health;
    public float MinHealth = 500f;

    public float MaxHealth { get; set; }

    [Header("Speed 4 -> 8 MC")]
    public float Speed;
    public float MinSpeed = 4f;

    [Header("RELOADTIME 1.5 -> 3.0")]
    public float ShootDelay;
    public float MinShootDelay = 1.5f;

    [Header("BLASTRADIUS 0.6 -> 1.5 R")]
    public float BlastRadius;
    public float MinBlastRadius = 0.7f;

    public bool CanBeDamaged { get; set; }
    public static bool AbleToShoot;

    public static int TimesShooted;
    public static int currentMultiKillCount;
    public static int MaxMultiKillCount;

    private void Awake()
    {
        agreedToWatchAd = false;
        CanBeDamaged = true;
        AbleToShoot = true;
        TimesShooted = 0;
        currentMultiKillCount = 0;
        MaxMultiKillCount = 0;

        GameObject busHP = GameObject.FindGameObjectWithTag("BusHP");
        BusHP = busHP.GetComponent<TextMeshProUGUI>();

        GameObject hp = GameObject.FindGameObjectWithTag("HP");
        if (hp != null)
            sliderHP = hp.GetComponent<Image>();
        else
            Debug.LogError("CAN NOT FIND HP! ");

        cameraFollow = GameObject.FindGameObjectWithTag("cameraFollow");
        if (cameraFollow == null)
            Debug.LogError("CAN NOT FIND CAMERA FOLLOW! ");

        animatorOfCharacters = Characters.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("PLAYER");

        // 500 -> 1300 HP
        MaxHealth = MinHealth + PlayerPrefs.GetInt(PlayerPrefs.GetInt("currentModel") + "HEALTH") * 20f;
        Health = MaxHealth;

        // 4 -> 8 MC
        Speed = MinSpeed + PlayerPrefs.GetInt(PlayerPrefs.GetInt("currentModel") + "SPEED") * 0.06f;

        // RELOAD TIME 1.5 -> 2.5   2.5 -> 1.5
        ShootDelay = 4f - (MinShootDelay + PlayerPrefs.GetInt(PlayerPrefs.GetInt("currentModel") + "RELOADTIME") * 0.028f);

        // 0.6 -> 1.5 R
        BlastRadius = 0.6f + PlayerPrefs.GetInt(PlayerPrefs.GetInt("currentModel") + "BLASTRADIUS") * 0.02f;

    }

    public static bool agreedToWatchAd;

    public void takeDamage(float amount)
    {
        if (CanBeDamaged)
        Health -= amount;

        sliderHP.fillAmount = Health / MaxHealth;

        if (Health <= 0)
        {
            if (Application.internetReachability != NetworkReachability.NotReachable && !PlayerController.HealedByWatchingAd && PlayerPrefs.GetString("currentMode") == "Campaign")
            {
                agreedToWatchAd = true;
                GameObject SecondChanceCanvas = GameObject.FindGameObjectWithTag("Secondchance");
                SecondChanceCanvas.transform.GetChild(0).gameObject.SetActive(true);
                
            }
            else
            {
                agreedToWatchAd = false;
                PlayerController.Lost = true;
                DeathActions();
            }
        }
    }

    public void Revive()
    {
        Health = MinHealth;

        FindObjectOfType<SecondChanceButton>().ManageCols(true);
        AudioManager.instance.UnPauseAllSounds();
        GameObject SecondChanceCanvas = GameObject.FindGameObjectWithTag("Secondchance");
        SecondChanceCanvas.transform.GetChild(0).gameObject.SetActive(false);

        agreedToWatchAd = false;
        SecondChanceButton.agreedToWatchAd = false;
        PlayerController.HealedByWatchingAd = true;
    }

    public void DeathActions()
    {
        AudioManager.instance.Play("Explosion1");

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

        Instantiate(particleOnDeath, transform.position, Quaternion.identity);
        player.SetActive(false);

        if (PlayerPrefs.GetString("currentMode") == "Campaign")
            cameraFollow.GetComponent<Destructible2D.D2dFollow>().ifPlayerLost();
        else
            cameraFollow.GetComponent<Destructible2D.D2dFollow>().ifPlayerLostDeathMatch();
    }

    private void Update()
    {
        BusHP.text = Health.ToString();

        sliderHP.fillAmount = Health / MaxHealth;

        cooldown -= Time.deltaTime;

        bool checkForShooting = (((sputnikCount.count > 0 && PlayerPrefs.GetInt("currentLevel") >= 15) || (PlayerPrefs.GetInt("currentLevel") < 15))
            || (PlayerPrefs.GetString("currentMode") == "Deathmatch"));

        if (AbleToShoot && cooldown <= 0.0f && FireButton.Pressed && checkForShooting)
        {
            if (CurrentWeapon == Weapons.Bomb)
            {
                animatorOfCharacters.SetTrigger("IsFiringBomb");
                
                if (PlayerPrefs.GetInt("0currentAlteration") == 0)
                    bombManager.Shoot();
                else if (PlayerPrefs.GetInt("0currentAlteration") == 1)
                    bombManager.ShootFire();
                else if (PlayerPrefs.GetInt("0currentAlteration") == 2)
                    bombManager.ShootSplit();

                AudioManager.instance.Play("Whistle1");

                CompleteActionsAfterFiring();
            }

            else if (CurrentWeapon == Weapons.ToyGun)
            {
                animatorOfCharacters.SetTrigger("IsFiringToygun");

                if (PlayerPrefs.GetInt("1currentAlteration") == 0)
                    toyGunManager.Shoot();
                else if (PlayerPrefs.GetInt("1currentAlteration") == 1)
                    toyGunManager.ShootAiming();
                else if (PlayerPrefs.GetInt("1currentAlteration") == 2)
                    toyGunManager.ShootGiant();

                string nameSound = "ToyGun" + Random.Range(1, 3).ToString();
                AudioManager.instance.Play(nameSound);
                AudioManager.instance.Play("ToyGun3");

                CompleteActionsAfterFiring();

            }

            else if (CurrentWeapon == Weapons.Lasers)
            {
                if (PlayerPrefs.GetInt("2currentAlteration") == 0)
                {
                    animatorOfCharacters.SetTrigger("IsFiringLasers");
                    lasersManager.Shoot();

                    CompleteActionsAfterFiring();
                }
                    
                else if (PlayerPrefs.GetInt("2currentAlteration") == 1)
                {
                    animatorOfCharacters.SetTrigger("IsFiringLasersLong");
                    lasersManager.ShootLonger();

                    cooldown = ShootDelay + 2f;
                    sputnikCount.count--;
                }
                   
                else if (PlayerPrefs.GetInt("2currentAlteration") == 2)
                {
                    animatorOfCharacters.SetTrigger("IsFiringLasers");
                    lasersManager.ShootFreezing();

                    CompleteActionsAfterFiring();
                }
            }

            else if (CurrentWeapon == Weapons.Dynamite)
            {
                animatorOfCharacters.SetTrigger("IsFiringBomb");

                if (PlayerPrefs.GetInt("3currentAlteration") == 0)
                    dynamiteManager.Shoot();
                else if (PlayerPrefs.GetInt("3currentAlteration") == 1)
                    dynamiteManager.ShootWithLittleTntsAtTheBottom();
                else if (PlayerPrefs.GetInt("3currentAlteration") == 2)
                    dynamiteManager.ShootSplit();

                AudioManager.instance.Play("Whistle2");
                CompleteActionsAfterFiring();
            }

            else if (CurrentWeapon == Weapons.Blasters)
            {
                animatorOfCharacters.SetTrigger("IsFiringBlasters");

                if (PlayerPrefs.GetInt("4currentAlteration") == 0)
                    blastersManager.Shoot();
                else if (PlayerPrefs.GetInt("4currentAlteration") == 1)
                    blastersManager.ShootMore();
                else if (PlayerPrefs.GetInt("4currentAlteration") == 2)
                    blastersManager.ShootWildFire();

                CompleteActionsAfterFiring();
            }
        }
    }

    private void CompleteActionsAfterFiring()
    {
        cooldown = ShootDelay;

        if (PlayerPrefs.GetInt("currentLevel") >= 15) sputnikCount.count--;

        TimesShooted++;
    }
}
