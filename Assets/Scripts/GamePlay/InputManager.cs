using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;


public class InputManager : MonoBehaviour
{
    private JudgementSystem judgementSystem;
    void Awake()
    {
        judgementSystem = GetComponent<JudgementSystem>();
        if (judgementSystem == null)
        {
            Debug.LogError("没有JudgementSystem", this);
            enabled = false;
        }
    }

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }
    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    void Update()
    {
         
        var activeTouches = ETouch.Touch.activeTouches;

        foreach (var touch in activeTouches)
        {
            int fingerID = touch.finger.index;//记录一次触摸对应的手指
            Vector2 position = touch.screenPosition;
            if(touch.began)
            {
                judgementSystem.ProcessTapInput(position, fingerID);
            
            }
            if(touch.isInProgress)
            {
                if(touch.delta.magnitude > 0.5f)//过滤
                {
                    judgementSystem.ProcessFlickInput(position, touch.delta);
                }  
            }
            if(touch.ended)
            {
                judgementSystem.ProcessReleaseInput(fingerID);
            }
        }
    }
}