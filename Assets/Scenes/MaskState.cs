using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class MaskState : MonoBehaviour
{
    public MaskDatabasae maskData;
    void Awake()
    {
        GetComponent<Image>().sprite = maskData.sprite;
    }
}
