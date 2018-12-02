using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour {

    public RectTransform label;

    public float size = 4;
    public float percentage;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        int sheepCount = SheepManager.GetManager().sheeps.Count;
        
        label.pivot = new Vector2(percentage * size * 2 - size + 0.5f, 0.5f);
    }
}
