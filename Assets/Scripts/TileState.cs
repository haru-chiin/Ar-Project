using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TileState : MonoBehaviour
{
    [SerializeField] private bool isTiled = false;
    [SerializeField] private bool isWatered = false;
    [SerializeField] private bool isSeeded = false;

    public void SetTiled()
    {
        isTiled = !isTiled;
    }

    public void SetWatered()
    {
        isWatered = !isWatered;
    }

    public void SetSeeded()
    {
        isSeeded = !isSeeded;
    }

    public bool GetIsTiled()
    {
        return isTiled;
    }

    public bool GetIsWatered()
    {
        return isWatered;
    }

    public bool GetSeeded()
    {
        return isSeeded;
    }
}