namespace MyFirstARGame
{
    using UnityEngine;
    using Photon.Pun;
    using System.Threading;
    using UnityEditor.Build.Content;

    /// <summary>
    /// Controls projectile behaviour. In our case it currently only changes the material of the projectile based on the player that owns it.
    /// </summary>
    public class UFOBehaviour : MonoBehaviourPun
    {

        public Vector3 wanderTarget = Vector3.zero;
        public float speed = 10;
        public float rotationSpeed = 2;

        [SerializeField]
        private float wanderRadius = 10;
        [SerializeField]
        private float wanderDistance = 10;
        [SerializeField]
        private float wanderJitter = 1;

        public float timeToFlee = 10;

        public bool active = true;
        NetworkCommunication networkCommunication = null;

        private void Seek(Vector3 targetWorld){
            Vector3 desiredVelocity = (targetWorld - transform.position).normalized * speed;
            transform.Translate(desiredVelocity * Time.deltaTime);
            Vector3 direction = targetWorld - transform.position;
            direction.y = 0;
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        
        private void Wander(){
            if(Vector3.Distance(wanderTarget,transform.position) < 0.2f){
                Vector3 newTarget = new Vector3(
                    Random.Range(-1.0f,1.0f) * wanderJitter
                    ,Random.Range(-1.0f,1.0f) * wanderJitter
                    ,Random.Range(-1.0f,1.0f) * wanderJitter);
                
                newTarget.Normalize();
                newTarget *= wanderRadius;
                newTarget +=  new Vector3(0,0,wanderDistance);
                wanderTarget = transform.InverseTransformVector(newTarget);
            }
            
            Seek(wanderTarget);
        }

        private void Start(){
            wanderTarget = transform.position;
        }

        int GetBulletCount(){
            if (networkCommunication == null)networkCommunication = FindObjectOfType<NetworkCommunication>();
            if (networkCommunication == null)Debug.Log("Can't find networkCommunication");
            return networkCommunication.GetBullet();
        }

        void SetBulletCount(int amt){
            if (networkCommunication == null)networkCommunication = FindObjectOfType<NetworkCommunication>();
            if (networkCommunication == null)Debug.Log("Can't find networkCommunication");
            networkCommunication.SetBullet(amt);
        }

        int GetScore(){
            if (networkCommunication == null)networkCommunication = FindObjectOfType<NetworkCommunication>();
            if (networkCommunication == null)Debug.Log("Can't find networkCommunication");
            return networkCommunication.GetScore();
        }

        void SetScore(int amt){
            if (networkCommunication == null)networkCommunication = FindObjectOfType<NetworkCommunication>();
            if (networkCommunication == null)Debug.Log("Can't find networkCommunication");
            networkCommunication.SetScore(amt);
        }

        private void BecomeScrap(){
            Debug.Log("Become Scrap");
            active = false;
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.useGravity = true;
            
        }

        public void GetCollected(){
            photonView.RPC("Network_GetCollected",RpcTarget.All);
        }
        private void GetCollectedMine(){
            SetBulletCount(GetBulletCount() + 2);
            SetScore(GetScore() + 5);
            PhotonNetwork.Destroy(gameObject);
        }
        [PunRPC]
        private void Network_GetCollected(){
            if(photonView.IsMine){
                GetCollectedMine();
            }
        }

        public void GetShot(){
            photonView.RPC("Network_GetShot",RpcTarget.All);
        }
        private void GetShotMine(){
            BecomeScrap();
            SetScore(GetScore() + 10);
        }
        [PunRPC]
        private void Network_GetShot(){
            if(photonView.IsMine){
                GetShotMine();
            }
        }

        private void OnCollisionEnter(Collision other) 
        {
            if (!active && other.gameObject.CompareTag("Collector") && photonView.IsMine)
            {
                GetCollectedMine();
            }
            else if (active && other.gameObject.CompareTag("Bullet") && photonView.IsMine)
            {
                GetShotMine();
            }
        }
        
        private void Awake()
        {
        }

        private void Update() {
            if (photonView.IsMine)
            {
                timeToFlee -= Time.deltaTime;
                if(timeToFlee < 0){
                    if (networkCommunication == null)
                    {
                        networkCommunication = FindObjectOfType<NetworkCommunication>();
                    }
                    if (networkCommunication == null)
                    {
                        Debug.Log("Can't find networkCommunication");
                    }
                    if(active)networkCommunication.SetEnemyFled(networkCommunication.GetEnemyFled() + 1);
                    PhotonNetwork.Destroy(gameObject);
                }
                if (active)
                {
                   Wander();
                }
            }
        }
    }
}
