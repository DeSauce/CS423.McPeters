using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDissolve : MonoBehaviour
{
    private Material dissolveMaterial;
    private bool isDissolving = false;
    private float dissolveDuration = 4f; // Duration of dissolve effect in seconds

    void Start()
    {
        // Get the material with the Unlit/PyrShader
        dissolveMaterial = GetComponent<Renderer>().material;
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player character has made contact with the object
        if (other.CompareTag("Player") && !isDissolving)
        {
            // Start the dissolve coroutine
            StartCoroutine(DissolveCoroutine());
        }
    }

    private IEnumerator DissolveCoroutine()
    {
        isDissolving = true;

        float elapsedTime = 0f;
        float startDissolveCutoff = dissolveMaterial.GetFloat("_DissolveCutoff");
        float targetDissolveCutoff = 1f;

        while (elapsedTime < dissolveDuration)
        {
            float newDissolveCutoff = Mathf.Lerp(startDissolveCutoff, targetDissolveCutoff, elapsedTime / dissolveDuration);
            dissolveMaterial.SetFloat("_DissolveCutoff", newDissolveCutoff);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the object is completely dissolved
        dissolveMaterial.SetFloat("_DissolveCutoff", targetDissolveCutoff);

        // Object has completely dissolved, you can perform any additional actions here if needed
        Destroy(gameObject);
    }
}

