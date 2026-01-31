using UnityEngine;
using System.Collections.Generic;

public class PlayerStateManager : MonoBehaviour
{
    public static PlayerStateManager Instance;

    // 当前基础面具 (例如 "Anderson", "Thief")
    public string currentBaseMask = "Thief";

    // 已获得的线索列表
    public List<string> collectedClues = new List<string>();

    void Awake() { Instance = this; }

    // 核心功能：用交互物和面具ID对账，对出当前面具ID
    public string GetEffectiveMaskID()
    {
        // 示例：如果你是安德森，且你有情书，则升级为L2
        if (currentBaseMask == "Anderson")
        {
            if (collectedClues.Contains("LoveLetter")) return "Anderson_L2";
            return "Anderson_L1";
        }
        // 默认返回基础ID
        return currentBaseMask + "_L1";
    }

    public void AddClue(string clueID)
    {
        if (!string.IsNullOrEmpty(clueID) && !collectedClues.Contains(clueID))
        {
            collectedClues.Add(clueID);
            Debug.Log("获得了新线索: " + clueID);
        }
    }
}
