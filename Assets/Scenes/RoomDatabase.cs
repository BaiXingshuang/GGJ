using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomDatabase", menuName = "GameJam/RoomDatabase")]
public class RoomDatabase : ScriptableObject
{
    public string roomID;
    public List<GameObject> NPCInRoom;
    public List<GameObject> objects;
}
