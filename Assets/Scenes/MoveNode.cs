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

    public RoomDatabase roomDatabase;

    public NodeNum GetNodeNum()
    {
        return curNodeNum;
    }

    public void CallPlayerMoveToNextNode()
    {
        PlayerMovement.Instance.MoveToNextNode(this);
        Debug.Log("Move");
    }

    private bool objectsShowed;
    public void ShowObjects()
    {
        if(objectsShowed)return;

        objectsShowed = true;
        Debug.Log("Show Objects");
    }
}
