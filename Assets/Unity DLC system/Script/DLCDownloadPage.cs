using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;
public class DLCDownloadPage : MonoBehaviour
{
    [Header("UI DLC")]
    public Transform rootDicContainer;
    public DLC dlcPrefeb;
    public bool isDownloadRunning = false;
    public GameObject downloadRunningPopup;
    public static DLCDownloadPage main
    {
        get;
        private set;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackBtnAction();
        }
    }
    public static string dlcPath;
    private void Start()
    {
        main = this;
        dlcPath = DLCCache.dlcFolder;
        isDownloadRunning = false;
        ShowDLC();
    }

    public void ShowDLC()
    {
        if (DLCCache.LoadMagazineListFromServer())
        {
            ReloadDLCGameObject();
        }
    }


    public void ReloadDLCGameObject()
    {
        List<Magazine> magazineList = DLCCache.magazinelist;
        foreach (Transform t in rootDicContainer)
        {
            Destroy(t.gameObject);
        }
        for (int i = 0; i < DLCCache.magazinelist.Count; i++)
        {
            var clone = Instantiate(dlcPrefeb.gameObject) as GameObject;
            clone.transform.SetParent(rootDicContainer);
            clone.GetComponent<DLC>().Inti(magazineList[i]);
            clone.SetActive(true);
        }
    }

    public void BackBtnAction()
    {
        if(isDownloadRunning)
        {
            downloadRunningPopup.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("MagazineList");
        }
    }
}
