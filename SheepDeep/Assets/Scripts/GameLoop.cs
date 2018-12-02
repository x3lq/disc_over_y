using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using UnityEngine.UI;

public class GameLoop : MonoBehaviour {

	public int wolfBites = 0, shepherdBirth = 0;
	static GameLoop instance;
	public Image faderImage;
	public static FinishScreen.WinType winType; 
	public int wolfWin, shepartWin;

	private static Stopwatch stopwatch = new Stopwatch();
	// Use this for initialization
	void Start () {
		if(instance == null) {
			instance = this;
		} else {
			Destroy (this);
		}
		//stopwatch = new Stopwatch();
		stopwatch.Start();
	}
	
	// Update is called once per frame
	void Update () {
		int numberOfSheeps = SheepManager.GetManager().sheeps.Count;
		UnityEngine.Debug.Log(numberOfSheeps);
		if(numberOfSheeps >= shepartWin) {
			shepartWins();
		}

		if(numberOfSheeps <= wolfWin) {
			wolfWins();
		}
	}

	private void shepartWins(){
		stopwatch.Stop();
		winType = new FinishScreen.WinType();
		winType.shepparWin = true;
		winType.populationWin = true;
		winType.elapsedTime = stopwatch.Elapsed.Seconds;
		winType.bites = wolfBites;
		winType.born = shepherdBirth;
		StartCoroutine(transitionToHighScore());
	}

	private void wolfWins() {
		stopwatch.Stop();
		winType = new FinishScreen.WinType();
		winType.shepparWin = false;
		winType.populationWin = true;
		winType.elapsedTime = stopwatch.Elapsed.Seconds;
		winType.bites = wolfBites;
		winType.born = shepherdBirth;
		StartCoroutine(transitionToHighScore());
	}

	public static void wolfCaught() {
		stopwatch.Stop();
		winType = new FinishScreen.WinType();
		winType.shepparWin = true;
		winType.populationWin = false;
		winType.elapsedTime = stopwatch.Elapsed.Seconds;
		winType.bites = instance.wolfBites;
		winType.born = instance.shepherdBirth;
		instance.StartCoroutine(instance.transitionToHighScore());
	}

	IEnumerator transitionToHighScore() {

		while(faderImage.color.a < 0.98) {
			float a = faderImage.color.a + Time.deltaTime;
			faderImage.color = new Color(0, 0, 0, a);
			yield return null;
		}
		SceneManager.LoadScene(3);
	}

	public static GameLoop getInstance() {
		return instance;
	}
}
