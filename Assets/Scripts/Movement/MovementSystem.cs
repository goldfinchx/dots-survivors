using Unity.Burst;
using Unity.Entities;

namespace Movement {
    public partial struct MovementSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<MovementComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            float deltaTime = SystemAPI.Time.DeltaTime;
            foreach (MovementAspect aspect in SystemAPI.Query<MovementAspect>()) {
                aspect.Move(deltaTime);
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }
    }
}