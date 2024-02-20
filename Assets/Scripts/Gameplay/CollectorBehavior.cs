using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFirstARGame
{
    using Photon.Pun;
    public class CollectorBehavior : MonoBehaviour
    {
        public Joystick controlPanel;
        public GameObject collectorToControl;
        public GameObject collectorPrefab;
        public float speed = 5;
        // Start is called before the first frame update
        void Start()
        {
        
        }
        private void FixedUpdate() {
            float movement = speed * Time.deltaTime;
            if(controlPanel!=null && collectorToControl!=null){
                collectorToControl.transform.Translate(new Vector3(controlPanel.Horizontal * movement,0,controlPanel.Vertical * movement));
            }else{
                Debug.Log("No panel");
            }
        }

        private void OnDisable() {
            Debug.Log("You're not collector");
            if(controlPanel!=null)controlPanel.gameObject.SetActive(false);
            if(collectorToControl!=null){
                Destroy(collectorToControl);
                collectorToControl = null;
            }
        }

        private void OnEnable() {
            Debug.Log("You're collector");
            if(controlPanel!=null)controlPanel.gameObject.SetActive(true);
            if(collectorToControl!=null)Destroy(collectorToControl);
            collectorToControl = PhotonNetwork.Instantiate(this.collectorPrefab.name, Vector3.zero, Quaternion.identity);
        }
        // Update is called once per frame
        void Update()
        {
        }
    }
}
