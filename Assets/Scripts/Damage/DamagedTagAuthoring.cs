using Health;
using Unity.Entities;
using UnityEngine;

namespace Damage {

    public class DamagedTagAuthoring : MonoBehaviour {
        
        private class DamageComponentAuthoringBaker : Baker<DamagedTagAuthoring> {
            public override void Bake(DamagedTagAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new DamagedTag());
                AddBuffer<DamageEvent>(entity);
                SetComponentEnabled<DamagedTag>(entity, false);
            }
        }
        
    }
}