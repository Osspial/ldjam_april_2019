using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SatanTalkToggle : MonoBehaviour
{
    public MusicTrack theme;
    public void ToggleTalk(bool talk)
    {
        GetComponent<Animator>().SetBool("Talking", talk);
    }

    public void StartTheme()
    {
        FindObjectOfType<MusicController>().StartTrack(theme);
    }

    public void KillVelocity()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void FadeOutTheme()
    {
        FindObjectOfType<MusicController>().StartTrack(null);
    }

    public void StartGameAgain(MusicTrack track)
    {
        FindObjectOfType<MusicController>().StartTrack(track);
        var player = FindObjectOfType<Player>();
        player.enabled = true;
        player.GetComponent<Health>().maxHealth += 2;
        FindObjectOfType<CheckpointController>().Save();
    }
}
