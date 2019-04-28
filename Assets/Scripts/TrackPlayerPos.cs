using UnityEngine;

public class TrackPlayerPos : MonoBehaviour
{
    public Vector2Event playerPos;

    private Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private Vector2 playerPosition;
    void Update()
    {
        if (player != null)
        {
            playerPosition = player.transform.position;
        }
        playerPos.Invoke(playerPosition);
    }
}
