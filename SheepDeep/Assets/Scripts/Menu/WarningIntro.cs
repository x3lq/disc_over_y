using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarningIntro : MonoBehaviour {


	public void onAnimationEnd() {
		SceneManager.LoadScene(1);
	}
}
