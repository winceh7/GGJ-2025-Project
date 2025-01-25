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
    private (float, float) minRedOverlaySize;
    private (float, float) maxRedOverlaySize;
    public bool dead;
    private GameObject overlay;
    private void OnTriggerEnter2D(Collider2D other)
    {
        breathing = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        breathing = false;
    }

    private void Start()
    {
        remainingOxygen = maxOxygenCount;
        overlay = GameObject.FindGameObjectWithTag("DyingIndicator");
        Drowning();
    }

    private void Update()
    {
        if (breathing)
        {
            remainingOxygen = Math.Min(remainingOxygen + Time.deltaTime * recoveryFactor, maxOxygenCount);
        }else
        {
            remainingOxygen = Math.Max(remainingOxygen - Time.deltaTime, 0);
        }
        dead = remainingOxygen <= 0;
        Drowning();
        UpdateDeathStatus();
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
        if (dead)
        {
            GameObject diver = GameObject.FindGameObjectWithTag("Player");
            Animator animator = diver.GetComponent<Animator>();
            animator.SetTrigger("death");
        }
    }

}