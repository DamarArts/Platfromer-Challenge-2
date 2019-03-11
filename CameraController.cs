using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float offsetx;
    public float offsety;
    public float CamSmoothing;
    public PlayerControls PlayerScript;

    public AudioClip musicClipOne;
    public AudioClip musicClipWon;
    public AudioSource musicSource;
    public int Juls;

    private void Start()
    {
        Juls = 0;
        musicSource.clip = musicClipOne;
        musicSource.Play();
    }

    void Update()
    {
        if (Juls == 21)
        {
            musicSource.clip = musicClipWon;
            musicSource.Play();
        }
    }
    private void FixedUpdate()
    {   
        Vector3 CamPosition = new Vector3(player.position.x + offsetx, player.position.y + offsety,-1) ;
        Vector3 SmoothCam = Vector3.Lerp(transform.position, CamPosition, CamSmoothing * Time.deltaTime);
        transform.position = SmoothCam;

        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }



    }
}
