using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyFirstARGame
{
    public class GameResources : MonoBehaviour
    {
        // Start is called before the first frame update
        public int score;
        public int bullet;

        public int enemyFled;
        public void setBullet(int amt){
            bullet = amt;
        }
        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            GUILayout.Label("Score:" + score, new GUIStyle { normal = new GUIStyleState { textColor = Color.black }, fontSize = 22 });
            GUILayout.Label("Bullet:" + bullet, new GUIStyle { normal = new GUIStyleState { textColor = Color.black }, fontSize = 22 });
            GUILayout.Label("Fled Enemy:" + enemyFled, new GUIStyle { normal = new GUIStyleState { textColor = Color.black }, fontSize = 22 });

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}
