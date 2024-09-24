using Movement;
using Movements;
using Player;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace PlayerTargeting {
    public partial struct PlayerTargetSystem : ISystem {
        
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<PlayerTag>();
            state.RequireForUpdate<PlayerTargeting.PlayerTargeterTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            state.Dependency = new TargetingJob {
                PlayerLocation = GetPlayerLocation(ref state)
            }.Schedule(state.Dependency);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }


        [BurstCompile]
        private float2 GetPlayerLocation(ref SystemState state) {
            Entity playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
            LocalTransform localTransform = SystemAPI.GetComponent<LocalTransform>(playerEntity);
            float2 playerLocation = localTransform.Position.xy;
            return playerLocation;
        }
        
        [BurstCompile]
        public partial struct TargetingJob : IJobEntity {
            public float2 PlayerLocation;

            [BurstCompile]
            private void Execute(PlayerTargeterTag _, ref MovementComponent movement) {
                movement.Target = PlayerLocation;
            }
        }
        
      
    }
}
