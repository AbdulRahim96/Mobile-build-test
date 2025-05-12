using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initiate : MonoBehaviour
{
    public PaperPlaneController paperPlane;
    public Transform hand, forceDirection;
    public CanvasGroup allCanvas;
    public GameObject camera, arrow;
    public float forceApplied = 10;
    void Start()
    {
        paperPlane.enabled = false;
        paperPlane.transform.GetComponent<Rigidbody>().isKinematic = true;
        paperPlane.transform.SetParent(hand);
        paperPlane.transform.position = hand.transform.position;
        paperPlane.transform.rotation = hand.transform.rotation;
        allCanvas.alpha = 0;
        arrow.SetActive(false);
    }

    public void ThrowPlane()
    {
        StartCoroutine(thrown());
    }

    IEnumerator thrown()
    {
        paperPlane.transform.GetComponent<Rigidbody>().isKinematic = false;
        paperPlane.transform.parent = null;
        paperPlane.transform.rotation = forceDirection.rotation;
        paperPlane.transform.GetComponent<Rigidbody>().AddForce(paperPlane.transform.forward * forceApplied, ForceMode.Impulse);
        
        yield return new WaitForSeconds(1);
        camera.SetActive(false);
        paperPlane.enabled = true;
        allCanvas.DOFade(1, 0.5f).SetEase(Ease.Linear);
        arrow.SetActive(true);
    }
}
