using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Bootstrapper : MonoBehaviour
{
    public List<GameObject> List = new();

    private void Awake()
    {
        Application.targetFrameRate = 165;
        foreach (var item in List)
        {
            foreach (var b in item.GetComponents<IBootstrap>())
            {
                b.Bootstrap();
            }
        }
    }
}
