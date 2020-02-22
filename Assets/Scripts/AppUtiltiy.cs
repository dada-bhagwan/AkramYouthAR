using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppUtiltiy : MonoBehaviour
{
    // Start is called before the first frame update


  public void OpenARapp()
    {
        Application.OpenURL("http://tiny.cc/akramyouthar");
    }


    public void OpenAkramYouth()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=org.dadabhagwan.youth");
    }


    public void OpenQuiz()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSc1hr_zad72XA0sThfzUQDIXQeYqVGWb_9yaHvHaAOE41yi_g/viewform?ts=5e272841");
    }
    public void OpenFacebookPage()
    {
        Application.OpenURL("https://www.facebook.com/akramyouth.mag/");
    }


    public void OpenPodcast()
    {
        Application.OpenURL("http://tiny.cc/AY-podcast");
    }

    public void OpenPodcastYoutubePlaylist()
    {
        Application.OpenURL("https://www.youtube.com/playlist?list=PLodZ8kFsDGFX5kco3t9xqYa1CHj72mL1E");
    }

    public void FeedbackMail()
    {
        Application.OpenURL("mailto:gncapps@googlegroups.com?subject=Feedback/Bug%20Report%20of%20Akram%20Youth%20AR");
    }

    public static bool IsInternetConnected()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return false;
        }
        return true;
    }
}
