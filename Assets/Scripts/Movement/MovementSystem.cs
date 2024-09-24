using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Movement {
    public partial struct MovementSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<Movement>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            MovementJob job = new() { DeltaTime = SystemAPI.Time.DeltaTime };
            job.Schedule();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }
    }

    [BurstCompile]
    public partial struct MovementJob : IJobEntity {

        public float DeltaTime;

        [BurstCompile]
        private void Execute(MovementAspect aspect) {
            aspect.Move(DeltaTime);
        }
    }

}