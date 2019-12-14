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
        string jsonResponse = GETWebServiceCall("http://youthapi.dbf.ooo:8000/get_magazine_list", null);
        //string jsonResponse = "{"data":[{"index":1.0,"name":"AY - Nov","size":45.2,"url":"https://drive.google.com/uc?export=download&id=1CM8IS34glZ674p-5vqd4l3wZqsLjhhjm"},{"index":2.0,"name":"AY-Dec","size":45.2,"url":"https://drive.google.com/uc?export=download&id=1HgZRUxGdX9--P56IWiJdEOWZvMS1fMBO"}]}"
        Debug.Log("@@@@@@@@@ get_magazine_list called, Response:" + jsonResponse);
        if(jsonResponse != null)
        {
            MagazineList magazineList = JsonUtility.FromJson<MagazineList>(jsonResponse);
            if (magazineList != null && magazineList.data != null && magazineList.data.Count > 0)
            {
                PlayerPrefs.SetString(DLCCache.PrefMagazineList, jsonResponse);
            }
            return magazineList;
        }
        return null;
    }

    public static void IncMagazineDownload(String magazineId)
    {
        string json = "{\"id\": \"" + magazineId + "\"}";
        string jsonResponse = GETWebServiceCall("http://youthapi.dbf.ooo:8000/ay_score_increment", json);
        //string json = "{\"id\":\"test\"," + "\"password\":\"bla\"}";
    }

    static string GETWebServiceCall(string url, string json)
    {
        try
        {
            Debug.Log("url:" + url + "\tjson:" + json);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            if (json != null)
            {
                request.Method = "POST";
                request.ContentType = "application/json";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                }
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string jsonResponse = reader.ReadToEnd();
            Debug.Log("@@@@@@@@ URL " + url + "\t Response:" + jsonResponse);
            return jsonResponse;
        }
        catch (Exception ex)
        {
            Debug.LogError("Error while calling url:" + url + "\t" + ex.StackTrace);
        }
        return null;
    }
}