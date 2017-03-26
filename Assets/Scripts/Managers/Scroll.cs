using UnityEngine;

public class Scroll : MonoBehaviour
{

    public float speed = 0.01f;

    private Renderer _background;
    private Material _material;

    void Start()
    {
        _background = GetComponent<Renderer>();
        _material = _background.material;
    }

    void Update()
    {
        Vector2 offset = _material.mainTextureOffset;
        offset.y -= Time.deltaTime * speed;
        _material.mainTextureOffset = offset;
    }

}
