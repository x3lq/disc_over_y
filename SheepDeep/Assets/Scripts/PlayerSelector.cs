using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelector : MonoBehaviour {

    public int maxPlayers = 4;

    public List<int> activePlayers;

    public GameObject sheppardPrefab, wolfPrefab;

    public float spawnFieldSize;

	// Use this for initialization
	void Start () {
        AddPlayersToScene();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void AddPlayersToScene()
    {
        int wolfPlayerID = Random.Range(1,maxPlayers+1);
        while (!activePlayers.Contains(wolfPlayerID)){
            wolfPlayerID = Random.Range(1, maxPlayers+1);
        }

        for(int i=1; i<=maxPlayers; i++)
        {
            if (!activePlayers.Contains(i)) { continue; }

            Player newPlayer;
            if (wolfPlayerID == i)
            {
                newPlayer = Instantiate(wolfPrefab).GetComponent<Player>();
            }
            else
            {
                newPlayer = Instantiate(sheppardPrefab).GetComponent<Player>();
            }

            newPlayer.playerID = i;

            SetSpawnPosition(newPlayer);
        }
    }

    void SetSpawnPosition(Player player)
    {
        player.transform.position = new Vector2(Random.Range(-spawnFieldSize, spawnFieldSize), Random.Range(-spawnFieldSize, spawnFieldSize));
    }
}
