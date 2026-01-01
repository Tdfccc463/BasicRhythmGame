//从服务器下载数据
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DownloadData : MonoBehaviour
{   
    public PlayerDataManager playerDataManager;
    public Button DownloadButton;
    void Start()
    {
        DownloadButton.onClick.AddListener(onDownloadButtonClick);
    }

    void onDownloadButtonClick()
    {
        StartCoroutine(download());
    }

    IEnumerator download()
    {
        playerDataManager = PlayerDataManager.Instance;
        string uid = playerDataManager.playerData.player_uid;
        string url = "http://localhost:8000/get_save?player_uid=" + uid;

        UnityWebRequest req = new UnityWebRequest(url,"GET");
        req.downloadHandler = new DownloadHandlerBuffer();

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning("No cloud save");
            yield break;
        }

        string response = req.downloadHandler.text;
        PlayerData data = JsonUtility.FromJson<PlayerData>(response);
        playerDataManager.playerData = data;
        
    }
}
