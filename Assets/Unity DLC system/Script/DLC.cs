using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading;

public class DLC : MonoBehaviour
{
    [Header("UI Staff")]
    public Image image;
    public Image background;
    public Text nameText;
    public Slider progressbar;
    public Button downloadbutton;
    public Button deleteButton;
    public RadialFill radial;
    [Header("Appearance")]
    public Color avilavleColor, downloadedColor;

    string bundalUrl, filePath;

    public void Inti(Magazine magazine)
    {
        string dlcName = magazine.name;
        string dlcUrl = magazine.url;
        nameText.text = dlcName;
        bundalUrl = dlcUrl;
        filePath = LoadMagazine.dlcPath + magazine.fileName;
        bool downloaded = File.Exists(filePath);
        background.color = downloaded ? downloadedColor : avilavleColor;
        //progressbar.value = downloaded ? 1 : 0;
        radial.CurrentValue = downloaded ? 1 : 0;
        downloadbutton.gameObject.SetActive(!downloaded);
        deleteButton.gameObject.SetActive(downloaded);
    }
    public void Download()
    {
        StartCoroutine(CoDownload());
    }
    public void Delete()
    {
        if (File.Exists(filePath))
        {
            if (Path.GetExtension(filePath) != ".meta")
            {
                File.Delete(filePath);
            }
        }
        else if (Directory.Exists(filePath))
        {
            string[] dlcFiles = Directory.GetFiles(filePath);
            foreach (var item in dlcFiles)
            {
                if (Path.GetExtension(item) != ".meta")
                {
                    File.Delete(item);
                    //bool used = false;
                    //var filedata = dlcUrls.FirstOrDefault(x => x.EndsWith(item));
                    //if (filedata != null)
                    //{
                    //}
                }
            }
        }
        DLCDownloadPage.main.ReloadDLCGameObject();
    }
    
    IEnumerator CoDownload()
    {
        downloadbutton.gameObject.SetActive(false);

        //Delete.gameObject.SetActive(false);
        //using (WebClient webClient = new WebClient())
        //{
        //    try
        //    {
        //        if (bundalUrl.Contains("drive.google.com/"))
        //        {
        //            if (bundalUrl.Contains("1l9Zwbmh66JpcS881MsJbFrvS2tZ43XFQ"))
        //            {
        //                filePath = UnityDLC.dlcPath + "mydlc.dlc";
        //            }
        //            else
        //            {
        //                filePath = UnityDLC.dlcPath + "primitive.dlc";
        //            }
        //        }

        //        webClient.DownloadFile(bundalUrl, filePath);

        //        //#if !UNITY_WEBPLAYER
        //        //                File.WriteAllBytes(filePath, byt);
        //        //#endif
        //        //UnityDLC.main.ShowDLC();

        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    yield return null;

        //}

        //Thread thread = new Thread(() => DownloadFile(bundalUrl, filePath)) { Name = "Thread 1" };
        //thread.Start();
        //DownloadFile(bundalUrl,filePath);
        //DownloadMultipleFilesAsync(new List<string>() { bundalUrl });


        //yield return null;


        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //request.Method = "PUT";
        //request.ContentType = "application/octet-stream";
        //request.ContentLength = data.Length;
        //Stream stream = request.GetRequestStream();
        //stream.Write(data, 0, data.Length);
        //stream.Close();
        //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //StreamReader reader = new StreamReader(response.GetResponseStream());
        //temp = reader.();
        //reader.Close();
        /*if (bundalUrl.Contains("drive.google.com/"))
        {
            if (bundalUrl.Contains("1l9Zwbmh66JpcS881MsJbFrvS2tZ43XFQ"))
            {
                filePath = UnityDLC.dlcPath + "mydlc.dlc";
            }
            else
            {
                filePath = UnityDLC.dlcPath + "primitive.dlc";
            }
        }*/
        using (WWW www = new WWW(bundalUrl))
        {
            while (!www.isDone && string.IsNullOrEmpty(www.error))
            {
                //progressbar.value = www.progress;
                radial.CurrentValue = www.progress;
                yield return null;
            }

            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError(www.error);
                yield break;
            }
            String tmpFilePath = filePath + ".tmp"; 
#if !UNITY_WEBPLAYER
            File.WriteAllBytes(tmpFilePath, www.bytes);
            System.IO.File.Move(tmpFilePath, filePath);
#endif
        }
        deleteButton.gameObject.SetActive(true);
        DLCDownloadPage.main.ReloadDLCGameObject();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
	
	void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
        try
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            //progressbar.value = int.Parse(Math.Truncate(percentage).ToString());
            radial.CurrentValue = int.Parse(Math.Truncate(percentage).ToString());
        }
        catch (Exception ex)
        {
            Debug.LogError("@@@@@@@@@@@@@ from DownloadChanged : " + ex.Message);

        }
    }
    //public bool GetDownloadFile()
    //{
    //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://youthapi.dbf.ooo:8000/ay_version");
    //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
    //    StreamReader reader = new StreamReader(response.GetResponseStream());
    //    string jsonResponse = reader.ReadToEnd();
    //    Debug.Log("######################### " + jsonResponse);
    //    return true;
    //}
    private void DownloadFile(string bundalUrl, string filePath)
    {
        try
        {
            //            using (WWW www = new WWW(bundalUrl))
            //            {
            //                while (!www.isDone && string.IsNullOrEmpty(www.error))
            //                {
            //                    progressbar.value = www.progress;
            //                    //yield return null;
            //                }

            //                if (!string.IsNullOrEmpty(www.error))
            //                {
            //                    Debug.LogError(www.error);
            //                    //yield break;
            //                }
            //#if !UNITY_WEBPLAYER
            //                File.WriteAllBytes(filePath, www.bytes);
            //#endif
            //            }

            using (WebClient webClient = new WebClient())
            {
                /*if (bundalUrl.Contains("drive.google.com/"))
                {
                    if (bundalUrl.Contains("1l9Zwbmh66JpcS881MsJbFrvS2tZ43XFQ"))
                    {
                        filePath = UnityDLC.dlcPath + "mydlc.dlc";
                    }
                    else
                    {
                        filePath = UnityDLC.dlcPath + "primitive.dlc";
                    }
                }*/
                webClient.DownloadProgressChanged += client_DownloadProgressChanged;
                webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
                webClient.DownloadFile(bundalUrl, filePath);

                //Add them to the local
                //Context.listOfLocalDirectories.Add(downloadToDirectory);
            }

        }
        catch (Exception ex)
        {
            Debug.LogError("@@@@@@@@@@@@@: " + ex.Message);
        }
    }

    private void WebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
    {
        try
        {
            System.Threading.Thread.Sleep(5000);
            DLCDownloadPage.main.ReloadDLCGameObject();
        }
        catch (Exception ex)
        {
            Debug.LogError("@@@@@@@@@@@@@ from DownloadCompleted : " + ex.Message);

        }
    }

    //private async Task DownloadMultipleFilesAsync(List<string> doclist)
    //{
    //    await Task.WhenAll(doclist.Select(doc => DownloadFile(doc)));
    //}
}
