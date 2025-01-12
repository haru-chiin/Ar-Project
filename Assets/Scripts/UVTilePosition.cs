using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVTilePosition : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Vector2[] originalUVs;

    void Start()
    {
        // Get the MeshRenderer component
        meshRenderer = GetComponent<MeshRenderer>();

        // Store the original UVs
        if (meshRenderer != null)
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                originalUVs = meshFilter.mesh.uv;
            }
        }
    }

    // Method to get the object's current UV position
    public Vector2 GetCurrentUVPosition()
    {
        if (meshRenderer != null)
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                Debug.Log(originalUVs);
                return meshFilter.mesh.uv[0]; // Return the UV position of the first vertex

            }
        }
        return Vector2.zero; // Return zero if no UVs are found
    }

    // Method to set the UV X position to a specified value

    public void ShiftUVX(float xShift)
    {
        if (meshRenderer != null)
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                Vector2[] uvs = meshFilter.mesh.uv;

                for (int i = 0; i < uvs.Length; i++)
                {
                    uvs[i].x += xShift; // Shift the X position
                }

                meshFilter.mesh.uv = uvs; // Update the mesh UVs
            }
        }
    }

    // Method to shift the UV Y position by a specified amount
    public void ShiftUVY(float yShift)
    {
        if (meshRenderer != null)
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                Vector2[] uvs = meshFilter.mesh.uv;

                for (int i = 0; i < uvs.Length; i++)
                {
                    uvs[i].y += yShift; // Shift the Y position
                }

                meshFilter.mesh.uv = uvs; // Update the mesh UVs
            }
        }
    }

    public void ResetUVs()
    {
        if (meshRenderer != null && originalUVs != null)
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                meshFilter.mesh.uv = originalUVs; // Reset UVs to the original values
            }
        }
    }
}