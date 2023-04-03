using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ISScript : MonoBehaviour
{
    // Start is called before the first frame update

    string appkey= "1942a5b85";
    public GameObject AdPanel;
    public GameObject NoAdPanel;
    public bool isAdCompleted = false;
    public static ISScript Instance;

    private void Awake()
    {
        Instance = this;  
    }
    void Start()
    {
       
        IronSource.Agent.init(appkey, IronSourceAdUnits.REWARDED_VIDEO);

    }

    private void OnEnable()
    {
        IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;

    }
 
   
    void OnApplicationPause(bool isPaused)
    {
        IronSource.Agent.onApplicationPause(isPaused);
    }


    private void SdkInitializationCompletedEvent() 
    {
        IronSource.Agent.validateIntegration();
    }

    //Rewarded
    public void ShowRewardedAd()
    {
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            IronSource.Agent.showRewardedVideo();
            
        }
        else
        {
            AdPanel.SetActive(false);
            NoAdPanel.SetActive(true);
            
        }
    }


}
