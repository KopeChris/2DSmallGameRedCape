using UnityEngine;

public class BossFightManager : MonoBehaviour
{
    public GameObject boss;
    private bool bossFightStarted = false;
    void Start()
    {
        // Deactivate the boss at the start of the scene
        boss.SetActive(false);
    }

    // Other boss fight-related methods can be added here, such as handling the boss's defeat, etc.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartBossFight();
        }
    }

    public void StartBossFight()
    {
        if (!bossFightStarted)
        {
            bossFightStarted = true;
            // Activate the boss and start the fight
            boss.SetActive(true);

            // You can add any additional logic here, like playing a cutscene, dialogue, or special effects
        }
    }

}
