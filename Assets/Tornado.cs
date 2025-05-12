using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : HittingEffect
{
    public float centerRadius = 5f;
    public float forceFactor = 10;
    [SerializeField] private CapsuleCollider CapsuleCollider;
    private Transform player;
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                float playerHieght = other.transform.position.y;
                Vector3 direction = (transform.position - playerRigidbody.transform.position) + new Vector3(0, playerHieght, 0);
                float distance = direction.magnitude;

                player = other.transform;

                // Apply force to the player
                Vector3 force = direction.normalized * (CapsuleCollider.radius - distance) * forceFactor; // Adjust the multiplier as needed
                playerRigidbody.AddForce(force);

                if (distance < centerRadius)
                {
                    this.enabled = false;
                    Hit(other.gameObject);
                }
            }
        }
    }
    public override void Hit(GameObject playerObject)
    {
        GameManager.Instance.InvokeGameOver(false);
        playerObject.transform.SetParent(transform);
        GetComponent<DOTweenAnimation>().DOPlay();

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + CapsuleCollider.center, new Vector3(centerRadius, CapsuleCollider.height, centerRadius));

        if(player)
        {
            Gizmos.DrawLine(transform.position + new Vector3(0, player.transform.position.y, 0), player.transform.position);
        }
    }
}
