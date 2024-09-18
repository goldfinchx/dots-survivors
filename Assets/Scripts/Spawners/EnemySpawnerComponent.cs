using Unity.Entities;

namespace Spawners {
    public struct EnemySpawnerComponent : IComponentData {
        public float SpawnRate;
        public int SpawnRadius;
        public double LastSpawnTime;
    }
}