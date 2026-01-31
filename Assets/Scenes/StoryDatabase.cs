using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "StoryDatabase", menuName = "GameJam/StoryDatabase")]
public class StoryDatabase : ScriptableObject
{
    [System.Serializable]
    public class InteractionEntry
    {
        public string interactableID; // 交互物ID，如 "Tape_01"
        public string maskID;         // 面具ID，如 "Anderson_L2" (支持模糊匹配)

        [TextArea]
        public string resultText;     // 记事本显示的文本
        public string unlockClueID;   // 交互后获得的线索ID (用于升级面具)
        public float stealthRiskMultiplier = 1.0f; // 潜行消耗倍率 (1=正常, 0=安全, 2=危险)
    }

    public List<InteractionEntry> entries;
   


    // 核心查询方法
    public InteractionEntry GetEntry(string objID, string currentMaskID)
    {
       // 简单逻辑：遍历查找匹配项。如果找不到特定面具的，可以返回一个默认项
        return entries.Find(e => e.interactableID == objID && e.maskID == currentMaskID);
    }
}
