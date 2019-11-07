using UnityEngine;
using System.Collections;

public class EmissionGlow : MonoBehaviour
{
    public float fadeTime = 1;
    public float minGlow = 0.2f;
    public float maxGlow = 1.0f;
    public Color emissionColour;
    public bool showEmission;
    public bool animate;

    new Renderer renderer;
    float sign = 1;
    
    float alpha;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        renderer.material.EnableKeyword("_EMISSION");

        alpha = minGlow;
        showEmission = false;
        animate = false;
    }

    void Update()
    {
        float step = (maxGlow - minGlow) * Time.deltaTime * (1 / fadeTime);

        if (showEmission && !animate)
        {
            // Fade it in
            if (alpha < maxGlow)
            {
                alpha += step * 4;
                sign = 1;
            }
            else
            {
                alpha = maxGlow;
                sign = -1;
            }
        }
        else if (showEmission && animate)
        {
            step *= sign;

            if (alpha + step > maxGlow)
            {
                alpha = maxGlow;
                sign *= -1;
            }
            else if (alpha + step <= minGlow)
            {
                alpha = minGlow;
                sign *= -1;
            }
            else
            {
                alpha += step;
            }
        }
        else
        {
            // Fade it out
            if (alpha > 0)
            {
                alpha -= step * 4;
                sign = -1;
            }
            else
            {
                alpha = 0;
                sign = 1;
            }
        }

       
        renderer.material.SetColor("_EmissionColor", emissionColour * Mathf.LinearToGammaSpace(alpha));
    }
}
