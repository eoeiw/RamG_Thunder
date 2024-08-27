using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // 씬 전환을 위해 추가

public class PlayerHealth : Singleton<PlayerHealth>
{
    public Sprite[] heartSprites;  // Unity Inspector에서 설정

    public int CurrentHealth { get; private set; }

    [SerializeField] private float damageRecoveryTime = 1f;
    [SerializeField] private float timebetweenHeal = 20;

    private Slider healthSlider;
    private Animator animator; // Animator 컴포넌트를 참조할 변수

    private bool canTakeDamage = true;
    private bool isHealing = false;

    private Transform heartContainer;
    private int currentHealth;
    [SerializeField] private int maxHealth = 5;
    const string HEART_CONTAINER_TEXT = "Heart Container";
    const string HIT_TRIGGER = "Hit"; // 애니메이션 트리거 이름

    protected override void Awake()
    {
        base.Awake();

        currentHealth = maxHealth;
        CurrentHealth = maxHealth;

        // Animator 컴포넌트 가져오기
        animator = GetComponent<Animator>();
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

        if (enemy && canTakeDamage)
        {
            TakeDamage(1);
        }
    }

    private void HealPlayer()
    {
        if (currentHealth < maxHealth)
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

        // 애니메이션 트리거 발동
        animator.SetTrigger("Attacked");

        StartCoroutine(DamageRecoveryRoutine());

        UpdateHeartImage();
        CheckIfPlayerDeath();
    }

    private IEnumerator DamageRecoveryRoutine()
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
        for (int i = 0; i < maxHealth; i++)
        {
            if (i <= CurrentHealth - 1)
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
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animator.SetTrigger("Die");
            Debug.Log("Player Death");
            SceneManager.LoadScene("EndingScene");
        }
    }
}
