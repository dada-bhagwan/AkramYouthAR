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
    [Header("UI DLC")]
    public Transform rootDicContainer;
    public DLC dlcPrefeb;

    [Header("UI SCENE LIST")]
    public Transform rootContainer;
    public Button prefeb;
    public Text labelText;
    public GameObject Space;

    Magazine[] magazineList;

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
        dlcPath = (Application.platform == RuntimePlatform.Android ? Application.persistentDataPath : Application.dataPath) + "/DLC/";
        Init();
    }
    IEnumerator LoadAssets()
    {
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
        /*int i = 0;
        while (i < magazineList.Length)
        {
            string path = dlcPath + magazineList[i].fileName;
            if (File.Exists(path))
            {
                AssetBundle bundle = AssetBundle.LoadFromFile(path);
                assetBundles.Add(bundle);
                scensNames.AddRange(bundle.GetAllScenePaths());

                *//*var bundleRequest = AssetBundle.LoadFromFileAsync(path);
                yield return bundleRequest;

                assetBundles.Add(bundleRequest.assetBundle);

                scensNames.AddRange(bundleRequest.assetBundle.GetAllScenePaths());*//*
            }
            i++;
            yield return null;
        }

        ////DELETE UNused
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
        }*/
        if(Directory.Exists(dlcFolder))
        {
            string[] dlcFiles = Directory.GetFiles(dlcPath);
            foreach (var item in dlcFiles)
            {
                /*if (Path.GetExtension(item) != ".meta")
                {
                    var filedata = magazineList.FirstOrDefault(x => x.url.EndsWith(item));
                    if (filedata != null)
                    {
                        File.Delete(item);
                    }
                }*/ 
                if(Path.GetExtension(item) == ".dlc")
                {
                    Debug.Log("item:" + item);
                    try
                    {
                        AssetBundle bundle = AssetBundle.LoadFromFile(item);
                        assetBundles.Add(bundle);
                        scensNames.AddRange(bundle.GetAllScenePaths());
                    } catch (Exception e)
                    {
                        Debug.Log(e.StackTrace);
                    }
                    

                    /*var bundleRequest = AssetBundle.LoadFromFileAsync(item);
                    yield return bundleRequest;

                    assetBundles.Add(bundleRequest.assetBundle);

                    scensNames.AddRange(bundleRequest.assetBundle.GetAllScenePaths());*/
                }
                yield return null;
            }
        }
        RefreshSceneList();
    }
    public void ShowDLC()
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
    }
    public void RefreshSceneList()
    {
        foreach (Transform item in rootContainer)
        {
            Destroy(item.gameObject);
        }
        Debug.Log("List of scenes:" + scensNames);
        foreach (var item in scensNames)
        {
            Debug.Log("!!!!!!!!! scenes Added:" + item);
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
        MagazineList magazineListObj = GetMagaizneListFromPref();
        if (magazineListObj != null)
        {
            magazineList = magazineListObj.data;
        }
        Thread thread = new Thread(() => WSCall.GetMagazineList()) { Name = "TGetMagazineList" };
        thread.Start();
    }

    public bool LoadMagazineListFromServer()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Error. Check internet connection!");
            return false;
        } else
        {
            MagazineList magazineListObj = WSCall.GetMagazineList();
            if (magazineListObj != null)
                magazineList = magazineListObj.data;
            return true;
        }
    }

    public MagazineList GetMagaizneListFromPref()
    {
        string jsonResponse = PlayerPrefs.GetString("magazinelist");
        if(jsonResponse == null)
        {
            WSCall.GetMagazineList();
        }
        jsonResponse = PlayerPrefs.GetString("magazinelist");
        return JsonUtility.FromJson<MagazineList>(jsonResponse);
    }
}
