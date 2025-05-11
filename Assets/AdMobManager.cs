using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;
using System.Threading.Tasks;

public class AdMobManager : MonoBehaviour
{
    public static AdMobManager Instance;

    [Header("Ad Unit IDs")]
    public string bannerAdUnitId;
    public string interstitialAdUnitId;
    public string rewardedAdUnitId;

    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;
    private bool isInitialized = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            LoadAd();
            LoadInterstitialAd();
            LoadRewardedAd();

            ShowBanner();
            // This callback is called once the MobileAds SDK is initialized.
        });
    }

    #region Banner

    public void CreateBannerView()
    {
        Debug.Log("Creating banner view");

        // If we already have a banner, destroy the old one.
        if (bannerView != null)
        {
            bannerView = null;
        }

        // Create a 320x50 banner at top of the screen
        bannerView = new BannerView(bannerAdUnitId, AdSize.Banner, AdPosition.Top);
    }

    public void LoadAd()
    {
        // create an instance of a banner view first.
        if (bannerView == null)
        {
            CreateBannerView();
        }

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        Debug.Log("Loading banner ad.");
        bannerView.LoadAd(adRequest);
    }

    public void ShowBanner()
    {
        if (!isInitialized)
        {
            Debug.LogWarning("AdMob not initialized yet.");
            return;
        }

        if (bannerView == null)
        {
            bannerView = new BannerView(bannerAdUnitId, AdSize.Banner, AdPosition.Bottom);
            AdRequest request = new AdRequest();
            bannerView.LoadAd(request);
        }
        else
        {
            bannerView.Show();
        }
    }

    public void HideBanner()
    {
        bannerView?.Hide();
    }

    #endregion

    #region Interstitial

    public void LoadInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        InterstitialAd.Load(interstitialAdUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                interstitialAd = ad;
            });
    }

    public void ShowInterstitial()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("Interstitial not ready.");
        }
    }

    #endregion

    #region Rewarded

    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(rewardedAdUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardedAd = ad;
            });
    }

    public void ShowRewardedAd()
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
    }

    #endregion
}
