using Enemies;
using Player;
using Skills;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Damage {
    
    [BurstCompile]
    [UpdateInGroup(typeof(PhysicsSystemGroup))]
    [UpdateAfter(typeof(PhysicsSimulationGroup))]
    public partial struct CollisionDamageSystem : ISystem {
        
        private ComponentLookup<PlayerTag> playerTagLookup;
        private ComponentLookup<EnemyTag> enemyTagLookup;
        private ComponentLookup<SkillTag> skillTagLookup;
        private BufferLookup<DamageEvent> damageEventsLookup;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<SimulationSingleton>();
            
            playerTagLookup = state.GetComponentLookup<PlayerTag>();
            enemyTagLookup = state.GetComponentLookup<EnemyTag>();
            skillTagLookup = state.GetComponentLookup<SkillTag>();
            damageEventsLookup = state.GetBufferLookup<DamageEvent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            enemyTagLookup.Update(ref state);
            playerTagLookup.Update(ref state);
            skillTagLookup.Update(ref state);
            damageEventsLookup.Update(ref state);
            
            EntitiesCollisionJob collisionJob = new() {
                PlayerTagLookup = playerTagLookup,
                EnemyTagLookup = enemyTagLookup,
                SkillTagLookup = skillTagLookup,
                DamageEventsLookup = damageEventsLookup
            };

            state.Dependency = collisionJob.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }
    }
    
    [BurstCompile]
    public struct EntitiesCollisionJob : ICollisionEventsJob {
        public BufferLookup<DamageEvent> DamageEventsLookup;
        [ReadOnly] public ComponentLookup<PlayerTag> PlayerTagLookup;
        [ReadOnly] public ComponentLookup<EnemyTag> EnemyTagLookup;
        [ReadOnly] public ComponentLookup<SkillTag> SkillTagLookup;

        [BurstCompile]
        public void Execute(CollisionEvent collisionEvent) {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;
            
            (Entity victim, Entity attacker) = GetActors(entityA, entityB);
            if (victim == Entity.Null || attacker == Entity.Null) {
                return;
            }

            DamageEvent damageEvent = new(attacker, 10);
            DynamicBuffer<DamageEvent> damageEvents = DamageEventsLookup[victim];
            damageEvents.Add(damageEvent);
        }
        
        [BurstCompile]
        private (Entity victim, Entity attacker) GetActors(Entity entityA, Entity entityB) {
            if (PlayerTagLookup.HasComponent(entityA) && EnemyTagLookup.HasComponent(entityB)) {
                return (entityA, entityB);
            }

            if (PlayerTagLookup.HasComponent(entityB) && EnemyTagLookup.HasComponent(entityA)) {
                return (entityB, entityA);
            }
            
            if (SkillTagLookup.HasComponent(entityA) && EnemyTagLookup.HasComponent(entityB)) {
                return (entityB, entityA);
            }
            
            if (SkillTagLookup.HasComponent(entityB) && EnemyTagLookup.HasComponent(entityA)) {
                return (entityA, entityB);
            }
    
            return (Entity.Null, Entity.Null);
        }
        
    }

    
}