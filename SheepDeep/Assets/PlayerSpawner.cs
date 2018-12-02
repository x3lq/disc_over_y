using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{

    public int maxPlayers = 4;

    public List<PlayerSelector.PlayerObject> activePlayers;

    public GameObject redShepherdPrefab, blueShepherdPrefab, greenShepherdPrefab, wolfPrefab;

    public float spawnFieldSize;

    // Use this for initialization
    void Start()
    {
        AddPlayersToScene();
        //
        //Player redPlayer = Instantiate(redShepherdPrefab, transform.position, Quaternion.identity).GetComponent<Player>();
        //redPlayer.playerID = 1;
        //
        //Player newPlayer = Instantiate(wolfPrefab, transform.position + new Vector3(5, 5), Quaternion.identity).GetComponent<Player>();
        //newPlayer.playerID = 2;
        //
        //CameraBehavior.playerObjects = new List<Player>();
        //CameraBehavior.playerObjects.Add(newPlayer);
        //CameraBehavior.playerObjects.Add(redPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.PlayBirthShepherd();
        }
    }

    void AddPlayersToScene()
    {
        CameraBehavior.playerObjects = new List<Player>();

        activePlayers = PlayerSelector.activePlayers;

        Debug.Log(PlayerSelector.activePlayers.Count);

        int color = 0;

        foreach(PlayerSelector.PlayerObject player in activePlayers)
        {
            switch (player.color)
            {
                case PlayerSelector.Color.WOLF:
                    Player newPlayer = Instantiate(wolfPrefab, transform.position + new Vector3(5, 5), Quaternion.identity).GetComponent<Player>();
                    newPlayer.playerID = player.playerID;
                    CameraBehavior.playerObjects.Add(newPlayer);
                    break;
                default:
                    switch (color)
                    {
                        case 0:
                            Player greenPlayer = Instantiate(greenShepherdPrefab, transform.position, Quaternion.identity).GetComponent<Player>();
                            greenPlayer.playerID = player.playerID;
                            CameraBehavior.playerObjects.Add(greenPlayer);
                            break;
                        case 1:
                            Player redPlayer = Instantiate(redShepherdPrefab, transform.position, Quaternion.identity).GetComponent<Player>();
                            redPlayer.playerID = player.playerID;
                            CameraBehavior.playerObjects.Add(redPlayer);
                            break;
                        default:
                            Player bluePlayer = Instantiate(blueShepherdPrefab, transform.position, Quaternion.identity).GetComponent<Player>();
                            bluePlayer.playerID = player.playerID;
                            CameraBehavior.playerObjects.Add(bluePlayer);
                            break;
                    }
                    color++;
                    break;
            }
        }
    }

    void SetSpawnPosition(Player player)
    {
        player.transform.position = new Vector2(Random.Range(-spawnFieldSize, spawnFieldSize), Random.Range(-spawnFieldSize, spawnFieldSize));
    }
}