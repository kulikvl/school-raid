using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tabManager : MonoBehaviour
{
    [SerializeField] private GameObject tabPrefab;

    public float X;
    private GameObject parent;
    private tabFollow tab;
    private Camera cam;

    private bool isRight;
    private GameObject rightCam;
    private GameObject leftCam;

    private void Start()
    {
        rightCam = GameObject.FindGameObjectWithTag("rightCam");
        leftCam = GameObject.FindGameObjectWithTag("leftCam");

        isRight = (gameObject.name.Contains("Right")) ? true : false;

        if (isRight)
            parent = GameObject.FindGameObjectWithTag("enemyTabsRight");      
        else
            parent = GameObject.FindGameObjectWithTag("enemyTabsLeft");

        if (parent == null) { Debug.LogError("CAN NOT FIND PARENT! "); }

        cam = Camera.main;

        CreateEnemyTab();
    }


    public void deleteTab()
    {
        gameObject.GetComponent<tabManager>().enabled = false;
        Destroy(tab.gameObject);
    }

    private void CreateEnemyTab()
    {
        GameObject pref = Instantiate(tabPrefab, new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z), Quaternion.identity, parent.transform);
        tab = pref.GetComponent<tabFollow>();

        tab.isRight = isRight;
        tab.posOfEnemyY = transform.position.y;

        if (isRight)
            tab.GetComponent<RectTransform>().localPosition = new Vector3(-63f, transform.position.y, transform.position.z);
        else
            tab.GetComponent<RectTransform>().localPosition = new Vector3(63f, transform.position.y, transform.position.z);
    }

    private void Update()
    {
        X = transform.position.x;
        tab.posOfEnemyX = X;

        if (gameObject.name.Contains("School")) // компенсируем высоту
        {
            tab.posOfEnemyY = transform.position.y - 0.25f;
        }
        else
        {
            tab.posOfEnemyY = transform.position.y;
        }

        bool checkingX = (isRight) ? X < rightCam.transform.position.x : X > leftCam.transform.position.x;

        if (checkingX)
            tab.gameObject.SetActive(false);
        else
            tab.gameObject.SetActive(true);

    }


}
