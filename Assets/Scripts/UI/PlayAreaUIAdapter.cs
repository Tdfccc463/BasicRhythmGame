//将UI与实际游玩区域绑定
using UnityEngine;

public class PlayAreaUIAdapter : MonoBehaviour
{
    public RectTransform playAreaUIRoot;
    public Canvas canvas;

    void Start()
    {
        ApplyPlayArea();
        // Debug.Log(playAreaUIRoot.sizeDelta);
    }

    void ApplyPlayArea()
    {
        RectTransform canvasRt = canvas.transform as RectTransform;
        var cam = Camera.main;

        // PlayArea的角
        Vector3 bl = new Vector3(-PlayAreaController.Instance.PlayAreaWidth/2, -PlayAreaController.Instance.PlayAreaHeight/2, 0);
        Vector3 tr = new Vector3( PlayAreaController.Instance.PlayAreaWidth/2,  PlayAreaController.Instance.PlayAreaHeight/2, 0);

        // 屏幕坐标
        Vector3 blScreen = cam.WorldToScreenPoint(bl);
        Vector3 trScreen = cam.WorldToScreenPoint(tr);

        // 转 Canvas 本地坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRt, blScreen, canvas.worldCamera, out Vector2 min);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRt, trScreen, canvas.worldCamera, out Vector2 max);

        playAreaUIRoot.anchorMin = playAreaUIRoot.anchorMax = new Vector2(0.5f,0.5f);
        playAreaUIRoot.pivot = new Vector2(0.5f,0.5f);
        playAreaUIRoot.anchoredPosition = (min + max)/2;
        playAreaUIRoot.sizeDelta = max - min;
    }
}
