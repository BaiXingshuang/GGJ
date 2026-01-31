using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("身份设置")]
    public MaskType myFaction; // 我是谁？(Security, Doctor 等)

    [Header("视野设置")]
    public float viewDistance = 6f;
    public float viewAngle = 90f;
    public LayerMask obstructionMask; // 必须设置为 "Wall" 层，防止穿墙透视

    [Header("巡逻设置")]
    public float moveSpeed = 2f;
    public Transform[] patrolPoints;
    private int currentPointIndex = 0;
    private float waitTimer = 0f;

    [Header("引用")]
    private Transform player;

    void Start()
    {
        // 获取玩家 Transform (假设 PlayerController 是单例)
        if (PlayerController.Instance != null)
        {
            player = PlayerController.Instance.transform;
        }
    }

    void Update()
    {
        if (GameManager.Instance.isGameOver) return;

        Patrol();
        DetectPlayer();
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        // 简单的移动逻辑
        Transform target = patrolPoints[currentPointIndex];
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        // 面向移动方向 (简单的 Scale 翻转)
        if (target.position.x > transform.position.x) transform.localScale = new Vector3(1, 1, 1);
        else transform.localScale = new Vector3(-1, 1, 1);

        // 到达检测
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer > 2f) // 停顿2秒
            {
                waitTimer = 0;
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
            }
        }
    }

    void DetectPlayer()
    {
        if (player == null) return;

        // 1. 距离判定
        float distToPlayer = Vector2.Distance(transform.position, player.position);
        if (distToPlayer > viewDistance) return; // 太远看不见

        // 2. 角度判定 (简单的朝向判断)
        Vector2 dirToPlayer = (player.position - transform.position).normalized;
        // 假设右边是正方向 (1,0)
        Vector2 facingDir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        if (Vector2.Angle(facingDir, dirToPlayer) > viewAngle / 2) return; // 在背后看不见

        // 3. 物理阻挡判定 (防穿墙)
        RaycastHit2D hit = Physics2D.Linecast(transform.position, player.position, obstructionMask);
        if (hit.collider != null)
        {
            // Debug.DrawLine(transform.position, hit.point, Color.yellow);
            return; // 视线被墙挡住了
        }

        // 4. 看见了！开始社交伪装判定
        Debug.DrawLine(transform.position, player.position, Color.red);
        CheckHostility();
    }

    void CheckHostility()
    {
        MaskData playerMask = GameManager.Instance.currentMask;

        // 核心判定：如果玩家面具的"白名单"里包含我，那我就不抓他
        if (playerMask != null && playerMask.immuneToFactions.Contains(myFaction))
        {
            // 安全：无视玩家
            return;
        }

        // 危险：发现入侵者
        // 增加潜行值 (每秒增加 20 点，5秒就会被抓)
        float detectionRate = 20f;
        GameManager.Instance.UpdateStealth(detectionRate * Time.deltaTime);

        // 显示警告日志 (可选，防止刷屏可以加个计时器限制频率)
        // UIManager.Instance.AddLog("你被发现了！"); 
    }

    // 辅助显示视野范围
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }
}
