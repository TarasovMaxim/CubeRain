using UnityEngine;
using UnityEngine.Pool;
using System.Collections;
using Random = UnityEngine.Random;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _mainPlatform;
    [SerializeField] private float _height = 40;
    [SerializeField] private Cube _cubePrefab;
    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            CreateCube,
            ActionOnGet,
            ActionOnRelease
        );
    }

    private Vector3 GeneratePosition()
    {
        Vector3 position = _mainPlatform.transform.position;
        Vector3 size = _mainPlatform.GetComponent<Renderer>().bounds.size;
        position.x += Random.Range(-size.x / 2, size.x / 2);
        position.z += Random.Range(-size.z / 2, size.z / 2);
        position.y = _mainPlatform.transform.localScale.y + _height;
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

    private void ActionOnGet(Cube cube)
    {
        cube.gameObject.SetActive(true);
        cube.transform.position = GeneratePosition();
        cube.CubeReleasingToPool += CubeRelease;
    }

    private void CubeRelease(Cube cube)
    {
        cube.gameObject.SetActive(false);
        _pool.Release(cube);
    }

    private void ActionOnRelease(Cube cube)
    {
        cube.CubeReleasingToPool -= CubeRelease;
    }

    private IEnumerator SpawnCube()
    {
        while (enabled)
        {
            float delay = 1f;
            yield return new WaitForSeconds(delay);
            var cube = _pool.Get();
        }
    }
}