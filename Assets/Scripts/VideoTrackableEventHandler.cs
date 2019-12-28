/*===============================================================================
Copyright (c) 2017 PTC Inc. All Rights Reserved.
 
Vuforia is a trademark of PTC Inc., registered in the United States and other
countries.
===============================================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class VideoTrackableEventHandler : DefaultTrackableEventHandler
{
    #region PROTECTED_METHODS

    public bool autoplay = true;
    static bool isSecondTargetOfAction = true;
    public GameObject actionCanvas;
    public static bool isTrakingFound = false;
    GameObject HelpPanel;
    string[] RotateTargetName = new string[4] { "01_Front_Page", "05_Chicken", "07_Glasses", "04_Gnani_With_Youth" };
    public bool isMultipleVideoWithAudio = false;
    public GameObject[] resetObj;
    Vector3[] startScale;
    Vector3[] startPos;
    Quaternion[] startRotation;


    /*private void Start()
    {
        base.Start();
        if(resetObj != null && resetObj.Length > 0)
        {
            startScale = new Vector3[resetObj.Length];
            startPos = new Vector3[resetObj.Length];
            startRotation = new Quaternion[resetObj.Length];
            Debug.Log("@@@@@@@@@@@@@@@@@@@@@@ Lenght:" + resetObj.Length);
            for (int i = 0; i < resetObj.Length; i++ )
            {
                startPos[i] = resetObj[i].transform.position;
                startRotation[i] = resetObj[i].transform.rotation;
                startScale[i] = resetObj[i].transform.localScale;
                Debug.Log("Initial Position:" + startPos[i]);
            }
        }
    }*/

    protected override void OnTrackingLost()
    {
        isTrakingFound = false;
        var objVideoCont=mTrackableBehaviour.GetComponentsInChildren<VideoController>();
        var objAudioCon = mTrackableBehaviour.GetComponentsInChildren<AudioSource>();
        Animation[] animationComponents = GetComponentsInChildren<Animation>();
        Animator[] animatorComponents = GetComponentsInChildren<Animator>();
        for (int i=0;i<objVideoCont.Length;i++) {
            mTrackableBehaviour.GetComponentsInChildren<VideoController>()[i].Pause();
        }

        for (int i=0;i< objAudioCon.Length;i++) {
            mTrackableBehaviour.GetComponentsInChildren<AudioSource>()[i].Pause();
        }

        foreach (Animation component in animationComponents) {
            component.Stop();
            // component.Rewind();
        }

        foreach (Animator component in animatorComponents)
        {
            component.enabled = false;
            // component.Rewind();
        }
        /*if (actionCanvas != null)
        {
            actionCanvas.SetActive(false);
        }*/

        //HelpPanel.SetActive(true);
        base.OnTrackingLost();
    }
    int playVideoIndex = 0;
    VideoController[] videoControllers;
     protected override void OnTrackingFound()
    {
        Debug.Log("!!!!!!!!!!!!!!! OnTrackingFound:" + mTrackableBehaviour.name);
        HelpPanel = GameObject.Find("HelpText");
        if (HelpPanel != null)
        {
            HelpPanel.SetActive(false);
        }
        isTrakingFound = true;
        base.OnTrackingFound();
        /*foreach (string x in RotateTargetName)
        {
            if (mTrackableBehaviour.TrackableName.Contains(x))
            {
                if (actionCanvas != null)
                {
                    actionCanvas.SetActive(true);
                }
                //reset();
            }
        }*/

        if (autoplay) {
            var objVideoCont=mTrackableBehaviour.GetComponentsInChildren<VideoController>();
            var objAudioCon = mTrackableBehaviour.GetComponentsInChildren<AudioSource>();
            Animation[] animationComponents = GetComponentsInChildren<Animation>();
            Animator[] animatorComponents = GetComponentsInChildren<Animator>();
            videoControllers = (VideoController[]) mTrackableBehaviour.GetComponentsInChildren<VideoController>().Clone();
            if(isMultipleVideoWithAudio)
            {
                for (int j = 0; j < videoControllers.Length; j++)
                {
                    videoControllers[j].videoPlayer.loopPointReached += HandleOnVideoFinished;
                }
                HandleOnVideoFinished(null);
            }
            else
            {
                for(int j = 0; j < videoControllers.Length; j++)
                {
                    videoControllers[j].Play();
                }
            }
 
            for (int i = 0; i < objAudioCon.Length; i++) {
                mTrackableBehaviour.GetComponentsInChildren<AudioSource>()[i].Play();
            }
            if (true)//isFirst)
            {
                Debug.Log("##########    animation:" + animationComponents);
                if(animationComponents != null)
                {
                    foreach (Animation component in animationComponents)
                    {
                        Debug.Log("########## ani:" + component);
                        component.Rewind();
                        component.Play();
                    }
                }
                if(animationComponents != null)
                {
                    foreach (Animator component in animatorComponents)
                    {
                        component.enabled = true;
                        component.Rebind();
                    }
                }
                isSecondTargetOfAction = false;
            }
        }
    }

    void HandleOnVideoFinished(UnityEngine.Video.VideoPlayer video)
    {
        if (playVideoIndex >= videoControllers.Length)
            playVideoIndex = 0;
        int curPlayingVidIndex = playVideoIndex - 1;
        if (curPlayingVidIndex == -1)
            curPlayingVidIndex = videoControllers.Length - 1;
        videoControllers[curPlayingVidIndex].gameObject.SetActive(false);
        videoControllers[curPlayingVidIndex].Pause();
        videoControllers[playVideoIndex].gameObject.SetActive(true);
        videoControllers[playVideoIndex].Play();
        playVideoIndex++;
    }

    /*public void reset()
    {
        if (resetObj != null && resetObj.Length > 0)
        {
            Debug.Log("Reset Position:" + startRotation);
            for (int i = 0; i < resetObj.Length; i++)
            {
                //resetObj[i].transform.rotation = startRotation[i];
                //resetObj[i].transform.position = startPos[i];
                resetObj[i].transform.localScale = startScale[i];
                Debug.Log("Reset Position:" + startPos[i]);
            }
        }
    }*/

    #endregion // PROTECTED_METHODS
}
