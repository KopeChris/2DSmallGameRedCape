using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject ExplosionPrefab;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            HitEffect();
        }
    }
    protected void HitEffect()
    {
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
    }
}
