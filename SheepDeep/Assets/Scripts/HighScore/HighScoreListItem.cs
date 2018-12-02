using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreListItem : MonoBehaviour {

	public GameObject name, score;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setText(string name, string score) {
		this.name.GetComponent<Text>().text = name;
		this.score.GetComponent<Text>().text = score;
	}
}
