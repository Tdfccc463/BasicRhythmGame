//用于调试数值的配置文件
using UnityEngine;

[CreateAssetMenu(fileName = "Debug")]
public class DebugData : ScriptableObject
{
    public float perfectWindow = 0.04f; 
    public float greatWindow = 0.08f;    
    public float badWindow = 0.15f;     
    public float flickThreshold = 60f;
    public float SpawnY = 6;
    public float SpawnZ = 1;
    public float TargetY = 0;
    public float TargetZ = 1;

}
