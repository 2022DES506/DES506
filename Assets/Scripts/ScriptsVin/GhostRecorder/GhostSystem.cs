using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSystem : MonoBehaviour
{

    private GhostRecorder[] recorders;
    private GhostActor[] ghostActors;

    public float recordDuration = 10;


    private void Start()
    {
        recorders = FindObjectsOfType<GhostRecorder>();
        ghostActors = FindObjectsOfType<GhostActor>();

        StartRecording(); 
    }

    private void Update()
    {
        SpawnGhost();
    }

    private void SpawnGhost()
    {
        if (GameManager.GM.curLap == 2)
        {
            StopRecording();
            StartReplay(); 
        }
        if (GameManager.GM.curLap == 3)
        {
            StopReplay(); 
        }
    }

    public void StartRecording()
    {
        for (int i = 0; i < recorders.Length; i++)
        {
            recorders[i].StartRecording(recordDuration);
        }

        OnRecordingStart();
    }

    public void StopRecording()
    {
        for (int i = 0; i < recorders.Length; i++)
        {
            recorders[i].StopRecording();
        }

        OnRecordingEnd();
    }

    public void StartReplay()
    {
        for (int i = 0; i < ghostActors.Length; i++)
        {
            ghostActors[i].StartReplay();
        }

        /* �ر���Ӱ�ߵ���Ⱦ 
        for (int i = 0; i < recorders.Length; i++)
        {
            recorders[i].GetComponent<Renderer>().enabled = false;
        }
        */

        OnReplayStart();
    }

    public void StopReplay()
    {
        for (int i = 0; i < ghostActors.Length; i++)
        {
            ghostActors[i].StopReplay();
        }

        for (int i = 0; i < recorders.Length; i++)
        {
            recorders[i].GetComponent<Renderer>().enabled = true;
        }

        OnReplayEnd();
    }

    #region Event Handlers
    public event EventHandler RecordingStarted;
    public event EventHandler RecordingEnded;
    public event EventHandler ReplayStarted;
    public event EventHandler ReplayEnded;
    #endregion

    #region Event Invokers
    protected virtual void OnRecordingStart()
    {
        if (RecordingStarted != null)
        {
            RecordingStarted.Invoke(this, EventArgs.Empty);
        }
    }

    protected virtual void OnRecordingEnd()
    {
        if (RecordingEnded != null)
        {
            RecordingEnded.Invoke(this, EventArgs.Empty);
        }
    }

    protected virtual void OnReplayStart()
    {
        if (ReplayStarted != null)
        {
            ReplayStarted.Invoke(this, EventArgs.Empty);
        }
    }

    protected virtual void OnReplayEnd()
    {
        if (ReplayEnded != null)
        {
            ReplayEnded.Invoke(this, EventArgs.Empty);
        }
    }
    #endregion
}
