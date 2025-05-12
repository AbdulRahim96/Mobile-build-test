using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDestination : MonoBehaviour
{
    public Animator character;
    public Transform hand;
    public Transform planePoint;
    public DOTweenAnimation cameraAnim;

    void Start()
    {
        GameManager.AllItemCollectedCallback += EnableDestination;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.OnFinish();
            GetComponent<Collider>().enabled = false;
            GetComponentInChildren<ParticleSystem>().Stop();
            character.Play("pick");

            cameraAnim.gameObject.SetActive(true);
            cameraAnim.DOPlay();
            other.transform.DOMove(planePoint.position, 1).OnComplete(() =>
            {
                other.transform.SetParent(hand);
                other.transform.GetComponent<Rigidbody>().isKinematic = true;
            });
        }
    }

    void EnableDestination()
    {
        GetComponent<Collider>().enabled = true;
        GetComponentInChildren<ParticleSystem>().Play();
        GameManager.Instance.navigation.AddTarget(transform);
    }
}
