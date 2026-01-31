using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableProp : MonoBehaviour
{
    [Header("基本信息")]
    public string propID;       // 例如 "Coffee_01"
    public string baseName;     // 例如 "一杯咖啡"
    public bool hasInteracted = false;

    // 定义每个面具看到的特殊内容
    [System.Serializable]
    public struct InteractionVariant
    {
        public MaskType requiredMask;   // 需要什么身份才能看懂？
        [TextArea] public string infoText; // 看到的信息
        public string triggerEventID;   // (可选) 交互后触发GameManager的事件，如 "GetSerum"
        public string unlockInfoID;     // (可选) 获得线索ID，如 "Info_Poison"
    }

    public List<InteractionVariant> variants; // 在 Inspector 里配置

    /// <summary>
    /// 当玩家准星指向物体时调用，获取浮窗文字
    /// </summary>
    public string GetDescription()
    {
        // 1. 获取玩家当前面具
        MaskType currentType = GameManager.Instance.currentMask.type;

        // 2. 遍历查找是否有特殊信息
        foreach (var v in variants)
        {
            if (v.requiredMask == currentType)
            {
                return v.infoText; // 找到了对应面具的特殊描述
            }
        }

        // 3. 没找到，返回默认
        return baseName + " (看起来没什么特别的)";
    }

    /// <summary>
    /// 玩家按下 E 键时调用
    /// </summary>
    public void Interact()
    {
        if (hasInteracted) return; // (可选) 如果是一次性道具

        // 1. 获取当前匹配的配置（为了触发事件）
        MaskType currentType = GameManager.Instance.currentMask.type;
        InteractionVariant currentVariant = new InteractionVariant();
        bool foundVariant = false;

        foreach (var v in variants)
        {
            if (v.requiredMask == currentType)
            {
                currentVariant = v;
                foundVariant = true;
                break;
            }
        }

        // 2. 核心逻辑处理
        if (foundVariant)
        {
            // 如果配置了获得线索
            if (!string.IsNullOrEmpty(currentVariant.unlockInfoID))
            {
                GameManager.Instance.UnlockInfo(currentVariant.unlockInfoID);
            }

            // 如果配置了触发剧情事件
            if (!string.IsNullOrEmpty(currentVariant.triggerEventID))
            {
                GameManager.Instance.SetFlag(currentVariant.triggerEventID, true);
            }

            // 记事本记录
            UIManager.Instance.AddLog($"你检查了 {baseName}：{currentVariant.infoText}");
        }
        else
        {
            UIManager.Instance.AddLog($"你检查了 {baseName}，但一无所获。");
        }

        hasInteracted = true;

        // 播放音效 (可以使用 AudioSource.PlayClipAtPoint)
        // AudioSource.PlayClipAtPoint(interactSound, transform.position);
    }
}
