using Health;
using Unity.Entities;
using UnityEngine;

namespace Damage {
    public class DamageableAuthoring : MonoBehaviour {

        [SerializeField] private float maxDamage = 10f;
        [SerializeField] private float cooldown = 0.2f;
        
        private class DamageableAuthoringBaker : Baker<DamageableAuthoring> {
            public override void Bake(DamageableAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddBuffer<DamageEvent>(entity);
                AddComponent(entity, new Damageable(authoring.maxDamage, authoring.cooldown));
            }
        }
        
    }
}