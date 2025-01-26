using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempClick : MonoBehaviour
{
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameManager.LoadScene("Level");
        }
    }
}
