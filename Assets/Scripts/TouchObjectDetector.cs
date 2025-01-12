using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TouchObjectDetector : MonoBehaviour
{
    private XROrigin xrOrigin;
    private ActiveObjectManager activeObjectManager;

    void Start()
    {
        xrOrigin = FindObjectOfType<XROrigin>();
        activeObjectManager = FindObjectOfType<ActiveObjectManager>();
    }

    void Update()
    {
        // Check for touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Check if the touch phase is Began
            if (touch.phase == TouchPhase.Began)
            {
                // Create a ray from the touch position
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                // Perform the raycast
                if (Physics.Raycast(ray, out hit))
                {
                    // Check if the hit object has the "Touchable" tag
                    if (hit.collider.CompareTag("Touchable"))
                    {
                        // Set the touched object as the active object
                        activeObjectManager.SetActiveObject(hit.collider.gameObject);
                    }
                }
            }
        }
    }
}