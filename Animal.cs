using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public Vector3 targetPositionSpawn;

    public Vector3 targetPositionMov;

    public Species species;

    public Species diet;

    bool danger;

    public float hunger;

    private float timeToDeathByHunger = 200;

    public float visionRadius = 10;

    public float speed = 8;

    public int reproductionScore;

    public int reproductionScoreCap;

    Environment environment;

    // Start is called before the first frame update
    void Start()
    {
        environment = FindObjectOfType<Environment>();
        targetPositionSpawn = environment.getRandomSpawnPosition();
        targetPositionMov = transform.position;
        SphereCollider trigger = gameObject.AddComponent<SphereCollider>();
        trigger.radius = visionRadius;
        trigger.isTrigger = true;
        danger = false;
        hunger = 0.6f;
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, new Vector3(targetPositionMov.x, transform.position.y, targetPositionMov.z));

        if (dist == 0)
        {
           Vector3 vec = new Vector3(environment.getRandomNum(), transform.position.y, environment.getRandomNum());
           vec.Normalize();
           targetPositionMov += vec * environment.getRandomNum();
           speed = 8;
        }

        float hungerTime = speed / 10;

        hunger += Time.deltaTime * hungerTime / timeToDeathByHunger;
        
        isOutOfRange(targetPositionMov);
        if (hunger >= 1)
        {
            Debug.Log(gameObject + " starved");
            Destroy(gameObject);
        }
        Move(targetPositionMov);
    }
    
    void Move(Vector3 targetPositionMov)
    {
        Vector3 target = new Vector3(targetPositionMov.x, transform.position.y, targetPositionMov.z);
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        transform.LookAt(target);
    }

    private void OnTriggerStay(Collider other) 
    {
        float dist = Vector3.Distance(transform.position, new Vector3(targetPositionMov.x, transform.position.y, targetPositionMov.z));
        if(other.TryGetComponent<LiveEntity>(out LiveEntity entity))
        {
            if(entity.diet == species)
            {   
                targetPositionMov = other.transform.position;
                speed = -9;
                danger = true;
            }
            
            if(entity.species == diet && !danger && hunger > 0.3
            && other.transform.position.x < environment.environmentRange
            && other.transform.position.x > -environment.environmentRange
            && other.transform.position.z < environment.environmentRange
            && other.transform.position.z > -environment.environmentRange)
            {
                targetPositionMov = other.transform.position;
                speed = 10;
            }

            if(entity.species == species && reproductionScore >= reproductionScoreCap)
            {
                targetPositionMov = other.transform.position;
                speed = 10;
            }
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.TryGetComponent<LiveEntity>(out LiveEntity entity))
        {
            if(entity.diet == species)
            {
                danger = false;
                speed = 8;
                targetPositionMov = transform.position;
            }
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        LiveEntity entity = other.gameObject.GetComponent<LiveEntity>();
        if(other.gameObject.tag != "Scene" && entity.species == diet)
        {
            hunger = 0;
            reproductionScore++;
            Debug.Log(other.gameObject + " eaten by " + gameObject);
            Destroy(other.gameObject);
        }

        if(other.gameObject.tag != "Scene" && entity.species == species && reproductionScore >= reproductionScoreCap)
        {
            environment.moreReproduction(other.gameObject.tag, transform.position);
            reproductionScore += -reproductionScoreCap;
        }
    }

    void isOutOfRange(Vector3 vec)
    {
        if (vec.x > environment.environmentRange)
        {
            targetPositionMov.x += -1;
        }
        if (vec.x < -environment.environmentRange)
        {
            targetPositionMov.x +=  1;
        }
        if (vec.z > environment.environmentRange)
        {
            targetPositionMov.z += -1;
        }
        if (vec.z < -environment.environmentRange)
        {
            targetPositionMov.z +=  1;
        }
    }
}
