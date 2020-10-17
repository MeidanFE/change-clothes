using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinWithMouse : MonoBehaviour
{
    private bool isClick = false;
    private Vector3 nowPos;
    private Vector3 olaPos;
    public float length = 5;

     void OnMouseUp()
    {
        isClick = false;
    }

    void OnMouseDown()
    {
        isClick = true;
    }


    private void Update()
    {
        nowPos = Input.mousePosition;
        if (isClick)
        {
            Vector3 offset = nowPos - olaPos;
            if (Mathf.Abs(offset.x) > Mathf.Abs(offset.y) && Mathf.Abs(offset.x)>length) {
                transform.Rotate(Vector3.up, -offset.x);
            }
        }
        olaPos = Input.mousePosition;
    }

}
