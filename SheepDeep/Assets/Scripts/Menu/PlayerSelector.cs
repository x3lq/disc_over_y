using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerSelector : MonoBehaviour {

    public class PlayerObject {
        public Color color;
        public int playerID;
    }

    public enum Color {
        BLUE, RED, GREEN, WOLF
    }
    public int maxPlayers = 4;

    public static List<PlayerObject> activePlayers;
    public Sprite redShepherdSprite, blueShepherdSprite, greenShepherdSprite, wolfSprite;
    public GameObject[] playerImages;
    public Image[] buttons;
    public Sprite[] enabled, disabled;
    public bool[] active = new bool[4];

	// Use this for initialization
	void Start () {
        activePlayers = new List<PlayerObject>();
	}
	
	// Update is called once per frame
	void Update () {
        for(int i=1; i<= maxPlayers; i++){
		    if(Input.GetKeyDown("joystick " + i + " button 7")) {
                startGame();
            }
        }

        for(int i=1; i<= maxPlayers; i++){
		    if(Input.GetKeyDown("joystick " + i + " button 0")) {
                if(active[i -1]) {
                    deselectPlayer(i);
                } else {
                    spawnNewPlayer(i);
                }
            }
        }

	}

    private void startGame() {
        int i=0;
        foreach(bool b in active) {
            if(b) {
                i++;
            }
        }

        if(active[0]) {
            PlayerObject one = new PlayerObject();
            one.playerID = 1;
            activePlayers.Add(one);
        }

        if(active[1]) {
            PlayerObject one = new PlayerObject();
            one.playerID = 2;
            activePlayers.Add(one);
        }

        if(active[2]) {
            PlayerObject one = new PlayerObject();
            one.playerID = 2;
            activePlayers.Add(one);
        }

        if(active[3]) {
            PlayerObject one = new PlayerObject();
            one.playerID = 3;
            activePlayers.Add(one);
        }

        if(i > 2) {
            int ran = (int) (Random.Range(0, activePlayers.Count));
            activePlayers[ran].color = Color.WOLF;
            SceneManager.LoadScene(2);
        }
    }

    private void spawnNewPlayer(int id) {
        buttons[id - 1].sprite = enabled[id - 1]; 
        active[id -1] = true;
        playerImages[id - 1].SetActive(true);
    }

    private void deselectPlayer(int id) {
        buttons[id - 1].sprite = disabled[id - 1];
        active[id -1] = false;
        playerImages[id - 1].SetActive(false); 
    }  
}
