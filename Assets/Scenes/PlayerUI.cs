using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("场景索引配置")]
    public int StartScene = 0;
    public int GameScene = 1;
    public int SetScene = 2;

    [Header("UI按钮引用")]
    public Button btnToGame;    // 在Inspector拖入“进入游戏”按钮
    public Button btnToStart;   // 在Inspector拖入“返回初始”按钮
    public Button btnToSet;     // 在Inspector拖入“设置”按钮

    private void Start()
    {
        // 绑定按钮点击事件
        if (btnToGame != null)
            btnToGame.onClick.AddListener(LoadGameScene);
        if (btnToStart != null)
            btnToStart.onClick.AddListener(LoadStartScene);
        if (btnToStart != null)
            btnToStart.onClick.AddListener(LoadSetScene);

    }

    // 跳转游戏场景
    public void LoadGameScene()
    {
        SceneManager.LoadScene(GameScene);
    }

    // 跳转初始场景
    public void LoadStartScene()
    {
        SceneManager.LoadScene(StartScene);
    }
    //跳转设置场景
    public void LoadSetScene()
    {
        SceneManager.LoadScene(SetScene);
    }
}
