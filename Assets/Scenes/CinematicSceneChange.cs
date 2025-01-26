using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CinematicSceneChange : MonoBehaviour
{
    public VideoPlayer cinematic;

    // Start is called before the first frame update
    void Awake()
    {
        cinematic = GetComponent<VideoPlayer>();
        cinematic.Play();
        cinematic.loopPointReached += CheckOver;
    }

    // Update is called once per frame
    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        SceneManager.LoadScene(3);
    }
}
