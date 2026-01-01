//管理玩家本地信息
using System.IO;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;
    private string path;
    public PlayerData playerData;
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        path = Application.persistentDataPath + "/playerdata.json";//持久化存储
        GetPlayData();
    }

    void GetPlayData()
    {
        if(File.Exists(path))//已存在信息，直接读取
        {
            string json = File.ReadAllText(path);
            playerData = JsonUtility.FromJson<PlayerData>(json);
        }
        else//不存在信息，生成
        {
            playerData = new PlayerData();
            playerData.player_uid = System.Guid.NewGuid().ToString();
            string json = JsonUtility.ToJson(playerData);
            File.WriteAllText(path, json);
        }
    }
}
