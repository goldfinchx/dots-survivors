using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Spawners {
    public class EnemySpawnerComponentAuthoring : MonoBehaviour {
        [SerializeField] private int spawnRadius;
        [SerializeField] private float spawnRate;
        [SerializeField] private List<GameObject> prefabs;

        public class EnemySpawnerComponentBaker : Baker<EnemySpawnerComponentAuthoring> {
            public override void Bake(EnemySpawnerComponentAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new EnemySpawnerComponent {
                    SpawnRadius = authoring.spawnRadius,
                    SpawnRate = authoring.spawnRate,
                    LastSpawnTime = 0
                });

                DynamicBuffer<BufferedEnemyPrefab> buffer = AddBuffer<BufferedEnemyPrefab>(entity);
                foreach (GameObject prefab in authoring.prefabs) {
                    Entity prefabEntity = GetEntity(prefab, TransformUsageFlags.Dynamic);
                    buffer.Add(new BufferedEnemyPrefab { Value = prefabEntity });
                }
                
            }

        }
    }
}