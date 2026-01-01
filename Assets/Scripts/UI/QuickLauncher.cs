//根据输入的ID加载谱面
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuickLauncher : MonoBehaviour
{
    //引用区域
    private SceneTransitionManager sceneTransitionManager;
    private GameContext gameContext;
    public TMP_InputField idInputField;
    public Button startButton;
    public ChartDatabase chartDatabase;//索引列表
    
    async void OnEnable()
    {
        sceneTransitionManager = SceneTransitionManager.Instance;
        gameContext = GameContext.Instance;
        chartDatabase = await ChartDatabaseLoader.Load();
        startButton = GetComponentInChildren<Button>();
        idInputField = GetComponentInChildren<TMP_InputField>();
        startButton.onClick.AddListener(OnStartButtonClicked);
    }
 
    void OnStartButtonClicked()
    {
        string inputId = idInputField.text.Trim();

        if (string.IsNullOrEmpty(inputId))
        {
            Debug.LogError("未输入ID");
            return;
        }
        string selectedChartPath = chartDatabase.GetChart(int.Parse(inputId)).ChartPath;//获取存放在ChartDataBase中的路径

        if (selectedChartPath!= null)
        {
            gameContext.musicPath = selectedChartPath;//将路径存入GameContext
            gameContext.chartPath = selectedChartPath;
            sceneTransitionManager.TransitionToScene("GamePlayScene");//切换场景
        }
        else
        {
            Debug.LogError("无效ID");
        }
    }
}