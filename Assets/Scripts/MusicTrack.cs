using UnityEngine;

[CreateAssetMenu(fileName = "Track", menuName = "Music Track", order = 1)]
public class MusicTrack : ScriptableObject
{
    public float introTime;
    public AudioClip intro;
    public float bodyTime;
    public AudioClip body;
    public AudioClip outro;
    public float fadeOut = 0;
}
