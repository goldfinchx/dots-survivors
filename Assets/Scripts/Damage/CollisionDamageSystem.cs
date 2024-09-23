
using Enemies;
using Player;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

namespace Damage {
    
    [BurstCompile]
    [UpdateInGroup(typeof(PhysicsSystemGroup))]
    [UpdateAfter(typeof(PhysicsSimulationGroup))]
    public partial struct CollisionDamageSystem : ISystem {
        
        private ComponentLookup<PlayerTag> playerTagLookup;
        private ComponentLookup<EnemyTag> enemyTagLookup;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate(SystemAPI.QueryBuilder().WithAll<DamagedTag>().Build());
            state.RequireForUpdate<SimulationSingleton>();
            
            playerTagLookup = state.GetComponentLookup<PlayerTag>();
            enemyTagLookup = state.GetComponentLookup<EnemyTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            enemyTagLookup.Update(ref state);
            playerTagLookup.Update(ref state);
            
            EntitiesCollisionJob collisionJob = new() {
                PlayerTagLookup = playerTagLookup,
                EnemyTagLookup = enemyTagLookup
            };

            state.Dependency = collisionJob.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }
    }
    
    [BurstCompile]
    public struct EntitiesCollisionJob : ICollisionEventsJob {
        [ReadOnly] public ComponentLookup<PlayerTag> PlayerTagLookup;
        [ReadOnly] public ComponentLookup<EnemyTag> EnemyTagLookup;

        [BurstCompile]
        public void Execute(CollisionEvent collisionEvent) {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;
            
            bool differentEntities = PlayerTagLookup.HasComponent(entityA) && EnemyTagLookup.HasComponent(entityB) ||
                                     PlayerTagLookup.HasComponent(entityB) && EnemyTagLookup.HasComponent(entityA);
            
            if (!differentEntities) {
                return;
            }
            
            // todo apply events, add cooldown
            
            Debug.Log("Collision detected between player and enemy");
        }
    }

    
}