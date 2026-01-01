//根据分数刷新UI
using UnityEngine;
using TMPro;
using System.Collections;

public class UIScoreUpdater : MonoBehaviour
{
    private ScoreManager scoreManager;
    public TextMeshProUGUI scoreText;//分数显示
    public TextMeshProUGUI comboText;//连击数显示
    public TextMeshProUGUI judgementText;//判定文字显示
    public float judgementDisplayDuration = 0.5f; // 判定结果显示时长 (秒)
    
    
    private Coroutine hideJudgementCoroutine;

    void Awake()
    {
        scoreManager = GetComponent<ScoreManager>();
        
        if (scoreManager == null)
        {
            Debug.LogError("引用错误", this);
            return;
        }

        scoreManager.OnScoreChanged += UpdateScoreDisplay;
        scoreManager.OnComboChanged += UpdateComboDisplay;
        scoreManager.OnJudgementIssued += UpdateJudgementDisplay;
        
        //初始化
        UpdateScoreDisplay(scoreManager.CurrentScore);
        UpdateComboDisplay(scoreManager.CurrentCombo);
        judgementText.text = "";
    }

    private void UpdateScoreDisplay(long newScore)
    {
        if (scoreText != null)
        {
            scoreText.text = newScore.ToString("D7");
        }
    }
    private void UpdateComboDisplay(int Combo)
    {
        if (comboText != null)
        {
            if (Combo > 0)
            {
                comboText.text = Combo.ToString();
            }
            else
            {
                comboText.text = "";
            }
        }
    }

    private void UpdateJudgementDisplay(JudgementResult result)
    {
        if (judgementText == null) return;
        
        //内容与颜色
        string text = result.ToString().ToUpper();
        Color color = GetColorFromResult(result);
        
        judgementText.text = text;
        judgementText.color = color;
        
        //中断还没结束的显示
        if (hideJudgementCoroutine != null)
        {
            StopCoroutine(hideJudgementCoroutine);
        }
        
        hideJudgementCoroutine = StartCoroutine(HideJudgementAfterDelay());
    }
    
    private Color GetColorFromResult(JudgementResult result)//给判定文字附加颜色
    {
        switch (result)
        {
            case JudgementResult.Perfect: return Color.yellow;
            case JudgementResult.Great: return Color.green;
            case JudgementResult.Bad: return Color.red;
            case JudgementResult.Miss: return Color.gray;
            default: return Color.white;
        }
    }

    //判定文字显示时长
    private IEnumerator HideJudgementAfterDelay()
    {
        yield return new WaitForSeconds(judgementDisplayDuration);
        judgementText.text = "";
    }
    
    private void OnDestroy()
    {
        if (scoreManager != null)
        {
            scoreManager.OnScoreChanged -= UpdateScoreDisplay;
            scoreManager.OnComboChanged -= UpdateComboDisplay;
            scoreManager.OnJudgementIssued -= UpdateJudgementDisplay;
        }
    }
}