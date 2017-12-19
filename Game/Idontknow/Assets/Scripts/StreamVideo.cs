using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Text.RegularExpressions;


/// <summary>
/// Unity VideoPlayer Script for Unity 5.6 (currently in beta 0b11 as of March 15, 2017)
/// Blog URL: http://justcode.me/unity2d/how-to-play-videos-on-unity-using-new-videoplayer/
/// YouTube Video Link: https://www.youtube.com/watch?v=nGA3jMBDjHk
/// StackOverflow Disscussion: http://stackoverflow.com/questions/41144054/using-new-unity-videoplayer-and-videoclip-api-to-play-video/
/// Code Contiburation: StackOverflow - Programmer
/// </summary>


public class StreamVideo : MonoBehaviour
{

    public VideoClip videoToPlay;
    public string videoURL;

    private VideoPlayer videoPlayer;
    private VideoSource videoSource;

    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {

        // Will attach a VideoPlayer to the main camera.
        GameObject camera = GameObject.Find("Main Camera");

        //Add VideoPlayer to the GameObject
        videoPlayer = camera.AddComponent<VideoPlayer>();

        //Add AudioSource
        audioSource = camera.AddComponent<AudioSource>();
    }

    public void execute()
    {
        Application.runInBackground = true;
        StartCoroutine(playVideo());
    }


    IEnumerator playVideo()
    {

        //Disable Play on Awake for both Video and Audio
        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;
        audioSource.Pause();

        if (videoToPlay != null)
        {
            //Use clip instead of url
            videoPlayer.source = VideoSource.VideoClip;
        }
        
        else
        {
            // Video clip from Url
            videoPlayer.source = VideoSource.Url;

            videoURL = Regex.Replace(videoURL, "( )+", "");

            if (videoURL != "")
            {
                videoPlayer.url = videoURL;
            }

            else
            {
                videoPlayer.url = "https://archive.org/download/RickAstleyNeverGonnaGiveYouUp_201603/Rick%20Astley%20-%20Never%20Gonna%20Give%20You%20Up.mp4";
            }
        
        }

        // old url http://www.quirksmode.org/html5/videos/big_buck_bunny.mp4


        //Set Audio Output to AudioSource
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        //Assign the Audio from Video to AudioSource to be played
        videoPlayer.controlledAudioTrackCount = 1;
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        //Set video To Play then prepare Audio to prevent Buffering
        videoPlayer.clip = videoToPlay;
        videoPlayer.Prepare();

        //Wait until video is prepared
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        Debug.Log("Done Preparing Video");

        //Play Video
        videoPlayer.Play();

        //Play Sound
        audioSource.Play();

        //Debug.Log("Playing Video");
        //while (videoPlayer.isPlaying)
        //{
        //    Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)videoPlayer.time));
        //    yield return null;
        //}

        Debug.Log("Done Playing Video");

        videoPlayer.loopPointReached += EndReached;
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        TriggerVidya triggervidya = GetComponent<TriggerVidya>();
        TriggerVidya.triggered= false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}