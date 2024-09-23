using Player;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace PlayerTarget {
    public partial struct PlayerTargetSystem : ISystem {
        
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<PlayerTag>();
            state.RequireForUpdate<PlayerTargeterComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            float2 playerLocation = GetPlayerLocation(ref state);
            foreach (PlayerTargeterAspect targeter in SystemAPI.Query<PlayerTargeterAspect>()) {
                targeter.Target(playerLocation);
            }
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
        
      
    }
}
