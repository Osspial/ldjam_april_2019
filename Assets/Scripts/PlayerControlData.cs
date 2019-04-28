using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovement", menuName = "PlayerControl", order = 1)]
public class PlayerControlData : ScriptableObject
{
    public float walkingDrag = 2;
    public float neutralDrag = 4;
    public float moveSpeedMultiplier = 35;
    [Tooltip("max speed in relation to time since movement start")]
    public AnimationCurve maxSpeedCurve;
}
