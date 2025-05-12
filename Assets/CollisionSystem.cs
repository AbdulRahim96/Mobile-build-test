using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionSystem : MonoBehaviour
{
    public UnityEvent onCollisionEnterEvent, ontriggerEvent;

    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.isGameOver) return;

        onCollisionEnterEvent.Invoke();
    }
    private void OnTriggerEnter(Collider other)
    {
        ontriggerEvent.Invoke();
    }

}
