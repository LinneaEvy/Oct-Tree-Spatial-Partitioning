using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class QubeTreeManager : SystemBase
{
    private float time = 0;
    private NativeArray<Entity> tom;
    private static bool created = false;
    private StaticAproach QuadTreeInformation;
    protected override void OnCreate()
    {
        
    }

    protected override void OnStartRunning()
    {
        var entityQuery = EntityManager.CreateEntityQuery(typeof(StaticAproach));
        
        QuadTreeInformation = EntityManager.GetComponentData<StaticAproach>(entityQuery.GetSingletonEntity());
            
        OctTreeHolder.QuadTree = new QubeTree(QuadTreeInformation.dimension, QuadTreeInformation.maxInside, QuadTreeInformation.origin);
    }

    protected override void OnUpdate()
    {
       float3 it = new float3();
       GetEntityQuery(typeof(Translation), ComponentType.ReadWrite<QuadTreeData>());
       time += Time.DeltaTime;
       
       

       Entities.ForEach((ref Bird_ComponentData tom, in Translation trans) =>//tom is just used so that the player is not added to the tree
       {
            
            // clear tree add each entity every second (all 30 frames)
            if (tom.lastUpdated > 50 &&created)//recalcs all 5 seconds hopefully
            {
                OctTreeHolder.QuadTree.updateBird(trans.Value, tom.num); // may not be correct
                tom.lastUpdated = 0;

            }
            else if (created)
            {
                tom.lastUpdated ++;
            }

            

            if (time > 6&&!created)
            {
                OctTreeHolder.QuadTree.insert(trans.Value, tom.num, true); // may not be correct entityManager.GetComponentData<Translation>(mEntity).Value
            }
            // do distance check with the player from the tree each frame
       }).WithoutBurst().Run();
       if (time > 6 && !created)
       {
           created = true;
       }

       if (time > 19)
       {
            time = 0;
       }
       
       
    }

    protected override void OnDestroy()
    {
        OctTreeHolder.QuadTree = null;
    }
}
