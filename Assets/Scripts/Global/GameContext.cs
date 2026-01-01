//存储需要多次取用的的游戏局内数据
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameContext : MonoBehaviour
{
    public static GameContext Instance { get; private set; }
    public string chartPath; 
    public string musicPath;
    public string id;
    
    public int PerfectCount;
    public int GreatCount;
    public int BadCount;
    public int MissCount;
    public long Score;
    void Awake()
    {
        if (Instance == null) 
        { 
            Instance = this; 
            DontDestroyOnLoad(gameObject); 
        }
        else
        { 
            Destroy(gameObject); 
        }
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "SelectScene")
        {
            Init();
        }
    }
    void Init()
    {
        chartPath = null;
        musicPath = null;
        PerfectCount = 0;
        GreatCount = 0;
        BadCount = 0;
        MissCount = 0;
        Score = 0;
    }
}