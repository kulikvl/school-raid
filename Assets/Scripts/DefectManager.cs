using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefectManager : MonoBehaviour
{
    public List<GameObject> Fire;
    public List<GameObject> FireInitial;
    [SerializeField] private Transform Container;
    public List<Transform> Blocks;
    private bool[] done = new bool[10];

    public List<GameObject> Windows;
    public List<GameObject> Enviroment;

    public bool isCenter = false;

    [Space]

    public GameObject Clock, DestroyedClock, Tab, DestroyedTab, WindowSmash1, WindowSmash2;

    private void Start()
    {
        Renderer[] sprites = GetComponentsInChildren<Renderer>();
        foreach (Renderer sp in sprites) sp.sortingLayerID = SortingLayer.NameToID("School");

        Transform[] trs = Container.GetComponentsInChildren<Transform>();
        Blocks.AddRange(trs);
        Blocks.RemoveAt(0);

        foreach(Transform t in Blocks)
        {
            t.gameObject.SetActive(false);
        }

        foreach (GameObject go in Enviroment)
        {
            Transform[] tr = go.GetComponentsInChildren<Transform>();

            foreach (Transform t in tr)
            {
                if (t.gameObject.name == "Light")
                {
                    if (Random.Range(0, 2) == 0)
                        t.gameObject.GetComponent<Animation>().Play("light1");
                    else
                        t.gameObject.GetComponent<Animation>().Play("light2");
                }
            }
        }

        SetWindows();

        if (name == "Center")
        {
            DestroyedClock.SetActive(false);
            DestroyedTab.SetActive(false);
            WindowSmash1.SetActive(false);
            WindowSmash2.SetActive(false);
        }

        for (int i = 0; i < Fire.Count; ++i)
        {
            FireInitial.Add(Fire[i]);
        }
    }

    public void StopAllFires()
    {
        foreach(GameObject go in FireInitial)
        {
            go.GetComponent<ParticleSystem>().Stop();
        }
    }

    public void ChangeAppearence(float HP)
    {
        if (HP <= 90f && done[0] == false)
        {
            AudioManager.instance.Play("SchoolImpact" + Random.Range(1, 4).ToString());
            BlocksAppear(3);
            SetSOS();
            
            done[0] = true;
        }
        if (HP <= 80f && done[1] == false)
        {
            AudioManager.instance.Play("SchoolImpact4");
            AudioManager.instance.Play("SchoolImpact" + Random.Range(1, 4).ToString());
            BlocksAppear(3);
            SetSOS();
            done[1] = true;
        }
        if (HP <= 70f && done[2] == false)
        {
            AudioManager.instance.Play("SchoolImpact" + Random.Range(1, 4).ToString());
            DestroyClock();
            BrokeWindow();

            BlocksAppear(3);
            SetSOS();
            done[2] = true;
        }
        if (HP <= 60f && done[3] == false)
        {
            AudioManager.instance.Play("SchoolImpact4");
            AudioManager.instance.Play("SchoolImpact" + Random.Range(1, 4).ToString());
            SetFire();

            BlocksAppear(3);
            SetSOS();
            done[3] = true;
        }
        if (HP <= 50f && done[4] == false)
        {
            AudioManager.instance.Play("SchoolImpact" + Random.Range(1, 4).ToString());
            BlocksAppear(3);
            BrokeWindow();
            done[4] = true;
        }
        if (HP <= 40f && done[5] == false)
        {
            AudioManager.instance.Play("SchoolImpact" + Random.Range(1, 4).ToString());
            DestroyTab();
            BlocksAppear(3);
            done[5] = true;
        }
        if (HP <= 30f && done[6] == false)
        {
            AudioManager.instance.Play("SchoolImpact4");
            WindowSmash();
            BrokeWindow();
            BlocksAppear(3);
            done[6] = true;
        }
        if (HP <= 20f && done[7] == false)
        {
            AudioManager.instance.Play("SchoolImpact" + Random.Range(1, 4).ToString());
            BlocksAppear(3);
            SetFire();
            SetSOS();
            done[7] = true;
        }
        if (HP <= 10f && done[8] == false)
        {
            if (isCenter) SetFire();

            BrokeWindow();
            BlocksAppear(3);
            done[8] = true;
            AudioManager.instance.Play("SchoolImpact" + Random.Range(1, 4).ToString());
        }
    }

    private void BlocksAppear(int num)
    {
        for (int i = 0; i < num; i++)
        {
            int rand = Random.Range(0, Blocks.Count);

            Blocks[rand].gameObject.SetActive(true);

            Blocks.RemoveAt(rand);
        }
    }

    private void BrokeWindow()
    {
        int rand = Random.Range(0, Windows.Count);

        Transform[] trs = Windows[rand].GetComponentsInChildren<Transform>();

        foreach (Transform t in trs)
        {
            if (t.gameObject.name == "Window")
            {
                Transform[] trs2 = t.GetComponentsInChildren<Transform>();
                foreach (Transform t2 in trs2)
                {
                    if (t2.gameObject.name != t.gameObject.name)
                        t2.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
            if (t.gameObject.name == "WindowBroken")
            {
                Transform[] trs2 = t.GetComponentsInChildren<Transform>();
                foreach (Transform t2 in trs2)
                {
                    if (t2.gameObject.name != t.gameObject.name)
                        t2.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                }
            }
        }

        Windows.RemoveAt(rand);
    }

    private void SetWindows()
    {
        for(int i = 0; i < Windows.Count; ++i)
        {
            Transform[] trs = Windows[i].GetComponentsInChildren<Transform>();

            foreach (Transform t in trs)
            {
                if (t.gameObject.name == "Window")
                {
                    Transform[] trs2 = t.GetComponentsInChildren<Transform>();

                    foreach (Transform t2 in trs2)
                    {
                        if (t2.gameObject.name != t.gameObject.name)
                            t2.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    }   
                }
                if (t.gameObject.name == "WindowBroken")
                {
                    Transform[] trs2 = t.GetComponentsInChildren<Transform>();
                    foreach (Transform t2 in trs2)
                    {
                        if (t2.gameObject.name != t.gameObject.name)
                            t2.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    }     
                }
            }
        }
    }

    private void SetSOS()
    {
        int rand = Random.Range(0, Enviroment.Count);
        Transform[] trs = Enviroment[rand].GetComponentsInChildren<Transform>();

        string _name = (Random.Range(0, 7) == 0) ? "Light" : "SOS";

        foreach (Transform t in trs)
        {
            if (t.gameObject.name == _name)
            {
                if (_name == "Light") t.gameObject.SetActive(false);
                else
                {
                    t.gameObject.GetComponent<Animation>().Play();
                }
            }
        }

        Enviroment.RemoveAt(rand);
    }

    private void DestroyTab()
    {
        if (isCenter)
        {
            Tab.SetActive(false);
            DestroyedTab.SetActive(true);
        } 
    }

    private void DestroyClock()
    {
        if (isCenter)
        {
            Clock.SetActive(false);
            DestroyedClock.SetActive(true);
        }
    }

    private void SetFire()
    {
        int rand = Random.Range(0, Fire.Count);

        Fire[rand].gameObject.SetActive(true);

        Fire.RemoveAt(rand);
    }

    private void WindowSmash()
    {
        if (isCenter)
        {
            if (Random.Range(0, 2) == 0)
                WindowSmash1.SetActive(true);
            else
                WindowSmash2.SetActive(true);
        }
        
    }

    //private void OnMouseUpAsButton()
    //{
    //    GetComponent<School>().takeDamage(0.05f);
    //}
}
