using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MyFirstARGame
{
    public class Selectable : MonoBehaviour, IPointerClickHandler
    {
        public Text selectedText;

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Button clicked!");

            if(selectedText != null)
            {
                selectedText.text = GetComponent<Text>().text;
            }
        }
    }
}
