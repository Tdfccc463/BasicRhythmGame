//根据设备的屏幕比例，计算出游玩区域的范围
using UnityEngine;

public class PlayAreaController : MonoBehaviour
{
    public static PlayAreaController Instance{get;private set;}
    private Camera cam;
    public float aspect_pad = 4f / 3f;//预设平板比例
    public float aspect_phone = 16f / 9f;//预设手机比例
    public float apply_aspect;
    public float PlayAreaHeight;
    public float PlayAreaWidth;
    public float ScreenHeight;
    public float ScreenWidth;
    public float note_unit_width;//音符的基础宽度（随比例变化）
    public float note_min_PositionX;//音符能生成的最左边位置
    public float note_max_PositionX;//音符能生成的最右边位置

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        cam = Camera.main;
        if(Mathf.Abs(cam.aspect - aspect_pad) < Mathf.Abs(cam.aspect - aspect_phone))//判断设备长宽比更接近哪种预设
        {
            apply_aspect = aspect_pad;
        }
        else
        {
            apply_aspect = aspect_phone;
        }
        PlayAreaHeight = cam.orthographicSize * 2;
        PlayAreaWidth = PlayAreaHeight * apply_aspect;
        ScreenHeight = cam.orthographicSize * 2;
        ScreenWidth = ScreenHeight * cam.aspect;

        note_unit_width = PlayAreaWidth / 10;
        note_max_PositionX = (PlayAreaWidth / 2f) - (PlayAreaWidth * 0.1f);//留出10%的边界
        note_min_PositionX = -note_max_PositionX;
    }

}
