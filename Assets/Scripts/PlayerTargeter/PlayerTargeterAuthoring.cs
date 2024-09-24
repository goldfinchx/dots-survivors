﻿using Unity.Entities;
using UnityEngine;

namespace PlayerTargeter {
    public class PlayerTargeterAuthoring : MonoBehaviour {
        private class PlayerTargeterAuthoringBaker : Baker<PlayerTargeterAuthoring> {
            public override void Bake(PlayerTargeterAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new PlayerTarget.PlayerTargeter());
            }
        }
    }
}