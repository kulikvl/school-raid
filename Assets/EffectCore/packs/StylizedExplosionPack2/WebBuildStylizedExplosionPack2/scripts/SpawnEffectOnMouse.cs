using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnEffectOnMouse : MonoBehaviour
{

    [System.Serializable]
    public class chainEffect
    {
        [System.NonSerialized]

        public int firstvariableisinvisible;
        public string name;
        public bool isPlayed = false;
        public float Yoffset = 0;
        public bool RotateRandomizer = false;
       
        public GameObject Effect;      
    }
    public chainEffect[] chainEffectList;
    public int activevariation;
    //   public Transform effectLocator;
    public Text UIText;

    Ray ray;
    RaycastHit hit;

    // Use this for initialization
    void Start()
    {
        CheckName();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D) || Input.GetButtonDown("Fire2"))
        {
            NextEffect(true);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            NextEffect(false);
        }
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetButtonDown("Fire1"))
            {
                GameObject obj = Instantiate(chainEffectList[activevariation].Effect, new Vector3(hit.point.x, hit.point.y+chainEffectList[activevariation].Yoffset, hit.point.z), Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;//Quaternion.identity
            }
        }
    }

    void NextEffect(bool increase)
    {
        if (increase)
        { 
           activevariation++;
            if (activevariation >= chainEffectList.Length)
            {
                activevariation = 0;
            }       
        }

        if (!increase)
        {
            activevariation--;
            if (activevariation < 0)
            {
                activevariation = chainEffectList.Length-1;
            }          
        }
        CheckName();
    }

    void CheckName()
    {
      //  chainEffectList[activevariation].Effect.name = "#" + activevariation + 1 + " " + UIText.name;
        UIText.text = "#" + activevariation + " " + chainEffectList[activevariation].name;
    }
}
