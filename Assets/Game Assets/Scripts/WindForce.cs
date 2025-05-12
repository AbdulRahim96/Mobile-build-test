using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindForce : ExternalForce
{
    public float dragCoefficient = 0.05f;
    public override void ApplyForce(Rigidbody rigidbody)
    {
        rigidbody.AddForce(transform.forward * intensity, ForceMode.Force);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            ApplyForce(rb);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.GetComponent<PaperPlaneController>().ApplyForce(dragCoefficient);
        }
    }

}
