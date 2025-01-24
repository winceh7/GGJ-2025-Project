using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float parallaxMultiplier;

    private Transform cameraTransform;
    private Vector3 previousCameraPosition;
    private float spriteWidth, spriteHeight;
    private float startX, startY;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        spriteHeight = GetComponent<SpriteRenderer>().bounds.size.y;
        startX = transform.position.x;
        startY = transform.position.y;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float deltaX = (cameraTransform.position.x - previousCameraPosition.x) * parallaxMultiplier;
        float deltaY = (cameraTransform.position.y - previousCameraPosition.y) * parallaxMultiplier;
        float xMovement = cameraTransform.position.x * (1 - parallaxMultiplier);
        float yMovement = cameraTransform.position.y * (1 - parallaxMultiplier);
        transform.Translate(new Vector3(deltaX, deltaY, 0));
        previousCameraPosition = cameraTransform.position;

        if (xMovement > startX + spriteWidth)
        {
            transform.Translate(new Vector3(spriteWidth, 0, 0));
            startX += spriteWidth;
        }
        else if (xMovement < startX - spriteWidth){
            transform.Translate(new Vector3(-spriteWidth, 0, 0));
            startX -= spriteWidth;
        }

        if (yMovement > startY + spriteHeight)
        {
            transform.Translate(new Vector3(0, spriteHeight, 0));
            startY += spriteHeight;
        }
        else if (yMovement < startY - spriteHeight)
        {
            transform.Translate(new Vector3(0, -spriteHeight, 0));
            startY -= spriteHeight;
        }
    }
}
