using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundOption : MonoBehaviour {

    public ESoundGroup GroupIdx;

	// Use this for initialization
	void Start () {
        Toggle Tg = null;

        Tg = gameObject.GetComponent<Toggle>();

        // init option
        if( Tg != null && SoundManager.instance != null)
        {
            Tg.isOn = !SoundManager.instance.IsMute((int)GroupIdx);
        }

        // add cb event
        Tg.onValueChanged.AddListener((value) =>
        {
            onValueChanged(value);
        });
    }

    public void onValueChanged(bool value)
    {
        if(SoundManager.instance != null)
        {
            SoundManager.instance.SetMute((int)GroupIdx, !value);
        }
    }
}
