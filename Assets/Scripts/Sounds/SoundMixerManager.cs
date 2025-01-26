using System;
using System.Collections;
using System.Collections.Generic;
using DependencyInjection;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


namespace Sounds
{
    public class SoundMixerManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;

        
        private void Start()
        {
            float FXVolume = 0;
            float musicVolume = 0;
            
            if (PlayerPrefs.HasKey("SoundEffectsVolume"))
            {
                FXVolume = PlayerPrefs.GetFloat("SoundEffectsVolume");
            }
            audioMixer.SetFloat("SoundEffectsVolume", Mathf.Log10(FXVolume) * 20f);
        
            if (PlayerPrefs.HasKey("MusicVolume"))
            {
                musicVolume = PlayerPrefs.GetFloat("MusicVolume");
            }
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20f);
        }
        
    
        public void SetSoundFXVolume(float level)
        {
            audioMixer.SetFloat("SoundEffectsVolume", Mathf.Log10(level) * 20f);
            PlayerPrefs.SetFloat("SoundEffectsVolume", level);

            if (level <= 0.0001f)
            {
                SettingsUIManager.Instance.FXVolumeButtonDesactivated();
               // PlayerPrefs.SetString("FXWasMuted", "true");

            }
            else
            {
                SettingsUIManager.Instance.FXVolumeButtonToZeroActivated();
               // PlayerPrefs.SetString("FXWasMuted", "false");

            }
        }
    
    
        public void SetMusicVolume(float level)
        {
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20f);
            PlayerPrefs.SetFloat("MusicVolume", level);
            
            if (level <= 0.0001f)
            {
                SettingsUIManager.Instance.MusicVolumeButtonDesactivated();
                PlayerPrefs.SetString("MusicWasMuted", "true");
            }
            else
            {
                SettingsUIManager.Instance.MusicVolumeButtonActivated();
                PlayerPrefs.SetString("MusicWasMuted", "false");
            }
        }
    }
}

