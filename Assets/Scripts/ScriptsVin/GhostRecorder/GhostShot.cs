using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��¼ÿ֡���ݵ��� 
[System.Serializable]
public class GhostShot
{
    public bool isFinal; // �Ƿ����һ֡���

    public float timeMark = 0.0f; // ��һ֡��ʱ���

    public Vector3 posMark; // λ�ñ��
    public bool dirMark;        // ת����

    public GhostShot()
    {
        isFinal = false; 
    }
}
