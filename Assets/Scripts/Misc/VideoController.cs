using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    private VideoPlayer _videoPlayer;

    // Start is called before the first frame update
    void Start()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        _videoPlayer.loopPointReached += EndVideo;

        Player_StaticActions.DisableHumanMovement();
        Player_StaticActions.DisableDogMovement();
        ScreenFade.current.FadeToBlack();
    }

    // Update is called once per frame
    void EndVideo(VideoPlayer vp)
    {
        Player_StaticActions.EnableHumanMovement();
        Player_StaticActions.EnableDogMovement();
        ScreenFade.current.FadeToTransparent();
        Destroy(gameObject);
    }
}
