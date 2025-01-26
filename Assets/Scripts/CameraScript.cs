using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        int DistanceAway = 10;
        Vector3 PlayerPOS = GameObject.FindGameObjectWithTag("Player").transform.position;
        Camera.main.transform.position = new Vector3(PlayerPOS.x, PlayerPOS.y, PlayerPOS.z - DistanceAway);
    }
}
