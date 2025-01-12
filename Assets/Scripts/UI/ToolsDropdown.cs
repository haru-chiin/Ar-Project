using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsDropdown : MonoBehaviour
{
    public void ToolSelecter(int index)
    {
        switch(index)
        {
            case 0:Debug.Log("Hoe is active");break;
            case 1:Debug.Log("Watering Can is active");break;
        }
    }
}
