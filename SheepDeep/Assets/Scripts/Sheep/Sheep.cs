using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SheppardInteraction()
    {
        Destroy(gameObject);
    }

    public void WolfInteraction()
    {
        Destroy(gameObject);
    }

    public void GiveBirth(Vector3 position) {
        GameObject newBorn = Instantiate(SheepManager.GetManager().sheepPrefab, position, Quaternion.identity);
        newBorn.transform.parent = SheepManager.GetManager().herd.transform;
        SheepManager.GetManager().sheeps.Add(newBorn);
        Debug.Log("New Sheep");
    }
}
