using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleMobileAdsScript : MonoBehaviour
{
    private string _adUnitIdLeft = "ca-app-pub-4578399964404904/8348179529";
    private string _adUnitIdRight = "ca-app-pub-4578399964404904/4408934516";

    public static GoogleMobileAdsScript Instance;

    BannerView _bannerViewLeft;
    BannerView _bannerViewRight;

	private void Awake()
	{
		if (Instance == null)
		{
            Instance = this;
            DontDestroyOnLoad(this);
		}
        else if (Instance != this)
		{
            Destroy(this);
		}
	}

	public void Start()
    {
        // When true all events raised by GoogleMobileAds will be invoked
        // on the Unity main thread. The default value is false.
        // MobileAds.RaiseAdEventsOnUnityMainThread = true;

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });
    }

    public void CreateBannerView()
	{
        if (_bannerViewLeft != null || _bannerViewRight != null)
		{
            DestroyAd();
		}

        _bannerViewLeft = new BannerView(_adUnitIdLeft, AdSize.Banner, AdPosition.BottomLeft);
        _bannerViewRight = new BannerView(_adUnitIdRight, AdSize.Banner, AdPosition.BottomRight);
    }

    public void LoadAd()
	{
        if (_bannerViewLeft == null || _bannerViewRight == null)
		{
            CreateBannerView();
		}

        ListenToAdEventsLeft();
        ListenToAdEventsRight();

        var adRequestLeft = new AdRequest.Builder().AddKeyword("gaming").Build();
        var adRequestRight = new AdRequest.Builder().AddKeyword("gaming").Build();

        _bannerViewLeft.LoadAd(adRequestLeft);
        _bannerViewRight.LoadAd(adRequestRight);
    }

    private void ListenToAdEventsLeft()
    {
        // Raised when an ad is loaded into the banner view.
        _bannerViewLeft.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner left view loaded an ad with response : "
                + _bannerViewLeft.GetResponseInfo());
        };
        // Raised when an ad fails to load into the banner view.
        _bannerViewLeft.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner left view failed to load an ad with error : "
                + error);
        };
        // Raised when the ad is estimated to have earned money.
        _bannerViewLeft.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(string.Format("Banner left view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        _bannerViewLeft.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner left view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        _bannerViewLeft.OnAdClicked += () =>
        {
            Debug.Log("Banner left view was clicked.");
        };
        // Raised when an ad opened full screen content.
        _bannerViewLeft.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner left view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        _bannerViewLeft.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner left view full screen content closed.");
        };
    }

    private void ListenToAdEventsRight()
    {

        // Raised when an ad is loaded into the banner view.
        _bannerViewRight.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner right view loaded an ad with response : "
                + _bannerViewRight.GetResponseInfo());
        };
        // Raised when an ad fails to load into the banner view.
        _bannerViewRight.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner right view failed to load an ad with error : "
                + error);
        };
        // Raised when the ad is estimated to have earned money.
        _bannerViewRight.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(string.Format("Banner right view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        _bannerViewRight.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner right view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        _bannerViewRight.OnAdClicked += () =>
        {
            Debug.Log("Banner right view was clicked.");
        };
        // Raised when an ad opened full screen content.
        _bannerViewRight.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner right view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        _bannerViewRight.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner right view full screen content closed.");
        };
    }

    public void DestroyAd()
    {
        if (_bannerViewLeft != null)
        {
            _bannerViewLeft.Destroy();
            _bannerViewLeft = null;
        }

        if (_bannerViewRight != null)
        {
            _bannerViewRight.Destroy();
            _bannerViewRight = null;
        }
    }
}
