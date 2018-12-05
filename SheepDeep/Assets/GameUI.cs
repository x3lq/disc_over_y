using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

    public RectTransform shepherdsLabel;
    public RectTransform wolfsLabel;

    public Image wolfIcon;
    public Image shepherdsIcon;

    public float size = 4;
    public float percentage;
    public int fakeCount;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        int sheepCount = SheepManager.GetManager().sheeps.Count;
        int wolfWin = GameLoop.getInstance().wolfWin;
        int shepherdWin = GameLoop.getInstance().shepartWin;

        percentage = ((float)(sheepCount - wolfWin)) / (shepherdWin - wolfWin);

        if(percentage < 0.5f)
        {
            wolfIcon.enabled = true;
            shepherdsIcon.enabled = false;
        }
        else
        {
            wolfIcon.enabled = false;
            shepherdsIcon.enabled = true;
        }

        shepherdsLabel.pivot = new Vector2(percentage * size * 2 - size + 0.5f, 0.5f);
        wolfsLabel.pivot = new Vector2(percentage * size * 2 - size + 0.5f, 0.5f);
    }
}
