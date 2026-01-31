using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotatement : MonoBehaviour
{
    public bool rotating = true;
    public float rotateSpeed = 180f;
    [Range(0.1f, 1.5f)]
    public float xAxisRatio = 1f;
    public Transform centerTransform;
    private Vector3 CenterPos;
    private Vector3 r;
    // Start is called before the first frame update
    void Awake()
    {
        CenterPos = centerTransform.transform.position;
        r = transform.position - CenterPos;
    }

    // Update is called once per frame
    void Update()
    {
        if(!rotating)return;
        r = Quaternion.AngleAxis(rotateSpeed * Time.deltaTime, Vector3.forward) * r;
        var r2 = r;
        r2.x *= xAxisRatio;
        transform.position = CenterPos + r2;//* Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad))
    }

    public void SwitchRotateState(bool rotating)
    {
        this.rotating = rotating;
    }
}
