using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESoundGroup
{
    Music,
    Effect,
    MAX,
}

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public class SoundGroup
    {
        public GameObject CachedObj;
        public AudioSource AudioComponent;
        public float fVolume;
        public bool bIsMute;

        public bool InitSoundGroup(GameObject GameObj)
        {
            if (GameObj == null)
            {
                return false;
            }

            CachedObj = GameObj;
            // Create New Audio Source
            if (AudioComponent == null)
            {
                AudioComponent = CachedObj.AddComponent<AudioSource>();
            }

            fVolume = 1.0f;
            bIsMute = AudioComponent.mute;
            fVolume = AudioComponent.volume;

            return true;
        }

        public bool Mute
        {
            get
            {
                return bIsMute;
            }

            set
            {
                bIsMute = value;
                if (AudioComponent != null)
                {
                    AudioComponent.mute = bIsMute;

                    if( bIsMute )
                    {
                        AudioComponent.Pause();
                    }
                    else
                    {
                        AudioComponent.UnPause();
                    }
                }
            }
        }

        public float Volume
        {
            get
            {
                return fVolume;
            }

            set
            {
                fVolume = value;
                if (AudioComponent != null)
                {
                    AudioComponent.volume = fVolume;
                }
            }
        }

        public bool Play(AudioClip Clip, bool bLoop)
        {
            if (AudioComponent != null)
            {
                AudioComponent.clip = Clip;
                AudioComponent.loop = bLoop;

                if (!Mute)
                {
                    AudioComponent.Play();
                }

                return true;
            }

            return false;
        }
    }

    // BGM
    public List<AudioClip> BGMList = new List<AudioClip>();
    public AudioClip CachedEffectSound = null;
    public AudioClip CachedGetAbilSound = null;    

    // singleton instance
    public static SoundManager instance = null;

    public SoundGroup[] CachedSoundGroup = null;

    // Use this for initialization
    void Start()
    {
        // singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        // Init Sound Group
        InitSoundGroup();

        // Play BGM
        if (BGMList.Count > 0)
        {
            PlaySound((int)ESoundGroup.Music, BGMList[Random.Range(0, BGMList.Count)], true);
        }
    }

    // init sound group
    bool InitSoundGroup()
    {
        // if initialized, pass.
        if (CachedSoundGroup.Length > 0)
        {
            return false;
        }

        CachedSoundGroup = new SoundGroup[(int)ESoundGroup.MAX];

        for (int i = 0; i < (int)ESoundGroup.MAX; ++i)
        {
            CachedSoundGroup[i] = new SoundGroup();

            if (CachedSoundGroup[i] == null)
            {
                return false;
            }
            CachedSoundGroup[i].InitSoundGroup(gameObject);
        }

        return true;
    }

    // Play sound
    public bool PlaySound(ESoundGroup GroupIdx, AudioClip Clip, bool bLoop = false)
    {
        if (CachedSoundGroup.Length <= (int)GroupIdx || CachedSoundGroup[(int)GroupIdx] == null)
        {
            return false;
        }

        return CachedSoundGroup[(int)GroupIdx].Play(Clip, bLoop);
    }

    // Play Cached Effect sound
    public bool PlayEffectSound()
    {
        if( CachedEffectSound != null )
        {
            return PlaySound(ESoundGroup.Effect, CachedEffectSound);
        }

        return false;
    }

    public bool PlayGetAbilitySound()
    {
        if (CachedGetAbilSound != null)
        {
            return PlaySound(ESoundGroup.Effect, CachedGetAbilSound);
        }

        return false;
    }    

    // Get mute
    public bool IsMute(int GroupIdx)
    {
        if (CachedSoundGroup.Length <= GroupIdx || CachedSoundGroup[GroupIdx] == null)
        {
            return true;
        }

        return CachedSoundGroup[GroupIdx].Mute;
    }

    // Set mute
    public void SetMute(int GroupIdx, bool bMute)
    {
        if (CachedSoundGroup.Length <= GroupIdx || CachedSoundGroup[GroupIdx] == null)
        {
            return;
        }

        CachedSoundGroup[GroupIdx].Mute = bMute;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
