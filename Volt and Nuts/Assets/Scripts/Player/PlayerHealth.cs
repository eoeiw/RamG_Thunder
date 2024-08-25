using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Singleton<PlayerHealth>
{
    public Sprite[] heartSprites;  // Unity Inspector���� �巡�� �� ������� ����

    public int CurrentHealth {  get; private set; }

    //[SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;
    [SerializeField] private float timebetweenHeal = 20;

    private Slider healthSlider;

    private bool canTakeDamage = true;
    private bool isHealing = false;

    //private KnockBack knockBack;
    //private Flash flash;

    private Transform heartContainer;
    private int currentHealth;
    [SerializeField] private int maxHealth = 5;
    const string HEART_CONTAINER_TEXT = "Heart Container";

    protected override void Awake()
    {
        base.Awake();
        //flash = GetComponent<Flash>();
        //knockBack = GetComponent<KnockBack>();

        currentHealth = maxHealth;
        CurrentHealth = maxHealth;
    }

    private void Start()
    {
        heartContainer = GameObject.Find(HEART_CONTAINER_TEXT).transform;
    }

    private void Update()
    {
        if (currentHealth < maxHealth && !isHealing)
        {
            StartCoroutine(RefreshHealthRoutine());
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();

        if(enemy && canTakeDamage)
        {
            TakeDamage(1);
            //knockBack.GetKnockedBack(other.gameObject.transform, knockBackThrustAmount);
            //StartCoroutine(flash.FlashRoutine());
        }
    }

    private void HealPlayer()
    {
        if(currentHealth < maxHealth)
        {

            currentHealth += 1;
            CurrentHealth += 1;
        }
        UpdateHeartImage();
    }

    private void TakeDamage(int damageAmount)
    {
        canTakeDamage = false;
        currentHealth -= damageAmount;
        CurrentHealth -= damageAmount;
        StartCoroutine(DamageRecoveryRoutine()); // �ٽ� ������ �ޱ������ �ð�

        UpdateHeartImage();
        CheckIfPlayerDeath();
    }

    private IEnumerator DamageRecoveryRoutine() // �ٽ� ������ �ޱ���� �ڷ�ƾ
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private IEnumerator RefreshHealthRoutine()
    {
        isHealing = true;
        yield return new WaitForSeconds(timebetweenHeal);
        HealPlayer();
        isHealing = false;
    }

    private void UpdateHeartImage()
    {
        for(int i = 0; i< maxHealth; i++)
        {
            if(i<=CurrentHealth - 1)
            {
                heartContainer.GetChild(i).GetComponent<Image>().sprite = heartSprites[4];
            }
            else
            {
                heartContainer.GetChild(i).GetComponent<Image>().sprite = heartSprites[0];
            }
        }
    }

    private void CheckIfPlayerDeath()
    {
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Player Death");
        }
    }
}
