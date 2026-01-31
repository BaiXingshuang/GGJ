using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MindPalace : MonoBehaviour
{
    public static MindPalace Instance; // 为了方便 UI 按钮调用

    [Header("合成状态")]
    public List<string> selectedClues = new List<string>();

    // UI 反馈 (可选)
    public Text feedbackText;

    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 当玩家点击UI上的线索图标时调用
    /// </summary>
    public void SelectClue(string clueID)
    {
        if (selectedClues.Contains(clueID))
        {
            selectedClues.Remove(clueID); // 反选
        }
        else
        {
            selectedClues.Add(clueID);

            // 限制最多选2个
            if (selectedClues.Count > 2) selectedClues.RemoveAt(0);
        }

        // 自动尝试合成
        if (selectedClues.Count == 2)
        {
            TryCombine();
        }
    }

    /// <summary>
    /// 尝试合成逻辑 (Hardcode for GameJam)
    /// </summary>
    public void TryCombine()
    {
        string resultMsg = "这两个线索似乎没有关联...";

        // 逻辑公式 1: 药单 + 诊断书 = 勒索信
        if (HasClue("Info_DrugOrder") && HasClue("Info_Diagnosis"))
        {
            // 只有当玩家还没获得这个结果时才触发
            if (!GameManager.Instance.collectedInfoIDs.Contains("Secret_Blackmail"))
            {
                GameManager.Instance.UnlockInfo("Secret_Blackmail");
                resultMsg = "合成成功！发现了安德森的秘密交易！";

                // 也可以设置 Flag
                GameManager.Instance.SetFlag("Secret_Blackmail_Unlocked", true);
            }
        }
        // 逻辑公式 2: 借条 + 恐吓信 = 漏洞
        else if (HasClue("Info_DebtNote") && HasClue("Info_ThreatLetter"))
        {
            GameManager.Instance.UnlockInfo("Weakness_RadarOff");
            resultMsg = "合成成功！得知今晚雷达会关闭！";
        }

        UIManager.Instance.AddLog(resultMsg);

        // 合成后清空选择
        selectedClues.Clear();
    }

    private bool HasClue(string id)
    {
        return selectedClues.Contains(id);
    }
}
