using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusController : MonoBehaviour
{
    public float actualHeath, maxHealth;
    public float actualHunger, maxHunger;
    public float actualThirst, maxThirst;
    public float actualStamina, maxStamina;
    [SerializeField] private float hungerDecay;
    [SerializeField] private float thirstDecay;
    public float actualDelayToRestoreStamina, delayToRestoreStamina;

    public bool receivingHungerDamage, receivingThirstDamage;
    public bool restoringStamina;

    private float acumulatedDamage;

    private GameObject healthUI, hungerUI, thirstUI, staminaUI;

    private void Start()
    {
        healthUI = GameObject.Find("UI/Status/Health/Holder/Fill").gameObject;
        hungerUI = GameObject.Find("UI/Status/Hunger/Holder/Fill").gameObject;
        thirstUI = GameObject.Find("UI/Status/Thirst/Holder/Fill").gameObject;
        staminaUI = GameObject.Find("UI/Status/Stamina/Holder/Fill").gameObject;
    }

    private void FixedUpdate()
    {
        if (actualHunger > 0)
        {
            actualHunger -= hungerDecay;
            if (receivingHungerDamage)
            {
                receivingHungerDamage = false;
                acumulatedDamage -= .05f;
            }
        }
        else if (actualHunger <= 0 && !receivingHungerDamage)
        {
            receivingHungerDamage = true;
            acumulatedDamage += .05f;
        }
        if (actualThirst > 0)
        {
            actualThirst -= thirstDecay;
            if (receivingThirstDamage)
            {
                receivingThirstDamage = false;
                acumulatedDamage -= .05f;
            }
        }
        else if (actualThirst <= 0 && !receivingThirstDamage)
        {
            receivingThirstDamage = true;
            acumulatedDamage += .05f;
        }

        if (receivingHungerDamage || receivingThirstDamage) ReceiveDamage(acumulatedDamage);

        RestoreStamina();
    }

    private void Update()
    {
        healthUI.transform.localScale = new Vector3(1, actualHeath / maxHealth, 1);
        hungerUI.transform.localScale = new Vector3(1, actualHunger / maxHunger, 1);
        thirstUI.transform.localScale = new Vector3(1, actualThirst / maxThirst, 1);
        staminaUI.transform.localScale = new Vector3(1, actualStamina / maxStamina, 1);
    }

    public void ReceiveDamage(float _damage)
    {
        actualHeath -= _damage;
        if (actualHeath < 0)
        {
            ResetStatus();
            this.transform.position = this.GetComponent<CommonInfos>().spawnLocation;
            GameObject.Find("Main Camera").transform.position = new Vector3(this.transform.position.x, this.transform.position.y, GameObject.Find("Main Camera").transform.position.z);
        }
    }

    public void IncreaseLife(float _quantity)
    {
        actualHeath += _quantity;
        if (actualHeath > maxHealth) actualHeath = maxHealth;
    }

    public void DecreaseHunger(float _quantity)
    {
        actualHunger -= _quantity;
        if (actualHunger < 0) actualHunger = 0;
    }

    public void IncreaseHunger(float _quantity)
    {
        actualHunger += _quantity;
        if (actualHunger > maxHealth) actualHunger = maxHunger;
    }

    public void DecreaseThirst(float _quantity)
    {
        actualThirst -= _quantity;
        if (actualThirst < 0) actualThirst = 0;
    }

    public void IncreaseThirst(float _quantity)
    {
        actualThirst += _quantity;
        if (actualThirst > maxHealth) actualThirst = maxThirst;
    }

    public void DecreaseStamina(float _quantity)
    {
        actualStamina -= _quantity;
        if (actualStamina < 0) actualStamina = 0;
        actualDelayToRestoreStamina = delayToRestoreStamina;
        restoringStamina = false;
    }

    public void IncreaseStamina(float _quantity)
    {
        actualStamina += _quantity;
        if (actualStamina > maxHealth) actualStamina = maxStamina;
    }

    public void ResetStatus()
    {
        actualHeath = maxHealth;
        actualHunger = maxHunger;
        actualThirst = maxThirst;
        actualStamina = maxStamina;
    }

    private void RestoreStamina()
    {
        if (!restoringStamina)
        {
            if (actualDelayToRestoreStamina > 0) actualDelayToRestoreStamina -= .01f;
            else
            {
                actualDelayToRestoreStamina = delayToRestoreStamina;
                restoringStamina = true;
            }
        }
        if (restoringStamina && actualStamina < maxStamina) IncreaseStamina(.05f);
    }
}
