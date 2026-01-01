//开始页用，点击后进入游戏
using UnityEngine;
using  UnityEngine.InputSystem;

public class TapToPlay : MonoBehaviour
{
    private SceneTransitionManager sceneTransitionManager;
    void Start()
    {
        sceneTransitionManager = SceneTransitionManager.Instance;
    }
    void Update()
    {
        var touchscreen = Touchscreen.current;
        if (touchscreen == null) return;

        foreach (var touch in touchscreen.touches)
        {
            if (touch.press.wasPressedThisFrame)
            {
                sceneTransitionManager.TransitionToScene("SelectScene");
                return;
            }
        }
    }
}
