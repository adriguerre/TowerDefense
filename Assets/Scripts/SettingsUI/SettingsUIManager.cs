using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SettingsUIManager : Singleton<SettingsUIManager>
{

    #region Public Fields 
    #endregion 	

    #region Private Fields
    
    [SerializeField] private Slider fxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;

    [SerializeField] private Button muteMusicButton;
    [SerializeField] private Button muteFXButton;

    [Header("Volume Button sprites")]
    [SerializeField] private Sprite volumeTurnOff;
    [SerializeField] private Sprite volumeTurnOn;
    
    private bool _isFXIsMuted = false;
    private bool _isMusicIsMuted = false;
    private Canvas _settingsCanvas;
    private bool _settingsOpened = false;
    #endregion 	

    #region Properties 
    #endregion 
	
    #region Events 
    #endregion 	

    #region Unity Methods

    private void Awake()
    {
        _settingsCanvas = GetComponentInParent<Canvas>();
    }

    void Start()
    {
        LoadVolumeValues();
        muteFXButton.onClick.AddListener(() => MuteFXVolume());
        muteMusicButton.onClick.AddListener(() => MuteMusicVolume());
    }

    #endregion

    #region Public Methods

    public void SwapSettingsVisibility()
    {
        _settingsOpened = !_settingsOpened;
        _settingsCanvas.enabled = _settingsOpened; 
        this.gameObject.SetActive(_settingsOpened);
    }

    public void CloseSettingsUI()
    {
        this.gameObject.SetActive(false);
        _settingsCanvas.enabled = false;
    }

    #endregion 	

    #region Private Methods

    private void MuteFXVolume()
    {
        if (fxVolumeSlider.value > 0.0001f || _isFXIsMuted)
        {
            _isFXIsMuted = !_isFXIsMuted;
            float newVolume = _isFXIsMuted ? 0 : PlayerPrefs.GetFloat("FXVolumeBeforeMute");
            if (_isFXIsMuted)
            {
                PlayerPrefs.SetString("FXWasMuted", "true");
                PlayerPrefs.SetFloat("FXVolumeBeforeMute",  fxVolumeSlider.value);

                muteFXButton.gameObject.GetComponent<Image>().sprite = volumeTurnOff;
            }
            else
            {
                PlayerPrefs.SetString("FXWasMuted", "false");
                muteFXButton.gameObject.GetComponent<Image>().sprite = volumeTurnOn;
            }
            fxVolumeSlider.value = newVolume;
        }
    }

    private void MuteMusicVolume()
    {
        if (musicVolumeSlider.value > 0.0001f || _isMusicIsMuted)
        {
            _isMusicIsMuted = !_isMusicIsMuted;
            float newVolume = _isMusicIsMuted ? 0 : PlayerPrefs.GetFloat("MusicVolumeBeforeMute");
            if (_isMusicIsMuted)
            {
                PlayerPrefs.SetString("MusicWasMuted", "true");
                PlayerPrefs.SetFloat("MusicVolumeBeforeMute",  musicVolumeSlider.value);
                muteMusicButton.gameObject.GetComponent<Image>().sprite = volumeTurnOff;

            }else
            {
                PlayerPrefs.SetString("MusicWasMuted", "false");
                muteMusicButton.gameObject.GetComponent<Image>().sprite = volumeTurnOn;
            }
            musicVolumeSlider.value = newVolume;
        }
    }
    
    public void LoadVolumeValues()
    {
        if (PlayerPrefs.GetString("FXWasMuted").Equals("true"))
        {
            muteFXButton.gameObject.GetComponent<Image>().sprite = volumeTurnOff;
            _isFXIsMuted = true;
            fxVolumeSlider.value = 0f; 
        }
        else
        {
            if (PlayerPrefs.HasKey("SoundEffectsVolume"))
                fxVolumeSlider.value = PlayerPrefs.GetFloat("SoundEffectsVolume");
            else
                fxVolumeSlider.value = 0.5f;   
            
            muteFXButton.gameObject.GetComponent<Image>().sprite = volumeTurnOn;

        }
        
        if (PlayerPrefs.GetString("MusicWasMuted").Equals("true"))
        {
            muteMusicButton.gameObject.GetComponent<Image>().sprite = volumeTurnOff;
            _isMusicIsMuted = true;
            musicVolumeSlider.value = 0f; 
        }
        else
        {
            if (PlayerPrefs.HasKey("MusicVolume"))
                musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            else
                musicVolumeSlider.value = 0.5f;
            
            muteMusicButton.gameObject.GetComponent<Image>().sprite = volumeTurnOn;

        }
    }

    public void MusicVolumeButtonDesactivated()
    {
        muteMusicButton.gameObject.GetComponent<Image>().sprite = volumeTurnOff;
    }
    
    public void FXVolumeButtonDesactivated()
    {
        muteFXButton.gameObject.GetComponent<Image>().sprite = volumeTurnOff;
    }   
    
    public void MusicVolumeButtonActivated()
    {
        muteMusicButton.gameObject.GetComponent<Image>().sprite = volumeTurnOn;
    }
    
    public void FXVolumeButtonToZeroActivated()
    {
        muteFXButton.gameObject.GetComponent<Image>().sprite = volumeTurnOn;
    }
    #endregion 

    #region Getter & Setters 
    #endregion 	


}