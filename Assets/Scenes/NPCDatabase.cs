using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCDatabase", menuName = "GameJam/NPCDatabase")]
public class NPCDatabase : ScriptableObject
{
    [System.Serializable]
    public enum NPCProp
    {
        医护,路人,工人,保安
    }
    public string NPCID;
    public NPCProp prop;
    
    [System.Serializable]
    public class NPCContent
    {
        public int index;
        [TextArea]
        public string content;
    }

    public List<NPCContent> NPCContents;
}
