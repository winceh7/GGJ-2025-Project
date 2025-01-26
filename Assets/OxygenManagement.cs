using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OxygenManagement : MonoBehaviour
{

    [SerializeField] private float maxOxygenCount;
    [SerializeField] private float recoveryFactor;

    private bool breathing = false;
    public float remainingOxygen;
    public float initialOverlayFactor;
    public bool prevDead, dead;
    private GameObject overlay;
    private bool exhale, drowning;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bubble")
        {
            breathing = true;
            AudioSource enterSound = GameObject.FindGameObjectWithTag("EnterBubble").GetComponent<AudioSource>();
            AudioSource exitSound = GameObject.FindGameObjectWithTag("ExitBubble").GetComponent<AudioSource>();
            exitSound.Stop();
            enterSound.Play();
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Bubble")
        {
            breathing = false;
            AudioSource enterSound = GameObject.FindGameObjectWithTag("EnterBubble").GetComponent<AudioSource>();
            AudioSource exitSound = GameObject.FindGameObjectWithTag("ExitBubble").GetComponent<AudioSource>();
            enterSound.Stop();
            exitSound.Play();
        }
    }

    private void Start()
    {
        remainingOxygen = maxOxygenCount;
        overlay = GameObject.FindGameObjectWithTag("DyingIndicator");
        Drowning();
    }

    private void Exhale()
    {
        exhale = true;
        GameObject[] exhaleSounds = GameObject.FindGameObjectsWithTag("BreathingSounds");
        int randomNumber = UnityEngine.Random.Range(0, exhaleSounds.Length);
        exhaleSounds[randomNumber].GetComponent<AudioSource>().Play();
    }

    private void Update()
    {
        if (breathing)
        {
            remainingOxygen = Math.Min(remainingOxygen + Time.deltaTime * recoveryFactor, maxOxygenCount);
            exhale = !(remainingOxygen == maxOxygenCount);
        }
        else
        {
            remainingOxygen = Math.Max(remainingOxygen - Time.deltaTime, 0);
        }
        prevDead = dead;
        dead = remainingOxygen <= 0;
        Drowning();
        UpdateDeathStatus();

        if (remainingOxygen < maxOxygenCount / 2 && !exhale)
        {
            Exhale();
        }
        if (remainingOxygen < 15 && !drowning)
        {
            drowning = true;
            GameObject.FindGameObjectWithTag("Drowning").GetComponent<AudioSource>().Play();
        }else if(breathing)
        {
            drowning = false;
            GameObject.FindGameObjectWithTag("Drowning").GetComponent<AudioSource>().Stop();
        }
    }

    private void Drowning()
    {
        float scale = remainingOxygen > 1 ?
            1 + ((remainingOxygen-1)/(maxOxygenCount-1)) * (initialOverlayFactor - 1)
            : 1;

        overlay.transform.localScale = new Vector3(scale, scale, 1);
    }

    private void UpdateDeathStatus()
    {
        if (dead && !prevDead)
        {
            GameObject.FindWithTag("DeathSound").GetComponent<AudioSource>().Play();
            GameObject diver = GameObject.FindGameObjectWithTag("Player");
            Animator animator = diver.GetComponent<Animator>();
            animator.SetTrigger("death");
        }
    }

}