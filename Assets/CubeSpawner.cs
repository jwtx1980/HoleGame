using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab; // Prefab to spawn
    public int numberOfCubes = 10; // Number of cubes to spawn
    public float spawnAreaSize = 5f; // Defines the spawn range

    void Start()
    {
        SpawnCubes(); // Ensure cubes spawn at the start of the game
    }

    public void SpawnCubes()
    {
        ClearExistingCubes(); // Remove old cubes

        for (int i = 0; i < numberOfCubes; i++)
        {
            Vector3 randomPosition = GetRandomPosition();
            Instantiate(cubePrefab, randomPosition, Quaternion.identity);
        }
    }

    void ClearExistingCubes()
    {
        GameObject[] existingCubes = GameObject.FindGameObjectsWithTag("Swallowable");
        foreach (GameObject cube in existingCubes)
        {
            Destroy(cube);
        }
    }

    Vector3 GetRandomPosition()
    {
        float x = Random.Range(-spawnAreaSize, spawnAreaSize);
        float z = Random.Range(-spawnAreaSize, spawnAreaSize);
        float y = 0.25f; // Keep cubes above the ground

        return new Vector3(x, y, z);
    }
}
