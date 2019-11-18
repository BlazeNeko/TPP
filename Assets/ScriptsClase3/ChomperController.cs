using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChomperController : MonoBehaviour
{
    [Range(0,1)]
    public float animationSpeed;
    public bool run;
    public float speed;
    public float runSpeed;
    public float angle;
    public float rotationTime;
    public float m_MinDistance = 2.0f;

    public Transform[] m_waypoints;
    private Transform m_CurrentWaypoint = null;
    
    private Rigidbody rigidBody;
    private ChomperAnimation chomperAnimation;
    private bool rotate;
    private float m_MinDistanceSqr = 0.0f;
    private int waypoint_index;

    private bool targetFound = false;
    private CharacterController controller;
    private Raycaster raycaster;
    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        chomperAnimation = GetComponent<ChomperAnimation>();
        rotate = false;

        m_MinDistanceSqr = m_MinDistance * m_MinDistance;
        gameObject.transform.position = m_waypoints[0].position;
        m_CurrentWaypoint = m_waypoints[1];
        waypoint_index = 1;
        controller = GetComponent<CharacterController>();
        raycaster = GetComponent<Raycaster>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //CAMBIAMOS DE DIRECCIÓN CON UNA PROBABILIDAD DEL 5%
      //  if (Random.Range(0f, 1f) >= 0.95f && !rotate)
        //    StartCoroutine(Rotate(Random.Range(1, 10) % 2 == 0));

        chomperAnimation.Updatefordward(animationSpeed);
        //La velocidad depende de si está corriendo o no
        float s = run ? runSpeed : speed;
        //#TODO: Habra que meter gravedad
        //#TODO: Habra que comprobar colisiones.
        if (!targetFound)
        {
            _DoMovement();
        }
        else
        {
            moveToTarget();
        }
    }

    void Update()
    {

        if (!targetFound)
            _CheckArrived();
        else
            CheckKill();

    }

    void _DoMovement()
    {
        // TODO 1 - Obtener la dirección del desplazamiento y normalizarla
        Vector3 direction = m_CurrentWaypoint.position - transform.position;
        direction.Normalize();
        // TODO 2 - Mover chomper en función de la dirección obtenida, la velocidad, y el deltaTime
        direction += Physics.gravity;

        transform.LookAt(m_CurrentWaypoint);
        controller.Move(direction * speed * Time.deltaTime);

    }

    void moveToTarget()
    {
        Vector3 direction = target.position - transform.position;
        direction.Normalize();
        transform.LookAt(target);
        direction += Physics.gravity;
        controller.Move(direction * speed * Time.deltaTime);
    }
    IEnumerator Rotate(bool inverse)
    {
        float time = 0;
        rotate = true;
        float realAngle = inverse ? -angle : angle;
        //Podemos rotar a izquierda o a derecha.
        Quaternion newRotation = Quaternion.Euler(transform.rotation.eulerAngles + Vector3.up * realAngle);
        Quaternion originalRotation = transform.rotation;
        while (time < rotationTime)
        {
            time += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(originalRotation, newRotation, time / rotationTime);
            
            yield return new WaitForEndOfFrame();
        }
        rotate = false;
    }

    void _CheckArrived()
    {
 
        if (raycaster.hasHit)
        {
            if (raycaster.m_hit.transform.tag == "Player")
            {
                target = raycaster.m_hit.transform;
                targetFound = true;

            }
        }
        // TODO 3 - Comprobar si chomper está a menos distancia de la distancia mínima
        float remDist = Vector3.SqrMagnitude(m_CurrentWaypoint.position - transform.position);
        if (remDist < m_MinDistanceSqr)
        {
            Debug.Log("Entra aqui");

            // TODO 4 - Cambiar el currentWaypoint
            m_CurrentWaypoint = m_waypoints[(waypoint_index + 1) % m_waypoints.Length];
            waypoint_index = waypoint_index < m_waypoints.Length ? waypoint_index + 1 : 0;
            Debug.Log(waypoint_index);
        }


    }
    void CheckKill()
    {
        GameObject m_GameManager = GameObject.FindGameObjectWithTag("GameManager");

        Vector3 direction = target.position - transform.position;
        if (direction.magnitude <= 1)
        {
            m_GameManager.SendMessage("RespawnPlayer", SendMessageOptions.DontRequireReceiver);
            targetFound = false;
        }

    }

}
