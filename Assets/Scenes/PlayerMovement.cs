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

    void Update()
    {
        // if(Input.GetMouseButtonDown(0))
        // {
        //     Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     clickPos.z = 0;
        //     MoveToNextPoint(clickPos);
        // }
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hitInfo =Physics2D.Raycast(new Vector2(ray.origin.x, ray.origin.y),Vector2.zero,Mathf.Infinity);
            if(hitInfo)
            {
                //Debug.Log(hitInfo.transform.name);
                MoveToNextNode(hitInfo.transform.GetComponent<MoveNode>());
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
        else
            Debug.Log("null of " + nextMoveNodeNum);
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
