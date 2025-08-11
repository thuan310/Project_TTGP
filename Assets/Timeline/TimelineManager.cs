using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour {
    public static TimelineManager instance;
    public bool isPausedBySignal = false;

    [SerializeField] private PlayableDirector director;
    [SerializeField] private List<TimelineEntry> timelineAssets;



    private bool skipNextPauseSignal = false;


    [SerializeField] private CanvasGroup blackScreen;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        instance = this;
        director = GetComponent<PlayableDirector>();
    }

    [System.Serializable]
    public class TimelineEntry {
        public string name;
        public TimelineAsset asset;
    }

    public void PlayTimeline(string name)
    {
        var entry = timelineAssets.Find(t => t.name == name);
        if (entry != null && entry.asset != null)
        {
            StartCoroutine(PlayTimelineWithFade(entry.asset));

        }
        else
        {
            Debug.LogWarning("Timeline not found: " + name);
        }
    }

    private IEnumerator PlayTimelineWithFade(TimelineAsset asset)
    {
        // Fade to black
        LeanTween.alphaCanvas(blackScreen, 1f, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);

        director.Play(asset);

        // Fade back in
        LeanTween.alphaCanvas(blackScreen, 0f, fadeDuration);
    }

    public void PauseTimeline()
    {
        if (skipNextPauseSignal)
        {
            skipNextPauseSignal = false;
            return;
        }

        if (director.state == PlayState.Playing)
        {
            //director.Pause();
            director.playableGraph.GetRootPlayable(0).SetSpeed(0); // Dừng
            isPausedBySignal = true;
            Debug.Log("Timeline paused by signal.");
        }
    }

    public void ResumeTimeline()
    {
        if (isPausedBySignal)
        {
            //director.Resume();
            director.playableGraph.GetRootPlayable(0).SetSpeed(1);
            isPausedBySignal = false;
            Debug.Log("Timeline resumed.");
        }
    }

    public bool IsTimeLinePlaying()
    {
        return !isPausedBySignal;
    }

    public void SkipToNextPause()
    {
        TimelineAsset timeline = director.playableAsset as TimelineAsset;
        if (timeline == null) return;

        double currentTime = director.time;
        double? nextPauseTime = null;

        foreach (var track in timeline.GetOutputTracks())
        {
            if (track is SignalTrack)
            {
                foreach (var marker in track.GetMarkers())
                {
                    if (marker is SignalEmitter signalEmitter && signalEmitter.asset != null)
                    {
                        if (signalEmitter.asset.name == "PauseSignal") // Or compare with a specific asset
                        {
                            if (signalEmitter.time > currentTime)
                            {
                                if (nextPauseTime == null || signalEmitter.time < nextPauseTime)
                                {
                                    nextPauseTime = signalEmitter.time;
                                }
                            }
                        }
                    }
                }
            }
        }

        if (nextPauseTime != null)
        {
            skipNextPauseSignal = true;
            director.time = (double)nextPauseTime;
            director.Evaluate();


        }
        else
        {

            ResumeTimeline();
        }
    }

    void Start()
    {
        // Step 1: Lấy Signal Receiver
        var sceneNavigationReceiver = SceneNavigationManager.instance.gameObject.GetComponent<SignalReceiver>();
        if (sceneNavigationReceiver == null)
        {
            Debug.LogError("no sceneNavigationReceiver reference");
            return;
        }

        // Step 2: Gán vào Signal Emitter
        // (Giả sử bạn đã tạo Signal Asset và Emitter sẵn trong Timeline)
        TimelineAsset timeline = timelineAssets[0].asset;
        if (timeline == null)
        {
            Debug.LogError("NO TimeLineAssest");
            return;
        }

        foreach (var track in timeline.GetOutputTracks())
        {
            if (track is SignalTrack signalTrack)
            {
                if (track.name == "ChangeSceneTrack")
                {
                    director.SetGenericBinding(signalTrack, sceneNavigationReceiver);
                }
                //if (track.name == "TutorialTriggerTrack")
                //{
                //    director.SetGenericBinding(signalTrack, tutorialTriggerReceiver);
                //}
            }
        }
    }

}
