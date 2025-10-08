using UnityEngine;
using UnityEngine.Pool;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _platform;
    [SerializeField] private float _height = 20;
    private ObjectPool<GameObject> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
            CreateCube,
            OnGetCube,
            OnReleaseCube,
            OnDestroyCube,
            false,
            5,
            10
        );
    }

    private Vector3 GeneratePosition()
    {
        Vector3 position = _platform.transform.position;
        Vector3 size = _platform.GetComponent<Renderer>().bounds.size;
        position.x += Random.Range(-size.x / 2, size.x / 2);
        position.z += Random.Range(-size.z / 2, size.z / 2);
        position.y = _platform.transform.localScale.y + _height;
        return position;
    }

    private GameObject CreateCube()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = GeneratePosition();
        cube.AddComponent<Rigidbody>();
        cube.AddComponent<Cube>();
        return cube;
    }

    private void Start()
    {
        InvokeRepeating(nameof(SpawnCube), 0.5f, 1f);
    }

    private void SpawnCube()
    {
        _pool.Get();
    }

    private void OnGetCube(GameObject cube)
    {
        cube.transform.position = GeneratePosition();

        if (cube.TryGetComponent<Cube>(out Cube cubeComponent))
        {
            cubeComponent.ReleaseAction = () => StartCoroutine(DelayedRelease(cube));
            cubeComponent.ContactWithPlatform += cubeComponent.ReleaseAction;
        }
    }

    private void OnReleaseCube(GameObject cube)
    {
        if (cube.TryGetComponent<Cube>(out Cube cubeComponent))
        {
            cubeComponent.ContactWithPlatform -= cubeComponent.ReleaseAction;
        }
    }

    private System.Collections.IEnumerator DelayedRelease(GameObject cube)
    {
        float minDelay = 2f;
        float maxDelay = 5f;
        yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        _pool.Release(cube);
    }

    private void OnDestroyCube(GameObject cube)
    {
        Destroy(cube);
    }
}
