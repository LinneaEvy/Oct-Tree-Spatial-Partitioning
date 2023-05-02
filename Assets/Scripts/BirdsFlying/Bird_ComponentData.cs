using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Bird_ComponentData : IComponentData
{
    public int num;
    public float cohesionBias;
    public float separationBias;
    public float alignmentBias;
    public float targetBias;
    public float perceptionRadius;
    public float speed;
    public float3 currentPosition;
    public float3 velocity;
    public float3 acceleration;
    public float step;
    public float3 target;
    public float cellSize;
    public int lastUpdated;
}
