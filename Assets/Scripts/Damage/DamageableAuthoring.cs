using Health;
using Unity.Entities;
using UnityEngine;

namespace Damage {

    public class DamageableAuthoring : MonoBehaviour {

        [SerializeField] private float maxHealth = 100f; 
        
        private class DamageableAuthoringBaker : Baker<DamageableAuthoring> {
            public override void Bake(DamageableAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new DamagedTag());
                AddComponent(entity, new HealthComponent(authoring.maxHealth));
                
                AddBuffer<DamageEvent>(entity);
                SetComponentEnabled<DamagedTag>(entity, false);
            }
        }
        
    }
}