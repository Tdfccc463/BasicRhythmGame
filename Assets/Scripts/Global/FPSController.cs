using UnityEngine;

public class FPSController : MonoBehaviour
{
    private static FPSController Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        SetFPS();
    }
    private void SetFPS()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
    }
}