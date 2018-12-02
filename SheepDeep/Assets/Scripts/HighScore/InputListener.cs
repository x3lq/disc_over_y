using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
public class InputListener : MonoBehaviour {

	public int posX, posY;
	private string name = "";
	// Use this for initialization
	public float blinkTimer;
	public GameObject cursor;

	private float inputBlocker;
	public float inputBlockerTimer;

	public int winnerId;
	private bool lockedInput;
	void Start () {
		StartCoroutine(cursorBlinking());
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("joystick " + winnerId + " button 7"))
        {
            lockedInput = true;
			GetComponent<FinishScreen>().setStartPressed();
        }

		if(lockedInput) {{
			return;
		}}

		if(Input.GetKeyDown("joystick " + winnerId + " button 0")) {
			if(posX == 2 && posY == 8) {
				if(name.Length > 0) {
					name = name.Substring(0, name.Length - 1);
				}
			}else {
				name = name + (char) (65 + 3* posY + posX);
			}

			GetComponent<FinishScreen>().showHighScore(name);
			Debug.Log(name);
		}

		if(inputBlocker > 0) {
			inputBlocker -= Time.deltaTime;
			return;
		}

		inputBlocker = inputBlockerTimer;

		winnerId = FinishScreen.winner.winnerId;
		float horizontalInput = -Input.GetAxisRaw("HorizontalP" + winnerId);
		float verticalInput = -Input.GetAxisRaw("VerticalP" + winnerId);
		
		if(horizontalInput != 0 && verticalInput != 0) {
			if(verticalInput < horizontalInput) {
				verticalInput = 0;
			}else {
				horizontalInput = 0;
			}
		}
		
		if(horizontalInput != 0) {
			if (horizontalInput > 0) {
				posX = (posX + 1) % 3;
			} else {
				posX = (posX - 1)  % 3;;

				if(posX < 0) {
					posX = 2;
				}
			}
		}

		if(verticalInput != 0) {
			if (verticalInput > 0) {
				posY = (posY + 1) % 9;
			} else {
				posY = (posY - 1) % 9;

				if(posY < 0) {
					posY = 8;
				}
			}
		}

		cursor.transform.localPosition = new Vector3( -51 + posX * 50 , -50 - posY * 50 ,0);
	}

	IEnumerator cursorBlinking() {
		
		while(true) {
			cursor.SetActive(false);
			yield return new WaitForSeconds(blinkTimer);
			cursor.SetActive(true);
			yield return new WaitForSeconds(blinkTimer);
		}
	}
}
