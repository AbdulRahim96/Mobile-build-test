using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantForce : ExternalForce
{
    public float dragCoefficient = 0.05f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            ApplyForce(rb);
            rb.GetComponent<PaperPlaneController>().ApplyForce(dragCoefficient);
        }
    }
    public override void ApplyForce(Rigidbody rigidbody)
    {
        rigidbody.AddForce(rigidbody.transform.forward * intensity, ForceMode.Impulse);
    }
}
