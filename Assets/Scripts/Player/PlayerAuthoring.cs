using Damage;
using Movements;
using Unity.Entities;
using UnityEngine;

namespace Player {
    
    [RequireComponent(typeof(DamageableAuthoring), typeof(MovementAuthoring))]
    public class PlayerAuthoring : MonoBehaviour {
        
        private class PlayerAuthoringBaker : Baker<PlayerAuthoring> {
            public override void Bake(PlayerAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new PlayerTag());
            }
        }
        
    }
}