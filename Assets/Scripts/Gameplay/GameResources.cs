using System.Collections;
using System.Collections.Generic;
using TMPro;
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

        public bool gameOver = false;
        public bool youWin = false;

        public GameObject gameOverText;
        public GameObject winText;

        public int winScore;
        public int loseScore;

        public void setScore(int amt){
            if(gameOver)return;
            score = amt;
            if(score >= winScore)
            {
                gameOver = youWin = true;
            }
        }
        public void setBullet(int amt){
            if(gameOver)return;
            bullet = amt;
        }
        public void SetEnemyFled(int amt){
            if(gameOver)return;
            enemyFled = amt;
            if(enemyFled > loseScore)
            {
                gameOver = true;
            }
        }
        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            if(youWin){
                winText.SetActive(true);
            }else if(gameOver){   
                gameOverText.SetActive(true);
                //GUILayout.Label("Score:" + score, new GUIStyle { normal = new GUIStyleState { textColor = Color.black }, fontSize = 22 });
            }else{
                GUILayout.Label("Score:" + score, new GUIStyle { normal = new GUIStyleState { textColor = Color.black }, fontSize = 22 });
                GUILayout.Label("Bullet:" + bullet, new GUIStyle { normal = new GUIStyleState { textColor = Color.black }, fontSize = 22 });
                GUILayout.Label("Fled Enemy:" + enemyFled, new GUIStyle { normal = new GUIStyleState { textColor = Color.black }, fontSize = 22 });
            }
            
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}
