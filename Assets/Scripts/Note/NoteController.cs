//负责控制音符自身：移动，判定，回收
//最终更新后检查：√
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    //引用区域
    private NoteManager manager;
    private JudgementSystem judgementSystem;
    private GameController gameController;
    private NoteTypeController noteTypeController;
    public DebugData Config;


    //变量区域
    //Note相关信息（实际使用）
    //生成位置
    [SerializeField]private float spawnXPosition = 0;
    [SerializeField]private float spawnZPosition = 0;
    [SerializeField]private float spawnYPosition = 0;
    //逻辑判定位置
    [SerializeField]private float hitXPosition = 0;
    [SerializeField]private float hitZPosition = 0;
    [SerializeField]private float hitYPosition = 0;
    private bool isRegistered = false; //记录音符是否进入过判定系统
    private bool initialized = false; //是否初始化
    public bool isholding = false; //Hold用：是否正在判定中
    private float NoteDuration; //音符在屏幕上显示的时间
    private SpriteRenderer SR;
    private SpriteRenderer holdbodySR;//用于设置hold的长度
    private Color SR_color;//用于击打效果
    private float holdFullLength;
    private int holdingFingerID = -1;
    public NoteData data;
    void Start()
    {
        gameController = GameController.Instance;
        manager = GetComponentInParent<NoteManager>();
    }
    public void Initialize(NoteData data, JudgementSystem system, float noteDuration ,PlayAreaController playAreaController)
    {
        isRegistered = false;
        judgementSystem = system;
        NoteDuration = noteDuration;
        this.data = data;
        spawnXPosition = Mathf.Lerp(playAreaController.note_min_PositionX, playAreaController.note_max_PositionX,data.positionX);
        if(data.direction == Note_Direction.Up)
        {
            spawnYPosition = -Config.SpawnY;
        }
        else
        {
            spawnYPosition = Config.SpawnY;
        }
        gameObject.SetActive(true);
        spawnZPosition = Config.SpawnZ;
        hitXPosition = spawnXPosition;
        hitYPosition = Config.TargetY;
        hitZPosition = Config.TargetZ;
        transform.position = new Vector3(spawnXPosition, spawnYPosition, spawnZPosition);//初始位置
        if(this.data.type == NoteType.Tap)
        {
            SR = transform.Find("Tap").GetComponent<SpriteRenderer>();
        }
        else if(this.data.type == NoteType.Flick)
        {
            SR = transform.Find("Flick").GetComponent<SpriteRenderer>();
        }
        else if(this.data.type == NoteType.Hold)
        {
            Transform holdRoot = transform.Find("Hold");
            SR = holdRoot.Find("HoldHead").GetComponent<SpriteRenderer>(); //判定头引用
            holdbodySR = holdRoot.Find("HoldBody").GetComponent<SpriteRenderer>();//判定身体引用
        }
        SR_color = SR.color;
        SetNoteSize(playAreaController, this.data.Note_Width_Factor);//设置宽度
        if(this.data.type == NoteType.Hold)
        {
            // Debug.Log("Hold");
            SetHoldBodySize(playAreaController, this.data.Note_Width_Factor);//设置Hold的长度
            holdFullLength = holdbodySR.transform.localScale.y;
        }
        //控制外观
        noteTypeController = GetComponent<NoteTypeController>();//在父物体激活后获取
        noteTypeController.Init(data);
        initialized = true;
    }

    void SetNoteSize(PlayAreaController playAreaController, float factor)
    { 
        float note_width = SR.sprite.bounds.size.x;
        float target_width = playAreaController.note_unit_width * factor;
        float scaleX = target_width / note_width;
        float scaleY = 0.2f;
        SR.transform.localScale = new Vector3(scaleX, scaleY, 1);
    }

    void SetHoldBodySize(PlayAreaController playAreaController, float factor)
    {
        //宽度设置同其他note
        float note_Width = holdbodySR.sprite.bounds.size.x;
        float target_Width = playAreaController.note_unit_width * factor;
        float scaleX = target_Width / note_Width;

        //hold长度/移动距离 = hold时长 / 总时长
        float totalDistance = Mathf.Abs(Config.SpawnY - Config.TargetY);
        float target_length = totalDistance / NoteDuration * data.Hold_duration;
    
        float hold_length = holdbodySR.sprite.bounds.size.y;
        float scaleY = target_length / hold_length;

        holdbodySR.transform.localScale = new Vector3(scaleX, scaleY, 1);

        //下落：不变 上落：旋转180度
        if (data.direction == Note_Direction.Up)
        {
            holdbodySR.transform.localRotation = Quaternion.Euler(0, 0, 180);
        }
        else
        {
            holdbodySR.transform.localRotation = Quaternion.identity;
        }
    }


    public Rect GetScreenRect(Camera cam)//获取Note范围供判定系统调用
    {
        Bounds bounds = SR.bounds;

        Vector3 min = cam.WorldToScreenPoint(bounds.min);
        Vector3 max = cam.WorldToScreenPoint(bounds.max);

        return Rect.MinMaxRect(Mathf.Min(min.x, max.x), Mathf.Min(min.y, max.y), Mathf.Max(min.x, max.x), Mathf.Max(min.y, max.y));
    }

    public void PlayHit()//击打效果
    {
        SR.color = Color.white;
        StartCoroutine(RestoreColor(0.05f));
    }
    private IEnumerator RestoreColor(float duration)
    {   
        yield return new WaitForSeconds(duration);
        SR.color = SR_color;
    }

    void Update()
    {
        if(!initialized) return;//保证已经完成初始
        Vector3 spawnPosition = new Vector3(spawnXPosition, spawnYPosition, spawnZPosition);
        Vector3 endPosition = new Vector3(hitXPosition, hitYPosition, hitZPosition);
        double currentTime = gameController.GetMusicTime();
        double timeRemaining = data.targetDspTime - currentTime;

        if (data.type == NoteType.Hold && timeRemaining <= 0)//hold头已经到达判定时间
        {
            float holdProgress = Mathf.Clamp01((float)((currentTime - data.targetDspTime) / data.Hold_duration));
            UpdateBodyClips(holdProgress);//逐渐缩短
        }
        if(isholding)//hold正在判定
        {
            transform.position = endPosition;//保持头部不动
            if (currentTime >= data.targetDspTime + data.Hold_duration)
            {
                CompleteHold();
            }
        }
        else//没有击中hold/是其他类型音符
        {
            float progress = Mathf.Clamp01(1 - (float)(timeRemaining / NoteDuration));
        
            Vector3 currentPos = Vector3.LerpUnclamped(spawnPosition, endPosition, (float)progress);
            transform.position = currentPos;

            float activationWindow = judgementSystem.badWindow * 2;
        
            if (!isRegistered && Mathf.Abs((float)timeRemaining) <= activationWindow)//可以进行判定的范围
            {
                judgementSystem.RegisterNote(this);
                isRegistered = true;
            }  
            
            double EndTime = data.targetDspTime + data.Hold_duration;//如果是hold会加上时长

            if (currentTime  > EndTime + judgementSystem.badWindow)//错过了最后的判定范围
            {
                HandleMiss();
            }
        }

    }

    private void UpdateBodyClips(float holdProgress)
    {
        float holdremaining = 1f - holdProgress;
        holdbodySR.transform.localScale = new Vector3(holdbodySR.transform.localScale.x, holdFullLength * holdremaining,1);
    }
    private void CompleteHold()
    {
        isholding = false;
        judgementSystem.UnregisterNote(this);
        PlayHit();
        var scoreManager = judgementSystem.GetComponent<ScoreManager>();
        if (scoreManager != null) scoreManager.RegisterJudgement(JudgementResult.Perfect);
    
        manager.ReturnNote(this);
    }

    private void HandleMiss()
    {
        if (isRegistered)//注销
        {
            judgementSystem.UnregisterNote(this);
        }
        var scoreManager = judgementSystem.GetComponent<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.RegisterJudgement(JudgementResult.Miss);//分数系统处理
        }

        manager.ReturnNote(this);//回收
    }

    public void BeginHold(int fingerID)
    {
        isholding = true;
        holdingFingerID = fingerID;
    }

    public void BreakHold()
    {
        if (isholding)
        {
            isholding = false;
            holdingFingerID = -1;
            HandleMiss(); 
        }
    }

    public bool IsHoldingByFinger(int fingerID)//获取触摸对应的hold
    {
        return isholding && holdingFingerID == fingerID;
    }
}