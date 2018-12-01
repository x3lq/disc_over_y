using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepManager : MonoBehaviour {

	private static SheepManager instance;
	public List<GameObject> sheeps;
    public static int numOfPregnantSheeps = 0;
    public static int numOfPregnantSheepsAllowed = 5;
	//anchor point is bottom left
	public int x_maxSize, y_maxSize;
	public GameObject sheepPrefab; 
	public GameObject herd;
    //public GameObject deathTimer;

    public static float newBornSpawnPositionOffset = 0.3f;
    public static float getPregnantUpperTime = 10.0f;
    public static float getPregnantLowerTime = 1.0f;

    public static float growWoolUpperTime = 5.0f;
    public static float growWoolLowerTime = 1.0f;

    public float deathInChildBirth = 5.0f;

    [Range(0, 0.9f)]
	public float distanceToBorder;
	public int amount;
    

	public int randomSpawnMedian;
	public float randomSpawnVariance;
	private float spawnTimer;

    #region crowdMovementVariables
    [Range(0.1f, 20.0f)]
    public float velocity = 6.0f;

    [Range(0.0f, 0.9f)]
    public float velocityVariation = 0.5f;

    [Range(0.1f, 20.0f)]
    public float rotationCoeff = 4.0f;

    [Range(0.1f, 10.0f)]
    public float neighborDist = 2.0f;

    private Vector2 targetPosition = Vector2.zero;
    #endregion

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

	void FixedUpdate() {
        if(numOfPregnantSheeps <= numOfPregnantSheepsAllowed)
        {
            //Debug.Log(sheeps.Count);
            int i = Random.Range(0, sheeps.Count-1);
            sheeps[i].GetComponent<Sheep>().StartCoroutine(sheeps[i].GetComponent<Sheep>().GetPregnant());
            numOfPregnantSheeps++;
        }

		/*if(spawnTimer > 0) {
			spawnTimer -= Time.deltaTime;
		}else {
			spawnTimer = randomSpawnMedian + Random.Range(-1, 1) * randomSpawnVariance * randomSpawnMedian;
			/*GameObject randomSheep = sheeps[Random.Range(0, sheeps.Count)];
			Sheep sheep = randomSheep.GetComponent("Sheep") as Sheep;

			if(sheep != null) {
				sheep.GiveBirth(randomSheep.transform.position);
			}*/
		}
	}
	
	void Update() {
	    int movementMotivation = Random.Range(0, 20);
        if(movementMotivation == 19)
        {
            chooseNewTarget();
        }
	}

	private int randomCoordinate(int maxSize) {
		return Random.Range((0 + (int) (maxSize*distanceToBorder)), maxSize - (int) (maxSize*distanceToBorder));
	}

	public static SheepManager GetManager() {
		return instance;
	}

    public Vector2 getTargetPosition()
    {
        return targetPosition;
    }

    private void chooseNewTarget()
    {
        targetPosition = new Vector2(Random.Range(-x_maxSize, x_maxSize + 1), Random.Range(-y_maxSize, y_maxSize + 1));
        StartCoroutine("DeleteSheepTarget");
    }

    IEnumerator DeleteSheepTarget()
    {
        yield return new WaitForSeconds(15f);
        targetPosition = Vector2.zero;
    }
}