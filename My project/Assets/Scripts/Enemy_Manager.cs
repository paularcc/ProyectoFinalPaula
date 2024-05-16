using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Enemy_Manager : MonoBehaviour
{
    public int life = 3;
    public TextMeshProUGUI lifeText;
    
    private void OnTriggerEnter(Collider Enemy)
    {
        if (Enemy.gameObject.tag == "Enemy") // Check if the collision is with the player
        {
            life--;
            Debug.Log("Enemy reached you");
            lifeText.text = "Life: " + life.ToString();
            if (life <= 0)
            {
                SceneManager.LoadScene("GameOver");
            } 
        }
    }
}


