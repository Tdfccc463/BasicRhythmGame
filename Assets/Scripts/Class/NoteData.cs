//音符定义
using System;

// 键型定义
public enum NoteType { 
    Tap,
    Flick,
    Hold
}
public enum Note_Direction
{
    Down,
    Up
}

[Serializable]
public struct NoteData
{
    public NoteType type;//音符类型
    public double targetDspTime;//判定时间
    public float positionX;//判定位置:按比例在最小位置和最大位置之间插值（0，1）
    public float Note_Width_Factor;//音符的宽度系数
    public Note_Direction direction;//上落/下落
    public float Hold_duration;//Hold音符的时长
}
