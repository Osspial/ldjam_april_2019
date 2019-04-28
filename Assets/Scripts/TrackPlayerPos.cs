using UnityEngine;

public class TrackPlayerPos : MonoBehaviour
{
    public Vector2Event playerPos;

    private Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        playerPos.Invoke(player.transform.position);
    }
}
