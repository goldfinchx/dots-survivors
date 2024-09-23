using Player;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Random = UnityEngine.Random;

namespace Spawners {
    public partial struct EnemySpawnerSystem : ISystem, ISystemStartStop {

        private Entity spawnerEntity;
        private Entity playerEntity;

        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<BeginInitializationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<EnemySpawnerComponent>();
            state.RequireForUpdate<PlayerTag>();
        }

        [BurstCompile]
        public void OnStartRunning(ref SystemState state) {
            spawnerEntity = SystemAPI.GetSingletonEntity<EnemySpawnerComponent>();
            playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            EnemySpawnerComponent spawnerComponent = SystemAPI.GetComponent<EnemySpawnerComponent>(spawnerEntity);
            if (!(SystemAPI.Time.ElapsedTime - spawnerComponent.LastSpawnTime > spawnerComponent.SpawnRate)) {
                return;
            }

            EntityCommandBuffer entityCommandBuffer = SystemAPI
                .GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            EnemySpawnerJob spawnerJob = new() {
                CommandBuffer = entityCommandBuffer,
                EnemyPrefab = GetRandomEnemyPrefab(ref state),
                SpawnPosition = GetRandomSpawnPosition(ref state),
                SpawnerEntity = spawnerEntity,
                ElapsedTime = SystemAPI.Time.ElapsedTime
            };

            state.Dependency = spawnerJob.Schedule(state.Dependency);
        }

        [BurstCompile]
        private Entity GetRandomEnemyPrefab(ref SystemState state) {
            DynamicBuffer<BufferedEnemyPrefab> buffer = SystemAPI.GetBuffer<BufferedEnemyPrefab>(spawnerEntity);
            return buffer[Random.Range(0, buffer.Length)].Value;
        }

        [BurstCompile]
        private float2 GetRandomSpawnPosition(ref SystemState state) {
            LocalTransform localTransform = SystemAPI.GetComponent<LocalTransform>(playerEntity);
            int spawnRadius = SystemAPI.GetComponent<EnemySpawnerComponent>(spawnerEntity).SpawnRadius;
            float2 spawnOffset = new(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius));
            return localTransform.Position.xy + spawnOffset;
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        public void OnStopRunning(ref SystemState state) { }
    }
    
    [BurstCompile]
    public partial struct EnemySpawnerJob : IJobEntity {
        public EntityCommandBuffer CommandBuffer;
        [ReadOnly] public Entity EnemyPrefab;
        [ReadOnly] public Entity SpawnerEntity;
        [ReadOnly] public float2 SpawnPosition;
        [ReadOnly] public double ElapsedTime;

        [BurstCompile]
        private void Execute(ref EnemySpawnerComponent spawnerComponent) {
            if (!(ElapsedTime - spawnerComponent.LastSpawnTime > spawnerComponent.SpawnRate)) {
                return;
            }

            Entity enemy = CommandBuffer.Instantiate(EnemyPrefab);
            float3 position = new(SpawnPosition.xy, 0);

            spawnerComponent.LastSpawnTime = ElapsedTime;
            CommandBuffer.SetComponent(enemy, LocalTransform.FromPosition(position));
            CommandBuffer.SetComponent(SpawnerEntity, spawnerComponent);
        }
    }
}