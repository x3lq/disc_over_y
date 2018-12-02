using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinishScreen : MonoBehaviour {

	public static WinType winner;
	private HighScoreManager highScoreManager;
	private ArrayList topTen;
	public GameObject sheppardWinKill, sheppardWinPopulation, wolfWin;
	public GameObject highScoreListPrefab, HighScoreList, highScoreInputList, highscoreTitle, highScoreImage;

	public GameObject uiImageBorn, uiTextBorn, uiImageDead, uiTextDead;
	private HighScoreManager.HighScoreType highScoreType;

	public Sprite sheep, wolf;
	private bool startPressed = false;
	private string name = "";
	public class WinType {
		public bool populationWin, shepparWin;
		public int winnerId;
		public int elapsedTime;

		public int born, bites;
	}

	void Awake()
	{
		highScoreManager = new HighScoreManager();
		topTen = highScoreManager.getHighScores(10, highScoreType);
        //winner = new WinType();
        //winner.winnerId = 1;
        //winner.populationWin = true;
        //winner.elapsedTime = 5;
        //winner.born = 15;
        //winner.bites = 10;
	}
	void Start () {
		winner = GameLoop.winType;

		if(winner.shepparWin) {
			if(winner.populationWin) {
				sheppardWinPopulation.SetActive(true);
			}else {
				sheppardWinKill.SetActive(true);
			}
		}else {
			wolfWin.SetActive(true);
		}

		setWinScreen(winner);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("joystick " + 1 + " button 7") || 
		Input.GetKeyDown("joystick " + 2 + " button 7") || 
		Input.GetKeyDown("joystick " + 3 + " button 7") ||
		Input.GetKeyDown("joystick " + 4 + " button 7"))
        {
			//HighScoreManager.HighScoreObject newHighScore = new HighScoreManager.HighScoreObject();
			//newHighScore.name = name;
			//newHighScore.elapsedtime = winner.elapsedTime;
			//highScoreManager.insertIntoTable(newHighScore, highScoreType);
            SceneManager.LoadScene(0);
        }
	}

	private void setWinScreen(WinType winner) {
		if(winner.shepparWin && winner.populationWin) {
			sheppardWinPopulation.SetActive(true);
			highScoreType = HighScoreManager.HighScoreType.SHEPARD;

		} else if(winner.shepparWin && !winner.populationWin) {
			sheppardWinKill.SetActive(true);
			highScoreType = HighScoreManager.HighScoreType.SHEPARD;
		}else {
			wolfWin.SetActive(true);
			highScoreType = HighScoreManager.HighScoreType.WOLF;
		}

	}

	public void showHighScore(string name) {
		this.name = name;
		int i = 0;

		foreach ( HighScoreManager.HighScoreObject highScore in topTen) {
			if(i == 9) {
				break;
			}

			if(highScore.elapsedtime < winner.elapsedTime) {
				newHighScoreItem(name, timeStringFromSecondes(winner.elapsedTime), i);

				i++;
			}

			newHighScoreItem(highScore.name, timeStringFromSecondes(highScore.elapsedtime), i);

			Destroy(HighScoreList.transform.GetChild(0).gameObject);
			i++;
		}

		if(topTen.Count == 0) {

			if(HighScoreList.transform.childCount > 0){
				Destroy (HighScoreList.transform.GetChild(0).gameObject);
			}
			newHighScoreItem(name, timeStringFromSecondes(winner.elapsedTime), i);
		}
	}

	public void setStartPressed() {
		startPressed = true;
		highScoreInputList.SetActive(false);
	}

	public void newHighScoreItem(string name, string time, int pos) {
			GameObject newItem = Instantiate(highScoreListPrefab, HighScoreList.transform.position, Quaternion.identity) as GameObject;
			newItem.transform.parent = HighScoreList.transform;
			newItem.transform.localPosition -= new Vector3(0, 50*pos, 0);
			newItem.GetComponent<HighScoreListItem>().setText(name, time);
	}

	private string timeStringFromSecondes(int secondes) {
		int min = secondes / 60;
		secondes = secondes % 60;

		return min + ":" + secondes;
	}

	public void onAnimationEnd() {
		Debug.Log("Animation End");

		/* if(topTen.Count < 10 ) {
			highScoreInputList.SetActive(true);
			showHighScore(this.name);
		}else {
			bool isInList = false;
			foreach ( HighScoreManager.HighScoreObject highScore in topTen) {
				if(highScore.elapsedtime < winner.elapsedTime) {
					highScoreInputList.SetActive(true);
					isInList = true;
				}
			}

			if(!isInList) {
				startPressed = true;
			}

			showHighScore(this.name);
		}

		selectHighScoreImage();
		highscoreTitle.SetActive(true);
		highScoreImage.SetActive(true);*/
		uiTextBorn.GetComponent<Text>().text = "" + winner.born;
        uiTextDead.GetComponent<Text>().text = "" + winner.bites;
        
		uiImageBorn.SetActive(true);
		uiTextBorn.SetActive(true);
		uiImageDead.SetActive(true);
		uiTextDead.SetActive(true);

        wolfWin.SetActive(false);
        sheppardWinKill.SetActive(false);
        sheppardWinPopulation.SetActive(false);
	}

	private void selectHighScoreImage() {
		if(winner.shepparWin){
			highScoreImage.GetComponent<Image>().sprite = sheep;
		}else {
			highScoreImage.GetComponent<Image>().sprite = wolf;
		}
	}

}
