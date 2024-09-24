using Unity.Entities;

namespace Health {
    public struct HealthComponent : IComponentData {
        public readonly float MaxValue;
        public float Value;
        
        public HealthComponent(float maxValue) {
            MaxValue = maxValue;
            Value = maxValue;
        }
    }
}