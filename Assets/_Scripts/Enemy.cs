using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float HP = 25;
    public GameObject ExplosionPrefab;
    public Transform[] Waypoints;
    public GameObject EyeObject;

    private NavMeshAgent Agent;
    private int CurrentDestination;
    private bool SpottedPlayer;
    private GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
       Agent = GetComponent<NavMeshAgent>();
        Agent.SetDestination(Waypoints[0].position);
        CurrentDestination = 0;
        SpottedPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfo;
        bool hit = Physics.Raycast(EyeObject.transform.position, transform.forward, out hitInfo);
        if (hit)
        {
           if (hitInfo.transform.CompareTag("Player"))
            {
                Debug.Log("Spotted Player!");
                SpottedPlayer = true;
                Player = hitInfo.transform.gameObject;
                Agent.SetDestination(hitInfo.transform.position);
            }
        }

        if (SpottedPlayer)
        {
            Agent.SetDestination(Player.transform.position);
        }



        if (!SpottedPlayer && Agent.pathStatus == NavMeshPathStatus.PathComplete && Agent.remainingDistance < .1f)
        {
            CurrentDestination++;
            if (CurrentDestination == Waypoints.Length)
            {
                CurrentDestination = 0;
            }
            Agent.SetDestination(Waypoints[CurrentDestination].position);
        }

    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        Debug.Log($"Took {damage} points, HP at {HP}");
        if (HP <= 0 ) 
        {
            Instantiate( ExplosionPrefab, transform.position + new Vector3 (0, .2f, 0), Quaternion.identity );
            Destroy(gameObject);
        }
    }
}
