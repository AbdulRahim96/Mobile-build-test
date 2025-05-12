using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float currentTime = 100;
    public int currentCollection;
    public int total = 15;
    public Navigation navigation;

    public static Action<int> itemCollectedCallback;
    public static Action AllItemCollectedCallback;
    public static Action<float> TimerUpdate;
    public static Action<bool> GameOverWithSuccessCallback;
    public static bool isGameOver = false;

    public Vector3 gravity = new Vector3(0, -9.81f, 0);

    private Tween timerTween;
    private float currentRemainingTime;
    private void Awake()
    {
        Instance = this;
        isGameOver = false;

        AllItemCollectedCallback = null;
        TimerUpdate = null;
        GameOverWithSuccessCallback = null;
        itemCollectedCallback = null;


        Physics.gravity = gravity;
        navigation.Init();
        total = navigation.letters.Count;
    }

    private void Start()
    {
        GameOverWithSuccessCallback +=  (bool isSuccess) => timerTween.Kill();
        StartCountDown();
        AdMobManager.Instance.ShowInterstitial();
    }

    private void Update()
    {
        navigation.UpdateToCloses();
    }

    public void ItemCollected(Transform letter)
    {
        navigation.RemoveLetter(letter);
        currentCollection++;
        PlaySound();
        itemCollectedCallback?.Invoke(currentCollection);

        if(currentCollection >= total)
        {
            AllItemCollectedCallback?.Invoke();
        }

    }

    public void PlaySound()
    {
        GetComponent<AudioSource>().Play();
    }

    public void OnFinish()
    {
        bool isSuccess = currentCollection >= total;
        InvokeGameOver(isSuccess);
        
    }

    [Button("Game Over")]
    public void InvokeGameOver(bool isSuccess)
    {
        isGameOver = true;
        GameOverWithSuccessCallback?.Invoke(isSuccess);
        navigation.arrow.gameObject.SetActive(false);

        if (isSuccess)
        {
            int currentSavedLevel = PlayerPrefs.GetInt("CurrentSavedLevel", 1);
            PlayerPrefs.SetInt("CurrentSavedLevel", currentSavedLevel + 1);
        }
        
    }

    private void StartCountDown()
    {
        float val = currentTime;
        timerTween = DOTween.To(() => val, x => val = x, 0, currentTime)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                currentRemainingTime = val;
                TimerUpdate?.Invoke(val);
            })
            .OnComplete(() =>
            {
                InvokeGameOver(false);
            });
    }

    public void UpdateTime(float val)
    {
        timerTween.Kill();
        currentTime = currentRemainingTime + val;
        StartCountDown();
    }

    [Serializable]
    public class Navigation
    {
        public Transform arrow;
        public List<Transform> letters;
        public void Init()
        {
            GameObject[] ltters = GameObject.FindGameObjectsWithTag("letter");
            foreach (var item in ltters)
                letters.Add(item.transform);
        }

        public void RemoveLetter(Transform letter)
        {
            letters.Remove(letter);
        }
        public void UpdateToCloses()
        {
            arrow.LookAt(GetClosest());
        }

        public void AddTarget(Transform trans)
        {
            letters.Add(trans);
        }

        Transform GetClosest()
        {
            float minDistance = Mathf.Infinity;
            Transform closest = null;
            foreach (var item in letters)
            {
                float distance = Vector3.Distance(item.position, arrow.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = item;
                }
            }
            return closest;
        }
    }
}
