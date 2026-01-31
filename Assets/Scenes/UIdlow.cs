using UnityEngine;

public class UI_Flow : MonoBehaviour
{
    [Header("移动幅度")]
    public float moveAmount = 15f; // UI的单位通常很大，这里建议填 10~50 试试

    [Header("平滑程度")]
    public float smoothTime = 5f;

    private Vector3 startPos;

    void Start()
    {
        // 记住 UI 放在画布上的初始坐标
        startPos = transform.localPosition;
    }

    void Update()
    {
        // 1. 获取鼠标偏移 (-0.5 ~ 0.5)
        float x = (Input.mousePosition.x / Screen.width) - 0.5f;
        float y = (Input.mousePosition.y / Screen.height) - 0.5f;

        // 2. 计算目标位置
        // 注意：这里我们用 startPos 减去 offset (反向移动)，这样会有"背景在后面"的深邃感
        // 如果想跟随鼠标，就把 - 改成 +
        Vector3 offset = new Vector3(x * moveAmount, y * moveAmount, 0);
        Vector3 targetPos = startPos - offset;

        // 3. 移动 UI
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * smoothTime);
    }
}
