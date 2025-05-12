using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ExternalForce : MonoBehaviour
{
    public float intensity = 10f;
    public abstract void ApplyForce(Rigidbody rigidbody);

}
