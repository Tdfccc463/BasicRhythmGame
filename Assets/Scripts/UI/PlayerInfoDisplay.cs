//显示玩家信息
using TMPro;
using UnityEngine;

public class PlayerInfoDisplay : MonoBehaviour
{
    private PlayerDataManager playerDataManager;
    public TMP_Text playerName;
    public TMP_Text playerUID;
    void Start()
    {
        playerDataManager = PlayerDataManager.Instance;
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        playerName.text = playerDataManager.playerData.player_name;
        playerUID.text = playerDataManager.playerData.player_uid;//TODO:也许可以隐藏
    }

    void Update()//检查名字有没有变化 TODO:也许可以在发生变化的地方调用
    {
        if(playerName.text != playerDataManager.playerData.player_name)
        {
            UpdateDisplay();
        }
    }
}
