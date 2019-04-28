using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class NoticeMeter : MonoBehaviour
{
    private AudioSource source;
    private Animator animator;
    public AudioClip notice0;
    public AudioClip notice1;
    public AudioClip notice2;
    public AudioClip notice0undo;
    public AudioClip notice1undo;
    // public AudioClip notice2undo;
    [SerializeField]
    private bool Noticing = false;
    public bool noticing
    {
        get => Noticing;
        set => Noticing = value;
    }
    private bool noticed = false;
    public UnityEvent onNotice;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    public void PlayNotice0()
    {
        if (animator.GetFloat("NoticeSpeed") < 0)
        {
            source.PlayOneShot(notice0undo);
        }
        else
        {
            source.PlayOneShot(notice0);
        }
    }

    public void PlayNotice1()
    {
        if (animator.GetFloat("NoticeSpeed") < 0)
        {
            source.PlayOneShot(notice1undo);
        }
        else
        {
            source.PlayOneShot(notice1);
        }
    }

    public void Notice()
    {
        noticed = true;
        source.PlayOneShot(notice2);
        onNotice.Invoke();
    }

    void Update()
    {
        var animationState = animator.GetCurrentAnimatorStateInfo(0);
        if (animationState.normalizedTime < 0)
        {
            animator.Play(animationState.fullPathHash, 0, 0);
        }
        if (noticing || noticed)
        {
            animator.SetFloat("NoticeSpeed", 1.0f);
        }
        else
        {
            animator.SetFloat("NoticeSpeed", -1.0f);
        }
    }
}
