using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class DLCCache
{
    static public String PrefMagazineList = "magazinelist";
    static public List<Magazine> magazinelist;

    static public String dlcFolder = (Application.platform == RuntimePlatform.Android ? Application.persistentDataPath : Application.dataPath) + "/DLC/";


    public static void LoadMagazineListFromPref()
    {
        List<Magazine> tmp = GetMagazineListFromPref();
        if(tmp != null)
            magazinelist = tmp;
    }

    public static List<Magazine> GetMagazineListFromPref() {
        string jsonResponse = PlayerPrefs.GetString(DLCCache.PrefMagazineList);
        if(jsonResponse == null)
        {
            return GetMagazineListFromServer(false);
        }
        else {
            MagazineList magazineListObj = JsonUtility.FromJson<MagazineList>(jsonResponse);
            if (magazineListObj != null)
                 return magazineListObj.magazineList;
        }
        return null;
    }

    public static bool LoadMagazineListFromServer()
    {
        List<Magazine> tmp = GetMagazineListFromServer(true);
        if(tmp != null)
            magazinelist = tmp;
        return true;
    }

    public static List<Magazine> GetMagazineListFromServer(bool displayPopup)
    {
        if (!CheckForInternetConnection())
        {
            Debug.Log("Error. Check internet connection!");
            if(displayPopup) {
                //Display Popup
            }
        } else
        {
            MagazineList magazineListObj = WSCall.GetMagazineList();
            if (magazineListObj != null)
                return magazineListObj.magazineList;
        }
        return null;
    }

    public static bool CheckForInternetConnection()
    {
        try
        {
            using (var client = new WebClient())
            using (client.OpenRead("http://google.com/generate_204"))
                return true;
        }
        catch
        {
            return false;
        }
    }
}