using Unity.Entities;
using UnityEngine;

namespace Enemies {
    public class EnemyTagAuthoring : MonoBehaviour {
        private class EnemyTagAuthoringBaker : Baker<EnemyTagAuthoring> {
            public override void Bake(EnemyTagAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new EnemyTag());
            }
        }
    }
}