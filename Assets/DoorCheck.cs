using TMPro;
using UnityEngine;

public class DoorCheck : MonoBehaviour
{
    private GameObject[] pedestals;
    private int count = 0;
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
        countPedestals();
        if (count != 0)
        {
            TextMeshProUGUI text = GameObject.FindGameObjectWithTag("ProgressText").GetComponent<TextMeshProUGUI>();
            text.enabled = true;
            text.SetText(count.ToString() + "/4");
        }
        else
        {
            TextMeshProUGUI text = GameObject.FindGameObjectWithTag("ProgressText").GetComponent<TextMeshProUGUI>();
            text.enabled = false;
        }
    }
}
