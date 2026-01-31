using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 定义面具/身份类型
public enum MaskType
{
    Spy,        // 0: 初始间谍
    Doctor,     // 1: 医生
    Security,   // 2: 安保人员
    Cleaner,    // 3: 清洁工
    Monster     // 4: 实验体/怪物
}

// 这一行非常关键！它会在 Unity 的右键菜单里添加 "Create -> Game -> Mask Data"
[CreateAssetMenu(fileName = "NewMaskData", menuName = "Game/Mask Data")]
public class MaskData : ScriptableObject
{
    [Header("身份标识")]
    public string maskID;       // 例如 "Mask_Doctor_L1"
    public string maskName;     // 例如 "安德森的主任医师证"

    [Header("UI 表现")]
    public Sprite icon;         // 在左侧 UI 显示的头像
    [TextArea]
    public string publicInfo;   // 显示在 UI 上的公开信息描述

    [Header("核心属性")]
    public MaskType type;       // 这个面具属于哪一类？

    [Header("社交网白名单 (核心机制)")]
    [Tooltip("如果列表中包含 Security，那么 Security 类型的敌人就会把你当做自己人，不增加潜行值。")]
    public List<MaskType> immuneToFactions;
}
