using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour {

    private Animator animator;

    private Renderer renderer;

    public bool hasWool;
    public bool hasBaby;
    public float getPregnantTime;
    GameObject deathTimer;
   
    

	// Use this for initialization
	void Start () {
        //StartCoroutine(GetPregnant());
        renderer = GetComponent<Renderer>();
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    public bool SheppardInteraction()
    {
        //pregnant sheeps cant lose wool
        if (hasBaby)
        {
            //TODO stop movement of pregnant sheep
            StopCoroutine(DeathInChildBirth());
            if (deathTimer != null) Destroy(deathTimer);
            hasBaby = false;
            SheepManager.numOfPregnantSheeps--;

            GiveBirth(new Vector3(transform.position.x, transform.position.y + SheepManager.newBornSpawnPositionOffset, transform.position.z));
            renderer.material.color = Color.white;
            //StartCoroutine(GetPregnant());
            return true;
        }

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

    public IEnumerator GetPregnant()
    {
        yield return new WaitForSeconds(Random.Range(SheepManager.getPregnantLowerTime, SheepManager.getPregnantUpperTime));
        hasBaby = true;

        animator.Play("Baby Coming");
        animator.speed = 1;

        StartCoroutine(DeathInChildBirth());


    }

    private IEnumerator DeathInChildBirth()
    {
        yield return new WaitForSeconds(SheepManager.GetManager().deathInChildBirth);
        //deathTimer = Instantiate(SheepManager.GetManager().deathTimer, new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), Quaternion.identity);
        SheepManager.GetManager().sheeps.Remove(gameObject);
        SheepManager.numOfPregnantSheeps--;
        //Destroy(deathTimer);
        Destroy(gameObject);
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
