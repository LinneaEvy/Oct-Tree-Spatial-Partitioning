using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class QueryEntity : MonoBehaviour
{
    public GameObject pos;
    private List<float3> octTreeChunk;
    private Vector3 position;
    Vector3 size;
    private bool startDrawing = false;

    private void OnEnable()
    {
        startDrawing = true;
    }

    private void OnDisable()
    {
        startDrawing = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        if (!startDrawing || OctTreeHolder.QuadTree == null)
        {
            return;
        }

        octTreeChunk = OctTreeHolder.QuadTree.query(10, null, pos.transform.position);

        
        
        if (!(octTreeChunk.Count >=2))
        {
            Gizmos.DrawWireCube(position, size);
            return;
        }
        position = octTreeChunk[0];
        size = octTreeChunk[1];

        Gizmos.DrawWireCube(position, size);
    }

}
