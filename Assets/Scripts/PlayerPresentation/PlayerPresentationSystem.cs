using Player;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace PlayerPresentation {
    public partial class PlayerPresentationSystem : SystemBase {
     
        protected override void OnCreate() {
            base.OnCreate();
            RequireForUpdate<PlayerTag>();
        }

        protected override void OnUpdate() {
            Entity playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
            if (!EntityManager.HasComponent<PlayerManagedData>(playerEntity)) {
                CreatePresentationObject(ref playerEntity);
                return;
            }
            
            SyncPresentationObjectTransform(ref playerEntity);
        }
        
        private void CreatePresentationObject(ref Entity playerEntity) {
            GameObject playerPresentation = new("PlayerPresentation");
            PlayerManagedData playerManagedData = new() { PresenterGameObject = playerPresentation };
            EntityManager.AddComponentObject(playerEntity, playerManagedData);
        }
        
        private void SyncPresentationObjectTransform(ref Entity playerEntity) {
            PlayerManagedData playerManagedData = EntityManager.GetComponentData<PlayerManagedData>(playerEntity);
            LocalTransform localTransform = EntityManager.GetComponentData<LocalTransform>(playerEntity);
            playerManagedData.PresenterGameObject.transform.position = localTransform.Position;
        }
    }
    
    
}