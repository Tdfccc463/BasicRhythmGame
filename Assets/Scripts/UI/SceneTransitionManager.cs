//在切换场景的时候先变暗再变亮
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;
    [SerializeField] private CanvasGroup faderCanvasGroup;//遮罩
    [SerializeField] private float fadeDuration = 0.8f;//淡入淡出时长
    [SerializeField] private float minWaitTime = 0.5f;//最短黑屏时间
    private bool isTransitioning = false;

    private void Awake()
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

        if (faderCanvasGroup == null)
        {
            Debug.LogError("引用错误",this);
            enabled = false;
            return;
        }

        faderCanvasGroup.alpha = 1f;
        faderCanvasGroup.blocksRaycasts = true;
    }

    private void Start()
    {
        StartCoroutine(Fade(0f));//开始游戏时由暗到亮
    }
    public void TransitionToScene(string sceneName)
    {
        if (isTransitioning) return;
        StartCoroutine(ExecuteTransition(sceneName));
    }

    private IEnumerator ExecuteTransition(string sceneName)
    {
        isTransitioning = true;

        yield return StartCoroutine(Fade(1f));//渐入

        float startTime = Time.realtimeSinceStartup;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);//AsyncOperation获取异步操作的各种信息
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f || (Time.realtimeSinceStartup - startTime) < minWaitTime)
        {
            yield return null;
        }

        asyncLoad.allowSceneActivation = true;
        yield return null; // 等一帧，确保新场景完成Awake

        yield return StartCoroutine(Fade(0f));//渐出

        isTransitioning = false;
    }

    private IEnumerator Fade(float targetAlpha)//将透明度调整到指定值
    {
        faderCanvasGroup.blocksRaycasts = true;

        float startAlpha = faderCanvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / fadeDuration;
            faderCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            yield return null;
        }

        faderCanvasGroup.alpha = targetAlpha;//到达目标透明度

        if (targetAlpha <= 0f)
        {
            faderCanvasGroup.blocksRaycasts = false;//完全亮起时候启用输入
        }
    }
}
