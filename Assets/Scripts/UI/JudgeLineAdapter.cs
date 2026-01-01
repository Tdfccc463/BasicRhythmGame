//设定判定线长度与游玩区域宽度一致
using UnityEngine;

public class JudgeLineAdapter : MonoBehaviour
{
    private PlayAreaController playAreaController;
    public SpriteRenderer SR;
    void Start()
    {
        playAreaController = PlayAreaController.Instance;
        float LineLength = SR.sprite.bounds.size.x;
        float TargetLength = playAreaController.PlayAreaWidth;
        float sacleX = TargetLength / LineLength;
        SR.transform.localScale = new Vector3(sacleX, 0.1f, 1);
    }   
}
