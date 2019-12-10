using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//using JsonUtility;
using System;
using UnityEngine.Networking;
using System.IO;
using System.Net;

public class WSCall
{
    public WSCall()
    {

    }
    public static MagazineList GetMagazineList()
    {
        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://youthapi.dbf.ooo:8000/get_magazine_list");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string jsonResponse = reader.ReadToEnd();
            //string jsonResponse = "{"data":[{"index":1.0,"name":"AY - Nov","size":45.2,"url":"https://drive.google.com/uc?export=download&id=1CM8IS34glZ674p-5vqd4l3wZqsLjhhjm"},{"index":2.0,"name":"AY-Dec","size":45.2,"url":"https://drive.google.com/uc?export=download&id=1HgZRUxGdX9--P56IWiJdEOWZvMS1fMBO"}]}"
            Debug.Log("@@@@@@@@@ get_magazine_list called, Response:" + jsonResponse);
            MagazineList magazineList = JsonUtility.FromJson<MagazineList>(jsonResponse);
            if(magazineList != null && magazineList.data != null && magazineList.data.Count > 0)
            {
                PlayerPrefs.SetString(DLCCache.PrefMagazineList, jsonResponse);
            }
            return magazineList;
        }
        catch (Exception ex)
        {
            Debug.Log(ex.StackTrace);
        }
        return null;
    }
}