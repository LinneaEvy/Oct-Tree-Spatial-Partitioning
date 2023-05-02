using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu (fileName = "QuadTreeData", menuName = "OctTree/Data")]
public class QuadTreeData : ScriptableObject
{
    [SerializeField]public Prefab currentHolder;
    [SerializeField]public float dimension = 600;
    [SerializeField]public int maxInside = 4;
    [SerializeField]public float3 origin = float3.zero;
    [SerializeField]public Prefab Player;
    [SerializeField]public float searchRange = 200;
}
