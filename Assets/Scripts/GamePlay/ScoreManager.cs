//分数管理，根据判定结果处理分数
using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    //引用区域
    private GameContext gameContext;
    private GameController gameController;
    //供UI变化订阅
    public event Action<long> OnScoreChanged;
    public event Action<int> OnComboChanged;
    public event Action<JudgementResult> OnJudgementIssued;

    //变量区域
    [SerializeField]private int perfectScore;
    [SerializeField]private int greatScore;
    [SerializeField]private int badScore;
    [SerializeField]private int PerfectCount = 0;
    [SerializeField]private int GreatCount = 0;
    [SerializeField]private int BadCount = 0;
    [SerializeField]private int MissCount = 0;
    [SerializeField]private long currentScore;
    [SerializeField]private int currentCombo;
    private JudgementResult lastResult;

    public long CurrentScore => currentScore;
    public int CurrentCombo => currentCombo;

    void Start()
    {
        gameContext = GameContext.Instance;
        gameController = GameController.Instance;
    }
    public void initScore(int maxCombo)
    {
        perfectScore = 1000000 / maxCombo;
        greatScore = perfectScore / 2;
        badScore = greatScore / 2;
    }

    public void RegisterJudgement(JudgementResult result)
    {
        lastResult = result;

        switch (result)
        {
            case JudgementResult.Perfect:
                currentScore += perfectScore;
                currentCombo++;
                PerfectCount++;
                break;
            case JudgementResult.Great:
                currentScore += greatScore;
                currentCombo++;
                GreatCount++;
                break;
            case JudgementResult.Bad:
                currentScore += badScore;
                currentCombo++;
                BadCount++;
                break;
            case JudgementResult.Miss:
                currentCombo = 0;
                MissCount++;
                break;
            case JudgementResult.None:
                currentCombo = 0;
                break;
        }

        OnScoreChanged?.Invoke(currentScore);
        OnComboChanged?.Invoke(currentCombo);
        OnJudgementIssued?.Invoke(lastResult);
    }

    public void WriteScore()//在游戏结束时将数据写入gameContext，用于结算页面读取
    {
        gameContext.PerfectCount = PerfectCount;
        gameContext.GreatCount = GreatCount;
        gameContext.BadCount = BadCount;
        gameContext.MissCount = MissCount;
        gameContext.Score =  currentScore;
    }
}