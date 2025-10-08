using UnityEngine;
using UnityEngine.Pool;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _platform;
    [SerializeField] private float _height = 20;
    [SerializeField] private Cube _cubePrefab;
    private ObjectPool<Cube> _pool;
    private int _defaultCapacity=5;
    private int _maxSize=10;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            CreateCube,
            OnGetCube,
            OnReleaseCube,
            OnDestroyCube,
            false,
            _defaultCapacity,
            _maxSize
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

    private Cube CreateCube()
    {
        Cube cube = Instantiate(_cubePrefab);
        return cube;
    }

    private void Start()
    {
        StartCoroutine(SpawnCube());
    }

    private void OnGetCube(Cube cube)
    {
        cube.transform.position = GeneratePosition();
        cube.ReleaseAction = () => StartCoroutine(DelayedRelease(cube));
        cube.ContactWithPlatform += cube.ReleaseAction;
    }

    private void OnReleaseCube(Cube cube)
    {
        cube.ContactWithPlatform -= cube.ReleaseAction;
    }

    private System.Collections.IEnumerator DelayedRelease(Cube cube)
    {
        float minDelay = 2f;
        float maxDelay = 5f;
        yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        _pool.Release(cube);
    }

    private System.Collections.IEnumerator SpawnCube()
    {
        while (enabled)
        {
            float delay = 1f;
            yield return new WaitForSeconds(delay);
            _pool.Get();
        }
    }

    private void OnDestroyCube(Cube cube)
    {
        Destroy(cube);
    }
}