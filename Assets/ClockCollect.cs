using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockCollect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.UpdateTime(Random.Range(7f, 11f));
        GameManager.Instance.PlaySound();
        Destroy(gameObject);
    }
}
