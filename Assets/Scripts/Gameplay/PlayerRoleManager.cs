using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace MyFirstARGame
{
    public class PlayerRoleManager : MonoBehaviour
    {
        public int playerRole;

        public ShooterBehavior shooterBehavior;
        public CollectorBehavior collectorBehavior;

        public void BecomeCollector(){
            collectorBehavior.enabled = true;
            shooterBehavior.enabled = false;
        }

        public void BecomeShooter(){
            shooterBehavior.enabled = true;
            collectorBehavior.enabled = false;
        }
        
        void Start()
        {
            if(PhotonNetwork.IsMasterClient){
                BecomeShooter();
            }else{
                BecomeCollector();
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
