using System;
using UnityEngine;

[Serializable]
public struct MusicTrack
{
    public string trackID;
    public AudioClip clip;
}
public class MusicLibrary : MonoBehaviour
{
    public MusicTrack[] tracks;
    
    public AudioClip GetClipFromName(string name)
    {
        foreach (var track in tracks)
        {
            if (track.trackID == name)
            {
                return track.clip;
            }
        }
        return null;
    }
}
