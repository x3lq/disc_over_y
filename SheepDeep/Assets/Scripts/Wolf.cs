using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Player {

	// Use this for initialization
	void Start () {
		
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

        Collider2D collider = Physics2D.OverlapBox(transform.position + (Vector3)heading * 0.8f, new Vector2(interactionCheckBoxSize, interactionCheckBoxSize), 0);

        if (collider != null)
        {
            Debug.Log(collider.tag);
        }

        if (collider != null && collider.tag.Equals("Sheep"))
        {
            collider.GetComponent<Sheep>().WolfInteraction();
        }
    }
}
