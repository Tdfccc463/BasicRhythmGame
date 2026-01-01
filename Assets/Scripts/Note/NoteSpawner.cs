//根据SongChartData中的Note数据生成Note
using UnityEngine;
using System.Collections.Generic;


public class NoteSpawner : MonoBehaviour
{
    //引用区域
    private NoteManager noteManager;
    private JudgementSystem judgementSystem;
    private PlayerDataManager playerDataManager;
    private GameContext gameContext;
    private GameController gameController;
    private PlayAreaController playAreaController;

    //变量区域
    public float NoteDuration = 0f; //音符从生成到抵达判定线的时间（可以预留一点在画面外的时间）
    private Queue<NoteData> notesToSpawn;
    bool isStart = false;

    
    void Start()
    {
        gameContext = GameContext.Instance;
        gameController = GameController.Instance;
        playerDataManager = PlayerDataManager.Instance;
        playAreaController = PlayAreaController.Instance;//在Spawner里获取传给NoteController，防止调用生成时还没获取实例
        noteManager = GetComponent<NoteManager>();
        judgementSystem = GetComponent<JudgementSystem>();
        if (gameContext == null || gameController == null || noteManager == null || judgementSystem == null || playerDataManager == null)
        {
            Debug.LogError("引用错误", this);
            enabled = false;
            return;
        }

        NoteDuration = getNoteDuration(playerDataManager.playerData.player_setting.speed);
    }
    //加载SongChartData中的列表数据
    public void InitializeChartQueue(SongChartData songChartData)
    {
        isStart = true;
        songChartData.notes.Sort((a, b) => a.targetDspTime.CompareTo(b.targetDspTime));
        notesToSpawn = new Queue<NoteData>(songChartData.notes);
    }

    void Update()
    {
        if(!isStart) return;
        if(notesToSpawn.Count == 0) return;
        
        NoteData nextNote = notesToSpawn.Peek();//下一个Note
        double spawnDspTime = nextNote.targetDspTime - NoteDuration;//计算生成的时间（先映射到游戏时间，再计算生成点）
        if (gameController.GetMusicTime() >= spawnDspTime)//时间到达生成点
        {
            NoteData dataToSpawn = notesToSpawn.Dequeue();//出队
            SpawnNote(dataToSpawn);
        }
    }

    float getNoteDuration(int speed)
    {
        // Debug.Log("speed : " + speed);
        float t = speed / 20f;
        float duration = Mathf.Lerp(3f, 0.3f, t);//速度1-20控制显示时长3s-0.3s
        return duration;
    }

    private void SpawnNote(NoteData data)
    {
        NoteController note = noteManager.GetNote(); 
        note.Initialize(data, judgementSystem, NoteDuration,playAreaController);
    }
}