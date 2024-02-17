using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyFirstARGame
{
    using Photon.Pun;
    using Photon.Realtime;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        public GameObject scrollViewContent;
        public GameObject itemPrefab;

        void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            // Try to connect to the master server.
            PhotonNetwork.ConnectUsingSettings();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby(); // Join the lobby to get the room list
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("RoomListUpdate");
            UpdateRoomListUI(roomList);
        }

        void UpdateRoomListUI(List<RoomInfo> roomList)
        {
            ClearScrollView();
            foreach (RoomInfo room in roomList)
            {
                AddRoomList(room.Name);
            }
        }

        private void ClearScrollView()
        {
            if (scrollViewContent != null)
            {
                for (int i = scrollViewContent.transform.childCount - 1; i >= 0; i--)
                {
                    // Destroy the child GameObject
                    Destroy(scrollViewContent.transform.GetChild(i).gameObject);
                }
            }
        }
        private void AddRoomList(string name)
        {
            GameObject newItem = Instantiate(itemPrefab, scrollViewContent.transform);

            // Set the text of the item
            newItem.GetComponentInChildren<Text>().text = name;
        }

        public void CreateRoom()
        {
            string room_name = "TestRooms_" + PhotonNetwork.CountOfRooms;
            PhotonNetwork.CreateRoom(room_name, new RoomOptions());
        }
    }
}
