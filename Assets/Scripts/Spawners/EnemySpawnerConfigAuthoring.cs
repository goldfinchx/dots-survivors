using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Spawners {
    public class EnemySpawnerConfigAuthoring : MonoBehaviour {
        [SerializeField] private int spawnRadius;
        [SerializeField] private float spawnRate;
        [SerializeField] private List<GameObject> prefabs;

        public class EnemySpawnerComponentBaker : Baker<EnemySpawnerConfigAuthoring> {
            public override void Bake(EnemySpawnerConfigAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new EnemySpawnerConfig(authoring.spawnRate, authoring.spawnRadius));

                DynamicBuffer<BufferedEnemyPrefab> buffer = AddBuffer<BufferedEnemyPrefab>(entity);
                foreach (GameObject prefab in authoring.prefabs) {
                    Entity prefabEntity = GetEntity(prefab, TransformUsageFlags.Dynamic);
                    buffer.Add(new BufferedEnemyPrefab { Value = prefabEntity });
                }
                
            }

        }
    }
}