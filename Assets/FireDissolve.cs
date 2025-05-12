using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class FireDissolve : HittingEffect
{
    public float dissolveDuration = 1f;
    public AnimationCurve AnimationCurve;
    public UnityEvent ontrigger;


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Hit(other.gameObject);
            ontrigger.Invoke();
        }
    }

    [Button("dissolve")]
    public override void Hit(GameObject playerObject)
    {
        Material material = playerObject.GetComponentInChildren<Renderer>().material;

        // using dotween to animate the dissolve effect
        float val = 0;
        DOTween.To(() => val, x => val = x, 1, dissolveDuration)
            .SetEase(AnimationCurve)
            .OnUpdate(() =>
            {
                material.SetFloat("_Dissolve", val);
            })
            .OnComplete(() =>
            {
                playerObject.GetComponent<PaperPlaneController>().enabled = false;
                playerObject.GetComponent<Rigidbody>().isKinematic = true;
                GameManager.Instance.InvokeGameOver(false);
            });
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            Hit(other);
            ontrigger.Invoke();
        }
    }
}
