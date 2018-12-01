using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour {

    private Animator animator;

    private Renderer renderer;

    public bool hasWool;

    public float growWoolTime;

	// Use this for initialization
	void Start () {
        renderer = GetComponent<Renderer>();
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool SheppardInteraction()
    {
        if (hasWool)
        {
            hasWool = false;

            animator.Play("Shear");

            return true;
        }
        else
        {
            return false;
        }
    }

    private void GrowWool()
    {
        hasWool = true;

        animator.Play("Movement");
    }

    public void WolfInteraction()
    {
        animator.Play("Death");
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void GiveBirth(Vector3 position) {
        GameObject newBorn = Instantiate(SheepManager.GetManager().sheepPrefab, position, Quaternion.identity);
        newBorn.transform.parent = SheepManager.GetManager().herd.transform;
        SheepManager.GetManager().sheeps.Add(newBorn);
    }

    public void Death()
    {
        SheepManager.GetManager().sheeps.Remove(gameObject);
        Destroy(gameObject);
    }
}
