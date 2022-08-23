using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsOfParameters : MonoBehaviour
{ 
    private enum Parameters
    {
        HEALTH = 0,
        SPEED = 1,
        RELOADTIME = 2,
        BLASTRADIUS = 3,
        All = 4,
    }
    [SerializeField] private Parameters parameter;

    [SerializeField] private Image fillSprite;

    public float MAXVALUE;
    public float INCREASEVALUE;

    private void getParameterInfo(out string nameParam, out float initialValueParam)
    {
        GameObject go = GameObject.FindGameObjectWithTag("BusManager");
        int index = UpgradeButton.currentBusIndex;

        BusController busController = go.GetComponent<BusManager>().getBusByIndex(index).GetComponentInChildren<BusController>();
        if (busController == null) Debug.LogError("ERROR! BUSCONTROLLER WAS NOT FOUND!");

        switch (parameter)
        {
            case Parameters.HEALTH:
                nameParam = UpgradeButton.currentBusIndex.ToString() + "HEALTH";
                initialValueParam = busController.MinHealth;
                break;

            case Parameters.SPEED:
                nameParam = UpgradeButton.currentBusIndex.ToString() + "SPEED";
                initialValueParam = busController.MinSpeed;
                break;

            case Parameters.RELOADTIME:
                nameParam = UpgradeButton.currentBusIndex.ToString() + "RELOADTIME";
                initialValueParam = busController.MinShootDelay;
                break;

            case Parameters.BLASTRADIUS:
                nameParam = UpgradeButton.currentBusIndex.ToString() + "BLASTRADIUS";
                initialValueParam = busController.MinBlastRadius;
                break;

            default:
                nameParam = "";
                initialValueParam = 0;
                Debug.LogError("DOES NOT ALLOW DEFAULT!");
                break;
        }
    }

    public float InitialValueParam;
    private string NameParam;

    public float currentValue;

    private void Awake()
    {
        
    }

    private void Update()
    {
        getParameterInfo(out NameParam, out InitialValueParam);

        currentValue = InitialValueParam + (PlayerPrefs.GetInt(NameParam) * INCREASEVALUE);

        //localMin / 5 + 25 * x = localMax   =>   x = localMax - (localMin / 5) / 25
        // чем больше 5 тем больше x

        //const float k = 2;

        //float localMax = InitialValueParam + (25 * INCREASEVALUE);
        //float localMin = InitialValueParam;

        //float x = (localMax - (localMin / k)) / 25;
        //float valueToShow = (localMin / k) + (PlayerPrefs.GetInt(NameParam) * x);

        fillSprite.fillAmount = currentValue / MAXVALUE;
    }

    public float getInitialValueParam()
    {
        return InitialValueParam;
    }
    public string getNameParam()
    {
        return NameParam;
    }
}
