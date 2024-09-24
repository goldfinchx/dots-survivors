using Unity.Entities;
using Unity.Mathematics;

namespace Movement {
    public struct Movement : IComponentData {
        public float2 Target;
        public float Speed;
    }
}