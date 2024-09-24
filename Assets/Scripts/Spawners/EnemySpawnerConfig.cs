using Unity.Entities;

namespace Spawners {
    public struct EnemySpawnerConfig : IComponentData {
        public readonly float SpawnRate;
        public readonly int SpawnRadius;
        public double LastSpawnTime;
        
        public EnemySpawnerConfig(float spawnRate, int spawnRadius) {
            SpawnRate = spawnRate;
            SpawnRadius = spawnRadius;
            LastSpawnTime = -1;
        }
    }
}