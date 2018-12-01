using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shepherd : Player {

    private Animator animator;

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
            collider.GetComponent<Sheep>().SheppardInteraction();

            animator.speed = 1;
            animator.Play("Shear");
            movementEnabled = false;
        }

        if(collider != null && collider.tag.Equals("Wolf")) {
            collider.GetComponent<Wolf>().SheppardInteraction();
        }
    }

    void FinishShear()
    {
        movementEnabled = true;
        animator.Play("Movement");
    }
}
