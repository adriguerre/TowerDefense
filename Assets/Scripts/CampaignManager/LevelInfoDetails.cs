using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelInfoDetails : MonoBehaviour
{

    [SerializeField] private Button backButton;
    [SerializeField] private Button playButton;

    private LevelSO levelInfo;
    [SerializeField] private Image levelImage;
    [SerializeField] private TextMeshProUGUI levelDescription;
    [SerializeField] private Image levelGridVisualizer;

    private void Start()
    {
        backButton.onClick.AddListener(() => CloseInfoView());
    }

    private void CloseInfoView()
    {
        this.gameObject.SetActive(false);
    }
    private void OpenLevel()
    {
        CampaignManager.Instance.OpenLevel(levelInfo);
    }
    public void SetProperties(LevelSO levelInfo)
    {
        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(() => OpenLevel());
        this.levelInfo = levelInfo;
        levelImage.sprite = levelInfo.levelVillageIcon;
        levelDescription.text = levelInfo.levelDescription;
        levelGridVisualizer.sprite = levelInfo.levelGridVisualizer;
    }


}