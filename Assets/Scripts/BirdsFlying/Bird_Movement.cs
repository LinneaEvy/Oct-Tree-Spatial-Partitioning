using System.Collections;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class Bird_Movement : SystemBase
{
    public NativeParallelMultiHashMap<int, Bird_ComponentData> cellVsEntityPositions;

    public static int GetUniqueKeyForPosition(float3 position, int cellSize)
    {
        return (int) ((15 * math.floor(position.x / cellSize)) + 
                      (17 * math.floor(position.y / cellSize)) +
                      (19 * math.floor(position.z / cellSize)));
    }

    protected override void OnCreate()
    {
        cellVsEntityPositions = new NativeParallelMultiHashMap<int, Bird_ComponentData>(0, Allocator.Persistent);
    }

    protected override void OnDestroy()
    {
        cellVsEntityPositions.Dispose();
    }

    protected override void OnUpdate()
    {
        EntityQuery entityQueries = GetEntityQuery(typeof(Bird_ComponentData));
        cellVsEntityPositions.Clear();
        if (entityQueries.CalculateEntityCount() > cellVsEntityPositions.Capacity)
        {
            cellVsEntityPositions.Capacity = entityQueries.CalculateEntityCount();
        }

        NativeParallelMultiHashMap<int, Bird_ComponentData>.ParallelWriter cellVsEntityPositionsParallel =
            cellVsEntityPositions.AsParallelWriter();
        Entities.ForEach((ref Bird_ComponentData birdComponentData, ref Translation trans) =>
        {
            Bird_ComponentData bcValues = new Bird_ComponentData();
            bcValues = birdComponentData;
            bcValues.currentPosition = trans.Value;
            cellVsEntityPositionsParallel.Add(GetUniqueKeyForPosition(trans.Value, (int) birdComponentData.cellSize), bcValues);
        }).ScheduleParallel();

        float deltaTime = Time.DeltaTime;
        NativeParallelMultiHashMap<int, Bird_ComponentData> cellVsEntityPositionForJob = cellVsEntityPositions;
        Entities.WithBurst().WithReadOnly(cellVsEntityPositionForJob).ForEach((ref Bird_ComponentData birdComponentData, ref Translation trans, ref Rotation rot) =>
        {
            int key = GetUniqueKeyForPosition(trans.Value, (int) birdComponentData.cellSize);
            NativeParallelMultiHashMapIterator<int> nmhKeyIterator;
            Bird_ComponentData neighbour;
            int total = 0;
            float3 separation = float3.zero;
            float3 alignment = float3.zero;
            float3 cohesion = float3.zero;

            if (cellVsEntityPositionForJob.TryGetFirstValue(key, out neighbour, out nmhKeyIterator))
            {
                do
                {
                    if (!trans.Value.Equals(neighbour.currentPosition)&& math.distance(trans.Value,neighbour.currentPosition) < birdComponentData.perceptionRadius)
                    {
                        float3 distanceFromTo = trans.Value - neighbour.currentPosition;
                        separation += (distanceFromTo / math.distance(trans.Value, neighbour.currentPosition));
                        cohesion += neighbour.currentPosition;
                        alignment += neighbour.velocity;
                        total++;
                    }
                } while (cellVsEntityPositionForJob.TryGetNextValue(out neighbour, ref nmhKeyIterator));
            }

            if (total>0)
            {
                cohesion = cohesion / total;
                cohesion = cohesion - (trans.Value + birdComponentData.velocity);
                cohesion = math.normalize(cohesion) * birdComponentData.cohesionBias;
                
                separation = separation / total;
                separation = separation - birdComponentData.velocity;
                separation = math.normalize(separation) * birdComponentData.separationBias;

                alignment = alignment / total;
                alignment = alignment - birdComponentData.velocity;
                alignment = math.normalize(alignment) * birdComponentData.alignmentBias;

            }

            birdComponentData.acceleration += (cohesion + alignment + separation);
            rot.Value = math.slerp(rot.Value,
                quaternion.LookRotation(math.normalize(birdComponentData.velocity), math.up()), deltaTime * 10);
            birdComponentData.velocity += birdComponentData.acceleration;
            birdComponentData.velocity = math.normalize(birdComponentData.velocity) * birdComponentData.speed;
            trans.Value = math.lerp(trans.Value, (trans.Value + birdComponentData.velocity),
                deltaTime * birdComponentData.step);
            birdComponentData.acceleration =
                math.normalize(birdComponentData.target - trans.Value) * birdComponentData.targetBias;
        }).ScheduleParallel();
    }
}
