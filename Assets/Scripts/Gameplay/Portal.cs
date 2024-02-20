using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFirstARGame
{
    using Photon.Pun;
    public class Portal : MonoBehaviour
    {
        public GameObject enemyPrefab;
        public float countDown;

        public float spawnInterval;
        // Start is called before the first frame update

        void Start()
        {
            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.Instantiate(this.enemyPrefab.name, transform.position, Quaternion.identity);
        }

        // Update is called once per frame
        void Update()
        {
            //if(PhotonNetwork.IsMasterClient){
            //    countDown -= Time.deltaTime;
            //    if(countDown < 0){
            //        countDown = Random.Range(1, 1 + spawnInterval);
            //        PhotonNetwork.Instantiate(this.enemyPrefab.name, transform.position, Quaternion.identity);
            //    }
            //}
        }
    }
}
