using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveNode : MonoBehaviour
{
    public enum NodeNum
    {
        N1,N2,N3,N4,N5
    }
    public NodeNum curNodeNum;
    public List<NodeNum> nearNodes;

    public NodeNum GetNodeNum()
    {
        return curNodeNum;
    }

    public void CallPlayerMoveToNextNode()
    {
        PlayerMovement.Instance.MoveToNextNode(this);
        Debug.Log("Move");
    }
}
