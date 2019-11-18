using UnityEngine;
using System.Collections;

public class StickyFloor : MonoBehaviour
{

    private Vector3 m_EnterScale = Vector3.one;
    public Transform m_globalParent = null; //Por defecto el padre global será la raiz de la escena pero podría ser que no fuera así.
    public Transform m_transformToAttach;
    private Vector3 rotation_aux;
    void Start()
    {
        if (m_transformToAttach == null)
            m_transformToAttach = transform;
    }

    void OnTriggerEnter(Collider other)
    {
        //TODO 1: Cuando el objeto que caiga sea attachable, atachamos el objeto. Ojo, la scala puede cambiar!!!
        Attachable atachable = other.GetComponent<Attachable>();
        if (atachable && atachable.IsAttachable)
        {

            //Ampliación: Si la superficie está inclinada 45º o más, no attachar. Se usa el componente Raycaster de "other" para el cálculo del ángulo.
            Raycaster raycaster = other.GetComponent<Raycaster>();
            
            if (raycaster && Vector3.Angle(raycaster.m_hit.normal, Vector3.up) < 45){
                rotation_aux = other.transform.eulerAngles;
                m_EnterScale = other.transform.localScale;
                other.transform.parent = m_transformToAttach;
                atachable.IsAttached = true;
                other.transform.eulerAngles = new Vector3(0f, other.transform.eulerAngles.y, 0f);

            }
        }
    }

    private void OnTriggerStay(Collider other)
    {

        Attachable atachable = other.GetComponent<Attachable>();
        Raycaster raycaster = other.GetComponent<Raycaster>();
        //Si la inclinación de la plataforma supera los 45º, soltar attach.
        if (atachable && atachable.IsAttached && Vector3.Angle(raycaster.m_hit.normal, Vector3.up) > 45)
        {
            other.transform.parent = m_globalParent;
            other.transform.localScale = m_EnterScale;
            atachable.IsAttached = false;
            other.transform.eulerAngles = new Vector3(0f, other.transform.eulerAngles.y, 0f);

        }

    }

    void OnTriggerExit(Collider other)
    {
        //TODO 2: Cuando el objeto que caiga sea attachable, como estamos saliendo, desatachamos el objeto. Ojo, la scala puede cambiar!!!
        Attachable atachable = other.GetComponent<Attachable>();
        if (atachable && atachable.IsAttached)
        {
            other.transform.parent = m_globalParent;
            other.transform.localScale = m_EnterScale;
            atachable.IsAttached = false;
            other.transform.eulerAngles = new Vector3(0f, other.transform.eulerAngles.y, 0f);
        }
    }
}