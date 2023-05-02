using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct MoveData : IComponentData
{
    public float movoSpeed;
    public float3 targetDirection;
    public float rotateSpeed;
}
