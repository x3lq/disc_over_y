using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Sheep_Scr : MonoBehaviour
{
    public Rigidbody rigidbody;
    private SheepCrowdControllerScr controller;

    private int stepCount = 0;
    private int noStepCount = 0;

    private float movementCooldown = 0;

    float noiseOffset;

    public float animationSpeedVariation = 0.2f;

    // Use this for initialization
    void Start()
    {
        rigidbody = transform.GetComponent<Rigidbody>();
        controller = FindObjectOfType<SheepCrowdControllerScr>();

        noiseOffset = Random.value * 10.0f;

        var animator = GetComponent<Animator>();
        if (animator)
            animator.speed = Random.Range(-1.0f, 1.0f) * animationSpeedVariation + 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (movementCooldown <= 0)
        {
            movementCooldown = 1;
        }
        else
        {
            movementCooldown -= Time.deltaTime;
            return;
        }

        int randomDirection;
        if (stepCount > 30)
        {
            randomDirection = 0;
        }
        else
        {
            randomDirection = UnityEngine.Random.Range(noStepCount > 10 ? 1 : 0, 5);
        }

        switch (randomDirection)
        {
            case 1:
                //Move up
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y + 0.1f, 0);
                stepCount++;
                break;
            case 2:
                //Move right
                rigidbody.velocity = new Vector3(rigidbody.velocity.x + 0.1f, rigidbody.velocity.y, 0);
                stepCount++;
                break;
            case 3:
                //Move down
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y - 0.1f, 0);
                stepCount++;
                break;
            case 4:
                //Move left
                rigidbody.velocity = new Vector3(rigidbody.velocity.x - 0.1f, rigidbody.velocity.y, 0);
                stepCount++;
                break;
            default:
                rigidbody.velocity = new Vector3(0, 0, 0);
                stepCount = 0;
                noStepCount++;
                break;
        }
    }

    private Vector3 GetDistanceVector(Transform target)
    {
        Vector3 distanceVector = transform.position - target.transform.position;
        float distance = distanceVector.magnitude;
        float scale = Mathf.Clamp01(1.0f - distance / controller.neighborDist);
        return distanceVector * (scale / distance);
    }

    private Vector3 CalculateCrowdVelocity()
    {
        float noise = Mathf.PerlinNoise(Time.time, noiseOffset) * 2.0f - 1.0f;
        float directionVelocity = controller.velocity * (1 + noise * controller.velocityVariation);

        Vector3 distance = Vector3.zero;
        Vector3 direction = controller.transform.forward;
        Vector3 anchor = controller.transform.position;

        Collider[] neighbors = Physics.OverlapSphere(transform.position, controller.neighborDist, controller.searchLayer);

        foreach(Collider col in neighbors)
        {
            if (col.gameObject == gameObject) continue;
            Transform trans = col.transform;
            distance += GetDistanceVector(trans);
            direction += trans.forward;
            anchor += trans.position;
        }

        direction *= 1.0f / neighbors.Length;
        anchor *= 1.0f / neighbors.Length;
        anchor = (anchor - transform.position).normalized;

        Vector3 lookDirektion = distance + direction + anchor;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, lookDirektion.normalized);

        if(rotation != transform.rotation)
        {
            float factor = Mathf.Exp(controller.rotationCoeff * -Time.deltaTime);
            transform.rotation = Quaternion.Slerp(rotation, transform.rotation, factor);
        }

        return transform.forward * (directionVelocity * Time.deltaTime);
    }
}