using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public partial class PlayerSystem : SystemBase
{
    

    protected override void OnUpdate()
    {
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");
        Entities.ForEach((ref PlayerData input, ref MoveData move) =>
        {
            input.inputHorizontal = inputH;
            input.inputVertical = inputV;

            move.targetDirection = new Unity.Mathematics.float3(inputH, 0, inputV);

        }).Schedule();
    }
}
