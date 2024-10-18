using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPositionP1;
    [SerializeField] private Transform spawnPositionP2;
    [SerializeField] private Transform spawnPositionP3;
    [SerializeField] private Transform spawnPositionP4;

    private void Awake()
    {
        SpawnPlayer("P1", spawnPositionP1.position);
        SpawnPlayer("P2", spawnPositionP2.position, Color.red);
        SpawnPlayer("P3", spawnPositionP3.position, Color.yellow);
        SpawnPlayer("P4", spawnPositionP4.position, Color.green);
    }

    private void SpawnPlayer(string name, Vector2 position)
    {
        SpawnPlayer(name, position, Color.white);
    }


    private void SpawnPlayer(string name, Vector2 position, Color cor)
    {
        GameObject player = Instantiate(playerPrefab, position, Quaternion.identity);
        player.name = name;
        SpriteRenderer playerRenderer = player.GetComponent<SpriteRenderer>();
        playerRenderer.color = cor;
    }
}
