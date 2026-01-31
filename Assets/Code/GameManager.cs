using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 必须引用

public class GameManager : MonoBehaviour
{
    // 1. 单例实例
    public static GameManager Instance;

    [Header("核心状态")]
    [Range(0, 100)]
    public float stealthValue = 0f; // 0-100，潜行值
    public bool isGameOver = false;

    [Header("玩家数据")]
    // 修复1：删除了错误的 StringtabObject，只保留 MaskData
    public MaskData currentMask;

    // 已获得的信息列表
    public List<string> collectedInfoIDs = new List<string>();

    // 剧情开关字典
    public Dictionary<string, bool> worldFlags = new Dictionary<string, bool>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        worldFlags = new Dictionary<string, bool>();
    }

    void Start()
    {
        stealthValue = 0f;
        isGameOver = false;

        SetFlag("GameStarted", true);

        // 修复2：这里原本写的是 SceneManager (错了)，更新血条应该找 UIManager
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateStealthUI(stealthValue);
    }

    public void UpdateStealth(float amount)
    {
        if (isGameOver) return;

        stealthValue += amount;
        stealthValue = Mathf.Clamp(stealthValue, 0f, 100f);

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateStealthUI(stealthValue);
        }

        // 判定：如果警觉值满了，触发失败结局
        if (stealthValue >= 100f)
        {
            isGameOver = true;
            Debug.Log("潜行值已满！触发被捕结局。");

            // 修复3：这里要把名字改成和你那个脚本类名一模一样
            if (SceneController.Instance != null)
            {
                SceneController.Instance.TriggerEnding("BadEnd_Busted");
            }
        }
    }

    public void UnlockInfo(string infoID)
    {
        if (!collectedInfoIDs.Contains(infoID))
        {
            collectedInfoIDs.Add(infoID);
            Debug.Log($"获得新情报: {infoID}");

            if (UIManager.Instance != null)
            {
                UIManager.Instance.AddLog($"获得关键信息：{infoID}");
            }
            CheckGlobalFlags();
        }
    }

    public void SetFlag(string key, bool value)
    {
        if (worldFlags.ContainsKey(key))
        {
            worldFlags[key] = value;
        }
        else
        {
            worldFlags.Add(key, value);
        }
        CheckGlobalFlags();
    }

    public bool GetFlag(string key)
    {
        if (worldFlags.ContainsKey(key))
            return worldFlags[key];
        return false;
    }

    public void CheckGlobalFlags()
    {
        if (GetFlag("HasSerum") && !GetFlag("IdentityRevealed"))
        {
            Debug.Log("条件达成：可以触发完美结局");
        }

        if (collectedInfoIDs.Contains("Info_A") && collectedInfoIDs.Contains("Info_B"))
        {
            UnlockInfo("Info_Combined_C");
        }
    }

    public void ChangeMask(MaskData newMask)
    {
        currentMask = newMask;
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateMaskUI(newMask);
        }
    }
}