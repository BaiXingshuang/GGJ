using UnityEngine;

public class UI_Flow : MonoBehaviour
{
    [Header("移动幅度")]
    public float moveAmount = 15f; // UI的单位通常很大，这里建议填 10~50 试试

    [Header("平滑程度")]
    public float smoothTime = 5f;

    private Vector3 startPos;
    [Header("呼吸设置")]
    [Tooltip("呼吸的频率（速度），数值越大呼吸越快")]
    [Range(0.1f, 10f)]
    public float breathSpeed = 2.0f; // 呼吸速度

    [Tooltip("呼吸的幅度（强度），数值越大缩放越明显")]
    [Range(0.01f, 0.5f)]
    public float breathRange = 0.05f; // 呼吸幅度（例如 0.05 代表在 95% ~ 105% 之间波动）

    private RectTransform _rectTransform;
    private Vector3 _originalScale; // 记录初始大小
    void Start()
    {
        // 记住 UI 放在画布上的初始坐标
        startPos = transform.localPosition;
    }
    void Awake()
    {
        // 获取 UI 的 RectTransform 组件
        _rectTransform = GetComponent<RectTransform>();

        // 记录物体刚开始的缩放值，这样不会因为呼吸而导致物体越来越大或变样
        if (_rectTransform != null)
        {
            _originalScale = _rectTransform.localScale;
        }
        else
        {
            // 如果误挂到了非 UI 物体上，做一个兼容
            _originalScale = transform.localScale;
        }
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
        // 核心逻辑：利用正弦波 (Sin) 生成一个 -1 到 1 之间的平滑数值
        // Time.time * breathSpeed 控制波动的快慢
        float wave = Mathf.Sin(Time.time * breathSpeed);

        // 计算缩放系数：
        // 假设 breathRange 是 0.1，wave 是 1，则 factor = 1.1
        // 假设 wave 是 -1，则 factor = 0.9
        float scaleFactor = 1.0f + (wave * breathRange);

        // 应用缩放
        if (_rectTransform != null)
        {
            _rectTransform.localScale = _originalScale * scaleFactor;
        }
        else
        {
            transform.localScale = _originalScale * scaleFactor;
        }
    }
}
    
    

    
