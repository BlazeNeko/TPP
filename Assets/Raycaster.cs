using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    public RaycastHit hit;



    // Update is called once per frame
    void Update()
    {

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 2.5f))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.green);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 1000, Color.red);
        }
    }
}
