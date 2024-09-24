using Unity.Entities;

namespace Health {
    public struct Health : IComponentData {
        public readonly float MaxValue;
        public float Value;
        
        public Health(float maxValue) {
            MaxValue = maxValue;
            Value = maxValue;
        }
    }
}