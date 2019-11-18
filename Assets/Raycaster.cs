using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    public RaycastHit m_hit;
    public bool hasHit = false;
    private Vector3 m_direction;

    public float m_rayDistance;
    public enum Directions
    {
        UP = 0,
        DOWN = 1,
        FORWARD = 2,
        BACK = 3,
        LEFT = 4,
        RIGHT = 5,
    }
    public Directions m_rayDirection;
    // Update is called once per frame

    private void Start()
    {
        switch (m_rayDirection)
        {
            case Directions.UP:
                m_direction = Vector3.up;
                break;
            case Directions.DOWN:
                m_direction = Vector3.up * -1;
                break;
            case Directions.FORWARD:
                m_direction = Vector3.forward;
                break;
            case Directions.BACK:
                m_direction = Vector3.forward * -1;
                break;
            case Directions.LEFT:
                m_direction = Vector3.left;
                break;
            case Directions.RIGHT:
                m_direction = Vector3.left * -1;
                break;
        }
    }
    void Update()
    {

        if (Physics.Raycast(transform.position, transform.TransformDirection(m_direction), out m_hit, m_rayDistance))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(m_direction) * m_hit.distance, Color.green);
            hasHit = true;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(m_direction) * m_rayDistance, Color.red);
            hasHit = false;
        }
        if (tag == "Player" && m_hit.transform.tag == "Enemy")
            m_hit.transform.gameObject.SetActive(false);
    }
}
