using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostActor : MonoBehaviour
{
    public GhostRecorder recorder;     

    public float replayTimescale = 1; // 回放速度

    private GhostShot[] frames;

    private bool isReplaying;

    private int replayIndex = 0;
    private float replayTime = 0.0f;            // in milliseconds

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
        SetFrames(recorder.GetFrames());

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

    private void Update()
    {
        if (IsReplaying())
        {
            if (replayIndex < frames.Length)
            {
                GhostShot frame = frames[replayIndex];

                if (!frame.isFinal)
                {
                    if (replayTime < frame.timeMark)
                    {
                        if (replayIndex == 0)
                        {
                            replayTime = frame.timeMark;
                        }
                        else
                        {
                            DoLerp(frames[replayIndex - 1], frame);
                            replayTime += Time.smoothDeltaTime * 1000 * replayTimescale;
                        }
                    }
                    else
                    {
                        replayIndex++;
                    }
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

    private void DoLerp(GhostShot a, GhostShot b)
    {
        transform.position = Vector3.Slerp(a.posMark, b.posMark, Mathf.Clamp(replayTime, a.timeMark, b.timeMark));
        render.flipX = b.dirMark; 
    }

    public bool IsReplaying()
    {
        return isReplaying;
    }

    public void SetFrames(GhostShot[] frames)
    {
        this.frames = frames;
    }
}
