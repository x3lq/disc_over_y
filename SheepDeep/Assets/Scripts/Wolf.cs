using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wolf : Player {

    private Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Sheep"), LayerMask.NameToLayer("Wolf"));
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

        Collider2D collider = Physics2D.OverlapBox(transform.position + (Vector3)heading * 0.8f + Vector3.down * 0.9f, new Vector2(interactionCheckBoxSize, interactionCheckBoxSize), 0);

        if (collider != null)
        {
            Debug.Log(collider.tag);
        }

        if (collider != null && collider.tag.Equals("Sheep"))
        {
            AudioManager.PlayWolfAttack();

            collider.GetComponent<Sheep>().WolfInteraction();

            animator.Play("Wolf Attack");
            //movementEnabled = false;
        }
    }

    public void SheppardInteraction() {
        GameLoop.wolfCaught();
        animator.Play("Wolf Found");
        movementEnabled = false;
    }

    public void WolfFoundAnimationFinished()
    {
   		SceneManager.LoadScene(3);
    }

    public void AttackFinished()
    {
        Debug.Log("Finished");
        animator.Play("Movement");
        GameLoop.getInstance().wolfBites++;
        movementEnabled = true;
    }
}
