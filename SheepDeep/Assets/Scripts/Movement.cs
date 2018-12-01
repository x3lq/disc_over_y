using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour {

    private Rigidbody2D rb;

    private Player player;
    
    private float horizontalInput;
    private float verticalInput;

    public float horizontalVelocity;
    public float verticalVelocity;

    public float speed;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
        HandleInputs();

        HandleMovement();
	}

    void HandleInputs()
    {
        horizontalInput = -Input.GetAxis("HorizontalP" + player.playerID);
        verticalInput = Input.GetAxis("VerticalP" + player.playerID);

        horizontalVelocity = horizontalInput;
        verticalVelocity = verticalInput;
    }

    void HandleMovement()
    {
        Vector3 movementDirection = Vector3.up * verticalVelocity + Vector3.right * horizontalVelocity;
        rb.velocity = movementDirection * speed;
    }
}
