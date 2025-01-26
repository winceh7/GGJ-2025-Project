using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPressed : MonoBehaviour
{
    GameManager gameManager;
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnButtonPress);
    }

    void OnButtonPress()
    {
        GameManager sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<GameManager>();
        switch (this.name)
        {
            case "Play":
                sceneManager.LoadScene("Cinematic");
                break;

            case "Credits":
                sceneManager.LoadScene("Credits");
                break;
            case "Back":
                sceneManager.LoadScene("Menu");
                break;
        }
    }

}
