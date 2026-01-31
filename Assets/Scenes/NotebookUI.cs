using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotebookUI : MonoBehaviour
{
    public static NotebookUI Instance;
    public TMP_Text contentText; // 拖入UI Text组件
    //public ScrollRect scrollRect;

    void Awake() { Instance = this; }

    public void AddLog(string newText)
    {
        // 加这一句！看看传进来的到底是啥
        Debug.Log("【UI接收】尝试显示文字: " + newText);

        if (contentText != null)
        {
            contentText.text += "\n\n> " + newText;
            // ... 其他代码 ...
        }
    }
}
