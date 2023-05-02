using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;
using UnityEngine;

public class Bird_Spawner : MonoBehaviour
{
    public int birdsPerInterval;
    public int birdsToSpawn;
    public float interval;
    public float cohesionBias;
    public float separationBias;
    public float alignmentBias;
    public float targetBias;
    public float perceptionRadius;
    public float3 target;
    public Material material;
    public float maxSpeed;
    public float step;
    public int cellSize;
    private EntityManager entityManager;
    private Entity entity;
    private float elapsedTime;
    private int totalSpawnedBirds = 0;
    private EntityArchetype entityArchetype;
    private float3 currentPosition;
    public GameObject Prefab;
    public Entity ConvertedPrefab;
    public QuadTreeData data;
    
    
    void Start()
    {
        var defaultWorld = World.DefaultGameObjectInjectionWorld;
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(defaultWorld, null);
        ConvertedPrefab = Unity.Entities.GameObjectConversionUtility.ConvertGameObjectHierarchy(Prefab, settings);
        entityManager = defaultWorld.EntityManager;
        currentPosition = this.transform.position;
        entityArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(LocalToWorld),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(Bird_ComponentData)
        );
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= interval)
        {
            elapsedTime = 0;
            for (int i = 0; i <= birdsPerInterval; i++)
            {
                if (totalSpawnedBirds == birdsToSpawn)
                {
                    break;
                }

                //Entity e = entityManager.CreateEntity(entityArchetype);
                Entity mEntity = entityManager.Instantiate(ConvertedPrefab);
                entityManager.AddComponentData(mEntity, new Translation
                {
                    Value = currentPosition
                });
                entityManager.AddComponentData(mEntity, new Bird_ComponentData
                {
                    velocity = math.normalize(UnityEngine.Random.insideUnitSphere) * maxSpeed,
                    perceptionRadius = perceptionRadius,
                    speed = maxSpeed,
                    step = step,
                    cohesionBias = cohesionBias,
                    separationBias = separationBias,
                    alignmentBias = alignmentBias,
                    target = target,
                    targetBias = targetBias,
                    cellSize = cellSize,
                    num = totalSpawnedBirds,
                    lastUpdated = totalSpawnedBirds
                });
                
                /*entityManager.AddSharedComponentData(e, new RenderMesh
                {
                    mesh = mesh,
                    material = material,
                    castShadows = UnityEngine.Rendering.ShadowCastingMode.Off
                });*/
                totalSpawnedBirds++;
            }
        }
    }
}
