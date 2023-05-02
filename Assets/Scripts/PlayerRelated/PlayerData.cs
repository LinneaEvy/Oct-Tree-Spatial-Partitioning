using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct PlayerData : IComponentData
{
    
    [HideInInspector] public float inputHorizontal;
    [HideInInspector] public float inputVertical;

}
