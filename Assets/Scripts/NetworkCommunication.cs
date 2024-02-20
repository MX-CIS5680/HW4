namespace MyFirstARGame
{
    using Photon.Pun;
    using Photon.Realtime;
    using UnityEngine;
    
    /// <summary>
    /// You can use this class to make RPC calls between the clients. It is already spawned on each client with networking capabilities.
    /// </summary>
    public class NetworkCommunication : MonoBehaviourPun
    {

        [SerializeField]
        private Scoreboard scoreboard;
        [SerializeField]
        private GameResources resources;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        
        public void IncrementScore()
        {
            var playerName = $"Player {PhotonNetwork.LocalPlayer.ActorNumber}";
            var currentScore = this.scoreboard.GetScore(playerName);
            photonView.RPC("Network_SetPlayerScore", RpcTarget.All, playerName, currentScore + 1);
        }

        [PunRPC]
        public void Network_SetPlayerScore(string playerName, int newScore)
        {
            Debug.Log($"Player {playerName} score!");
            scoreboard.SetScore(playerName, newScore);
        }

        public void SetBullet(int amt){
            photonView.RPC("Network_SetBullet", RpcTarget.All, amt);
        }
        public int GetBullet(){
            return resources.bullet;
        }
        [PunRPC]
        public void Network_SetBullet(int amt)
        {
            Debug.Log("Set Bullet: " + amt);
            resources.setBullet(amt);
        }
 
        public void SetScore(int amt){
            photonView.RPC("Network_SetScore", RpcTarget.All, amt);
        }
        public int GetScore(){
            return resources.score;
        }
        [PunRPC]
        public void Network_SetScore(int amt){
            Debug.Log("Set Score: " + amt);
            resources.setScore(amt);
        }

        public void SetEnemyFled(int amt){
            photonView.RPC("Network_SetEnemyFled", RpcTarget.All, amt);
        }
        public int GetEnemyFled(){
            return resources.enemyFled;
        }
        [PunRPC]
        public void Network_SetEnemyFled(int amt){
            Debug.Log("Set Enemey Fled: " + amt);
            resources.SetEnemyFled(amt);
            if(resources.gameOver && PhotonNetwork.IsMasterClient){
                Invoke("QuitRoom",2.0f);
            }
        }

        void QuitRoom(){
            PhotonNetwork.LeaveRoom();
        }
    }

}