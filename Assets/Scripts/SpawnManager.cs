using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        player.transform.position = new Vector2(0f, -7f);
    }
}
