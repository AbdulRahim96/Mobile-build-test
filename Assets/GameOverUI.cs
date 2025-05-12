using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public DOTweenAnimation successMenu, failMenu;
    public TextMeshProUGUI itemCollectText;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.GameOverWithSuccessCallback += GameOverWithSuccess;
        gameObject.SetActive(false);
    }

    void GameOverWithSuccess(bool isSuccess)
    {
        gameObject.SetActive(true);
        print("Game Over: " + isSuccess);
        if (isSuccess)
            successMenu.DOPlay();
        else
            failMenu.DOPlay();

        itemCollectText.text = "Collected: " + GameManager.Instance.currentCollection + "/" + GameManager.Instance.total;
    }
}
