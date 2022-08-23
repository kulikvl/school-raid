using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dFollow))]
	public class D2dFollow_Editor : D2dEditor<D2dFollow>
	{
		protected override void OnInspector()
		{
            DrawDefault("y");
            DrawDefault("cam");
            DrawDefault("Conf1");
            DrawDefault("LoseWindow");

            DrawDefault("secondsAfterDeath");
        }
	}
}
#endif

namespace Destructible2D
{
	public class D2dFollow : MonoBehaviour
	{
        [Tooltip("deadline y")]
        public float y;

        [Tooltip("Camera")]
        public GameObject cam;

        [Tooltip("Confetti #1")]
        public GameObject Conf1;

        [Tooltip("Window opens if player loses")]
        public GameObject LoseWindow;

        public float secondsAfterDeath = 3f;

        private Transform Target;

        private void Start()
        {
            GameObject _target = GameObject.FindGameObjectWithTag("BUS");

            if (_target != null)
                Target = _target.transform;
            else
                Debug.LogError("CAN NOT FIND BUS! ");


            //PlayerPrefs.SetInt("currentLevel", 1); //todo
            //ifPlayerWon();
        }

        protected virtual void Update()
        {
            if ((!PlayerController.Won && PlayerPrefs.GetString("currentMode") == "Campaign") || (!PlayerController.Lost && PlayerPrefs.GetString("currentMode") == "Deathmatch"))
            {
                if (TutorialSputnik.instance.activateCameraOnSputnik && TutorialSputnik.instance.firstSputnik != null)
                    CameraOnSputnik();
                else
                    UpdatePosition();
            } 
            else 
            {
                if (beforeMenu)
                {
                    BeforeMenu();
                }
                else
                {
                    AfterMenu();
                }
            }

        }

