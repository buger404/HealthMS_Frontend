using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    public TMP_Text DebugInfo;
    private List<string> deltaTime = new();
    void Update()
    {
        deltaTime.Add((Time.deltaTime * 1000f).ToString("F2"));
        if (deltaTime.Count > 120)
        {
            deltaTime.RemoveAt(0);
        }

        DebugInfo.text = string.Join("  ", deltaTime);
    }
}
