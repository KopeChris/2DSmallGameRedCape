using UnityEngine;

public class FlowerBossController : MonoBehaviour
{
    public enum BossState { Idle, Attacking, Defeated }
    public BossState currentState;
    public int maxHealth = 100;
    public int currentHealth;
    public float attackInterval = 5f;
    public Transform[] attackPoints; // Define spawn points for projectiles or vine attacks

    private float timeSinceLastAttack = 0f;
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        currentState = BossState.Idle;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (currentState == BossState.Attacking)
        {
            timeSinceLastAttack += Time.deltaTime;

            if (timeSinceLastAttack >= attackInterval)
            {
                ChooseAttackPattern();
                timeSinceLastAttack = 0f;
            }
        }
    }

    void ChooseAttackPattern()
    {
        int randomPattern = Random.Range(1, 5);
        switch (randomPattern)
        {
            case 1:
                AttackPattern1();
                break;
            case 2:
                AttackPattern2();
                break;
            case 3:
                AttackPattern3();
                break;
            case 4:
                AttackPattern4();
                break;
        }
    }

    void AttackPattern1()
    {
        // Implement the seed attack
        // Instantiate projectiles and shoot them upwards
    }

    void AttackPattern2()
    {
        // Implement the vine attack
        // Instantiate vines at different attack points on the ground
    }

    void AttackPattern3()
    {
        // Implement the boomerang attack
        // Instantiate boomerang projectiles and shoot them in a circular path
    }

    void AttackPattern4()
    {
        // Implement the acorn rain attack
        // Instantiate acorns and drop them randomly from the top of the screen
    }

    public void TakeDamage(int damage)
    {
        if (currentState != BossState.Defeated)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            if (currentHealth <= 0)
            {
                currentState = BossState.Defeated;
                // Play defeat animation and handle boss death
            }
        }
    }

    public void StartAttacking()
    {
        currentState = BossState.Attacking;
    }
}
