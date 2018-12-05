using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shepherd : Player {

    private Animator animator;

    private Sheep motherSheep;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        HandleInputs();
    }

    void HandleInputs()
    {
        if (Input.GetKeyDown("joystick " + playerID + " button 0"))
        {
            TriggerInteraction();
        }
    }

    void TriggerInteraction()
    {
        Collider2D collider = Physics2D.OverlapBox(transform.position + (Vector3)heading*0.8f + Vector3.down * 0.9f, new Vector2(interactionCheckBoxSize, interactionCheckBoxSize), 0);
        
        if (collider != null && collider.tag.Equals("Sheep"))
        {
            Sheep sheep = collider.GetComponent<Sheep>();

            animator.speed = 1;
            if (sheep.hasBaby)
            {
                motherSheep = sheep;
                animator.Play("Baby Born");
            }
            else
            {
                animator.Play("Shear");
            }
            movementEnabled = false;

            sheep.SheppardInteraction();
        }

        if(collider != null && collider.tag.Equals("Wolf")) {
            collider.GetComponent<Wolf>().SheppardInteraction();
        }
    }

    private void callSheep()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, 5);
        foreach(Collider2D coll in objects)
        {
            if (coll.tag.Equals("Sheep"))
            {
                coll.gameObject.GetComponent<Sheep>().setFleePosition(transform.position);
            }
        }
    }

    void FinishShear()
    {
        movementEnabled = true;
        animator.Play("Movement");
    }

    void FinishBaby()
    {
        movementEnabled = true;
        animator.Play("Movement");
        
        motherSheep.GiveBirth(motherSheep.transform.position);

        motherSheep = null;
    }
}
