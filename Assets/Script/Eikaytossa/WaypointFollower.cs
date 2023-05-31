using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: PC, Phatchanon Chuchat
// move the object towards the current waypoint

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] GameObject[] waypoint; // an array of gameobjects representing the waypoints to follow
    int currentWaypointIndex = 0; // index of the current waypoint being followed

    [SerializeField] float speed = 1f; // the speed at which the object moves between waypoints

    // Update is called once per frame
    void Update()
    {
        // if the object has reached the current waypoint, move on to the next one
        if (Vector3.Distance(transform.position, waypoint[currentWaypointIndex].transform.position) < .1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoint.Length)
            {
                currentWaypointIndex = 0;
            }
        }

        // move the object towards the current waypoint
        transform.position = Vector3.MoveTowards(transform.position, waypoint[currentWaypointIndex].transform.position, speed * Time.deltaTime);
    }
}