using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Buildings.CivilianBuildings
{
    public class BuildingFillAmount : MonoBehaviour
    {
        public event EventHandler onBuildingFinished;
        private Image _fillAmountImage;
        private float _timetoBuild;
        private bool isBuilding;

        
        private void OnEnable()
        {
            _fillAmountImage = gameObject.GetComponentInChildren<Image>();
        }

  

        public void StartFilling(int startAmount, float timeToBuild)
        {
            this.gameObject.SetActive(true);
            this._timetoBuild = timeToBuild;
            _fillAmountImage.fillAmount = startAmount;
            StartCoroutine(FillOverTime(timeToBuild));
        }

        IEnumerator FillOverTime(float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                _fillAmountImage.fillAmount = Mathf.Clamp01(elapsedTime / duration);
                yield return null;
            }
            _fillAmountImage.fillAmount = 1f; // Asegura que el fill sea completo al final
            this.gameObject.SetActive(false);
            onBuildingFinished?.Invoke(this, EventArgs.Empty);
        }
        
    }
}