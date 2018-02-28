using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapher1 : MonoBehaviour
{
    public enum FunctionOption
    {
        Liner,
        Exponential,
        Parabola,
        Sine
    }

    [Range(10, 100)]
    public int resolution = 10;

    public FunctionOption function;

    private ParticleSystem.Particle[] points;
    private ParticleSystem ps;

    private int currentResolution;

    private delegate float FunctionDelegate(float x);
    private static FunctionDelegate[] functionDelegate = new FunctionDelegate[]
    {
        Liner,
        Exponential,
        Parabola,
        Sine
    };

    // Use this for initialization
    void Start()
    {
        CreatePoints();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentResolution != resolution || points == null)
        {
            CreatePoints();
        }

        FunctionDelegate f = functionDelegate[(int)function];

        for (int i = 0; i < resolution; i++)
        {
            Vector3 p = points[i].position;
            p.y = f(p.x);
            points[i].position = p;

            Color c = points[i].startColor;
            c.g = p.y;
            points[i].startColor = c;
        }

        ps.SetParticles(points, points.Length);
    }

    private void CreatePoints()
    {
        if (resolution < 10 || resolution > 100)
        {
            Debug.LogWarning(
                "Grapher resolution out of bounds, resetting to minimum.", this);
            resolution = 10;
        }
        currentResolution = resolution;

        points = new ParticleSystem.Particle[resolution];
        ps = GetComponent<ParticleSystem>();

        float increment = 1f / (resolution - 1);
        for (int i = 0; i < resolution; i++)
        {
            float x = i * increment;
            points[i].position = new Vector3(x, 0f, 0f);
            points[i].startColor = new Color(x, 0f, 0f);
            points[i].startSize = 0.1f;
        }
    }

    private static float Liner(float x)
    {
        return x;
    }

    private static float Exponential(float x)
    {
        return x * x;
    }

    private static float Parabola(float x)
    {
        x = 2f * x - 1f;
        return x * x;
    }

    private static float Sine(float x)
    {
        return 0.5f + 0.5f * Mathf.Sin(2 * Mathf.PI * x + Time.timeSinceLevelLoad);
    }

}
