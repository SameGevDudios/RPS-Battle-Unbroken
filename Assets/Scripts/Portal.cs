using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] Transform linkedPortal;
    [SerializeField] Vector3 offset;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == 8)
        {
            Teleport(other.gameObject);
        }
    }

    void Teleport(GameObject objectToTeleport)
    {
        objectToTeleport.transform.position = linkedPortal.position + offset;
        objectToTeleport.transform.eulerAngles = transform.eulerAngles - linkedPortal.eulerAngles;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + offset, .1f);
    }
}
