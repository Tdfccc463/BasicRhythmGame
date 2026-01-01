//谱面定义
using System.Collections.Generic;
using System;

[Serializable]
public class SongChartData
{
    //目前没有给制谱器用的节拍转换，暂时没用上BPM
    public string songName;
    public List<NoteData> notes = new List<NoteData>();//存储谱面中Note信息
}