using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 distanceFromPlayer;

    void Start()
    {
        distanceFromPlayer = transform.position - player.position; //keeps initial player-camera distance
    }

    void LateUpdate()
    {
        transform.position = player.position + distanceFromPlayer;
    }
}
