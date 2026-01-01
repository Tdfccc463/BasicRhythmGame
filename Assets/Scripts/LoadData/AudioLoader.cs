//读取music.mp3
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

public static class AudioLoader
{
    public static async Task<AudioClip> Load(string fullPath)
    {
        // Debug.Log("Loading : " + fullPath);
        using var req = UnityWebRequestMultimedia.GetAudioClip(fullPath, AudioType.UNKNOWN);
        
        var operation = req.SendWebRequest();

        while (!operation.isDone) await Task.Yield();  // 等待下一帧再继续循环

        // 检查结果
        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("音乐加载失败 " + req.error);
            return null;
        }

        return DownloadHandlerAudioClip.GetContent(req);
    }
}
