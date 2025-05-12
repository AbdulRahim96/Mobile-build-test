using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wolf : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    public Transform mouth, Camera;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(moving(other.transform));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            agent.SetDestination(other.transform.position);
            animator.SetFloat("Speed", 0);
            StopAllCoroutines();
        }
    }

    IEnumerator moving(Transform target)
    {
        while (Vector3.Distance(transform.position, target.position) > agent.stoppingDistance)
        {
            agent.SetDestination(target.position);
            animator.SetFloat("Speed", Mathf.Clamp01(agent.velocity.magnitude));
            yield return null;
        }
        print("Eating the plane");

        agent.Stop();
        animator.SetBool("Eat", true);

        // Game Over
        GameManager.Instance.InvokeGameOver(false);
        target.SetParent(mouth);
        target.transform.GetComponent<Rigidbody>().isKinematic = true;
        target.DOLocalMove(Vector3.zero, 0.3f).SetEase(Ease.InExpo);
        Camera.gameObject.SetActive(true);
    }

}
