using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatToggle : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        Toggle Tg = null;

        Tg = gameObject.GetComponent<Toggle>();

        // init option
        Tg.isOn = false;

        if (Tg != null && GameManager.Instance != null)
        {
            Tg.isOn = GameManager.Instance.bIsCheat;
        }

        // add cb event
        Tg.onValueChanged.AddListener((value) =>
        {
            onValueChanged(value);
        });
    }

    public void onValueChanged(bool value)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.bIsCheat = value;
        }
    }
}
