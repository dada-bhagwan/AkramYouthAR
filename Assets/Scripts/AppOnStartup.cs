// Demonstration of RuntimeInitializeOnLoadMethod and the argument it can take.
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

class AppOnStartup
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static  void OnBeforeSceneLoadRuntimeMethod()
    {

        Debug.Log("################## Before first Scene loaded");
        //LoadModelAsync();
        /*string ver = Application.version;
        Thread thread = new Thread(() => LoadModelAsync(ver)) { Name = "Thread 1" };
        thread.Start();*/
        Debug.Log("################## Before first Scene loaded Done");
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void OnAfterSceneLoadRuntimeMethod()
    {
        Debug.Log("############## After first Scene loaded");
    }

    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        Debug.Log("################### RuntimeMethodLoad: After first Scene loaded");
    }
    static void LoadModelAsync(string version)
    {
        ProcessCall processCall = new ProcessCall();
        var version1 = new Version(version);

        if (!processCall.CheckVersion(version))
        {
            Thread.Sleep(30000);
            Debug.Log("############################### Application Quit by fource!!");
            //ShowPopupExample.OpenWin();
            //System.Threading.Thread.Sleep(30000);
        }
        Debug.Log("############################### Application Quit by fource!!");

        //var assetBundle = await GetAssetBundle("www.my-server.com/myfile");
        //var prefab = await assetBundle.LoadAssetAsync<GameObject>("myasset");
        //GameObject.Instantiate(prefab);
        //assetBundle.Unload(false);
    }

    //async Task<AssetBundle> GetAssetBundle(string url)
    //{
    //    return (await new WWW(url)).assetBundle;
    //}
    static void CheckVersion()
    {
        try
        {

            Debug.Log("############################### Inside Startup");

            //ProcessCall processCall = new ProcessCall();
            //if (!processCall.CheckVersion())
            //{
            //    Debug.Log("############################### Application Quit by fource!!");
            //    //ShowPopupExample.OpenWin();
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
}
