using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepSpawner : MonoBehaviour {

	public static ArrayList sheeps;
	//anchor point is bottom left
	public int x_maxSize, y_maxSize;
	public GameObject sheepPrefab; 
	public GameObject herd;

	public float distanceToBorder;
	public int amount;

	// Use this for initialization
	void Start () {
		sheeps = new ArrayList();

		for(int i=0; i<amount; i++) {
			GameObject sheep = GameObject.Instantiate(sheepPrefab, new Vector3(randomCoordinate(x_maxSize), randomCoordinate(y_maxSize),0), Quaternion.identity);
			sheep.transform.parent = herd.transform;
		}
	}

	public static ArrayList getSheeps() {
		return sheeps;
	}

	private int randomCoordinate(int maxSize) {
		return Random.Range((0 + (int) (maxSize*distanceToBorder)), maxSize - (int) (maxSize*distanceToBorder));
	}
}
