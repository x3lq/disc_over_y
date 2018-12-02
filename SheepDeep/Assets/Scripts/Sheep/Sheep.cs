﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Renderer))]
public class Sheep : MonoBehaviour
{
    private Animator animator;
    private Renderer renderer;
    private Rigidbody2D rigidbody;
    private SheepManager manager;
    private Movement movement;

    public bool hasWool;
    public bool hasBaby;
    public float getPregnantTime;
    GameObject deathTimer;

    public float growWoolTime;

    private int stepCount = 0;
    private int noStepCount = 0;
    private bool isResting = false;
    private bool isRunning = false;

    float noiseOffset;

    public float animationSpeedVariation = 0.2f;

    private float targetIsActive = 0;
    private Vector2 targetPosition;
    private Vector2 estimatedVelocity = new Vector2();

    private Coroutine deathCoroutine;

    // Use this for initialization
    void Start()
    {
        //StartCoroutine(GetPregnant());
        renderer = GetComponent<Renderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        manager = FindObjectOfType<SheepManager>();
        movement = GetComponent<Movement>();

        noiseOffset = Random.value * 10.0f;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (isRunning)
        {
            if (targetIsActive > 0)
            {
                targetIsActive -= Time.deltaTime;
                estimatedVelocity += (targetPosition - (Vector2)transform.position).normalized * 0.1f;
            }
            else
            {
                isRunning = false;
                isResting = Random.Range(0, 10) < 6;
            }
        }
        else
        {
            if (isResting)
            {
                movement.horizontalVelocity = 0;
                movement.verticalVelocity = 0;
                return;
            }
        }

        int randomDirection;
        if (stepCount > 300)
        {
            randomDirection = 0;
        }
        else
        {
            randomDirection = Random.Range(noStepCount > 10 ? 1 : 0, 5);
        }
        switch (randomDirection)
        {
            case 1:
                //Move up
                estimatedVelocity = CalculateCrowdMovement(new Vector2(estimatedVelocity.x, estimatedVelocity.y + 1));
                stepCount++;
                break;
            case 2:
                //Move right
                estimatedVelocity = CalculateCrowdMovement(new Vector2(estimatedVelocity.x + 1, estimatedVelocity.y));
                stepCount++;
                break;
            case 3:
                //Move down
                estimatedVelocity = CalculateCrowdMovement(new Vector2(estimatedVelocity.x, estimatedVelocity.y - 1));
                stepCount++;
                break;
            case 4:
                //Move left
                estimatedVelocity = CalculateCrowdMovement(new Vector2(estimatedVelocity.x - 1, estimatedVelocity.y));
                stepCount++;
                break;
            default:
                estimatedVelocity = new Vector2(0, 0);
                stepCount = 0;
                noStepCount++;
                break;
        }
        Vector2 smoothedVelocity = Vector2.Lerp(rigidbody.velocity, estimatedVelocity, Time.deltaTime).normalized;
        movement.horizontalVelocity = smoothedVelocity.x;
        movement.verticalVelocity = smoothedVelocity.y;
    }

    public bool SheppardInteraction()
    {
        //pregnant sheeps cant lose wool
        if (hasBaby)
        {
            hasBaby = false;
            SheepManager.numOfPregnantSheeps--;
            animator.Play("Movement");
            //TODO stop movement of pregnant sheep
            StopCoroutine(deathCoroutine);

            deathCoroutine = null;
            if (deathTimer != null) Destroy(deathTimer);
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

    public void WolfInteraction()
    {
        animator.Play("Death");
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void GiveBirth(Vector3 position)
    {
        GameObject newBorn = Instantiate(SheepManager.GetManager().sheepPrefab, position, Quaternion.identity);
        newBorn.transform.parent = SheepManager.GetManager().herd.transform;
        SheepManager.GetManager().sheeps.Add(newBorn);
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------------------
    //          Coroutines
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------

    IEnumerator DelayTargetSet(Vector2 targetPosition)
    {
        yield return new WaitForSeconds(Random.Range(0f, 2f));
        isRunning = true;
        this.targetPosition = targetPosition;
        targetIsActive = Random.Range(3f, 8f);
    }

    public IEnumerator GetPregnant()
    {
        if (deathCoroutine == null)
        {
            yield return new WaitForSeconds(Random.Range(SheepManager.getPregnantLowerTime, SheepManager.getPregnantUpperTime));
            hasBaby = true;

            animator.Play("Baby Coming");
            animator.speed = 1;

            deathCoroutine = StartCoroutine(DeathInChildBirth());
        }
    }

    private IEnumerator DeathInChildBirth()
    {
        yield return new WaitForSeconds(SheepManager.GetManager().deathInChildBirth);SheepManager.GetManager().sheeps.Remove(gameObject);
        SheepManager.numOfPregnantSheeps--;
        animator.Play("Death");
        GetComponent<BoxCollider2D>().enabled = false;
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------------------
    //          Helper-Methods
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------

    private Vector2 GetDistanceVector(Transform target)
    {
        Vector2 distanceVector = transform.position - target.transform.position;
        float distance = distanceVector.magnitude;
        float scale = Mathf.Clamp01(1.0f - distance / manager.neighborDist);
        return distanceVector * (scale / distance);
    }

    private Vector2 CalculateCrowdVelocity()
    {
        float noise = Mathf.PerlinNoise(Time.time, noiseOffset) * 2.0f - 1.0f;
        float directionVelocity = manager.velocity * (1 + noise * manager.velocityVariation);

        Vector2 distance = Vector2.zero;
        Vector2 direction = manager.transform.forward;
        Vector2 anchor = manager.transform.position;

        Collider[] neighbors = Physics.OverlapSphere(transform.position, manager.neighborDist);

        foreach (Collider col in neighbors)
        {
            if (col.gameObject == gameObject || !col.tag.Equals("Sheep")) continue;
            Transform trans = col.transform;
            distance += GetDistanceVector(trans);
            direction += (Vector2)trans.forward;
            anchor += (Vector2)trans.position;
        }

        direction *= 1.0f / neighbors.Length;
        anchor *= 1.0f / neighbors.Length;
        anchor = (anchor - (Vector2)transform.position).normalized;

        Vector3 lookDirektion = distance + direction + anchor;

        return lookDirektion.normalized;
    }

    private Vector2 CalculateCrowdMovement(Vector2 ownVelocity)
    {
        Collider[] neighbors = Physics.OverlapSphere(transform.position, manager.neighborDist);

        if (neighbors.Length <= 1) return ownVelocity;
        int sheepCounter = 0;

        ownVelocity *= 2;

        foreach (Collider col in neighbors)
        {
            if (col.gameObject == gameObject || !col.tag.Equals("Sheep")) continue;
            GameObject obj = col.gameObject;
            sheepCounter++;
            ownVelocity += obj.GetComponent<Movement>().GetVelocity();
        }
        ownVelocity *= 1.0f / (sheepCounter + 2);
        return ownVelocity;
    }

    public Vector2 GetVelocity()
    {
        return rigidbody.velocity;
    }

    public void setTarget(Vector2 targetPosition)
    {
        StartCoroutine("DelayTargetSet", targetPosition);
    }

    public void Death()
    {
        SheepManager.GetManager().sheeps.Remove(gameObject);
        Destroy(gameObject);
    }
}