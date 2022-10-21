using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSystem : MonoBehaviour
{
    
    private GhostRecorder[] recorders;
    private GhostActor[] ghostActors;

    private bool tempFrameLock; // 保证代码只执行一次 

    private void Start()
    {
        recorders = FindObjectsOfType<GhostRecorder>();
        StartRecording();
        tempFrameLock = false; 
    }

    private void Update()
    {
        SpawnGhost();
    }

    private void SpawnGhost()
    {
        ghostActors = FindObjectsOfType<GhostActor>(); 
        if (GameManager.GM.curLap == 2)
        {
            if (tempFrameLock == false)
            {
                StopRecording();
                StartReplay();
                StartRecording();
                tempFrameLock = true; 
            }
        }
        if (GameManager.GM.curLap == 3)
        {
            if (tempFrameLock == true)
            {
                StopReplay(); 
                StopRecording();
                StartReplay();
                tempFrameLock = false; 
            }
        }
        if (GameManager.GM.curLap == 4)
        {
            if (tempFrameLock == false)
            {
                StopRecording(); 
                tempFrameLock = true; 
            }
        }
    }

    public void StartRecording()
    {
        for (int i = 0; i < recorders.Length; i++)
        {
            recorders[i].StartRecording();
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

        OnReplayStart();
    }

    public void StopReplay()
    {
        for (int i = 0; i < ghostActors.Length; i++)
        {
            ghostActors[i].StopReplay();
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
