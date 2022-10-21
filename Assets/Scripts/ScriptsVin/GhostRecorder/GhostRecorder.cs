using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRecorder : MonoBehaviour
{
    [SerializeField] 
    public List<GhostShot> frames = new List<GhostShot>();

    private bool isRecording;

    private int recordIndex = 0;
    private float recordTime = 0.0f;          

    private SpriteRenderer render; 

    #region Event Handlers
    public event EventHandler RecordingStarted;
    public event EventHandler RecordingEnded;
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
    #endregion

    private void Awake()
    {
        render = GetComponent<SpriteRenderer>(); 
    }

    public void StartRecording()
    {
        if (!IsRecording())
        {
            recordIndex = 0;
            recordTime = Time.time; 

            isRecording = true;

            GameManager.GM.canStartRecording = false;

            OnRecordingStart();

            Debug.LogFormat("Recording of {0} started", gameObject.name);
        }
    }

    public void StopRecording()
    {
        if (IsRecording())
        {
            frames[recordIndex - 1].isFinal = true;

            isRecording = false;

            GameManager.GM.canStopRecording = false;

            OnRecordingEnd();

            Debug.LogFormat("Recording of {0} ended at frame {1}", gameObject.name, recordIndex);

        }
    }

    private void FixedUpdate()
    {
        recordTime += Time.fixedDeltaTime; 
        if (IsRecording())
        {
            RecordFrame();

            GameManager.GM.canStopRecording = true; 
        }
    }

    private void RecordFrame()
    {
        GhostShot newFrame = new GhostShot()
        {
            timeMark = recordTime,
            posMark = transform.position,
            dirMark = render.flipX, 
        };

        frames.Add(newFrame); 

        recordIndex++;
    }

    public bool IsRecording()
    {
        return isRecording;
    }

    public List<GhostShot> GetFrames()
    {
        return frames; 
    }

    public void ClearFrames()
    {
        frames.Clear(); 
    }
}
