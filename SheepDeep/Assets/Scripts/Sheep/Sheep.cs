using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour {

    private Renderer renderer;

    public bool hasWool;

    public float growWoolTime;

	// Use this for initialization
	void Start () {
        renderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool SheppardInteraction()
    {
        if (hasWool)
        {
            hasWool = false;

            renderer.material.color = Color.white * 0.5f;

            StartCoroutine(GrowWool());

            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator GrowWool()
    {
        yield return new WaitForSeconds(growWoolTime);

        hasWool = true;

        renderer.material.color = Color.white;
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
