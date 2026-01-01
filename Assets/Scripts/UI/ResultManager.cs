//将从GameContext获得的结算信息显示出来
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    //引用区域
    private GameContext gameContext;
    private GameController gameController;
    [SerializeField]private TMP_Text nameDisplay;
    [SerializeField]private TMP_Text scoreDisplay;
    [SerializeField]private TMP_Text perfectDisplay;
    [SerializeField]private TMP_Text greatDisplay;
    [SerializeField]private TMP_Text badDisplay;
    [SerializeField]private TMP_Text missDisplay;
    [SerializeField]private LayoutGroup DataGroup;
    //变量区域
    private string SongName;
    private int perfect;
    private int great;
    private int bad;
    private int miss;
    private long score;
   

    void Awake()
    {
        gameContext = GameContext.Instance;
        gameController = GameController.Instance;
    }

    void Start()
    {
        SongName = gameController.currentChart.songName;
        perfect = gameContext.PerfectCount;
        great = gameContext.GreatCount;
        bad = gameContext.BadCount;
        miss = gameContext.MissCount;
        score = gameContext.Score;

        nameDisplay.text = SongName;
        scoreDisplay.text = score.ToString();
        perfectDisplay.text = perfect.ToString();
        greatDisplay.text = great.ToString();
        badDisplay.text = bad.ToString();
        missDisplay.text = miss.ToString();
        LayoutRebuilder.ForceRebuildLayoutImmediate(DataGroup.GetComponent<RectTransform>());//防止更新完数据显示之后LayoutGroup错误
    }

}
