using System.Collections;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif
using UnityEngine.Networking;

namespace Rich.Base.Runtime.Concrete.Injectable.Controller
{
    public class IdfaSendCommand : EventCommand
    {
        [Inject(ContextKeys.CONTEXT_VIEW)] public GameObject contextView { get; set; }
        public override void Execute()
        {
            Retain();
            
            Debug.Log("/IdfaSendCommand/ --> Execute");
            
            if (Application.isEditor)
            {
                Debug.Log("IDFA send node is in editor skipping");
            }
            else
            {
                #if UNITY_IOS
                string platformName = "ios";
        
                if (Application.platform == RuntimePlatform.Android)
                {
                    platformName = "android";
                }

                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    platformName = "ios";
                }
                
                string deviceID = string.IsNullOrEmpty(Device.advertisingIdentifier) ? "Unity" : Device.advertisingIdentifier;

                string bundleID = Application.identifier;

                string url = "https://adcrea.net/api/save_idfa?platform=" + platformName + "&idfa=" + deviceID +
                             "&package_name=" + bundleID;
        
                Debug.Log("Sending => "+ url);

                contextView.GetComponent<MonoBehaviour>().StartCoroutine(GetRequest(url));
                #endif
            }
            
        }
        private IEnumerator GetRequest(string url)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();
            
            if (www.isNetworkError || www.isHttpError)
                Debug.Log("IDFA send error = " + www.error);
            else
                Debug.Log("IDFA send request SUCCESSFUL!");
            
            Release();
        }
    }
}
