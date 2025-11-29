using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private float _angleZ = -1;

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        IHitSpike hitSpike = collision.gameObject.GetComponent<IHitSpike>();
        if (hitSpike != null)
        {
            float angleZ = _angleZ;
            if (angleZ == -1)
            {
                Vector2 direction = collision.transform.position - transform.position;
                angleZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            }
            
            hitSpike.HitSpike(angleZ);
        }
    }
}
