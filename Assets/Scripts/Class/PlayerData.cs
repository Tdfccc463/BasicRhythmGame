//玩家信息
using System;

[Serializable]
public class PlayerData
{
    public string player_name = "player_0";
    public string player_uid;
    public Setting player_setting = new Setting();
}

[Serializable]
public class Setting
{
    public int speed = 5;
    public int player_offset = 0;//在使用时要/10
}