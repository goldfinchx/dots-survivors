using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Movement {
    public class MovementAuthoring : MonoBehaviour {
        [SerializeField] private float2 target;
        [SerializeField] private float speed = 1;

        public class MovementAuthoringBaker : Baker<MovementAuthoring> {
            public override void Bake(MovementAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new MovementComponent { Target = authoring.target, Speed = authoring.speed });
            }
        }
    }
}