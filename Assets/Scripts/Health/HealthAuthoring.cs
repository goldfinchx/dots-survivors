using Damage;
using Unity.Entities;
using UnityEngine;

namespace Health {

    public class HealthAuthoring : MonoBehaviour {
        [SerializeField] private float maxHealth = 100f; 
        
        private class HealthAuthoringBaker : Baker<HealthAuthoring> {
            public override void Bake(HealthAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Health(authoring.maxHealth));
            }
        }
        
    }
}