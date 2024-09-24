using Unity.Entities;

namespace Spawners {
    public struct EnemySpawnerConfig : IComponentData {
        public float SpawnRate;
        public int SpawnRadius;
        public double LastSpawnTime;
    }
}