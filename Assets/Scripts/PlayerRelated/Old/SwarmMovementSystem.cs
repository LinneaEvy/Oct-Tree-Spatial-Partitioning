using System.Threading.Tasks;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Jobs;
/*
public partial class SwarmMovementSystem : SystemBase//this is live it is now active if we want it or not
{
    protected override void OnUpdate()
    {
        float ElapsedTime = (float) Time.ElapsedTime;
        Entities.ForEach((ref Translation trans, ref MovementComponent moveSpeed) =>
        {
            float zPosition = math.sin(ElapsedTime * moveSpeed.Value);
            trans.Value = new float3(trans.Value.x, zPosition, trans.Value.z);
        }).ScheduleParallel();
    }
}*/
