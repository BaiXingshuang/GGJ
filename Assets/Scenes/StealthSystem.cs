using UnityEngine;
using UnityEngine.UI;

public class StealthSystem : MonoBehaviour
{
    public static StealthSystem Instance;
    public float currentStealth = 100f;
    public float maxStealth = 100f;

    void Awake() { Instance = this; }

    public void DrainStealth(float amount)
    {
        currentStealth -= amount;
        if (currentStealth <= 0)
        {
            currentStealth = 0;
           // GameOver(); // 触发失败逻辑
        }
        // 更新UI血条...
    }

    // 伪装技能回复或者消耗可以在这里加方法
}
