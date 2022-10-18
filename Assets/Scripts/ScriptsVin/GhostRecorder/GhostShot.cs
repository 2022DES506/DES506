using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 记录每帧数据的类 
[System.Serializable]
public class GhostShot
{
    public bool isFinal; // 是否最后一帧标记

    public float timeMark = 0.0f; // 这一帧的时间戳

    public Vector3 posMark; // 位置标记
    public bool dirMark;        // 转向标记

    public GhostShot()
    {
        isFinal = false; 
    }
}
