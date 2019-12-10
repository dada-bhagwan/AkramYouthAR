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
            } catch(Exception e)
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
        } catch(Exception e)
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
    /* public void ShowDLC()
    {
        if(LoadMagazineListFromServer())
        {
            foreach (Transform t in rootDicContainer)
            {
                Destroy(t.gameObject);
            }
            for (int i = 0; i < magazineList.Length; i++)
            {
                var clone = Instantiate(dlcPrefeb.gameObject) as GameObject;

                clone.transform.SetParent(rootDicContainer);

                clone.GetComponent<DLC>().Inti(magazineList[i]);
                clone.SetActive(true);
            }
        }
    } */
    public void RefreshSceneList()
    {
        foreach (Transform item in rootContainer)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in scensNames)
        {
            Debug.Log("########## Adding ScensNames:" + item);
            labelText.text = Path.GetFileNameWithoutExtension(item);
            var clone = Instantiate(prefeb.gameObject) as GameObject;
            clone.GetComponent<Button>().AddEventListener(labelText.text, LoadAssetBundelScens);

            clone.SetActive(true);
            clone.transform.SetParent(rootContainer);
            AddSpace();
            //Debug.Log(item);
            //Debug.Log(Path.GetFileNameWithoutExtension(item));
        }
        Debug.Log("########## All Scene added");
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
        Thread thread = new Thread(() => CheckForNewMagazineAvailability()) { Name = "TNewMagCheck" };
        thread.Start();
    }

    public void CheckForNewMagazineAvailability() {
        List<Magazine> oldMagazineList = DLCCache.magazinelist;
        List<Magazine> magazinesServer = DLCCache.GetMagazineListFromServer(false);
        if(magazinesServer != null) {
            IEnumerable<Magazine> newMagazine = magazinesServer.Except(DLCCache.magazinelist);
            if(newMagazine != null) {
                //New Magazine Available
            }
        }
    }
}
