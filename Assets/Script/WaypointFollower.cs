using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{

    [SerializeField] GameObject[] waypoint;
    int currentWaypointIndex = 0;

    [SerializeField] float speed = 1f;

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, waypoint[currentWaypointIndex].transform.position)< .1f)
        {
            currentWaypointIndex++;
            if(currentWaypointIndex >= waypoint.Length)
            {
                currentWaypointIndex = 0;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, waypoint[currentWaypointIndex].transform.position, speed * Time.deltaTime);
    }
}
