//设置页面，设置流速，名字 TODO:拆分成单独脚本
using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{
    //引用区域
    public PlayerDataManager playerDataManager;
    public Button SpeedplusButton;
    public Button SpeedreduceButton;
    public Button OffsetplusButton;
    public Button OffsetreduceButton;
    public Button ApplyButton;
    public TMP_Text speedData;
    public TMP_Text OffsetData;
    public GameObject settingInterface;
    public TMP_InputField nameInput;

    //变量区域
    public Action onNameChanged;
    string path;
    int speed;
    int offset;
    void OnEnable()
    {
        playerDataManager = PlayerDataManager.Instance;
        speed = playerDataManager.playerData.player_setting.speed;
        offset = playerDataManager.playerData.player_setting.player_offset;
        updateDisplay();
        SpeedplusButton.onClick.AddListener(onSpeedPlusButtonClick);
        SpeedreduceButton.onClick.AddListener(onSpeedReduceButtonClick);
        OffsetplusButton.onClick.AddListener(onOffsetPlusButtonClick);
        OffsetreduceButton.onClick.AddListener(onOffsetReduceButtonClick);
        ApplyButton.onClick.AddListener(onApplyButtonClick);
    }

    void onSpeedPlusButtonClick()
    {
        speed ++;
        speed = Mathf.Clamp(speed, 1, 20);
        updateDisplay();
    }
    void onSpeedReduceButtonClick()
    {
        speed --;
        speed = Mathf.Clamp(speed, 1, 20);
        updateDisplay();
    }
    void onOffsetPlusButtonClick()
    {
        offset ++;
        offset = Mathf.Clamp(offset, -20, 20);
        updateDisplay();
    }
    void onOffsetReduceButtonClick()
    {
        offset --;
        offset = Mathf.Clamp(offset, -20, 20);
        updateDisplay();
    }




    void onApplyButtonClick()//应用变更，将改动后的内容写入玩家信息并保存
    {
        if(nameInput.text != "")
        {
            playerDataManager.playerData.player_name = nameInput.text;
            nameInput.text = "";
        }
        playerDataManager.playerData.player_setting.speed = speed;
        playerDataManager.playerData.player_setting.player_offset = offset;
        path = Application.persistentDataPath + "/playerdata.json";
        string json = JsonUtility.ToJson(playerDataManager.playerData);
        File.WriteAllText(path, json);
        settingInterface.SetActive(false);
    }

    void updateDisplay()
    {
        speedData.text = speed.ToString();
        float displayOffset = offset / 10f;
        OffsetData.text = displayOffset.ToString();
    }

    void OnDisable()
    {
        SpeedplusButton.onClick.RemoveAllListeners();
        SpeedreduceButton.onClick.RemoveAllListeners();
        OffsetreduceButton.onClick.RemoveAllListeners();
        OffsetreduceButton.onClick.RemoveAllListeners();
        ApplyButton.onClick.RemoveAllListeners();
    }
}
