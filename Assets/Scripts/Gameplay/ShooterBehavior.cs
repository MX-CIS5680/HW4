using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFirstARGame
{
    using Photon.Pun;
    public class ShooterBehavior : PressInputBase
    {
        [SerializeField]
        private Rigidbody projectilePrefab;
        public NetworkCommunication networkCommunication; 

        [SerializeField]
        private float initialSpeed = 1.2f;

        void Start(){
            Debug.Log("try to find networkcommunication in shooterbehavior");
            networkCommunication = FindObjectOfType<NetworkCommunication>();
            if(networkCommunication == null){
                Debug.Log("Failed to find networkcommunication");
            }
        }

        int GetBulletCount(){
            return networkCommunication.GetBullet();
        }

        void SetBulletCount(int amt){
            networkCommunication.SetBullet(amt);
        }
        

        protected override void OnPressBegan(Vector3 position)
        {
            if (this.projectilePrefab == null || !NetworkLauncher.Singleton.HasJoinedRoom)
                return;

            // Ensure user is not doing anything else.
            var uiButtons = FindObjectOfType<UIButtons>();
            if (uiButtons != null && (uiButtons.IsPointOverUI(position) || !uiButtons.IsIdle))
                return;

            if(GetBulletCount() < 1)return;

            // We send our current player number as data so that the projectile can pick its material based on the player that owns it.
            var initialData = new object[] { PhotonNetwork.LocalPlayer.ActorNumber };

            // Cast a ray from the touch point to the world. We use the camera position as the origin and the ray direction as the
            // velocity direction.
            var ray = this.GetComponent<Camera>().ScreenPointToRay(position);
            var projectile = PhotonNetwork.Instantiate(this.projectilePrefab.name, ray.origin, Quaternion.identity, data: initialData);

            // By default, the projectile is kinematic in the prefab. This is because it should not be affected by physics
            // on clients other than the one owning it. Hence we disable kinematic mode and let the physics engine take over here.
            // It might make sense to have all game physics run on the server for a more complex scenario. You could transfer
            // ownership here to the server.
            var rigidbody = projectile.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            rigidbody.velocity = ray.direction * initialSpeed;

            SetBulletCount(GetBulletCount() - 1);
        }
    }
}
