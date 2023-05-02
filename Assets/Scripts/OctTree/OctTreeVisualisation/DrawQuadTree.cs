using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawQuadTree : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        if (OctTreeHolder.QuadTree == null)
        {
            return;
        }
        List<Bounds> all = OctTreeHolder.QuadTree.getBounds(new List<Bounds>(), 0);
        Gizmos.color = Color.red;
        foreach (Bounds b in all)
        {
            Gizmos.DrawWireCube(b.center, b.size);
        }
    }
}
