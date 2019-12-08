using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;

public class CheckVersion : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Popup;
   async void  Start()
    {
        try
        {
            Debug.Log("############################### Inside Start");
            string ver = Application.version;

            Thread thread =  new Thread(() => IsAppUpdateAvailable(ver)) { Name = "Thread 1" };
            thread.Start();

             //await IsAppUpdateAvailable();
            //if (isDone)
            //{
            //    Debug.Log("############################### App Update Version available");
            //    Popup.SetActive(true);
            //    //System.Threading.Thread.Sleep(30000);
            //}

            // EditorApplication.update += Update;
            //Debug.Log("Up and running app version: " + Application.version + " Unity version: " + Application.unityVersion);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //public async Task IsAppUpdateAvailable()
    private  void IsAppUpdateAvailable(string version)
    {
        try
        {
            //Thread.Sleep(30000);

            ProcessCall processCall = new ProcessCall();
            MyClass myClass = processCall.GetVersion();
            string playstoreVersion = myClass.ay_android_version;
            Debug.Log("########## Playstor Version:" + playstoreVersion);
            var appVersion = version;
            Debug.Log("########## App Version:" + appVersion);
            var version1 = new Version(playstoreVersion);
            var version2 = new Version(appVersion);

            var result = version1.CompareTo(version2);
            Debug.Log("########## Result:" + result);
            if (result > 0)
            {
                //Thread.Sleep(30000);

                Debug.Log("############################### App Update Version available");
                Popup.SetActive(true);
                GameObject desc = GameObject.Find("UpdateDesc");
                if(desc != null)
                {
                    desc.GetComponent<Text>().text = myClass.desc;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    
}
