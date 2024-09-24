using Unity.Entities;

namespace Damage {
    public struct Damageable : IComponentData {
        public readonly float MaxValue;
        public readonly double Cooldown;
        public double LastDamageTime;
        
        public Damageable(float maxValue, double cooldown) {
            MaxValue = maxValue;
            Cooldown = cooldown;
            LastDamageTime = -1;
        }
    }
}