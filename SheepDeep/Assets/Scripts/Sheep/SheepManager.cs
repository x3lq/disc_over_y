using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepManager : MonoBehaviour {

	private static SheepManager instance;
	public List<GameObject> sheeps;
	//anchor point is bottom left
	public int x_maxSize, y_maxSize;
	public GameObject sheepPrefab; 
	public GameObject herd;

	public float distanceToBorder;
	public int amount;

	public int randomSpawnMedian;
	public float randomSpawnVariance;
	private float spawnTimer;

	void Awake()
	{
		if(instance == null){
			instance = this;		
		} else if (instance != this) {
			Destroy(this);
		}
	}
	void Start () {

		sheeps = new List<GameObject>();
		for(int i=0; i<amount; i++) {
			GameObject sheep = GameObject.Instantiate(sheepPrefab, new Vector3(randomCoordinate(x_maxSize),
					randomCoordinate(y_maxSize),0), Quaternion.identity);
			sheep.transform.parent = herd.transform;
			sheeps.Add(sheep);
		}
	}

	void Update() {

		if(spawnTimer > 0) {
			spawnTimer -= Time.deltaTime;
		}else {
			spawnTimer = randomSpawnMedian + Random.Range(-1, 1) * randomSpawnVariance * randomSpawnMedian;
			GameObject randomSheep = sheeps[Random.Range(0, sheeps.Count)];
			Sheep sheep = randomSheep.GetComponent("Sheep") as Sheep;

			if(sheep != null) {
				sheep.GiveBirth(randomSheep.transform.position);
			}
		}
	}

	private int randomCoordinate(int maxSize) {
		return Random.Range((0 + (int) (maxSize*distanceToBorder)), maxSize - (int) (maxSize*distanceToBorder));
	}

	public static SheepManager GetManager() {
		return instance;
	}
}
