using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelector : MonoBehaviour
{

    public int maxPlayers = 4;

    public List<int> activePlayers;

    public GameObject redShepherdPrefab, blueShepherdPrefab, greenShepherdPrefab, wolfPrefab;

    public Vector2 spawnFieldSize;

    // Use this for initialization
    void Start()
    {
        AddPlayersToScene();
        spawnFieldSize = new Vector2(SheepManager.x_maxSize * (1 - SheepManager.distanceToBorder), SheepManager.y_maxSize * (1 - SheepManager.distanceToBorder)) / 2f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void AddPlayersToScene()
    {
        CameraBehavior.playerObjects = new List<Player>();

        int wolfPlayerID = Random.Range(1, maxPlayers + 1);
        while (!activePlayers.Contains(wolfPlayerID))
        {
            wolfPlayerID = Random.Range(1, maxPlayers + 1);
        }

        int shepherdCount = 0;

        for (int i = 1; i <= maxPlayers; i++)
        {
            if (!activePlayers.Contains(i)) { continue; }

            Player newPlayer;
            if (wolfPlayerID == i)
            {
                newPlayer = Instantiate(wolfPrefab).GetComponent<Player>();
                newPlayer.isWolf = true;
            }
            else
            {
                GameObject shepherd;

                switch (shepherdCount)
                {
                    case 0:
                        shepherd = redShepherdPrefab;
                        break;
                    case 1:
                        shepherd = blueShepherdPrefab;
                        break;
                    case 2:
                        shepherd = greenShepherdPrefab;
                        break;
                    default:
                        shepherd = redShepherdPrefab;
                        break;
                }

                newPlayer = Instantiate(shepherd).GetComponent<Player>();

                shepherdCount++;
            }

            CameraBehavior.playerObjects.Add(newPlayer);

            newPlayer.playerID = i;

            SetSpawnPosition(newPlayer);
        }
    }

    void SetSpawnPosition(Player player)
    {
        if (player.isWolf)
        {
            player.transform.position = new Vector2(Random.Range(-spawnFieldSize.x, spawnFieldSize.x), Random.Range(-spawnFieldSize.y, spawnFieldSize.y));
        }
        else
        {
            float x_pos = ((player.playerID % 2) * 2 - 1) * spawnFieldSize.x;
            float y_pos = ((player.playerID / 2) * 2 - 1) * spawnFieldSize.y;
            player.transform.position = new Vector2(x_pos, y_pos);
        }
    }
}
