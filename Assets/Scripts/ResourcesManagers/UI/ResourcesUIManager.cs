using System.Collections;
using TMPro;
using UnityEngine;

namespace GameResources
{
  public class ResourcesUIManager : MonoBehaviour
{
	[Header("Resources UI Text")]
	[SerializeField] private TextMeshProUGUI foodAvailableText;
	[SerializeField] private TextMeshProUGUI woodAvailableText;
	[SerializeField] private TextMeshProUGUI stoneAvailableText;
	[SerializeField] private TextMeshProUGUI ironAvailableText;
	[SerializeField] private TextMeshProUGUI goldAvailableText;
    
	[Header("Number values for lerp")]
	[SerializeField] private int CountFPS = 30;
	[SerializeField] private float duration = 1f;
	
	private Coroutine foodCoroutine;
	private Coroutine woodCoroutine;
	private Coroutine stoneCoroutine;
	private Coroutine ironCoroutine;
	private Coroutine goldCoroutine;
    
    void Awake()
    {
        ResourcesManager.Instance.onVariableChange += VariableChanged;
    }

	
    private void UpdateText(Coroutine coroutine, double newValue, double previousValue, TextMeshProUGUI text, bool isProduction)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(WriteTextAnimation(newValue, previousValue, text, isProduction));
    }
    
    private IEnumerator WriteTextAnimation(double newValue, double previousVal, TextMeshProUGUI textMeshProUGUI, bool isProduction)
    {
        WaitForSeconds wait = new WaitForSeconds(1f / CountFPS);
        double previousValue = previousVal;
        int stepAmount;
        if (newValue - previousValue < 0)
            stepAmount = Mathf.FloorToInt((long) (newValue - previousValue) / (CountFPS * duration)); // (20 - 0) / (30 * 1) => -0.66 -> 0
        else
            stepAmount = Mathf.CeilToInt((long) (newValue - previousValue) / (CountFPS * duration));

        if (previousValue < newValue)
        {
            while (previousValue < newValue)
            {
                previousValue += stepAmount;
                if (previousValue > newValue)
                {
                    previousValue = newValue;
                }
                if(!isProduction)
                    textMeshProUGUI.text = IntHelper.numStr(previousValue);
                else
                    textMeshProUGUI.text = IntHelper.numStr(previousValue) + " /s";
                yield return wait;
            }
        }
        else
        {
            while (previousValue > newValue)
            {
                previousValue += stepAmount;
                if (previousValue < newValue)
                {
                    previousValue = newValue;
                }

                if(!isProduction)
                    textMeshProUGUI.text = IntHelper.numStr(previousValue);
                else
                    textMeshProUGUI.text = IntHelper.numStr(previousValue) + " /s";
                yield return wait;
            }
        }
    }
    
	private void VariableChanged(double currentValue, double newvalue, ResourceType resourcetype)
    {
        switch (resourcetype)
        {
            case ResourceType.Food:
                UpdateText(foodCoroutine, newvalue, currentValue, foodAvailableText, false);
                break;
            case ResourceType.Wood:
                //soldiersAvailableAvailableText.text = newvalue.ToString();
                UpdateText(woodCoroutine, newvalue, currentValue, woodAvailableText, false);
                break;
            case ResourceType.Stone:
                UpdateText(stoneCoroutine, newvalue, currentValue, stoneAvailableText, false);
                break;
            case ResourceType.Iron:
                UpdateText(ironCoroutine, newvalue, currentValue, ironAvailableText, false);
                break;
            case ResourceType.Gold:
                UpdateText(goldCoroutine, newvalue, currentValue, goldAvailableText, false);
                break;
        }
    }
}  
}
