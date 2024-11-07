using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour,IDataPersistence
{
    [SerializeField] private AudioSource playOnScreenAudio;
    [SerializeField] private bool isMuted = false;
    [SerializeField] private bool isUnvibrate = false;
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }
    
    
    public void InitSoundManager()
    {
        if(instance == null)
        { 
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Button[] allButtons = Resources.FindObjectsOfTypeAll<Button>();
        foreach (Button button in allButtons)
        {
            button.onClick.AddListener(() => PlayClickSound());
        }
    }

    [SerializeField] private SoundList[] soundLists;
    [SerializeField] private SoundPool soundPool;

    public SoundList[] SoundLists { get => soundLists; set => soundLists = value; }
    public bool IsMuted { get => isMuted; set => isMuted = value; }
    public bool IsUnvibrate { get => isUnvibrate; set => isUnvibrate = value; }
    public void PlayThisOnScreen(AudioClip audioClipToPlay, float volume)
    {
        if (isMuted) return; 

        playOnScreenAudio.volume = volume;
        playOnScreenAudio.PlayOneShot(audioClipToPlay);
    }

    public void PlayThisOnWorld(AudioClip audioClipToPlay, float volume, Vector3 position)
    {
        if (isMuted) return; 

        SoundSource soundSource = soundPool.Get();
        soundSource.Initialize(soundPool, position);
        soundSource.PlayAndReturnToPool(audioClipToPlay, volume);
    }

    public void PlayClickSound()
    {
        if(isMuted) return;
        SoundList clip = SoundLists.First(soundList => soundList.SoundListName == Enum.SoundType.UIButton);
        PlayThisOnScreen(clip.Sounds[0],0.1f);
    }
    public void Vibrate()
    {
        if(isUnvibrate) return;
        Handheld.Vibrate();
    }
    public void ToggleMute()
    {
        isMuted = !isMuted;
    }
    public void ToggleVibrate()
    {
        isUnvibrate = !isUnvibrate;
    }

    public void LoadData(GameData gameData)
    {
        isMuted = gameData.settingData.isMute;
        isUnvibrate = gameData.settingData.isUnvibrate;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.settingData.isMute = isMuted;
        gameData.settingData.isUnvibrate = isUnvibrate;
    }
}

[Serializable]
public class SoundList
{
    [SerializeField] private Enum.SoundType soundListName;
    [SerializeField] private AudioClip[] sounds;

    public Enum.SoundType SoundListName { get => soundListName; set => soundListName = value; }
    public AudioClip[] Sounds { get => sounds; set => sounds = value; }
}
