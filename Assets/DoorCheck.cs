using TMPro;
using UnityEngine;

public class DoorCheck : MonoBehaviour
{
    private GameObject[] pedestals;
    private int count = 0;
    private bool openable = false;
    private float openTime = 0;
    private bool ended = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && count == 4)
        {
            openable = true;
            GameObject textObj = GameObject.FindGameObjectWithTag("ProgressText");
            textObj.GetComponent<TextMeshProUGUI>().SetText("Press \"F\" to open the door");
            textObj.GetComponent<TextMeshProUGUI>().enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            openable = false;
            GameObject textObj = GameObject.FindGameObjectWithTag("ProgressText");
            textObj.GetComponent<TextMeshProUGUI>().enabled = false;
        }
    }
    
    void Start()
    {
        pedestals = GameObject.FindGameObjectsWithTag("Pedestals");
    }

    private void countPedestals()
    {
        count = 0;
        foreach (GameObject pedestal in pedestals)
        {
            bool holding = pedestal.GetComponent<PedestalVariables>().holding;
            count += holding ? 1 : 0;
        }
    }

    void Update()
    {
        bool openPressed = Input.GetKeyDown(KeyCode.F);
        countPedestals();
        if (ended)
        {
            GameObject textObj = GameObject.FindGameObjectWithTag("ProgressText");
            textObj.GetComponent<TextMeshProUGUI>().enabled = false;
        }
        else if (count != 0 && count != 4)
        {
            TextMeshProUGUI text = GameObject.FindGameObjectWithTag("ProgressText").GetComponent<TextMeshProUGUI>();
            text.enabled = true;
            text.SetText(count.ToString() + "/4");
        }
        else if (count == 4 && !openable)
        {
            GameObject textObj = GameObject.FindGameObjectWithTag("ProgressText");
            textObj.GetComponent<TextMeshProUGUI>().enabled = true;
            textObj.GetComponent<TextMeshProUGUI>().SetText("All orbs have been placed,\n go to the door to open it");
            textObj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 590f);
            textObj.GetComponent<RectTransform>().transform.localPosition = new Vector3(0f, 316f,0f);
        }
        else if (!openable)
        {
            TextMeshProUGUI text = GameObject.FindGameObjectWithTag("ProgressText").GetComponent<TextMeshProUGUI>();
            text.enabled = false;
        }
        if (openable && openPressed)
        {
            GameObject.FindGameObjectWithTag("DoorSound").GetComponent<AudioSource>().Play();
            GetComponent<BoxCollider2D>().enabled = false;
            openTime = Time.time;
            ended = true;
        }
        if (openTime > 0 && Time.time >= openTime + 13.5f)
        {
            GameObject.FindGameObjectWithTag("SceneManager").GetComponent<GameManager>().LoadScene("BookScene");
        }
    }
}
