//判定系统
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;

//判定结果
public enum JudgementResult { 
    None, 
    Miss, 
    Bad, 
    Great, 
    Perfect 
}

public class JudgementSystem : MonoBehaviour
{
    //配置区域
    public DebugData Config;//配置文件
    //判定窗口
    public float perfectWindow = 0f; 
    public float greatWindow = 0f;    
    public float badWindow = 0f;     
    public float flickThreshold = 0f;//触发Flick需要的位移

    //引用区域
    private Camera mainCamera;
    private NoteManager noteManager;//在此处处理回收逻辑
    private GameController gameController;//提供时间
    private ScoreManager scoreManager;//处理判定对应的分数逻辑

    private List<NoteController> activeNotes = new List<NoteController>();//所有进入判定窗口的Note

    void Start()
    {
        mainCamera = Camera.main;
        gameController = GameController.Instance;
        noteManager = GetComponent<NoteManager>();
        scoreManager = GetComponent<ScoreManager>();
        if (noteManager == null || scoreManager == null || gameController == null)
        {
             Debug.LogError("引用错误", this);
             enabled = false;
        }
        InitWindow();
    }
    void InitWindow()
    {
        perfectWindow = Config.perfectWindow; 
        greatWindow = Config.greatWindow;    
        badWindow = Config.badWindow;     
        flickThreshold = Config.flickThreshold;
    }
   
    
    public void ProcessTapInput(Vector2 screenPosition, int fingerID)
    {
        NoteController targetNote = FindClosestTargetNote(screenPosition);
        if (targetNote != null)
        {
            if(targetNote.data.type == NoteType.Tap)
            {
                CheckTimeAndHandleJudgement(targetNote);
            }
            else if(targetNote.data.type == NoteType.Hold)
            {
                HandleHoldStart(targetNote, fingerID);
            }
        }
    }
    public void ProcessFlickInput(Vector2 screenPosition, Vector2 deltaPosition)
    {
        NoteController targetNote = FindClosestTargetNote(screenPosition);
        if (targetNote != null && targetNote.data.type == NoteType.Flick)
        {
            if (deltaPosition.magnitude > flickThreshold)//检查滑动幅度
            {
                CheckTimeAndHandleJudgement(targetNote);
            }
        }
    }

    private NoteController FindClosestTargetNote(Vector2 screenPosition)//查找最近的对应类型的note
    {
        // Debug.Log("Active Notes: " + activeNotes.Count);
        double currentDspTime = gameController.GetMusicTime();

        var potentialTargets = activeNotes
            .Where(n => !(n.data.type == NoteType.Hold && n.isholding)) 
            .Select(n => 
            {    
                Rect screenRect = n.GetScreenRect(mainCamera);
                bool inRangeX = screenPosition.x >= screenRect.xMin && screenPosition.x <= screenRect.xMax;
                float timeDelta = Mathf.Abs((float)(n.data.targetDspTime - currentDspTime));
                return new 
                {
                    Note = n, 
                    InRangeX = inRangeX,
                    TimeDelta = timeDelta
                };
            })
            .Where(n => n.InRangeX)//在Note内
            .Where(n => n.TimeDelta <= badWindow)//在判定窗口内
            .OrderBy(n => n.TimeDelta) //优先选择时间差最小的
            .ToList();
        return potentialTargets.FirstOrDefault()?.Note;//返回第一个Note
    }
    
    private void HandleHoldStart(NoteController note, int fingerID)
    {
        double currentTime = gameController.GetMusicTime();
        double timeDelta = Math.Abs(note.data.targetDspTime - currentTime);
    
        JudgementResult result = CalculateJudgement(timeDelta);

        if (result != JudgementResult.None && result != JudgementResult.Miss)
        {
            note.BeginHold(fingerID);
            note.PlayHit();
            scoreManager.RegisterJudgement(result);
        }
    
    }
    public void ProcessReleaseInput(int fingerID)
    {
        // 在判定区域中找正在被这根手指按住的Hold
        foreach (var note in activeNotes.ToList())
        {
            if (note.data.type == NoteType.Hold && note.IsHoldingByFinger(fingerID))
            {
                note.BreakHold();//没有按完就松手
            }
        }
    }
 
    private void CheckTimeAndHandleJudgement(NoteController note)
    {    
        double currentTime = gameController.GetMusicTime();//当前时间
        double timeDelta = Math.Abs(note.data.targetDspTime - currentTime); //时间差
        
        JudgementResult result = CalculateJudgement(timeDelta);//得到判定结果

        if (result != JudgementResult.None)//有判定
        {
            //注销并回收
            UnregisterNote(note); 

            note.PlayHit();
            scoreManager.RegisterJudgement(result);//更新分数和连击
            StartCoroutine(ReturnNoteLater(note));//给击打效果留时间
            
        }
        else//无判定
        {
             UnregisterNote(note);
             noteManager.ReturnNote(note); 
             scoreManager.RegisterJudgement(JudgementResult.Miss);
        }
    }

    IEnumerator ReturnNoteLater(NoteController note)
    {
        yield return new WaitForSeconds(0.05f);
        noteManager.ReturnNote(note);
    }

    private JudgementResult CalculateJudgement(double delta)//根据时间差判断处于哪个判定时间窗口
    {
        if (delta <= perfectWindow) return JudgementResult.Perfect;
        if (delta <= greatWindow) return JudgementResult.Great;
        if (delta <= badWindow) return JudgementResult.Bad;
        return JudgementResult.None; 
    }
     public void RegisterNote(NoteController note)
    {
        if (!activeNotes.Contains(note))
        {
            activeNotes.Add(note);
        }
    }

    public void UnregisterNote(NoteController note)
    {
        activeNotes.Remove(note);
    }
}