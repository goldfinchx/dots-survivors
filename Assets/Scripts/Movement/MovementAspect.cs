using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Movement {
    public readonly partial struct MovementAspect : IAspect {

        private readonly RefRW<LocalTransform> localTransform;
        private readonly RefRO<Movement> movementComponent;

        private quaternion Rotation {
            get => localTransform.ValueRO.Rotation;
            set => localTransform.ValueRW.Rotation = value;
        }

        private float2 Position {
            get => localTransform.ValueRO.Position.xy;
            set => localTransform.ValueRW.Position.xy = value;
        }

        private float2 Target => movementComponent.ValueRO.Target;
        private float Speed => movementComponent.ValueRO.Speed;

        public void Move(float deltaTime) {
            if (math.distance(Position, Target) < 0.1f) {
                return;
            }
            
            float2 direction = math.normalize(Target - Position);
            float2 movement = direction * Speed * deltaTime;
            Position += movement;
            Rotation = quaternion.LookRotation(new float3(direction, 0), new float3(0, 0, 1));
        }
    }
}