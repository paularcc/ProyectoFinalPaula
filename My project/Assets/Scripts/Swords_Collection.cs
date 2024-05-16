using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Sword : MonoBehaviour
{
    public int swordCount = 0; // Variable to keep track of sword count
    public TextMeshProUGUI swordText;

    private void OnTriggerEnter(Collider Sword)
    {
        if (Sword.gameObject.tag == "Sword") // Check if the collision is with the player
        {
            swordCount++; // Increment the sword count
            Debug.Log("Arma capturada! Armas totales: " + swordCount);
            swordText.text = "Armas: " + swordCount.ToString();
            //Sword.gameObject.SetActive(false);
            Destroy(Sword.gameObject);

            if (swordCount >= 15)
            {
                SceneManager.LoadScene("WinGame");
            }
        }
    }
}
