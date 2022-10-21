using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostActor : MonoBehaviour
{
    public GhostRecorder recorder;     

    [SerializeField] 
    private List<GhostShot> frames; 

    private bool isReplaying;

    private int replayIndex = 0;
    private float replayTime = 0.0f;          

    private SpriteRenderer render;

    #region Event Handlers
    public event EventHandler ReplayStarted;
    public event EventHandler ReplayEnded;
    #endregion

    #region Event Invokers

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

    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }

    public void StartReplay()
    {
        if (frames.Count == 0)
        {
            SetFrames(recorder.GetFrames());
            recorder.ClearFrames(); 
        }

        if (!IsReplaying())
        {
            replayIndex = 0;
            replayTime = 0;

            transform.position = frames[0].posMark;
            render.flipX = frames[0].dirMark; 

            render.enabled = true; // 打开幽灵渲染

            isReplaying = true;

            OnReplayStart();

        }
    }

    public void StopReplay()
    {
        if (IsReplaying())
        {
            // 参数重置
            replayIndex = 0;
            replayTime = 0.0f;

            render.enabled = false;
            isReplaying = false;

            OnReplayEnd();

        }
    }

    private void FixedUpdate()
    {
        if (IsReplaying())
        {
            if (replayIndex < frames.Count)
            {
                GhostShot frame = frames[replayIndex];

                if (!frame.isFinal)
                {
                    transform.position = frame.posMark;
                    render.flipX = frame.dirMark;
                    replayTime = frame.timeMark; 
                    replayIndex++;
                }
                else
                {
                    StopReplay();
                }
            }
            else
            {
                StopReplay();
            }
        }
    }

    public bool IsReplaying()
    {
        return isReplaying;
    }

    public void SetFrames(List<GhostShot> frames)
    {
        Debug.Log(frames.Count); 
        foreach (var frame in frames)
        {
            this.frames.Add(frame); 
        }
    }
}
