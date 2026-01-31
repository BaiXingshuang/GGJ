using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 必须引用：用于重开游戏
using UnityEngine.UI; // 必须引用：用于处理结局UI

public class SceneController : MonoBehaviour
{
    // ==========================================
    // 1. 单例设置
    // ==========================================
    public static SceneController Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;

        // 确保游戏开始时，结局面板是隐藏的
        if (endingPanel != null) endingPanel.SetActive(false);
    }

    // ==========================================
    // 2. 楼层传送功能 (Teleport System)
    // ==========================================
    [Header("楼层传送设置")]
    public Transform spawnPoint1F; // 拖入 1F 空物体
    public Transform spawnPoint2F; // 拖入 2F 空物体
    public Transform spawnPointB3; // 拖入 B3 空物体
    public Transform playerTransform; // 拖入 Player

    public void TeleportPlayer(int floorIndex)
    {
        Transform target = spawnPoint1F;
        switch (floorIndex)
        {
            case 1: target = spawnPoint1F; break;
            case 2: target = spawnPoint2F; break;
            case 3: target = spawnPointB3; break;
        }

        if (playerTransform != null && target != null)
        {
            playerTransform.position = target.position;
            Debug.Log($"已传送至 {floorIndex} 层");
        }
    }

    // ==========================================
    // 3. 结局控制功能 (Ending System)
    // ==========================================
    [Header("结局 UI 设置")]
    public GameObject endingPanel;      // 拖入 Canvas 下的 EndingPanel
    public Text endingTextDisplay;      // 拖入 Panel 里的 Text
    public Image endingImageDisplay;    // 拖入 Panel 里的 Image

    [System.Serializable]
    public struct EndingData
    {
        public string id;              // 结局ID，如 "BadEnd_Busted"
        [TextArea] public string text; // 结局文字
        public Sprite image;           // 结局图片
    }

    public List<EndingData> endingList; // 在 Inspector 里配置结局列表

    /// <summary>
    /// GameManager 调用的核心函数
    /// </summary>
    public void TriggerEnding(string endingID)
    {
        Debug.Log("触发结局: " + endingID);

        // 1. 打开面板
        if (endingPanel != null) endingPanel.SetActive(true);

        // 2. 查找数据
        EndingData data = endingList.Find(x => x.id == endingID);

        // 3. 刷新 UI
        if (!string.IsNullOrEmpty(data.id))
        {
            if (endingTextDisplay != null) endingTextDisplay.text = data.text;
            if (endingImageDisplay != null) endingImageDisplay.sprite = data.image;
        }
        else
        {
            Debug.LogError($"找不到 ID 为 {endingID} 的结局配置！");
        }

        // 4. 暂停游戏
        Time.timeScale = 0;
    }

    /// <summary>
    /// 绑定给结局页面“重新开始”按钮
    /// </summary>
    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}