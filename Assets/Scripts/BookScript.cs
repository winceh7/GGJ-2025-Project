using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BookScript : MonoBehaviour
{
    private bool usable = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            TextMeshProUGUI text = GameObject.FindGameObjectWithTag("PlaceText").GetComponent<TextMeshProUGUI>();
            text.SetText("Press \"F\" to read the book");
            text.enabled = true;
            usable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            TextMeshProUGUI text = GameObject.FindGameObjectWithTag("PlaceText").GetComponent<TextMeshProUGUI>();
            text.enabled = false;
            usable = false;
        }
    }

    void Update()
    {
        bool fpressed = Input.GetKeyDown(KeyCode.F);

        if (fpressed && usable)
        {
            GameObject.FindGameObjectWithTag("BookLog").GetComponent<AudioSource>().Play();

        }
    }
}
