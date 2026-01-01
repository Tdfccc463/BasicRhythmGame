//读取chart.json
using UnityEngine;
using System.IO;
using System.Threading.Tasks;
using UnityEngine.Networking;

public static class ChartLoader
{
    public static async Task<SongChartData> Load(string chartPath)
    {
        string folder = Path.Combine(Application.streamingAssetsPath, chartPath);
        string chartFile = Path.Combine(folder, "chart.json");

        using var req = UnityWebRequest.Get(chartFile);
        var operation = req.SendWebRequest();

        while (!operation.isDone) await Task.Yield();

        string json = req.downloadHandler.text;
        SongChartData chartData = JsonUtility.FromJson<SongChartData>(json);

        return chartData;
    }
}
