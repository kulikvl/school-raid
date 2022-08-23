using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OKmission : MonoBehaviour
{
    public GameObject Panel;

    private void Start()
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void OnMouseUpAsButton()
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
        Panel.transform.position = new Vector2(1000f, 1000f);
        //UnFreezeEnemies();
    }

    //private void UnFreezeEnemies()
    //{
    //    RCONTROL.ALLOWED = true;

    //    GameObject[] g1 = GameObject.FindGameObjectsWithTag("Enemy");
    //    GameObject[] g2 = GameObject.FindGameObjectsWithTag("MiniEnemy");
    //    GameObject[] g3 = GameObject.FindGameObjectsWithTag("TankEnemy");

    //    GameObject[] gos = g1.Concat(g2).Concat(g3).ToArray();

    //    if (gos != null)
    //        foreach (GameObject go in gos)
    //        {
    //            if (go.GetComponent<FREEZE>() != null)
    //                go.GetComponent<FREEZE>().Freeze(false);
    //        }
    //}
}
