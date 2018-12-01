using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour {

    private Animator animator;

    private Rigidbody2D rb;

    private Player player;
    
    private float horizontalInput;
    private float verticalInput;

    public float horizontalVelocity;
    public float verticalVelocity;

    private Vector3 movementDirection;

    public float speed;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        HandleInputs();

        HandleMovement();
	}

    void HandleInputs()
    {
        if (player != null)
        {
            horizontalInput = -Input.GetAxis("HorizontalP" + player.playerID);
            verticalInput = Input.GetAxis("VerticalP" + player.playerID);

            horizontalVelocity = horizontalInput;
            verticalVelocity = verticalInput;
        }
    }

    void HandleMovement()
    {
        if (player != null && !player.movementEnabled)
        {
            movementDirection = Vector2.zero;
            rb.velocity = Vector2.zero;
            return;
        }

        movementDirection = Vector3.up * verticalVelocity / 1.5f + Vector3.right * horizontalVelocity;
        rb.velocity = movementDirection * speed;

        Vector3 direction = Vector3.up * verticalVelocity + Vector3.right * horizontalVelocity;

        if (animator != null)
        {
            if (direction.magnitude > 0.1)
            {
                animator.SetFloat("Horizontal", movementDirection.x);
                animator.SetFloat("Vertical", movementDirection.y);
                animator.speed = (Vector3.up * verticalVelocity + Vector3.right * horizontalVelocity).magnitude;
            }
            else
            {
                float x = animator.GetFloat("Horizontal");
                if (x != 0)
                {
                    animator.SetFloat("Horizontal", x / Mathf.Abs(x) * 0.1f);
                }
                float y = animator.GetFloat("Vertical");
                if (y != 0)
                {
                    animator.SetFloat("Vertical", y / Mathf.Abs(y) * 0.1f);
                }
                animator.speed = 1;
            }
        }

        if(player != null)
        {
            SetPlayerHeading();
        }
    }

    void SetPlayerHeading()
    {
        Vector2 normalizedHeading = new Vector2((int)(movementDirection.x*100), (int)(movementDirection.y*1000)).normalized;
        if(normalizedHeading != Vector2.zero)
        {
            Debug.Log("Name: " + name);
            player.heading = normalizedHeading;
        }
    }
}
