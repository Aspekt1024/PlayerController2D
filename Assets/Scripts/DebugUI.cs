using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour {

    public Text DebugTextBlock;

    [HideInInspector] public static DebugUI Instance;

    private static DebugUI instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public static void SetText(string text)
    {
        instance.DebugTextBlock.text = text;
    }
}
