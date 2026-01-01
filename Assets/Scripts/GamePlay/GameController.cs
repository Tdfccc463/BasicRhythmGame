//控制游玩界面的流程/数据
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    //引用区域
    private GameContext gameContext;//取用谱面和音乐路径
    private SceneTransitionManager sceneTransitionManager;
    private NoteSpawner noteSpawner;
    private ScoreManager scoreManager;
    private PlayerDataManager playerDataManager;

    //变量区域
    private double MusicStartDSPTime = 0;//音乐开始的时间
    private double TimeOffset = 2.0;
    private AudioSource audioSource;
    private float musicLength;
    private float player_offset;
    public bool isPlay = false;
    public SongChartData currentChart;
    
    async void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        
        gameContext = GameContext.Instance;
        sceneTransitionManager = SceneTransitionManager.Instance;
        playerDataManager = PlayerDataManager.Instance;
        noteSpawner = GetComponent<NoteSpawner>();
        scoreManager = GetComponent<ScoreManager>();

        player_offset = playerDataManager.playerData.player_setting.player_offset / 10f;//整数存储，实际取用/10
        currentChart = await ChartLoader.Load(gameContext.chartPath);//加载谱面
        // Debug.Log("Count : " + currentChart.notes.Count);
        scoreManager.initScore(currentChart.notes.Count);
    }
  
    async void Start()
    {
        string MusicFullPath = Path.Combine(Application.streamingAssetsPath, gameContext.musicPath, "music.mp3");//构建完整路径
        AudioClip clip = await AudioLoader.Load(MusicFullPath);

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
        musicLength = audioSource.clip.length;
        GameStart();
    }
    void Update()
    {
        if((float)GetMusicTime() > musicLength)
        {
            isPlay = false;
            GameOver();
        }
    }
    void GameStart()//游戏开始逻辑：记录状态，音乐开始时间，初始化音符队列，播放音乐
    {
        isPlay = true;
        MusicStartDSPTime = AudioSettings.dspTime + player_offset + TimeOffset;
        noteSpawner.InitializeChartQueue(currentChart);
        audioSource.PlayScheduled(MusicStartDSPTime);
    }

    public double GetMusicTime()//其他组件由此获取音乐时间
    {
        double currentTime = AudioSettings.dspTime;
        return currentTime - MusicStartDSPTime;
    }

    void GameOver()//游戏结束逻辑：写入分数，转场
    {
        scoreManager.WriteScore();
        sceneTransitionManager.TransitionToScene("ResultScene");
    }
}
