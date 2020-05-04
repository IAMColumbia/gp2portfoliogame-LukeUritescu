using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarManager : MonoBehaviour
{
    private Image barImage;
    private float maxValue;
    private float currentValue;
    private float minValue = 0;
    [SerializeField]
    private float updateSpeedSeconds;
    [SerializeField]
    public string EnterNameOfBarHere;

    public float GetCurrentValue()
    {
        return this.currentValue;
    }

    void Awake()
    {
        barImage = transform.Find(EnterNameOfBarHere).GetComponent<Image>();

    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetUpdateSpeedSecconds(float speedSeconds = 0.4f)
    {
        this.updateSpeedSeconds = speedSeconds;
    }

    public void SetBarManager(float SetMaxValue, float SetCurrentValue)
    {
        this.maxValue = SetMaxValue;
        this.currentValue = SetCurrentValue;
    }
    #region Increase/Decrease Functions
    /// <summary>
    /// Reusable code since every bar we will use(thruster, health,  progress) will need to either increase or decrease
    /// </summary>
    /// <param name="damageAmount"></param>

    public void DecreaseValue(float damageAmount)
    {
        currentValue -= damageAmount;
        if (currentValue < 0)
        {
            currentValue = 0;
        }
    }

    public void IncreaseValue(float healAmount)
    {
        currentValue += healAmount;
        if (currentValue > maxValue)
        {
            currentValue = maxValue;
        }
    }
    #endregion
    #region BarSmoother
    /// <summary>
    /// This region is the code behind the bar's increasing/decreasing gradually instead of instantly
    /// </summary>
    /// <param name="ValueNormalized"></param>

    public void HandleBarChange(float ValueNormalized)
    {
        StartCoroutine(ChangeBarAmount(ValueNormalized));
    }

    private IEnumerator ChangeBarAmount(float ValueNormalized)
    {
        float preChangePercent = barImage.fillAmount;
        float elapsedTime = 0f;

        while (elapsedTime < updateSpeedSeconds)
        {
            elapsedTime += Time.deltaTime;
            barImage.fillAmount = Mathf.Lerp(preChangePercent, ValueNormalized, elapsedTime / updateSpeedSeconds);
            yield return null;
        }

        barImage.fillAmount = ValueNormalized;
    }
    #endregion

    //This just normalizes the value so it can work with the filler component which bases off of percentage.
    public float GetNormalizedValue()
    {
        return (float)currentValue / maxValue;
    }
}
