using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyFirstARGame
{
    using Photon.Pun;
    using Photon.Realtime;
    using TMPro;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UIElements;
    using UnityEngine.XR.ARFoundation.Samples;

    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        public GameObject scrollViewContent;
        public GameObject itemPrefab;
        public GameObject connecting;

        public Text selectedText;

        public Dictionary<string, GameObject> currentRooms = new Dictionary<string, GameObject>();
        public TableLayout tableLayout;

        void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (!PhotonNetwork.IsConnectedAndReady)
            {
                PhotonNetwork.ConnectUsingSettings();
            }

            tableLayout = scrollViewContent.GetComponent<TableLayout>();
        }

        public override void OnConnected()
        {
            Text text = connecting.GetComponent<Text>();
            text.text = "Connected!";
            text.color = Color.green;
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log(PhotonNetwork.CountOfRooms);
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
            Debug.Log("RoomListUpdate: " + roomList.Count + " rooms");
            foreach (RoomInfo room in roomList)
            {
                if(room.RemovedFromList && currentRooms.ContainsKey(room.Name))
                {
                    GameObject obj = currentRooms[room.Name];
                    tableLayout.m_Cells.Remove(obj.GetComponent<RectTransform>());
                    Destroy(obj);
                    currentRooms.Remove(room.Name);
                    continue;
                }
                if(room.IsOpen && !currentRooms.ContainsKey(room.Name))
                {
                    if (room.PlayerCount > 0)
                    {
                        GameObject obj = AddRoomList(room.Name);
                        currentRooms[room.Name] = obj;
                        tableLayout.m_Cells.Add(obj.GetComponent<RectTransform>());
                    }
                }
            }
            tableLayout.Refresh();
            tableLayout.Refresh();
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
        private GameObject AddRoomList(string name)
        {
            GameObject newItem = Instantiate(itemPrefab, scrollViewContent.transform);

            // Set the text of the item
            newItem.GetComponent<Selectable>().selectedText = selectedText;
            newItem.GetComponentInChildren<Text>().text = name;

            return newItem;
        }

        public void CreateRoom()
        {
            RoomOptions options = new RoomOptions();
            options.EmptyRoomTtl = 100;
            string room_name = "rooms_" + PhotonNetwork.LocalPlayer.UserId;

            PhotonNetwork.CreateRoom(room_name, options);
        }

        public void JoinRoom()
        {
            if(selectedText != null && selectedText.text.Length > 0)
            {
                PhotonNetwork.JoinRoom(selectedText.text);
            }
        }

        public override void OnJoinedRoom()
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }
    }
}
