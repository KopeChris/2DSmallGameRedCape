using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Message : MonoBehaviour
{
    public TextMeshProUGUI messageText;

    void Start()
    {
        messageText.color = new Color(1, 1, 0, 0f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
            messageText.color = Color.white;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            messageText.color = new Color(1, 1, 0, 0f);
    }
}
