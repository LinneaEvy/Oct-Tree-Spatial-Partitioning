using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public partial class RotateTowards : SystemBase
{
    protected override void OnUpdate()
    {
        
        
        
        Entities.ForEach((ref Rotation rotation, in MoveData moveData) => {
            if (!moveData.targetDirection.Equals(float3.zero))
            {
                quaternion targetRotation = quaternion.LookRotationSafe(moveData.targetDirection, math.up());
                rotation.Value = math.slerp(rotation.Value, targetRotation, moveData.rotateSpeed);
            }
            

        }).Schedule();
    }
}
