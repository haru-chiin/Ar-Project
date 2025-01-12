using UnityEngine;

public class TouchableObject : MonoBehaviour
{
    [SerializeField] private bool isTilled = false;

    // This method will be called to toggle the object's status
    public void ToggleTouched()
    {
        isTilled = !isTilled;

        // Change the object's appearance based on its status
        if (isTilled)
        {
            // Change color to indicate it has been touched
            GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            // Change color back to indicate it is untouched
            GetComponent<Renderer>().material.color = Color.white;
        }
    }
}