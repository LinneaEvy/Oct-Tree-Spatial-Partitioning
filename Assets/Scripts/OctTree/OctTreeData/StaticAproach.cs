using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
[GenerateAuthoringComponent]

    public class StaticAproach : IComponentData
    {
        [SerializeField]public Prefab currentHolder;
        [SerializeField]public float dimension = 600;
        [SerializeField]public int maxInside = 4;
        [SerializeField]public float3 origin = float3.zero;
        [SerializeField]public Prefab Player;
        [SerializeField]public float searchRange = 200;
    }