        private void UpdatePosition()
		{
			if (Target != null)
			{
				var position = transform.position;

                ///// ГРАНИЦЫ ПО ОСИ X
                if (Target.position.x < -10f)
                {
                    position.x = -10f;
                }
                else if (Target.position.x > 5f)
                {
                    position.x = 5f;
                }
                else
                {
                    position.x = Target.position.x;
                }

                ///// ГРАНИЦЫ ПО ОСИ Y
                if (Target.position.y < y)
                {
                    position.y = -1f;
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(position.x, -1f, Target.position.z), 10f * Time.deltaTime);
                }
                else if (Target.position.y > 8f)
                {
                    position.y = 8f;
                    transform.position = position;
                }
                else
                {
                    position.y = Target.position.y;
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(position.x, Target.position.y, Target.position.z), 10f * Time.deltaTime);
                }
            }
		}

        private void CameraOnSputnik()
        {
            Vector3 target = TutorialSputnik.instance.firstSputnik.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, target, 10f * Time.deltaTime);
        }

        private bool beforeMenu = true;
        private void BeforeMenu()
        {
            //Time.timeScale = 1f;
            //Time.fixedDeltaTime = 0.02f * Time.timeScale;

            if (cameraPlayed == false)
            {
                cam.GetComponent<Animation>().Play();
                cameraPlayed = true;
            }

            cam.GetComponent<Player>().DisableBusControls();

            Target.gameObject.GetComponent<BusController>().enabled = false;

            if ((DeathmatchLevelController.newRecordSet && PlayerPrefs.GetString("currentMode") == "Deathmatch") || PlayerPrefs.GetString("currentMode") == "Campaign")
                Conf1.SetActive(true);

            if (PlayerPrefs.GetString("currentMode") == "Campaign")
            {
                Vector3 vec = new Vector3(Target.position.x, Target.position.y + 1f, Target.position.z);
                transform.position = Vector3.MoveTowards(transform.position, vec, 10f * Time.deltaTime);
            }
            else
            {
                if (survivedRandomEnemy != null)
                {
                    Vector3 vec = new Vector3(survivedRandomEnemy.position.x, survivedRandomEnemy.position.y + 1f, survivedRandomEnemy.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, vec, 10f * Time.deltaTime);
                }
                  
            }
                
        }

        private void AfterMenu()
        {
            //Time.timeScale = 1f;
            //Time.fixedDeltaTime = 0.02f * Time.timeScale;


            Camera cam = Camera.main;
            Vector3 point = new Vector3();
            Vector3 pointf = new Vector3();
            point = cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, cam.nearClipPlane));
            pointf = cam.ScreenToWorldPoint(new Vector3(0f, Screen.height / 2, cam.nearClipPlane));

            float X = (point.x - pointf.x) / 2;
            //Debug.Log("Center: " + point + " Left: " + pointf);
            //Debug.Log("Value: " + X);
            // 1920 - 2.3 4400
            // 2224 - 1.8 4000

            // -4 ; -0.6

            if (PlayerPrefs.GetString("currentMode") == "Campaign")
            {
                Vector3 vec = new Vector3(Target.position.x + X, Target.position.y + 1f, Target.position.z);

                transform.position = Vector3.MoveTowards(transform.position, vec, 5f * Time.deltaTime);
            }
            else
            {
                if (survivedRandomEnemy != null)
                {
                    Vector3 vec = new Vector3(survivedRandomEnemy.position.x + 2.3f, survivedRandomEnemy.position.y + 1f, survivedRandomEnemy.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, vec, 10f * Time.deltaTime);
                }   
            }  
        }

        private bool cameraPlayed = false;

        #region ifPlayerWon 
        public void ifPlayerWon()
        {
            StartCoroutine(WinningSounds());
            AudioManager.instance.Play("Cheering");

            PlayerController.SetStars();

            SettingsInGame.enabledToPressSettings = false;

            Debug.Log("IF PLAYER WON!!!");

            GameObject continueButton = GameObject.FindGameObjectWithTag("ContinueButton");

            // IF THIS LEVEL IS NEW
            if (PlayerPrefs.GetInt("unlockedLevels") == PlayerPrefs.GetInt("currentLevel"))
            {
                if (PlayerPrefs.GetInt("unlockedLevels") == 1)
                {
                    GameCenterManager.UnlockAchievement(GameCenterManager.AchievementID.Beginning);
                    PlayGamesManager.UnlockAchievement(PlayGamesManager.AchievementID.Beginning);
                }
                else if (PlayerPrefs.GetInt("unlockedLevels") == 5)
                {
                    GameCenterManager.UnlockAchievement(GameCenterManager.AchievementID.Amateur);
                    PlayGamesManager.UnlockAchievement(PlayGamesManager.AchievementID.Amateur);
                }
                else if (PlayerPrefs.GetInt("unlockedLevels") == 20)
                {
                    GameCenterManager.UnlockAchievement(GameCenterManager.AchievementID.Skilled);
                    PlayGamesManager.UnlockAchievement(PlayGamesManager.AchievementID.Skilled);
                }
                else if (PlayerPrefs.GetInt("unlockedLevels") == 40)
                {
                    GameCenterManager.UnlockAchievement(GameCenterManager.AchievementID.PRO);
                    PlayGamesManager.UnlockAchievement(PlayGamesManager.AchievementID.PRO);
                }

                continueButton.GetComponent<ContinueButton>().isPassedLevel = true;
                PlayerPrefs.SetInt("unlockedLevels", PlayerPrefs.GetInt("unlockedLevels") + 1);
            }
            else
                continueButton.GetComponent<ContinueButton>().isPassedLevel = false;


            continueButton.GetComponent<ContinueButton>().moveUI = true;

            StartCoroutine(ActionsAfterWinning(continueButton));
        }

        private IEnumerator ActionsAfterWinning(GameObject continueButton)
        {
            beforeMenu = true;
            yield return new WaitForSeconds(3f);

            // ad here
            PlayerController.TimesWithoutWatchingAd++;

            if (PlayerController.TimesWithoutWatchingAd == 2 && PlayerPrefs.GetInt("currentLevel") > 4)
            {
                PlayerController.TimesWithoutWatchingAd = 0;
                AdManager.instance.PlayInterstitialAd();

            }

            Conf1.GetComponent<Animation>().Play();

            beforeMenu = false;
            continueButton.GetComponent<ContinueButton>().SetFinalMenu();
        }
        #endregion


        public void ifPlayerLostDeathMatch()
        {
            SettingsInGame.enabledToPressSettings = false;
            Debug.Log("IF PLAYER LOST DEATHMATCH!!!");
            StartCoroutine(ActionsAfterLoosingDeathMatch());
        }
        Transform survivedRandomEnemy = null;
        private IEnumerator ActionsAfterLoosingDeathMatch()
        {
            GameObject continueButton = GameObject.FindGameObjectWithTag("ContinueButton");
            continueButton.GetComponent<ContinueButton>().moveUI = true;

            yield return new WaitForSeconds(0.1f);

            /// FIND RANDOM SURVIVED ENEMY
            GameObject[] gos = GameObject.FindGameObjectsWithTag("logic");
            List<GameObject> SurvivedEnemies = new List<GameObject>();

            if (gos.Length > 0)
                foreach (GameObject go in gos)
                {
                    if (!go.GetComponent<Enemy>().IsDead)
                    {
                        SurvivedEnemies.Add(go);
                    }
                }

            if (SurvivedEnemies.Count > 0)
            {
                
                survivedRandomEnemy = SurvivedEnemies[Random.Range(0, SurvivedEnemies.Count)].GetComponent<Enemy>().CenterOfEnemy;
                Debug.Log("FOUND!" + survivedRandomEnemy.gameObject.name);
            }

            /// FIND RANDOM SURVIVED ENEMY

            beforeMenu = false;

            yield return new WaitForSeconds(1f);

            if (DeathmatchLevelController.newRecordSet)
            Conf1.GetComponent<Animation>().Play();

            continueButton.GetComponent<ContinueButton>().SetFinalMenu();
        }


        #region ifPlayerLost 
        public void ifPlayerLost()
        {
            AudioManager.instance.Play("Loose");

            SettingsInGame.enabledToPressSettings = false;

            if (PlayerController.Lost)
            {
                PlayerController.SetFullHpSchool();
            }

            StartCoroutine(ActionsAfterDeath());
        }

        private IEnumerator ActionsAfterDeath()
        {
            yield return new WaitForSeconds(secondsAfterDeath);
            Lose();
        }

        private void Lose()
        {
            StartCoroutine(ReloadLevel());

            GameObject g = GameObject.FindGameObjectWithTag("tent");
            g.GetComponent<Animator>().SetTrigger("DARK");
        }

        private IEnumerator ReloadLevel()
        {
            yield return new WaitForSeconds(0.5f);
            PlayerController.SetFullHpSchool();
            SceneManager.LoadScene("Scene");
        }
        #endregion


        private IEnumerator WinningSounds()
        {
            for (int i = 0; i < 60; ++i)
            {
                yield return new WaitForSeconds(Random.Range(0.15f, 0.3f));
                AudioManager.instance.Play("Firework" + Random.Range(1, 3).ToString());
            }
        }
    }
}
