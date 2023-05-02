using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

public partial class MoveSystem : SystemBase
{
    

protected override void OnUpdate()
{
    /*float deltaTime = Time.DeltaTime;
    Entities.ForEach((ref Translation trans, in MoveData movement) =>
    {
        trans.Value += movement.targetDirection * movement.movoSpeed * deltaTime;
        
    }).Schedule();*/
}
}
