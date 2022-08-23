using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogSystem : MonoBehaviour
{
    [System.Serializable]
    public struct DialogWindowInfo
    {
        public bool enabled;
        public string[] quotes;
        public GameObject dialogWindowPrefab;
        public GameObject DialogWindow { get; set; }
        public Vector2 posOfDialog;
    }
    [Header("DialogWindow Info")]
    [SerializeField] private DialogWindowInfo dialogWindowInfo;

    private void Start()
    {
        if (enabled && Random.Range(0, 6) == 0 && SceneManager.GetActiveScene().name != "Intro")
        StartCoroutine(SpawnDialogWindow());
    }

    IEnumerator SpawnDialogWindow()
    {
        yield return new WaitForSeconds(Random.Range(5f, 10f));

        AudioManager.instance.Play("BtnDialog");
        GameObject go = GameObject.FindGameObjectWithTag("dialog");
        Vector3 vec = new Vector3(transform.position.x + dialogWindowInfo.posOfDialog.x, transform.position.y + dialogWindowInfo.posOfDialog.y, transform.position.z);
        dialogWindowInfo.DialogWindow = Instantiate(dialogWindowInfo.dialogWindowPrefab, vec, Quaternion.identity, go.transform);

        SetText();

        yield return new WaitForSeconds(Random.Range(3f, 5f));

        dialogWindowInfo.DialogWindow.GetComponent<Animation>().Play("dialogWindowClose");
        yield return new WaitForSeconds(0.5f);
        DestroyDialogWindow();
    }

    private void OnDestroy()
    {
        DestroyDialogWindow();
    }

    private void FixedUpdate()
    {
        if (dialogWindowInfo.DialogWindow != null)
        {
            Vector3 vec = new Vector3(transform.position.x + dialogWindowInfo.posOfDialog.x, transform.position.y + dialogWindowInfo.posOfDialog.y, transform.position.z);
            dialogWindowInfo.DialogWindow.transform.position = vec;
        }
    }

    private void SetText()
    {
        string quote = dialogWindowInfo.quotes[Random.Range(0, dialogWindowInfo.quotes.Length)];
        dialogWindowInfo.DialogWindow.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = quote;
    }

    public void DestroyDialogWindow()
    {
        if (dialogWindowInfo.DialogWindow != null)
            Destroy(dialogWindowInfo.DialogWindow);
    }
}
