using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PickUpOrbs : MonoBehaviour
{
    public string pickableOrb = "";
    public string pickedOrb = "";
    private Collider2D pedestalClose = null;
    private Animator animator;
    private (float, float) idleOffset, swimmingOffset;

    private void StopHolding(Collider2D other)
    {
        GameObject small = GameObject.FindGameObjectWithTag("Small" + char.ToUpper(pickedOrb[0]) + pickedOrb.Substring(1));
        small.GetComponent<SpriteRenderer>().enabled = false;
        small.GetComponent<Light2D>().enabled = false;
        animator.SetFloat("holding", 0);
        FindOrbByName("Big_" + pickedOrb).transform.position = new Vector2(other.transform.position.x, other.transform.position.y + 1.27f);
        pickedOrb = "";
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Orb" && pickedOrb == "")
        {
            GameObject.FindGameObjectWithTag("PickUpText").GetComponent<TextMeshProUGUI>().enabled = true;
            pickableOrb = other.name.Split("_")[1];
        }
        if (other.tag == "Pedestals" && !other.GetComponentInParent<PedestalVariables>().holding && pickedOrb != "")
        {
            GameObject.FindGameObjectWithTag("PlaceText").GetComponent<TextMeshProUGUI>().enabled = true;
            pedestalClose = other;

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Orb")
        {
            GameObject.FindGameObjectWithTag("PickUpText").GetComponent<TextMeshProUGUI>().enabled = false;
            pickableOrb = "";
        }
        if (other.tag == "Pedestals")
        {
            GameObject.FindGameObjectWithTag("PlaceText").GetComponent<TextMeshProUGUI>().enabled = false;
            pedestalClose = null;
        }
    }

    GameObject FindOrbByName(string name)
    {
        GameObject[] orbs = GameObject.FindGameObjectsWithTag("Orb");

        foreach (GameObject orb in orbs)
        {
            if (orb.name == name)
            {
                return orb;
            }
        }

        return null;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        idleOffset = (0.28f, 0.273f);
        swimmingOffset = (0.67f, -0.27f);
    }
    void Update()
    {
        bool pickUpPressed = Input.GetKeyDown(KeyCode.F);

        if (pickUpPressed && pickableOrb != "" && pickedOrb == "")
        {
            if (pedestalClose is not null && pedestalClose.GetComponentInParent<PedestalVariables>().holding && pickedOrb == "")
            {
                Debug.Log(pedestalClose.GetComponentInParent<PedestalVariables>().holding);
                pedestalClose.GetComponentInParent<PedestalVariables>().holding = false;
            }
            GameObject small = GameObject.FindGameObjectWithTag("Small" + char.ToUpper(pickableOrb[0]) + pickableOrb.Substring(1));
            GameObject orbToPick = FindOrbByName("Big_" + pickableOrb);
            orbToPick.transform.position = new Vector3(1000,1000,0);
            small.GetComponent<SpriteRenderer>().enabled = true;
            small.GetComponent<Light2D>().enabled = true;
            animator.SetFloat("holding", 1);
            pickedOrb = pickableOrb;
        }else if (pickUpPressed && pedestalClose is not null && !pedestalClose.GetComponentInParent<PedestalVariables>().holding && pickedOrb != "")
        {
            pedestalClose.GetComponentInParent<PedestalVariables>().holding = true;
            GameObject.FindGameObjectWithTag("PlaceText").GetComponent<TextMeshProUGUI>().enabled = false;
            StopHolding(pedestalClose);
        }

        if (pickedOrb != "" && animator.GetFloat("speed") < 0.1)
        {
            GameObject small = GameObject.FindGameObjectWithTag("Small" + char.ToUpper(pickedOrb[0]) + pickedOrb.Substring(1));
            small.transform.localPosition = new Vector2(idleOffset.Item1,idleOffset.Item2);
        }else if(pickedOrb != "")
        {
            GameObject small = GameObject.FindGameObjectWithTag("Small" + char.ToUpper(pickedOrb[0]) + pickedOrb.Substring(1));
            small.transform.localPosition = new Vector2(swimmingOffset.Item1, swimmingOffset.Item2);
        }
    }
}
