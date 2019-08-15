using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class Glow : MonoBehaviour
{
    [SerializeField] float fadeTime = 1;
    [SerializeField] float minGlow = 0.2f;
    [SerializeField] float maxGlow = 1.0f;

    new Renderer renderer;
    float sign = 1;
    Color emissionColour;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        emissionColour = renderer.material.GetColor("_EmissionColor");
    }
    
    void Update()
    {
        var colour = renderer.material.color;

        float step = (maxGlow - minGlow) * Time.deltaTime * (1 / fadeTime);
        step *= sign;

        if (colour.a + step > 1)
        {
            colour.a = 1;
            sign *= -1;
        }
        else if (colour.a + step <= minGlow)
        {
            colour.a = minGlow;
            sign *= -1;
        }
        else
        {
            colour.a += step;
        }
        
        renderer.material.color = colour;
        renderer.material.SetColor("_EmissionColor", emissionColour * Mathf.LinearToGammaSpace(colour.a));
    }
}
