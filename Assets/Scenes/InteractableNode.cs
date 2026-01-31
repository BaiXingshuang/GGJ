using UnityEngine;
using UnityEngine.EventSystems; // 必须引用这个

// 增加 IPointerExitHandler，防止鼠标滑出球体后还在继续判定
public class InteractableNode : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [Header("配置区域")]
    public string objectID;       // 记得在 Inspector 填 ID，如 NPC_A
    public StoryDatabase database; // 记得拖入数据库文件
    public float requiredHoldTime = 3.0f; // 长按需要几秒

    [Header("调试监测（不要手动改）")]
    public bool isHolding = false;
    public float holdTimer = 0f;
    public bool interactionTriggered = false; // 核心刹车锁

    void Update()
    {
        // 核心逻辑：只有当 (正在按住) 且 (还没触发过) 时，才运行
        if (isHolding && !interactionTriggered)
        {
            holdTimer += Time.deltaTime; // 计时

            // 这里可以加一行调试，看看每一帧是不是在跑
            // Debug.Log("正在读取..." + holdTimer);

            // 1. 每一帧扣除潜行值（这里调用你的潜行系统）
            if (database != null && PlayerStateManager.Instance != null && StealthSystem.Instance != null)
            {
                // 获取当前面具
                string effectiveMask = PlayerStateManager.Instance.GetEffectiveMaskID();
                // 查表
                var entry = database.GetEntry(objectID, effectiveMask);
                float risk = (entry != null) ? entry.stealthRiskMultiplier : 1.0f;
                // 扣血
                StealthSystem.Instance.DrainStealth(Time.deltaTime * risk);
            }

            // 2. 时间到了，触发完成
            if (holdTimer >= requiredHoldTime)
            {
                CompleteInteraction();
            }
        }
    }

    // 当鼠标按下
    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        holdTimer = 0f;
        interactionTriggered = false; // 重置刹车，准备开始新的交互
        Debug.Log("【开始】鼠标按下了物体");
    }

    // 当鼠标松开
    public void OnPointerUp(PointerEventData eventData)
    {
        ResetState("【取消】鼠标松开了");
    }

    // 当鼠标移出物体范围（防止按住拖走还在判定）
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isHolding)
        {
            ResetState("【中断】鼠标移出了范围");
        }
    }

    // 统一的重置逻辑
    void ResetState(string logMsg)
    {
        isHolding = false;
        holdTimer = 0f;
        // 注意：这里不要重置 interactionTriggered，否则如果是完成状态松手，会重复触发
        Debug.Log(logMsg);
    }

    // 完成交互
    void CompleteInteraction()
    {
        Debug.Log("【完成】长按交互成功！");

        // ★★★ 关键刹车：拉起手刹，防止下一帧继续触发 ★★★
        interactionTriggered = true;
        isHolding = false;

        // 显示文本逻辑
        if (database != null && PlayerStateManager.Instance != null)
        {
            string effectiveMask = PlayerStateManager.Instance.GetEffectiveMaskID();
            var entry = database.GetEntry(objectID, effectiveMask);

            if (NotebookUI.Instance != null)
            {
                string text = (entry != null) ? entry.resultText : "你看不出什么名堂。";
                NotebookUI.Instance.AddLog(text);
            }

            if (entry != null)
            {
                PlayerStateManager.Instance.AddClue(entry.unlockClueID);
            }
        }
    }
}
