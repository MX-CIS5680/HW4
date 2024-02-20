namespace MyFirstARGame
{
    using UnityEngine;
    using Photon.Pun;
    using Photon.Realtime;
    /// <summary>
    /// Controls projectile behaviour. In our case it currently only changes the material of the projectile based on the player that owns it.
    /// </summary>
    public class Bullet : MonoBehaviourPun
    {
        [SerializeField]
        private Material[] projectileMaterials;

        private void Awake()
        {
            // Pick a material based on our player number so that we can distinguish between projectiles. We use the player number
            // but wrap around if we have more players than materials. This number was passed to us when the projectile was instantiated.
            // See ProjectileLauncher.cs for more details.
            var photonView = this.transform.GetComponent<PhotonView>();
            var playerId = Mathf.Max((int)photonView.InstantiationData[0], 0);
            if (this.projectileMaterials.Length > 0)
            {
                var material = this.projectileMaterials[playerId % this.projectileMaterials.Length];
                this.transform.GetComponent<Renderer>().material = material;
            }
        }

        private void OnCollisionEnter(Collision other) {
            Debug.Log(other.gameObject.name);
            if(photonView.IsMine)
            {
                //PhotonNetwork.Destroy(gameObject);
                //photonView.RPC("DestroyMyself", RpcTarget.MasterClient);
            }
        }
    }
}
