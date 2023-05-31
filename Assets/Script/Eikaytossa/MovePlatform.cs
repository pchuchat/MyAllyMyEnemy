using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ©GameGurus - Heikkinen R., Hopeasaari J., Kantola J., Kettunen J., Kommio R, PC, Parviainen P., Rautiainen J.
// Creator: PC, Phatchanon Chuchat
// TODO:
//  
//
// 

public class MovePlatform: MonoBehaviour
{ 

    public float moveSpeed = 2.0f;
    public Vector3 moveDirection;
    public bool isActive = false;

    // Start is called before the first frame update
    public void Activate()
    {
        isActive = !isActive;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
    }
}
