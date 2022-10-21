using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSystem : MonoBehaviour
{
    
    private GhostRecorder[] recorders;
    private GhostActor[] ghostActors;

    private void Start()
    {
        recorders = FindObjectsOfType<GhostRecorder>();
        StartRecording(); 
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
            if (GameManager.GM.canStopRecording) 
            {
                // 录制了几帧后可以停止录制
                StopRecording();
            }
            if (!GameManager.GM.canStopRecording && GameManager.GM.canStartReplay)
            {
                // 幽灵生成后可以开始回放
                StartReplay();
            }
            if (!GameManager.GM.canStopRecording && !GameManager.GM.canStartReplay && GameManager.GM.canStartRecording)
            {
                // 录制交给幽灵，清空录影机存储帧后可以开始录制
                StartRecording();
            }
        }
        if (GameManager.GM.curLap == 3)
        {
            if (GameManager.GM.canStopRecording)
            {
                StopRecording();
            }
            if (GameManager.GM.canStartReplay)
            {
                StartReplay();
            }
        }
        if (GameManager.GM.curLap == 4)
        {
            if (GameManager.GM.canStopReplay)
            {
                StopReplay();
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
