using Unity.Entities;
using Unity.Mathematics;

namespace Movement {
    public struct MovementComponent : IComponentData {
        public float2 Target;
        public float Speed;
    }
}