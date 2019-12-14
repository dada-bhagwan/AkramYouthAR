using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppUtiltiy : MonoBehaviour
{
    // Start is called before the first frame update
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
