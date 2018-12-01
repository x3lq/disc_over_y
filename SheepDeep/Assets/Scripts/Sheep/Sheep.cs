using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Sheep : MonoBehaviour
{

    private Renderer renderer;
    private Rigidbody2D rigidbody;
    private SheepManager manager;

    public bool hasWool;
    public bool hasBaby;
    public float getPregnantTime;
    GameObject deathTimer;
   
    

    public float growWoolTime;

    private int stepCount = 0;
    private int noStepCount = 0;

    private float movementCooldown = 0;

    float noiseOffset;

    public float animationSpeedVariation = 0.2f;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(GrowWool());
        //StartCoroutine(GetPregnant());
        renderer = GetComponent<Renderer>();
        rigidbody = transform.GetComponent<Rigidbody2D>();
        manager = FindObjectOfType<SheepManager>();

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
            //movementCooldown = 1;
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
                rigidbody.velocity = CalculateCrowdMovement(new Vector2(rigidbody.velocity.x, rigidbody.velocity.y + 0.1f));
                stepCount++;
                break;
            case 2:
                //Move right
                rigidbody.velocity = CalculateCrowdMovement(new Vector2(rigidbody.velocity.x + 0.1f, rigidbody.velocity.y));
                stepCount++;
                break;
            case 3:
                //Move down
                rigidbody.velocity = CalculateCrowdMovement(new Vector2(rigidbody.velocity.x, rigidbody.velocity.y - 0.1f));
                stepCount++;
                break;
            case 4:
                //Move left
                rigidbody.velocity = CalculateCrowdMovement(new Vector2(rigidbody.velocity.x - 0.1f, rigidbody.velocity.y));
                stepCount++;
                break;
            default:
                rigidbody.velocity = new Vector2(0, 0);
                stepCount = 0;
                noStepCount++;
                break;
        }
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

            renderer.material.color = Color.white * 0.5f;

            StartCoroutine(GrowWool());
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator GrowWool()
    {
        yield return new WaitForSeconds(Random.Range(SheepManager.growWoolLowerTime, SheepManager.growWoolUpperTime));

        hasWool = true;

        renderer.material.color = Color.white;
    }

    public IEnumerator GetPregnant()
    {
        yield return new WaitForSeconds(Random.Range(SheepManager.getPregnantLowerTime, SheepManager.getPregnantUpperTime));
        hasBaby = true;
        renderer.material.color = Color.magenta;
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
        Destroy(gameObject);
    }

    public void GiveBirth(Vector3 position)
    {
        GameObject newBorn = Instantiate(SheepManager.GetManager().sheepPrefab, position, Quaternion.identity);
        newBorn.transform.parent = SheepManager.GetManager().herd.transform;
        SheepManager.GetManager().sheeps.Add(newBorn);
        Debug.Log("New Sheep");
    }

    private void checkTarget()
    {
        if(manager.getTargetPosition() != null)
        {
            rigidbody.velocity /= 2;
            rigidbody.velocity += ((Vector2)transform.position - manager.getTargetPosition()).normalized * 2;
        }
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
            ownVelocity += obj.GetComponent<Sheep>().GetVelocity();
        }
        ownVelocity *= 1.0f / (sheepCounter + 2);
        Debug.Log(ownVelocity);
        return ownVelocity;
    }

    public Vector2 GetVelocity()
    {
        return rigidbody.velocity;
    }
}