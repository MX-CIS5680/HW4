using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyFirstARGame
{
    using Photon.Pun;
    using Photon.Realtime;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UIElements;

    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        public GameObject scrollViewContent;
        public GameObject itemPrefab;
        public Text selectedText;

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
            UpdateRoomListUI(roomList);
        }

        void UpdateRoomListUI(List<RoomInfo> roomList)
        {
            ClearScrollView();
            Debug.Log("RoomListUpdate: " + roomList.Count + " rooms");
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
            newItem.GetComponent<Selectable>().selectedText = selectedText;
            newItem.GetComponentInChildren<Text>().text = name;
        }

        public void CreateRoom()
        {
            string room_name = "TestRooms_" + PhotonNetwork.CountOfRooms;
            PhotonNetwork.CreateRoom(room_name, new RoomOptions());
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }

        public void JoinRoom()
        {
            if(selectedText != null && selectedText.text.Length > 0)
            {
                PhotonNetwork.JoinRoom(selectedText.text);
                SceneManager.LoadScene("Game", LoadSceneMode.Single);
            }
        }
    }
}
