using Cinemachine;
using Player;
using Unity.Entities;
using UnityEngine;

namespace Camera {
    public partial class CameraSetupSystem : SystemBase {
        
        private CinemachineVirtualCamera virtualCamera;

        protected override void OnCreate() {
            base.OnCreate();
            RequireForUpdate<PlayerTag>();
            RequireForUpdate<PlayerManagedData>();
        }

        protected override void OnUpdate() {
            if (virtualCamera is not null) {
                return;
            }

            Entity playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
            PlayerManagedData playerManagedData = EntityManager.GetComponentData<PlayerManagedData>(playerEntity);
            virtualCamera = Object.FindFirstObjectByType<CinemachineVirtualCamera>();
            virtualCamera.Follow = playerManagedData.PresenterGameObject.transform;
        }
        
    }
}