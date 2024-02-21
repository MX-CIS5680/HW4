using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFirstARGame
{
    using Photon.Pun;
    using UnityEngine.XR.ARFoundation;
    using UnityEngine.XR.ARSubsystems;
    public class CollectorBehavior : MonoBehaviour
    {
        
        public Joystick controlPanel;
        public GameObject collectorToControl;
        public GameObject collectorPrefab;
        public float speed = 5;


        protected void OnDisable() {
            Debug.Log("You're not collector");
            //if(controlPanel!=null)controlPanel.gameObject.SetActive(false);
            //if(collectorToControl!=null){
            //    PhotonNetwork.Destroy(collectorToControl);
            //    collectorToControl = null;
            //}
        }

        protected void OnEnable() {
            Debug.Log("You're collector");
            //if(controlPanel!=null)controlPanel.gameObject.SetActive(true);
            //if(collectorToControl!=null)Destroy(collectorToControl);
            //collectorToControl = PhotonNetwork.Instantiate(this.collectorPrefab.name, new Vector3(0, 1, 0), Quaternion.identity);
        }

        private void useScreenTouch(){
            if (UnityEngine.InputSystem.Pointer.current == null)
                return;

            var touchPosition = UnityEngine.InputSystem.Pointer.current.position.ReadValue();

            // Ensure we are not over any UI element.
            var uiButtons = FindObjectOfType<UIButtons>();
            if (uiButtons != null && (uiButtons.IsPointOverUI(touchPosition)))
                return;

            // Raycast against layer "Enemy" using normal Raycasting for our artifical ground plane.
            var ray = Camera.main.ScreenPointToRay(touchPosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000, LayerMask.GetMask("Enemy")))
            {
                GameObject hitObj = hit.collider.gameObject;
                UFOBehaviour ufo  = hitObj.GetComponent<UFOBehaviour>();
                if(ufo!=null && !ufo.active){
                    ufo.GetCollected();
                }
            }
        }

        private void useCollector(){
            float movement = speed * Time.deltaTime;
            if (controlPanel!=null && collectorToControl!=null){
                
                Vector3 move = transform.localToWorldMatrix * new Vector3(controlPanel.Horizontal * movement, 0, controlPanel.Vertical * movement);
                move.y = 0;
                collectorToControl.transform.Translate(move);
            }
            else{
                Debug.Log("No panel");
            }
        }

        private void Update() {
            useScreenTouch();
        }
    }
}
