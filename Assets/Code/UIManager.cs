using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("HUD 组件")]
    public Slider stealthBar;           // 潜行条
    public Image currentMaskIcon;       // 左侧面具图标
    public Text currentMaskInfo;        // 面具信息文本
    public GameObject interactionPrompt; // "按E交互" 的提示UI
    public Text interactionText;        // 提示UI里的文字内容

    [Header("记事本 (Log)")]
    public Text logTextComponent;       // 显示日志的长文本框
    public ScrollRect logScrollRect;    // 用于滚动的组件
    private List<string> logHistory = new List<string>();

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Update()
    {
        // 实时平滑更新潜行条 UI (Lerp 效果更好)
        if (GameManager.Instance != null && stealthBar != null)
        {
            stealthBar.value = Mathf.Lerp(stealthBar.value, GameManager.Instance.stealthValue, Time.deltaTime * 5f);
        }
    }
    /// <summary>
    /// GameManager 调用的接口，修复报错用
    /// </summary>
    public void UpdateStealthUI(float value)
    {
        // 既然 Update() 里已经在平滑更新了，这里其实可以什么都不做。
        // 这个空函数的作用只是为了让 GameManager 不报错。
    }
    /// <summary>
    /// 添加一条日志到右下角记事本
    /// </summary>
    public void AddLog(string msg)
    {
        string time = System.DateTime.Now.ToString("HH:mm:ss");
        logHistory.Add($"<color=#FFD700>[{time}]</color> {msg}");

        // 如果日志太长，移除最早的
        if (logHistory.Count > 20) logHistory.RemoveAt(0);

        logTextComponent.text = string.Join("\n", logHistory);

        // 强制滚动到底部
        Canvas.ForceUpdateCanvases();
        logScrollRect.verticalNormalizedPosition = 0f;
    }

    /// <summary>
    /// 更新左侧面具状态
    /// </summary>
    public void UpdateMaskUI(MaskData data)
    {
        if (data == null) return;

        currentMaskIcon.sprite = data.icon;
        currentMaskInfo.text = $"{data.maskName}\n<size=12>{data.publicInfo}</size>";
    }

    /// <summary>
    /// 显示/隐藏交互浮窗
    /// </summary>
    public void ShowInteractionInfo(bool isVisible, string info = "")
    {
        interactionPrompt.SetActive(isVisible);
        if (isVisible && interactionText != null)
        {
            interactionText.text = info;
        }
    }
}
