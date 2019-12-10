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

    public static DLCDownloadPage main
    {
        get;
        private set;
    }
    public static string dlcPath;
    private void Start()
    {
        main = this;
        dlcPath = DLCCache.dlcFolder;
        ShowDLC();
    }

    public void ShowDLC()
    {
        if(DLCCache.LoadMagazineListFromServer())
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
}
