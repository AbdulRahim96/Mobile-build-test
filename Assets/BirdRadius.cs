using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdRadius : MonoBehaviour
{
    private DOTweenPath path;
    private Vector3 initialPos;
    public float speed = 1;
    private void Awake()
    {
        path = GetComponent<DOTweenPath>();
        initialPos = transform.position;
    }

    public void StartPath()
    {
        path.DORestart();
    }

    public void PausePath()
    {
        path.DOPause();
    }

    IEnumerator ChasingPlayer(Transform player)
    {
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            transform.LookAt(player.position);
            yield return null;
        }
        // will stop using StopCoroutine
    }

    [Button("triggered")]
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            path.DOPause();
            // chase player
            StartCoroutine(ChasingPlayer(other.transform));
            GetComponent<AudioSource>().Play();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            
        }
    }

    [Button("triggered undo")]
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            transform.LookAt(initialPos);
            transform.DOMove(initialPos, 3).OnComplete(() =>
            {
                StartPath();
            });
        }
    }
}
