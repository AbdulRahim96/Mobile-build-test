using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Counter : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.itemCollectedCallback += UpdateCount;
        timerText.text = "0/" + GameManager.Instance.total;
    }

    private void UpdateCount(int count)
    {
        
        timerText.text = count + "/" + GameManager.Instance.total;
    }
}
