using Health;
using Unity.Entities;
using Unity.Mathematics;

namespace Damage {
    public readonly partial struct DamagedEntityAspect : IAspect {

        private readonly Entity entity;
        private readonly RefRO<DamagedTag> damageTag;
        private readonly RefRW<HealthComponent> health;
        
        public Entity Entity => entity;

        public float Health {
            get => health.ValueRO.Value;
            set => health.ValueRW.Value = value;
        }

        public void Damage(int damage) {
            Health = math.max(0, Health - damage);
        }
    }
}