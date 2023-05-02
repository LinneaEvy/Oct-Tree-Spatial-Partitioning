using System.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Mesh unitMesh;
    [SerializeField] private Material unitMaterial;
    [SerializeField] private GameObject SpiderPrefab;
    
    [SerializeField] private int numOfEntitiesPerLine = 10;
    [SerializeField] private int GridDistance = 12;
    [SerializeField] private float minDistance = 2f;
    
    private Entity SpiderEntity;
    private World defaultWorld;
    private EntityManager ecsManager;
    
    
    void Start()
    {
        defaultWorld = World.DefaultGameObjectInjectionWorld;
        ecsManager = defaultWorld.EntityManager;
        
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(defaultWorld, null);
        SpiderEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(SpiderPrefab, settings);
        
        InstantiateEntityLine(numOfEntitiesPerLine, GridDistance/2, minDistance);
        InstantiateEntityLine(numOfEntitiesPerLine, -(GridDistance/2),minDistance);
    }

    private void InstantiateEntity(float3 position)
    {
        Entity mEntity = ecsManager.Instantiate(SpiderEntity);
        ecsManager.SetComponentData(mEntity, new Translation{Value = position});
    }

    private void InstantiateEntityLine(int numberOfEntitys, int locationz, float spacing = 1f)
    {
        for (int i = 0; i < numberOfEntitys; i++)
        {
            InstantiateEntity(new float3(i*spacing + Random.Range(0f,1f), 0f, locationz));
        }
    }

    /*private void MakeEntity()
    {
        EntityManager ecsManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityArchetype archetype = ecsManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(LocalToWorld)
            );
        Entity MySpider = ecsManager.CreateEntity(archetype);
        ecsManager.AddComponentData(MySpider, new Translation {Value = new float3(2f, 0f, 4f)});
        ecsManager.AddSharedComponentData(MySpider, new RenderMesh {mesh = unitMesh, material = unitMaterial});
    }*/
}
