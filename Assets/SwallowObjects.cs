using UnityEngine;
using System.Collections;

public class SwallowObjects : MonoBehaviour
{
    public Transform holeTransform;         // Reference to the hole's transform
    public float growthFactor = 0.2f;         // How much the hole grows per swallowed object
    public float fallSpeed = 5f;              // (Not used here, but can be used for speed adjustments)
    public AudioSource swallowSound;          // Audio source for the gulp sound

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Swallowable"))
        {
            // Start the sink coroutine on the swallowed object
            StartCoroutine(SinkObject(other.gameObject));
        }
    }

    IEnumerator SinkObject(GameObject obj)
    {
        // Play the gulp/swallow sound if assigned
        if (swallowSound != null)
        {
            swallowSound.Play();
        }

        float sinkDuration = 0.5f; // Time it takes for the object to sink
        float elapsedTime = 0f;
        Vector3 startPosition = obj.transform.position;
        Vector3 endPosition = startPosition - new Vector3(0, 2f, 0); // Sinks 2 units downward

        // Move the object downward over time
        while (elapsedTime < sinkDuration)
        {
            // Check if the object still exists
            if (obj == null)
                yield break;

            obj.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / sinkDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // After sinking, destroy the object if it still exists
        if (obj != null)
        {
            Destroy(obj);
        }

        // Grow the hole if the holeTransform is assigned
        if (holeTransform != null)
        {
            holeTransform.localScale += new Vector3(growthFactor, 0, growthFactor);
        }
    }
}

