using UnityEngine;
using System.Collections;
using TMPro;

public class Box : MonoBehaviour
{

    //private bool readytodie = false;
    //private bool killing = false;



    //private GameObject boom;
    //private GameObject partTaking;
    //private GameObject Coins;
    //public GameObject death;
    //public GameObject que;
    //private GameObject player;

    //void Start()
    //{
    //    killing = false;
    //    readytodie = false;


    //    boom = GameObject.FindGameObjectWithTag("boom");
    //    partTaking = GameObject.FindGameObjectWithTag("partTaking");
    //    Coins = GameObject.FindGameObjectWithTag("coins");
    //    player = GameObject.FindGameObjectWithTag("Player");
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //    if (collision.tag == "Player" && killing == false)
        //    {


        if (PlayerPrefs.GetInt("currentMission1level") == 6 && PlayerPrefs.GetString("currentMission1") != "true")
        {
            PlayerPrefs.SetInt("collectedBoxes3", PlayerPrefs.GetInt("collectedBoxes3") + 1);

            float val = PlayerPrefs.GetInt("collectedBoxes3");
            PlayerPrefs.SetFloat("C1value", val);

            if (PlayerPrefs.GetInt("collectedBoxes3") >= 3)
            {
                PlayerPrefs.SetInt("collectedBoxes3", 0);
                PlayerPrefs.SetString("currentMission1", "true");
            }
        }
    }

    //        if (PlayerPrefs.GetInt("currentMission1level") == 8 && PlayerPrefs.GetString("currentMission1") != "true")
    //        {
    //            PlayerPrefs.SetInt("collectedBoxes5", PlayerPrefs.GetInt("collectedBoxes5") + 1);

    //            float val = PlayerPrefs.GetInt("collectedBoxes5");
    //            PlayerPrefs.SetFloat("C1value", val);

    //            if (PlayerPrefs.GetInt("collectedBoxes5") >= 5)
    //            {
    //                PlayerPrefs.SetInt("collectedBoxes5", 0);
    //                PlayerPrefs.SetString("currentMission1", "true");
    //            }
    //        }

    //        if (PlayerPrefs.GetInt("currentMission2level") == 6 && PlayerPrefs.GetString("currentMission2") != "true")
    //        {
    //            PlayerPrefs.SetInt("collectedBoxes7", PlayerPrefs.GetInt("collectedBoxes7") + 1);

    //            float val = PlayerPrefs.GetInt("collectedBoxes7");
    //            PlayerPrefs.SetFloat("C2value", val);

    //            if (PlayerPrefs.GetInt("collectedBoxes7") >= 7)
    //            {
    //                PlayerPrefs.SetInt("collectedBoxes7", 0);
    //                PlayerPrefs.SetString("currentMission2", "true");
    //            }
    //        }

    //        if (PlayerPrefs.GetInt("currentMission2level") == 8 && PlayerPrefs.GetString("currentMission2") != "true")
    //        {
    //            PlayerPrefs.SetInt("collectedBoxes10", PlayerPrefs.GetInt("collectedBoxes10") + 1);

    //            float val = PlayerPrefs.GetInt("collectedBoxes10");
    //            PlayerPrefs.SetFloat("C2value", val);

    //            if (PlayerPrefs.GetInt("collectedBoxes10") >= 10)
    //            {
    //                PlayerPrefs.SetInt("collectedBoxes10", 0);
    //                PlayerPrefs.SetString("currentMission2", "true");
    //            }
    //        }

    //        if (PlayerPrefs.GetInt("currentMission3level") == 5 && PlayerPrefs.GetString("currentMission3") != "true")
    //        {
    //            PlayerPrefs.SetInt("collectedBoxes30", PlayerPrefs.GetInt("collectedBoxes30") + 1);

    //            float val = PlayerPrefs.GetInt("collectedBoxes30");
    //            PlayerPrefs.SetFloat("C3value", val);

    //            if (PlayerPrefs.GetInt("collectedBoxes30") >= 30)
    //            {
    //                PlayerPrefs.SetInt("collectedBoxes30", 0);
    //                PlayerPrefs.SetString("currentMission3", "true");
    //            }
    //        }




    //        int coins;

    //        if (gameObject.tag == "gold")
    //        {
    //            coins = 100;
    //            PlayerPrefs.SetInt(("Golds"), PlayerPrefs.GetInt("Golds") + 1);

    //            if (PlayerPrefs.GetInt("currentMission2level") == 5 && PlayerPrefs.GetString("currentMission2") != "true")
    //            {
               
    //                PlayerPrefs.SetFloat("C2value", 1);
    //                PlayerPrefs.SetString("currentMission2", "true");

    //            }

    //            if (PlayerPrefs.GetInt("currentMission3level") == 4 && PlayerPrefs.GetString("currentMission3") != "true")
    //            {
    //                PlayerPrefs.SetInt("collectedGBoxes5", PlayerPrefs.GetInt("collectedGBoxes5") + 1);

    //                float val = PlayerPrefs.GetInt("collectedGBoxes5");
    //                PlayerPrefs.SetFloat("C3value", val);

    //                if (PlayerPrefs.GetInt("collectedGBoxes5") >= 5)
    //                {
    //                    PlayerPrefs.SetInt("collectedGBoxes5", 0);
    //                    PlayerPrefs.SetString("currentMission3", "true");
    //                }
    //            }


    //        }
    //        else
    //        {
    //            if (PlayerPrefs.GetString("currentBall") == "10")
    //            {
    //                coins = Random.Range(30, 55);
    //            }
    //            else if (PlayerPrefs.GetString("currentBall") == "17")
    //            {
    //                coins = 77;
    //            }
    //            else
    //            {
    //                coins = Random.Range(5, 21);
    //            }
               
    //            PlayerPrefs.SetInt(("Catches"), PlayerPrefs.GetInt("Catches") + 1);

    //        }

    //        Coins.GetComponent<Coins>().GettingCoins(coins);

    //        if (boom != null)
    //        {
    //            GameObject taking = Instantiate(partTaking);
    //            taking.transform.position = new Vector3(transform.position.x, transform.position.y - 2f, transform.position.z);
    //            taking.GetComponent<ParticleSystem>().Play();
    //        }

    //        readytodie = true;
    //    }
    //    if (collision.tag == "Player" && killing == true)
    //    {


    //        if (boom != null)
    //        {
    //            GameObject Boom = Instantiate(boom);
    //            Boom.transform.position = new Vector3(transform.position.x, transform.position.y - 2f, transform.position.z);
    //            Boom.GetComponent<ParticleSystem>().Play();
    //        }

    //        readytodie = true;

    //        if (CurrentScore.CurrentScene == 0)
    //        {
    //            collision.GetComponent<PlayerScript>().DieFromHaz();
    //        }
    //        else
    //        {
    //            collision.GetComponent<newPlayer>().DieFromHaz();
    //        }

    //    }
    //}

    //private bool done = false;

    //void Update()
    //{
    //    if (player != null && gameObject.transform.position.x - player.transform.position.x < 30 && done == false)
    //    {
    //        gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
    //        StartCoroutine(Pass());
    //        StartCoroutine(TimeToStop());
    //        StartCoroutine(boomAnim());
    //        done = true;
    //    }

       

    //    if (readytodie == true)
    //        Destroy(gameObject);

    //}

    //IEnumerator Pass()
    //{
    //    yield return new WaitForSeconds(11.5f);

    //    if (boom != null)
    //    {
    //        GameObject Boom = Instantiate(boom);
    //        Boom.transform.position = new Vector3(transform.position.x ,transform.position.y - 2f, transform.position.z);
    //        Boom.GetComponent<ParticleSystem>().Play();
    //    }

    //    readytodie = true;  
    //}

    //IEnumerator boomAnim()
    //{
    //    yield return new WaitForSeconds(10.5f);

    //    if (gameObject.tag != "gold" && CurrentScore.CurrentScene > 2)
    //    {
    //        killing = true;

    //        if (death != null && que != null)
    //        {
    //            death.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    //            que.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 0f);
    //        }
    //    }

    //    gameObject.GetComponent<Animation>().Play();
    //}

    //IEnumerator TimeToStop()
    //{
    //    yield return new WaitForSeconds(10f);
    //    gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;

    //}
}
