using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    public float SetMaxHealth;
    private BarManager barManager;

    // Start is called before the first frame update
    void Start()
    {
        barManager = this.GetComponent<BarManager>();
        barManager.SetBarManager(SetMaxHealth, SetMaxHealth);
        SetHealthBar(barManager.GetNormalizedValue());
    }

    private void SetHealthBar(float HealthNormalized)
    {
        barManager.HandleBarChange(HealthNormalized);
    }

    public void Damage(float Damage)
    {
        barManager.DecreaseValue(Damage);
        SetHealthBar(barManager.GetNormalizedValue());
    }

    public void Heal(float Heal)
    {
        barManager.IncreaseValue(Heal);
        SetHealthBar(barManager.GetNormalizedValue());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
