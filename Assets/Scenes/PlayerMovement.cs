using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    //移动速度
    public float moveSpeed;
    private Animator _animator;
    private bool running;
    private float moveDealt;
    private Vector3 oriPos, newPos;
    public MoveNode curMoveNode;

    void Awake()
    {
        Instance = this;
        _animator = GetComponent<Animator>();
    }

    private Transform hitTransform;
    private Ray ray;
    private RaycastHit2D hitInfo;
    public float requireHoldTime = 3f;
    private float lastDown,lastUp, curDown, curUp;
    private bool holding;
    private MoveNode selectedNode;
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hitInfo =Physics2D.Raycast(new Vector2(ray.origin.x, ray.origin.y),Vector2.zero,Mathf.Infinity);
            if(hitInfo)
            {
                if(hitInfo.transform.TryGetComponent<MoveNode>(out var moveNode))
                {
                    if(selectedNode == moveNode)
                    {
                        lastDown = curDown;lastUp = curUp;
                    }

                    curDown = Time.timeSinceLevelLoad;
                    holding = true;
                }
            }
        }
        if(holding)
        {
            if(hitInfo.transform.TryGetComponent<MoveNode>(out var moveNode))
            {
                curUp = Time.timeSinceLevelLoad;
                if(curUp - curDown >= requireHoldTime && moveNode == curMoveNode)
                {
                    moveNode.ShowObjects();
                }
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            holding = false;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hitInfo =Physics2D.Raycast(new Vector2(ray.origin.x, ray.origin.y),Vector2.zero,Mathf.Infinity);
            if(hitInfo.transform.TryGetComponent<MoveNode>(out var moveNode))
            {
                if(curUp - lastDown < 0.5f)
                {
                    Debug.Log("doubleClick");
                    if(selectedNode == moveNode)
                    {
                        MoveToNextNode(moveNode);
                    }
                }
                else if(curUp - curDown < requireHoldTime)
                {
                    //MoveToNextNode(moveNode);
                    selectedNode = moveNode;
                }
            }
        }
        HandleMovement();
    }
    public void MoveToNextNode(MoveNode nextNode)
    {
        if(running)return;
        MoveNode.NodeNum nextMoveNodeNum = nextNode.GetNodeNum();
        if(curMoveNode.nearNodes.Contains(nextMoveNodeNum))
        {
            MoveToNextPoint(nextNode.transform.position);
            curMoveNode = nextNode;
        }
        // else
        //     Debug.Log("null of " + nextMoveNodeNum);
    }

    public void MoveToNextPoint(Vector3 nextPos)
    {
        //如果正在奔跑则不执行
        if(running)return;
        //是否翻转人物
        transform.localScale = nextPos.x < transform.position.x ? new Vector3(-1,1,1) : Vector3.one;
        //起始移动点
        oriPos = transform.position;
        //移动终点
        this.newPos = nextPos;
        newPos.z = oriPos.z;
        //开始奔跑
        running = true;
        //动画
        _animator.SetBool("Running", running);
    }

    public void HandleMovement()
    {
        if(!running) return;
        //到达终点
        if(moveDealt >= 1)
        {
            //停止移动
            moveDealt = 0;
            running = false;
            //动画
            _animator.SetBool("Running", running);
            return;
        }

        moveDealt += moveSpeed/Vector3.Distance(oriPos,newPos) * Time.deltaTime;
        transform.position = Vector3.Lerp(oriPos, newPos, moveDealt);
    }
}
