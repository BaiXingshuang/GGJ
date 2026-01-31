using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // =========================================================
    // 1. 单例 & 变量定义
    // =========================================================
    public static PlayerController Instance;

    [Header("移动设置")]
    public float moveSpeed = 5f;

    [Header("交互设置")]
    public float interactRadius = 1.5f;
    public LayerMask interactLayer; // 记得在 Inspector 里选 Interactable 层

    // 内部变量
    private Rigidbody2D rb;
    private Vector2 movementInput;

    // --- 桥接属性 ---
    // 这是一个快捷方式，让 EnemyAI 可以通过 PlayerController.Instance.currentMaskData 访问
    // 实际上数据是存在 GameManager 里的
    public MaskData CurrentMaskData
    {
        get { return GameManager.Instance.currentMask; } 
    }

    void Awake()
    {
        if (Instance == null) Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    // =========================================================
    // 2. 输入检测 (Update)
    // =========================================================
    void Update()
    {
        // --- 移动输入 (WASD / 箭头键) ---
        // GetAxisRaw 返回 -1, 0, 1，手感更干脆，没有惯性滑步
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // normalized 防止斜向移动速度变快
        movementInput = new Vector2(moveX, moveY).normalized;

        // --- 交互输入 (E键) ---
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    // =========================================================
    // 3. 物理移动 (FixedUpdate)
    // =========================================================
    void FixedUpdate()
    {
        // 直接修改速度，适合俯视角 RPG
        rb.velocity = movementInput * moveSpeed;
    }

    // =========================================================
    // 4. 交互逻辑
    // =========================================================
    void TryInteract()
    {
        // 发射圆形射线检测周围
        // 参数：中心点，半径，检测层级
        Collider2D hit = Physics2D.OverlapCircle(transform.position, interactRadius, interactLayer);

        if (hit != null)
        {
            // 尝试获取对方身上的 InteractableProp 脚本
            InteractableProp prop = hit.GetComponent<InteractableProp>();

            if (prop != null)
            {
                Debug.Log($"玩家尝试与 {prop.name} 交互");
                prop.Interact(); // 调用交互物的公开方法
            }
        }
        else
        {
            // 可选：如果没有检测到东西，给一点 Debug 反馈
            // Debug.Log("周围没有可交互物体");
        }
    }

    // =========================================================
    // 5. 辅助调试 (Gizmos)
    // =========================================================
    // 这个函数会让交互范围在 Scene 窗口里显示为一个黄色的圈，方便你调整数值
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
