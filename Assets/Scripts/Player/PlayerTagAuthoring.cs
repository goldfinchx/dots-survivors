using Unity.Entities;
using UnityEngine;

namespace Player {
    public class PlayerTagAuthoring : MonoBehaviour {
        
        private class PlayerTagAuthoringBaker : Baker<PlayerTagAuthoring> {
            public override void Bake(PlayerTagAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new PlayerTag());
            }
        }
        
    }
}