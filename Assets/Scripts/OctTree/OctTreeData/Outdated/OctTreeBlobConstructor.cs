using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class OctTreeBlobConstructor : GameObjectConversionSystem
{
    protected override void OnUpdate()
    {
        BlobAssetReference<OctTreeDataBlobAsset> FinalOctTreeRefernce;
        using (BlobBuilder blobBuilder = new BlobBuilder(Allocator.Temp))
        {
            ref OctTreeDataBlobAsset waypointBlobAsset = ref blobBuilder.ConstructRoot<OctTreeDataBlobAsset>();
            BlobBuilderArray<OctTreeDataBlob> tom = blobBuilder.Allocate(ref waypointBlobAsset.octTreeArray, 1);
            tom[0] = new OctTreeDataBlob{dimension = 600, maxInside = 4, origin= new float3(0f,0f,0f), searchRange = 20};
            FinalOctTreeRefernce = blobBuilder.CreateBlobAssetReference<OctTreeDataBlobAsset>(Allocator.Persistent);
            Debug.Log(FinalOctTreeRefernce.Value.octTreeArray.Length);
        }
    }
}
