using Damage;
using Unity.Entities;
using UnityEngine;

namespace Health {
    
    public class HealthComponentAuthoring : MonoBehaviour {
        
        [SerializeField] private float maxValue = 100f;
        
        private class HealthComponentAuthoringBaker : Baker<HealthComponentAuthoring> {
            public override void Bake(HealthComponentAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new HealthComponent(authoring.maxValue));
            }
        }
    }
}