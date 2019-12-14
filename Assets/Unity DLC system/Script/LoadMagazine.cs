using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;
using System;

public class LoadMagazine : MonoBehaviour
{

    [Header("UI SCENE LIST")]
    public Transform rootContainer;
    public Button prefeb;
    public Text labelText;
    public GameObject Space;
    public GameObject downloadHint;
    public GameObject InternetConnectPopup;
    public Text newMagazineTxt;
    public GameObject newMagazinePopup;
    public static LoadMagazine main
    {
        get;
        private set;
    }
    public static string dlcPath;

    static List<AssetBundle> assetBundles = new List<AssetBundle>();
    static List<string> scensNames = new List<string>();

    public void Init()
    {
        StartCoroutine(LoadAssets());
    }

    //private IEnumerator Start()
    private void Start()
    {

        main = this;
        dlcPath = DLCCache.dlcFolder;
        Init();
    }
    IEnumerator LoadAssets()
    {
        LoadMagazineList();
        if (!Directory.Exists(dlcPath))
        {
            Directory.CreateDirectory(dlcPath);
        }

        foreach (var item in assetBundles)
        {
            item.Unload(true);

        }
        assetBundles.Clear();
        scensNames.Clear();

        string dlcFolder = dlcPath;
        int i = 0;
        List<Magazine> magazineList = DLCCache.magazinelist;

        while (i < magazineList.Count)
        {
            try
            {
                string path = dlcPath + magazineList[i].fileName;
                if (File.Exists(path))
                {
                    AssetBundle bundle = AssetBundle.LoadFromFile(path);
                    assetBundles.Add(bundle);
                    scensNames.AddRange(bundle.GetAllScenePaths());

                    /*var bundleRequest = AssetBundle.LoadFromFileAsync(path);
                    /*yield return bundleRequest;

                    assetBundles.Add(bundleRequest.assetBundle);

                    scensNames.AddRange(bundleRequest.assetBundle.GetAllScenePaths());*/
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error while loading magazine" + magazineList[i] + e.StackTrace);
            }

            i++;
            yield return null;
        }

        ////DELETE UNused
        try
        {
            string[] dlcFiles = Directory.GetFiles(dlcPath);
            foreach (var item in dlcFiles)
            {
                if (Path.GetExtension(item) != ".meta")
                {
                    bool used = false;
                    var filedata = magazineList.FirstOrDefault(x => x.url.EndsWith(item));
                    if (filedata != null)
                    {
                        File.Delete(item);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("##### Error while deleting File" + e.StackTrace);
        }

        /*if(Directory.Exists(dlcFolder))
        {
            string[] dlcFiles = Directory.GetFiles(dlcPath);
            foreach (var item in dlcFiles)
            {
                if (Path.GetExtension(item) != ".meta")
                {
                    var filedata = magazineList.FirstOrDefault(x => x.url.EndsWith(item));
                    if (filedata != null)
                    {
                        File.Delete(item);
                    }
                }
                if(Path.GetExtension(item) == ".dlc")
                {
                    Debug.Log("item:" + item);
                    AssetBundle bundle = AssetBundle.LoadFromFile(item);
                    assetBundles.Add(bundle);
                    scensNames.AddRange(bundle.GetAllScenePaths());

                    var bundleRequest = AssetBundle.LoadFromFileAsync(item);
                    yield return bundleRequest;

                    assetBundles.Add(bundleRequest.assetBundle);

                    scensNames.AddRange(bundleRequest.assetBundle.GetAllScenePaths());
                }
                yield return null;
            }
        }*/
        RefreshSceneList();
    }

    public void RefreshSceneList()
    {
        foreach (Transform item in rootContainer)
        {
            Destroy(item.gameObject);
        }
        SetDownloadHintStatus();
        foreach (var item in scensNames)
        {
            try
            {
                Debug.Log("########## Adding ScensNames:" + item);
                labelText.text = Path.GetFileNameWithoutExtension(item);
                var clone = Instantiate(prefeb.gameObject) as GameObject;
                clone.GetComponent<Button>().AddEventListener(labelText.text, LoadAssetBundelScens);

                clone.SetActive(true);
                clone.transform.SetParent(rootContainer);
                AddSpace();
            }
            catch (Exception e)
            {
                Debug.LogError("@@@@@@@@@ Error while loading schene" + item + "\t" + e.StackTrace);
            }
            //Debug.Log(item);
            //Debug.Log(Path.GetFileNameWithoutExtension(item));
        }
        Debug.Log("########## All Scene added");
    }

    void SetDownloadHintStatus()
    {
        if (downloadHint != null)
        {
            if (scensNames == null || scensNames.Count == 0)
            {
                downloadHint.SetActive(true);
            }
            else
            {    
                downloadHint.SetActive(false);
            }
        }
    }

    public void GoToMagazineDownloadList()
    {
        if(!AppUtiltiy.IsInternetConnected())
        {
            InternetConnectPopup.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("MagDownloadList");
        }
        
    }
    public void LoadAssetBundelScens(string scenName)
    {
        SceneManager.LoadScene(scenName);
    }

    public void AddSpace()
    {
        var clone = Instantiate(Space.gameObject) as GameObject;
        clone.SetActive(true);
        clone.transform.SetParent(rootContainer);
    }

    public void LoadMagazineList()
    {
        DLCCache.LoadMagazineListFromPref();
        //CheckForNewMagazineAvailability();
        Thread thread = new Thread(() => CheckForNewMagazineAvailability()) { Name = "TNewMagCheck" };
        thread.Start();
    }

    public void CheckForNewMagazineAvailability()
    {
        List<Magazine> oldMagazineList = DLCCache.magazinelist;
        List<Magazine> magazinesServer = DLCCache.GetMagazineListFromServer(false);
        if (magazinesServer != null && DLCCache.magazinelist != null)
        {

            IEnumerable<Magazine> newMagazines = magazinesServer.Except(DLCCache.magazinelist, new MyClassEqualityComparer());
            if(newMagazines.Any())
            {
                string newMagazineList = "";
                Magazine first = newMagazines.First();
                foreach (Magazine newMagazine in newMagazines)
                {
                    if(newMagazine == first)
                    {
                        newMagazineList = newMagazine.name;
                    }
                    else
                    {
                        newMagazineList = newMagazineList + " & " + newMagazine.name;
                    }
                }
                newMagazineTxt.text = newMagazineList + " magazine AR are available to Download.";
                newMagazinePopup.SetActive(true);
            }
        }
    }

    class MyClassEqualityComparer : IEqualityComparer<Magazine>
    {
        public bool Equals(Magazine x, Magazine y)
        {
            return x.id == y.id;
        }

        public int GetHashCode(Magazine obj)
        {
            unchecked
            {
                if (obj == null)
                    return 0;
                int hashCode = obj.id.GetHashCode();
                hashCode = (hashCode * 397) ^ obj.id.GetHashCode();
                return hashCode;
            }
        }
    }

}
