using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MaskDatabasae", menuName = "GameJam/MaskDatabasae")]
public class MaskDatabasae : ScriptableObject
{
    public Sprite spriteBackground;
    public Sprite sprite;
    public string maskID;
    public NPCDatabase.NPCProp Prop;
    public enum MaskLevel
    {
        L1,L2,L3,L4
    };
    public MaskLevel level;
    [TextArea]
    public string text;
}
