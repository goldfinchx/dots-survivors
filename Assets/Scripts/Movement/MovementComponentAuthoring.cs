using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Movement {
    public class MovementComponentAuthoring : MonoBehaviour {
        [SerializeField] private float2 target;
        [SerializeField] private float speed = 1;

        public class MovementComponentBaker : Baker<MovementComponentAuthoring> {
            public override void Bake(MovementComponentAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new MovementComponent { Target = authoring.target, Speed = authoring.speed });
            }
        }
    }
}