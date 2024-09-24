
using Health;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


namespace Damage {
    [BurstCompile]
    public partial struct DamageProcessingSystem : ISystem {
        
        private BufferLookup<DamageEvent> eventsLookup;

        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<HealthComponent>();
            state.RequireForUpdate<DamageEvent>();
            eventsLookup = state.GetBufferLookup<DamageEvent>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            eventsLookup.Update(ref state);
            EntityCommandBuffer commandBuffer = new(Allocator.TempJob);
            
            state.Dependency = new DamageProcessingJob {
                CommandBuffer = commandBuffer,
                EventsLookup = eventsLookup,
                Time = SystemAPI.Time.ElapsedTime
            }.Schedule(state.Dependency);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }

    }
    
    [BurstCompile]
    public partial struct DamageProcessingJob : IJobEntity {
        public BufferLookup<DamageEvent> EventsLookup;
        public EntityCommandBuffer CommandBuffer;
        [ReadOnly] public double Time;
        
        [BurstCompile]
        private void Execute(Entity entity, ref HealthComponent health, ref Damageable damageable) {
            if (damageable.LastDamageTime + damageable.Cooldown > Time) {
                Debug.Log("COOLDOWN");
                return;
            }
            
            DynamicBuffer<DamageEvent> damageEvents = EventsLookup[entity];
            if (damageEvents.Length == 0) {
                return;
            }
            
            float damage = 0;
            foreach (DamageEvent damageEvent in damageEvents) {
                damage += damageEvent.Value;
            }
            
            Debug.Log("DAMAGE!!!");
            damage = math.min(damage, damageable.MaxValue);
            health.Value = math.max(0, health.Value - damage);
            
            damageable.LastDamageTime = Time;
            CommandBuffer.SetComponent(entity, health);
            CommandBuffer.SetComponent(entity, damageable);
            damageEvents.Clear();
        }
    }

    
}