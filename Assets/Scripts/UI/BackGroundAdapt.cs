//根据游玩区域，设置背景和遮挡
using UnityEngine;

public class BackGroundAdapt : MonoBehaviour
{
    [SerializeField]private SpriteRenderer Background;
    
    PlayAreaController playAreaController;
    public GameObject maskL;
    public GameObject maskR;
    void Start()
    {
        playAreaController = PlayAreaController.Instance;
        UpdateBackGround();
        UpdateMask();
    }

    void UpdateBackGround()
    {
        float Background_height = Background.sprite.bounds.size.y;
        float Background_width = Background.sprite.bounds.size.x;
        float scaleY = playAreaController.ScreenHeight / Background_height;
        float scaleX = playAreaController.ScreenWidth / Background_width;
        Background.transform.localScale = new Vector3(scaleX, scaleY, 1);
    }
    void UpdateMask()
    {
        float Target_height = playAreaController.PlayAreaHeight;
        float Target_width = (playAreaController.ScreenWidth - playAreaController.PlayAreaWidth) / 2;//屏幕宽度减去中间游玩区域宽度 / 2

        float Mask_height = Background.sprite.bounds.size.y;
        float Mask_width = Background.sprite.bounds.size.x;

        float scaleY = Target_height / Mask_height;
        float scaleX = Target_width / Mask_width;

        maskL.transform.localScale = new Vector3(scaleX,scaleY,1);
        maskR.transform.localScale = new Vector3(scaleX,scaleY,1);

        float TargetPozition = (playAreaController.PlayAreaWidth / 2) + (Target_width / 2);//屏幕正中左移半个游玩区域半个遮罩区域到达一边遮罩的中点
        
        maskL.transform.position = new Vector3(-TargetPozition, 0, 1);
        maskR.transform.position = new Vector3(TargetPozition, 0, 1);
    }
}
