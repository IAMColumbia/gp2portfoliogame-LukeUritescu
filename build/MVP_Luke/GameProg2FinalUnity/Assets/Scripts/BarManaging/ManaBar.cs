using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaBar : MonoBehaviour
{
    [SerializeField]
    public float SetMaxMP;
    private BarManager barManager;


    // Start is called before the first frame update
    void Start()
    {
        barManager = this.GetComponent<BarManager>();
        barManager.SetBarManager(SetMaxMP, SetMaxMP);
        SetManaBar(barManager.GetNormalizedValue());
    }

    private void SetManaBar(float HealthNormalized)
    {
        barManager.HandleBarChange(HealthNormalized);
    }

    public void Decrease(float ManaCost)
    {
        barManager.DecreaseValue(ManaCost);
        SetManaBar(barManager.GetNormalizedValue());
        
    }

    public void RestoreMana(float Mana)
    {
        barManager.IncreaseValue(Mana);
        SetManaBar(barManager.GetNormalizedValue());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
