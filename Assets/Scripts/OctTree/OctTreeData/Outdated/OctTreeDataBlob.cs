using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct OctTreeDataBlob
{
    public float dimension;
    public int maxInside;
    public float3 origin;
    public float searchRange;
}

public struct OctTreeDataBlobAsset
{
    public BlobArray<OctTreeDataBlob> octTreeArray;
}
