using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RandomizeAnimationTime : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        IEnumerator Randomize()
        {
            yield return null;
            var animationState = animator.GetCurrentAnimatorStateInfo(0);
            animator.Play(animationState.fullPathHash, 0, Random.value);
            enabled = false;
        }

        StartCoroutine(Randomize());
    }
}
