using UnityEngine;
using System.IO;

public class PlayerController : MonoBehaviour
{
    // TODO: Create the object to store the player's data

    private string filePath;

    void Start()
    {
        // Set the path to save the serialized file
        filePath = Application.persistentDataPath + "/playerData.json";
        Debug.Log("Data path: " + filePath);
    }

    void Update()
    {
        // Simple player movement using WASD
        float moveX = 0f;
        float moveZ = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            moveZ = Time.deltaTime * 5f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveZ = -Time.deltaTime * 5f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveX = -Time.deltaTime * 5f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveX = Time.deltaTime * 5f;
        }

        transform.Translate(moveX, 0, moveZ);

        // Update player data
        playerData.posX = transform.position.x;
        playerData.posY = transform.position.y;
        playerData.posZ = transform.position.z;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerData.score += 10; // Increment score for demonstration
            Debug.Log("Score updated: " + playerData.score);
        }

        // Save data on pressing "Q"
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SavePlayerData();
        }

        // Load data on pressing "E"
        if (Input.GetKeyDown(KeyCode.E))
        {
            LoadPlayerData();
        }
    }

    void SavePlayerData()
    {
        // TODO: put your code here to save the player's data to JSON
    }

    void LoadPlayerData()
    {
        // TODO: put your code here to restore the player's data from JSON
    }
}

// TODO: Add a PlayerData class to store the player's data