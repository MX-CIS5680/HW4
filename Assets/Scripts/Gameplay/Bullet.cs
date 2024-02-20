namespace MyFirstARGame
{
    using UnityEngine;
    using Photon.Pun;
    using Photon.Realtime;
    /// <summary>
    /// Controls projectile behaviour. In our case it currently only changes the material of the projectile based on the player that owns it.
    /// </summary>
    public class Bullet : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private Material[] projectileMaterials;

        public int PlayerCount;

        private void Awake()
        {
            // Pick a material based on our player number so that we can distinguish between projectiles. We use the player number
            // but wrap around if we have more players than materials. This number was passed to us when the projectile was instantiated.
            // See ProjectileLauncher.cs for more details.
            if (photonView.IsMine)
            {
                PlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;
                var photonView = this.transform.GetComponent<PhotonView>();
                var playerId = Mathf.Max((int)photonView.InstantiationData[0], 0);
                if (this.projectileMaterials.Length > 0)
                {
                    var material = this.projectileMaterials[playerId % this.projectileMaterials.Length];
                    this.transform.GetComponent<Renderer>().material = material;
                }

                Invoke("DestroyMyself", 5);
            }
        }

        private void OnCollisionEnter(Collision other) {
            photonView.RPC("TryDestroy", RpcTarget.All);
        }


        [PunRPC]
        public void TryDestroy()
        {
            if (photonView.IsMine && --PlayerCount == 0)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }

        public void DestroyMyself()
        {
            PhotonNetwork.Destroy(gameObject);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            ++PlayerCount;
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            --PlayerCount;
        }
    }
}
