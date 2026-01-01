//上传本地数据到服务器
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class UploadData : MonoBehaviour
{
    public PlayerDataManager playerDataManager;
    public Button UploadButton;
    void Start()
    {
        UploadButton.onClick.AddListener(onUploadButtonClick);
    }

    void onUploadButtonClick()
    {
        StartCoroutine(upload());
    }

    IEnumerator upload()
    {
        playerDataManager = PlayerDataManager.Instance;
        string url = "http://localhost:8000/upload_save";
        string json = JsonUtility.ToJson(playerDataManager.playerData);
        byte[] body = Encoding.UTF8.GetBytes(json);

        UnityWebRequest req = new UnityWebRequest(url,"POST");//TODO:替换成UnityWebRequest.POST的用法
        req.uploadHandler = new UploadHandlerRaw(body);
        yield return req.SendWebRequest();
        
        if (req.result != UnityWebRequest.Result.Success) Debug.LogError(req.error);
    }
}
