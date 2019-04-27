using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovement", menuName = "PlayerControl", order = 1)]
public class PlayerControlData : ScriptableObject
{
    public float linearDrag = 4;
    public float moveSpeedMultiplier = 35;
}
