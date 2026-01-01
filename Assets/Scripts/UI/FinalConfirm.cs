//最终确认按钮，返回选歌页
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinalConfirm : MonoBehaviour
{

    [SerializeField] private Button ConfirmButton;
    SceneTransitionManager sceneTransitionManager;
    void Start()
    {
        sceneTransitionManager = SceneTransitionManager.Instance;
        ConfirmButton.onClick.AddListener(TurnSelect);
    }

    void TurnSelect()
    {
        sceneTransitionManager.TransitionToScene("SelectScene");
    }
}
