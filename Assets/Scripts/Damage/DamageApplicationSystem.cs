using Health;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Damage {
    public partial struct DamageApplicationSystem : ISystem {

        private BufferLookup<DamageEvent> eventsLookup;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<BeginInitializationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate(SystemAPI.QueryBuilder().WithAll<DamagedTag>().Build());
            state.RequireForUpdate<HealthComponent>();
            
            eventsLookup = state.GetBufferLookup<DamageEvent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            // if (SystemAPI.Time.ElapsedTime % 5 < 0.01f) {
            //     foreach (DamagableEntityAspect damagedEntityAspectTest in SystemAPI.Query<DamagableEntityAspect>()) {
            //         Entity entity = damagedEntityAspectTest.Entity;
            //         SystemAPI.SetComponentEnabled<DamagedTag>(entity, true);
            //         DynamicBuffer<DamageEvent> damageEvents = SystemAPI.GetBuffer<DamageEvent>(entity);
            //         damageEvents.Add(new DamageEvent(entity, 10));
            //     }
            //     
            //     Debug.Log("DamageSystem: DamageEvent added to all entities with DamagedEntityAspect");
            //     return;
            // }
            

            eventsLookup.Update(ref state);
            EntityCommandBuffer commandBuffer = new(Allocator.TempJob);
            DamageJob damageJob = new() {
                CommandBuffer = commandBuffer,
                DamageEventsLookup = eventsLookup
            };

            state.Dependency = damageJob.Schedule(state.Dependency);


        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }
    }
    
    [BurstCompile]
    public partial struct DamageJob : IJobEntity {
        public EntityCommandBuffer CommandBuffer;
        public BufferLookup<DamageEvent> DamageEventsLookup;
        
        [BurstCompile]
        private void Execute(DamagedEntityAspect damagedEntityAspect) {
            Entity entity = damagedEntityAspect.Entity;
            DynamicBuffer<DamageEvent> damageEvents = DamageEventsLookup[entity];
            
            int damage = 0;
            foreach (DamageEvent damageEvent in damageEvents) {
                damage += damageEvent.Value;
            }
            
            Debug.Log($"DamageSystem: {damage} damage, current health: {damagedEntityAspect.Health}");
      
            damagedEntityAspect.Damage(damage);
            CommandBuffer.SetComponentEnabled<DamagedTag>(entity, false);
            damageEvents.Clear();
        }
    }

    
}