using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public void ChangeColor(Cube cube)
    {
        if (cube.TryGetComponent<Renderer>(out Renderer renderer))
            renderer.material.color = new Color(Random.value, Random.value, Random.value);
    }

    public void ChangeColor(Cube cube, Color color)
    {
        if (cube.TryGetComponent<Renderer>(out Renderer renderer))
            renderer.material.color = color;
    }

    public Color GetStartColor()
    {
        return GetComponent<Renderer>().material.color;
    }
}