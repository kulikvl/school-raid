using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class School : MonoBehaviour, IDamageable
{
    public GameObject particleCenter, bloodOnDeathParticle;

    [SerializeField] private Image HP;
    [SerializeField] public GameObject particleEffectOnDestroy; // todo

    public GameObject deathGameObject;

    public bool destroyed;

    private GameObject cameraFollow;

    public bool CanBeDamaged { get; set; }

    private void Awake()
    {
        CanBeDamaged = true;
        checkIfDestroyed();

        if (PlayerPrefs.GetString("currentMode") == "Deathmatch")
        {
            HP.transform.parent.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        destroyed = false;
        findLeftAndRightForCenter();

        cameraFollow = GameObject.FindGameObjectWithTag("cameraFollow");
        if (cameraFollow == null)
            Debug.LogError("CAN NOT FIND CAMERA FOLLOW! ");
    }

    public void takeDamage(float amount)
    {
        if (CanBeDamaged)
        {
            if (name == "Center")
            {
                PlayerController.centerHP -= amount;
                GetComponent<DefectManager>().ChangeAppearence((PlayerController.centerHP * 100));
            }
            if (name == "Left")
            {
                PlayerController.leftHP -= amount;
                GetComponent<DefectManager>().ChangeAppearence((PlayerController.leftHP * 100));
            }
            if (name == "Right")
            {
                PlayerController.rightHP -= amount;
                GetComponent<DefectManager>().ChangeAppearence((PlayerController.rightHP * 100));
            }

            showHpValueOnSlider();
            CheckForDeath();
        }
    }

    private void CheckForDeath()
    {
        if (HP != null && HP.fillAmount <= 0 && !destroyed)
        {
            Debug.Log("destroyed " + name);

            if (name == "Center")
            {
                if (Application.internetReachability != NetworkReachability.NotReachable && !PlayerController.HealedByWatchingAd && PlayerPrefs.GetString("currentMode") == "Campaign")
                {
                    GameObject SecondChanceCanvas = GameObject.FindGameObjectWithTag("Secondchance");
                    SecondChanceCanvas.transform.GetChild(0).gameObject.SetActive(true);
                    // open tab second chance
                }
                else
                DestroyCenter();
            }
            else
            {
                DestroyLeftOrRight();
            }
        } 
    }

    public void ReviveSchool()
    {
        FindObjectOfType<SecondChanceButton>().ManageCols(true);
        AudioManager.instance.UnPauseAllSounds();
        GameObject SecondChanceCanvas = GameObject.FindGameObjectWithTag("Secondchance");
        SecondChanceCanvas.transform.GetChild(0).gameObject.SetActive(false);

        GameObject[] gos = GameObject.FindGameObjectsWithTag("logic");

        if (gos.Length > 0)
            foreach (GameObject go in gos)
            {
                go.GetComponent<Enemy>().Die(false, false, false);
                string nameOfSound = "BloodSplash" + Random.Range(1, 7).ToString();
                AudioManager.instance.Play(nameOfSound);
                Instantiate(bloodOnDeathParticle, go.GetComponent<Enemy>().CenterOfEnemy.position, Quaternion.identity);
                go.transform.parent.gameObject.GetComponent<tabManager>().deleteTab();
                Destroy(go.transform.parent.gameObject);
            }

        SecondChanceButton.agreedToWatchAd = false;
        PlayerController.centerHP = 0.8f;
        Instantiate(particleCenter);
        GetComponent<DefectManager>().StopAllFires();
        PlayerController.HealedByWatchingAd = true;
        
    }

    private void Update()
    {
        showHpValueOnSlider();
    }

    public void DestroyCenter()
    {
        PlayerController.centerHP = 0f;
        PlayerController.Lost = true;
        destroyed = true;

        AudioManager.instance.Play("Explosion8");

        Vector3 vec = new Vector3(transform.position.x, transform.position.y - 1.2f, transform.position.z);
        Instantiate(particleEffectOnDestroy, vec, Quaternion.identity);

        HP.transform.parent.gameObject.SetActive(false);
        gameObject.SetActive(false);
        deathGameObject.SetActive(true);

        if (Right.GetComponent<School>().destroyed == false) // destroy Right
        {
            PlayerController.rightHP = 0f;
            Right.GetComponent<School>().destroyed = true;

            Vector3 vec2 = new Vector3(Right.transform.position.x, Right.transform.position.y - 1.2f, Right.transform.position.z);
            Instantiate(Right.GetComponent<School>().particleEffectOnDestroy, vec2, Quaternion.identity);

            Right.GetComponent<School>().deathGameObject.SetActive(true);
            Right.GetComponent<School>().HP.transform.parent.gameObject.SetActive(false);
            Right.SetActive(false);

        }
        if (Left.GetComponent<School>().destroyed == false) // destroy Left
        {
            PlayerController.leftHP = 0f;
            Left.GetComponent<School>().destroyed = true;

            Vector3 vec2 = new Vector3(Left.transform.position.x, Left.transform.position.y - 1.2f, Left.transform.position.z);
            Instantiate(Left.GetComponent<School>().particleEffectOnDestroy, vec2, Quaternion.identity);

            Left.GetComponent<School>().deathGameObject.SetActive(true);
            Left.GetComponent<School>().HP.transform.parent.gameObject.SetActive(false);
            Left.SetActive(false);
        }

        // запускаем анимацию победы у всех злодеев

        GameObject[] gos = GameObject.FindGameObjectsWithTag("logic");

        if (gos != null)
        {
            foreach (GameObject go in gos)
            {
                Enemy enemy = go.GetComponent<Enemy>();
                enemy.winAnimation();
            }
        }

        if (PlayerPrefs.GetString("currentMode") == "Campaign")
        cameraFollow.GetComponent<Destructible2D.D2dFollow>().ifPlayerLost();
        else
        cameraFollow.GetComponent<Destructible2D.D2dFollow>().ifPlayerLostDeathMatch();
    }

    public void DestroyLeftOrRight()
    {
        AudioManager.instance.Play("Explosion8");

        if (name == "Left")
        {
            PlayerController.leftHP = 0f;
        }
        if (name == "Right")
        {
            PlayerController.rightHP = 0f;
        }

        destroyed = true;

        Vector3 vec = new Vector3(transform.position.x, transform.position.y - 1.2f, transform.position.z);
        Instantiate(particleEffectOnDestroy, vec, Quaternion.identity);

        HP.transform.parent.gameObject.SetActive(false);
        gameObject.SetActive(false);
        deathGameObject.SetActive(true);

        ///// FOR ENEMIES TO STOP DAMAGING /////

        GameObject[] gos = GameObject.FindGameObjectsWithTag("logic");

        if (gos != null)
        {
            foreach (GameObject go in gos)
            {
                Enemy enemy = go.GetComponent<Enemy>();

                if (enemy.IsDamaging)
                    enemy.stopDamaging();
            }
        }
    }

    private GameObject Right, Left; // only for center
    private void findLeftAndRightForCenter()
    {
        if (gameObject.name == "Center")
        {
            Transform[] trs1 = GetComponentsInParent<Transform>();

            foreach (var ob1 in trs1)
            {
                if (ob1.gameObject.name == "school")
                {
                    Transform[] trs2 = ob1.GetComponentsInChildren<Transform>();

                    foreach (var ob2 in trs2)
                    {
                        if (ob2.gameObject.name == "Left")
                        {
                            Left = ob2.gameObject;
                        }
                        if (ob2.gameObject.name == "Right")
                        {
                            Right = ob2.gameObject;
                        }
                    }
                }
            }
        }
    }

    private void showHpValueOnSlider()
    {
        if (name == "Center")
        {
            HP.fillAmount = PlayerController.centerHP;
        }
        if (name == "Left")
        {
            HP.fillAmount = PlayerController.leftHP;
        }
        if (name == "Right")
        {
            HP.fillAmount = PlayerController.rightHP;
        }
    }

    private void checkIfDestroyed()
    {
        if (name == "Center")
        {
            if (PlayerController.centerHP <= 0)
            {
                HP.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
            else
            {
                HP.fillAmount = PlayerController.centerHP;
            }
        }
        if (name == "Left")
        {
            if (PlayerController.leftHP <= 0)
            {
                HP.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
            else
            {
                HP.fillAmount = PlayerController.leftHP;
            }
        }
        if (name == "Right")
        {
            if (PlayerController.rightHP <= 0)
            {
                HP.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
            else
            {
                HP.fillAmount = PlayerController.rightHP;
            }
        }
        
    }

}
