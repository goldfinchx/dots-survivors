using Movement;
using NUnit.Framework;
using Player;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerInput {

    [UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
    public partial class PlayerInputSystem : SystemBase {

        private Entity playerEntity;
        private PlayerInputActions actions;
        private InputAction moveAction;

        protected override void OnCreate() {
            RequireForUpdate<PlayerTag>();

            actions = new PlayerInputActions();
            moveAction = actions.Player.Move;
        }

        protected override void OnStartRunning() {
            playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
            actions.Enable();
        }

        protected override void OnStopRunning() {
            actions.Disable();
        }

        protected override void OnUpdate() {
            float2 input = moveAction.ReadValue<Vector2>();
            UpdateMovement(input);
        }

        private void UpdateMovement(float2 input) {
            Movement.Movement movement = EntityManager.GetComponentData<Movement.Movement>(playerEntity);
            LocalTransform transform = EntityManager.GetComponentData<LocalTransform>(playerEntity);

            float2 newTarget = transform.Position.xy + input;
            movement.Target = newTarget;

            EntityManager.SetComponentData(playerEntity, movement);
        }

    }
}