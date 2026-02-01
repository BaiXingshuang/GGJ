using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class AutoUp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("位置设置")]
    public float UpY = -330f;    // 弹出的目标 Y 轴位置
    public float DownY = -780f;  // 收回的目标 Y 轴位置

    [Header("动画设置")]
    [Tooltip("动画完成所需时间（秒）")]
    public float duration = 0.5f;


    [Tooltip("弹性曲线：请在 Inspector 中将曲线尾部拉高超过 1 来实现 Q 弹")]
    public AnimationCurve bounceCurve = new AnimationCurve(
        new Keyframe(0, 0),
        new Keyframe(0.7f, 1.1f), // 关键点：这里超过1，产生“冲过头”的效果
        new Keyframe(1, 1)        // 最后回到1
    );

    private RectTransform rectTransform;
    private Coroutine currentCoroutine;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        // 初始化位置到 DownY (或者保持原位，看需求)
        SetPosY(DownY);
    }

    // 鼠标进入：向上弹出
    public void OnPointerEnter(PointerEventData eventData)
    {
        MoveTo(UpY);
    }

    // 鼠标移出：向下收回
    public void OnPointerExit(PointerEventData eventData)
    {
        MoveTo(DownY);
    }

    // 核心移动逻辑
    private void MoveTo(float targetY)
    {
        // 如果有正在进行的动画，先停止，防止冲突
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(AnimateMove(targetY));
    }

    IEnumerator AnimateMove(float targetY)
    {
        float startY = rectTransform.anchoredPosition.y;
        float timePassed = 0f;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            // 计算当前进度 (0 到 1)
            float percent = timePassed / duration;

            // 核心：使用曲线计算“Q弹”的进度值
            // 如果曲线中间有大于1的值，这里就会算出一个大于1的 curvePercent
            float curvePercent = bounceCurve.Evaluate(percent);

            // LerpUnclamped 允许插值超过范围，从而实现回弹
            float newY = Mathf.LerpUnclamped(startY, targetY, curvePercent);

            SetPosY(newY);
            yield return null;
        }

        // 确保最后严丝合缝地停在目标位置
        SetPosY(targetY);
        currentCoroutine = null;
    }

    // 辅助方法：设置 Y 轴位置
    private void SetPosY(float y)
    {
        Vector2 pos = rectTransform.anchoredPosition;
        pos.y = y;
        rectTransform.anchoredPosition = pos;
    }

    // 编辑器辅助：脚本挂上去时自动生成一个默认的 Q 弹曲线
    void Reset()
    {
        bounceCurve = new AnimationCurve(
            new Keyframe(0, 0, 0, 2),       // 开始
            new Keyframe(0.6f, 1.1f, 0, 0), // 冲过头 (Q弹点)
            new Keyframe(0.85f, 0.95f, 0, 0),// 回弹一点
            new Keyframe(1, 1, 0, 0)        // 归位
        );
    }
}
