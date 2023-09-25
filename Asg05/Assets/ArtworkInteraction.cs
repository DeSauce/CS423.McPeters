using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtworkInteraction : MonoBehaviour
{
    public Material[] artworkMaterials;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int randomInteraction = Random.Range(0, 3); 

            switch (randomInteraction)
            {
                case 0:
                    DoNothing();
                    break;
                case 1:
                    ChangeArtwork();
                    break;
                case 2:
                    TeleportCharacter();
                    break;
            }
        }
    }

    // This function does nothing
    private void DoNothing()
    {}

    // This function changes the artwork
    private void ChangeArtwork()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null && artworkMaterials.Length > 0)
        {
            int randomMaterialIndex = Random.Range(0, artworkMaterials.Length);
            renderer.material = artworkMaterials[randomMaterialIndex];
        }
    }

    // This function teleports the character to a random location
    private void TeleportCharacter()
    {
        float minX = -5f;
        float maxX = 5f;
        float minZ = -5f;
        float maxZ = 5f;

        
        Vector3 randomPosition = new Vector3(
            Random.Range(minX, maxX),
            transform.position.y, 
            Random.Range(minZ, maxZ)
        );

        // Moves the player character to the random position
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = randomPosition;
        }
    }
}
