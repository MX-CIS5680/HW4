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
        public float spawnRadius;
        public int spawnNum;

        void Start()
        {
            if (PhotonNetwork.IsMasterClient) SpawnUFO();
        }
        void SpawnUFO(){
            Vector3 jitter = new Vector3(
                    Random.Range(-1.0f,1.0f),
                    Random.Range(-1.0f,1.0f),
                    Random.Range(-1.0f,1.0f));
            jitter.Normalize();
            PhotonNetwork.Instantiate(this.enemyPrefab.name, transform.position + jitter * spawnRadius, Quaternion.identity);
        }
        // Update is called once per frame
        void Update()
        {
            if(PhotonNetwork.IsMasterClient){
               countDown -= Time.deltaTime;
               if(countDown < 0){
                   countDown = Random.Range(1, 1 + spawnInterval);
                    for(int i = 0; i < spawnNum; ++i)
                    {
                        SpawnUFO();
                    } 
                   
               }
            }
        }
    }
}
