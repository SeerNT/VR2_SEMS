using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class MachineSettingsJSONManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI maxRotText;
    [SerializeField] TextMeshProUGUI modeText;
    [SerializeField] string url;
    private Jsonclass jsnData;

    void Start()
    {
        StartCoroutine(getData());
    }

    IEnumerator getData()
    {
        var uwr = new UnityWebRequest(url);
        uwr.method = UnityWebRequest.kHttpVerbGET;
        var resultFile = Path.Combine(Application.persistentDataPath, "result.json");
        var dh = new DownloadHandlerFile(resultFile);
        dh.removeFileOnAbort = true;
        uwr.downloadHandler = dh;
        yield return uwr.SendWebRequest();

        if (uwr.result != UnityWebRequest.Result.Success)
        {
            maxRotText.text = "ERROR";
            modeText.text = "ERROR";
        }
        else
        {
            jsnData = JsonUtility.FromJson<Jsonclass>(File.ReadAllText(Application.persistentDataPath + "/result.json"));
            maxRotText.text = "MAX ROTATION:\n" + jsnData.HANDLE_MAX_ROTATION;
            modeText.text = "OPERATING MODE:\n" + jsnData.OPERATING_MODE;
            yield return StartCoroutine(getData());
        }
    }

    [System.Serializable]
    public class Jsonclass
    {
        public string HANDLE_MAX_ROTATION;
        public string OPERATING_MODE;
    }
}
