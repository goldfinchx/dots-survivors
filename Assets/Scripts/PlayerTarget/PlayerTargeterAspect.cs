using Enemies;
using Movement;
using Unity.Entities;
using Unity.Mathematics;

namespace PlayerTarget {
    public readonly partial struct PlayerTargeterAspect : IAspect {

        private readonly RefRW<MovementComponent> movementComponent;
        private readonly RefRO<PlayerTargeterComponent> targeterComponent;
        
        private float2 MovementTarget {
            get => movementComponent.ValueRO.Target;
            set => movementComponent.ValueRW.Target = value;
        }
        
        public void Target(float2 playerLocation) {
            MovementTarget = playerLocation;
        }
    }
}