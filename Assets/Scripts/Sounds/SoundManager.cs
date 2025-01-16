using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Sounds
{

[Serializable]
public class Sound
{
    private string audioName;
    private AudioClip audioClip;

    public Sound(string audioName, AudioClip audioClip)
    {
        this.audioClip = audioClip;
        this.audioName = audioName;
    }

    public string GetSoundName()
    {
        return audioName;
    }
}

public class SoundManager : Singleton<SoundManager>
{
    
    private List<Sound> inventoryAudioClips;
    //Se tendría que ver como lo renombramos en resources para tener varios sonidos para lo mismo
    private Dictionary<SoundAction, AudioClip> audioDictionary;

    [SerializeField] private AudioSource soundFXObjectPrefab;
    [SerializeField] private AudioSource soundMusicObjectPrefab;
    private List<AudioSource> audiosPlaying;
    public event EventHandler onStopAudios;
    public event EventHandler onResumeAudios;
    
    public float sfxVolume { get; set; }
    public float musicVolume { get; set; }
    
    private void Start()
    {
        inventoryAudioClips = new List<Sound>();
        audioDictionary = new Dictionary<SoundAction, AudioClip>();
        LoadAllSounds();
    }
    

    /// <summary>
    /// Para cargar sonidos, lo que hay que hacer, es,
    /// 1. Comprobar que el sonido está metido en un de los diccionarios de abajo (Carpeta resources).
    /// 2. Ir a SoundAction, y meter dentro del enum, el nombre (identico) del sonido.
    /// Esto es para sonidos que vamos a escuchar por el personaje por asi decirlo, abrir inventario, gas, sonido del mundo
    /// </summary>
    private void LoadAllSounds()
    {
       //LoadSpecificSounds("Sounds/Inventory", inventoryAudioClips);
       //Debug.Log(inventoryAudioClips[0].GetSoundName());
       LoadSpecificSoundsInDictionary("Sounds/Inventory");
    }
    
    public void PauseSounds()
    {
        onStopAudios?.Invoke(this, EventArgs.Empty);
    }

    public void ResumeSounds()
    {
        onResumeAudios?.Invoke(this, EventArgs.Empty);
    }
    
    private void LoadSpecificSoundsInDictionary(string path)
    {
        List<UnityEngine.Object> allSpecificItems = UnityEngine.Resources.LoadAll(path).ToList();
        foreach (var sound in allSpecificItems)
        {
            SoundAction soundAction;
            if (Enum.IsDefined(typeof(SoundAction), sound.name))
            {
                soundAction = (SoundAction)(Enum.Parse(typeof(SoundAction), sound.name));  
            }
            else
            {
                soundAction = SoundAction.Undefined; 
                Debug.LogWarning("No sound to link for " + sound.name);
            }
            audioDictionary.Add(soundAction, (AudioClip) sound);
        }
   
    }
    
    private AudioClip GetAudioClipFromName(SoundAction audioAction)
    {
        if(audioDictionary.ContainsKey(audioAction))
            return audioDictionary[audioAction];
        Debug.LogWarning("THERE IS NO AUDIO FOR THAT NAME [" + audioAction.ToString() + "]");
        return null; 
    }

    public AudioSource ActivateSoundByName(SoundAction audioAction, Transform spawnTransform, bool isFX)
    {
        if (spawnTransform == null)
        {
            spawnTransform = this.transform;
        }
        AudioClip audioClip = GetAudioClipFromName(audioAction);
        if (audioClip != null)
        {
            AudioSource audioSource;
            //spawn gameObject
            if (isFX)
                audioSource = Instantiate(soundFXObjectPrefab, spawnTransform.position, Quaternion.identity);
            else
                audioSource = Instantiate(soundFXObjectPrefab, spawnTransform.position, Quaternion.identity);
            //audioClip
            audioSource.clip = audioClip;
            //play
            audioSource.Play();
            //Destroy after lengh
            float clipLength = audioSource.clip.length;
            
            Destroy(audioSource.gameObject, clipLength);
            
            return audioSource;
        }
        return null;
    }
    
    /// <summary>
    /// Not used 
    /// Coming from https://johnleonardfrench.com/how-to-fade-audio-in-unity-i-tested-every-method-this-ones-the-best/#first_method
    /// Check if it is needed or not, if we want to just desactivate the audio, or fade the audio
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="duration"></param>
    /// <param name="targetVolume"></param>
    /// <returns></returns>
    public IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        sfxVolume = start;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        audioSource.Stop();
        StopAllCoroutines();
        yield break;
    }
    
    
}
}

