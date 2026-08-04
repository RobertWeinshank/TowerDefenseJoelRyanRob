using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Collections.Generic;

public class TimelineController : MonoBehaviour
{
    public PlayableDirector director;
    private List<double> markerTimes = new List<double>();

    void Start()
    {
        FetchMarkers();
    }

    // Finds all markers placed on the timeline and sorts them by time
    private void FetchMarkers()
    {
        if (director == null || director.playableAsset == null) return;
        
        TimelineAsset timeline = director.playableAsset as TimelineAsset;
        if (timeline == null) return;

        markerTimes.Clear();

        // Check global marker track
        foreach (var marker in timeline.markerTrack.GetMarkers())
        {
            markerTimes.Add(marker.time);
        }

        // Check markers on individual tracks
        foreach (var track in timeline.GetOutputTracks())
        {
            foreach (var marker in track.GetMarkers())
            {
                markerTimes.Add(marker.time);
            }
        }

        markerTimes.Sort();
    }

    // Call this from your "Next" Button OnClick event
    public void JumpToNextElement()
    {
        if (director == null || markerTimes.Count == 0) return;

        double currentTime = director.time;

        // Find the first marker that is ahead of the current time
        foreach (double markerTime in markerTimes)
        {
            // Adding a small buffer (0.05s) prevents getting stuck on the current marker
            if (markerTime > currentTime + 0.05f)
            {
                director.time = markerTime;
                director.Evaluate(); // Force Timeline to instantly update the scene visuals
                return;
            }
        }

        // Optional: If no more markers are left, let the timeline play out or finish
    }
}
