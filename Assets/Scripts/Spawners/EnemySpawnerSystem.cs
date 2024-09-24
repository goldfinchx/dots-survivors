using Player;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Random = UnityEngine.Random;

namespace Spawners {
    public partial struct EnemySpawnerSystem : ISystem {
        

        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<BeginInitializationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<EnemySpawnerConfig>();
            state.RequireForUpdate<PlayerTag>();
        }
        

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            EnemySpawnerConfig spawnerConfig = SystemAPI.GetSingleton<EnemySpawnerConfig>();
            if (!(SystemAPI.Time.ElapsedTime - spawnerConfig.LastSpawnTime > spawnerConfig.SpawnRate)) {
                return;
            }

            EntityCommandBuffer entityCommandBuffer = SystemAPI
                .GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            Entity spawnerEntity = SystemAPI.GetSingletonEntity<EnemySpawnerConfig>();
            Entity playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
            EnemySpawnerJob spawnerJob = new() {
                CommandBuffer = entityCommandBuffer,
                EnemyPrefab = GetRandomEnemyPrefab(ref state, spawnerEntity),
                SpawnPosition = GetRandomSpawnPosition(ref state, spawnerEntity, playerEntity),
                SpawnerEntity = spawnerEntity,
                ElapsedTime = SystemAPI.Time.ElapsedTime
            };

            state.Dependency = spawnerJob.Schedule(state.Dependency);
        }

        [BurstCompile]
        private Entity GetRandomEnemyPrefab(ref SystemState state, Entity spawnerEntity) {
            DynamicBuffer<BufferedEnemyPrefab> buffer = SystemAPI.GetBuffer<BufferedEnemyPrefab>(spawnerEntity);
            return buffer[Random.Range(0, buffer.Length)].Value;
        }

        [BurstCompile]
        private float2 GetRandomSpawnPosition(ref SystemState state, Entity spawnerEntity, Entity playerEntity) {
            LocalTransform localTransform = SystemAPI.GetComponent<LocalTransform>(playerEntity);
            int spawnRadius = SystemAPI.GetComponent<EnemySpawnerConfig>(spawnerEntity).SpawnRadius;
            float2 spawnOffset = new(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius));
            return localTransform.Position.xy + spawnOffset;
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }
        
    }
    
    [BurstCompile]
    public partial struct EnemySpawnerJob : IJobEntity {
        public EntityCommandBuffer CommandBuffer;
        [ReadOnly] public Entity EnemyPrefab;
        [ReadOnly] public Entity SpawnerEntity;
        [ReadOnly] public float2 SpawnPosition;
        [ReadOnly] public double ElapsedTime;

        [BurstCompile]
        private void Execute(ref EnemySpawnerConfig spawnerConfig) {
            if (!(ElapsedTime - spawnerConfig.LastSpawnTime > spawnerConfig.SpawnRate)) {
                return;
            }

            Entity enemy = CommandBuffer.Instantiate(EnemyPrefab);
            float3 position = new(SpawnPosition.xy, 0);

            spawnerConfig.LastSpawnTime = ElapsedTime;
            CommandBuffer.SetComponent(enemy, LocalTransform.FromPosition(position));
            CommandBuffer.SetComponent(SpawnerEntity, spawnerConfig);
        }
    }
}