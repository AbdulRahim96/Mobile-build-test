using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform minimapCamera;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        GameManager.GameOverWithSuccessCallback += (isSuccess) =>
        {
            enabled = false;
            GetComponentInChildren<CanvasGroup>().DOFade(0, 1f);
        };
    }

    // Update is called once per frame
    void Update()
    {
        minimapCamera.position = new Vector3(player.position.x, player.position.y + 100, player.position.z);
        minimapCamera.rotation = Quaternion.Euler(90, player.eulerAngles.y, 0);

        // use lerp to smooth the camera rotation in y axis
       // transform.transform.rotation = Quaternion.Lerp(transform.transform.rotation, Quaternion.Euler(90, player.eulerAngles.y, 0), Time.deltaTime * rotationSpeed);
    }


}
