//读取ChartDataBase.txt
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class ChartDatabaseLoader
{
    public static async Task<ChartDatabase> Load()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "chart_database.txt");
        using var req = UnityWebRequest.Get(path);
        var operation = req.SendWebRequest();

        while(!operation.isDone) await Task.Yield();
        using var reader = new StringReader(req.downloadHandler.text);
        
        var list = new List<ChartDatabase.Chart>();
        string line;

        while ((line = reader.ReadLine()) != null)
        {
            var cols = line.Trim().Split(',');//去掉空字符，逗号分隔
            list.Add(new ChartDatabase.Chart{ChartId = int.Parse(cols[0]),ChartPath = cols[1]});
        }

        return new ChartDatabase
        {
            Charts = list.ToArray()
        };
    }
}