using Health;
using Unity.Entities;
using Unity.Mathematics;

namespace Damage {
    public readonly partial struct DamagableEntityAspect : IAspect {

        private readonly Entity entity;
        private readonly RefRW<HealthComponent> health;

        public Entity Entity => entity;
    }
}