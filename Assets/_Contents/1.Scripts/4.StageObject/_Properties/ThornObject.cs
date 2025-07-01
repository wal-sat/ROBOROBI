using UnityEngine;

public class ThornObject : MonoBehaviour
{
    [SerializeField] private StageManager _stageManager;

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _stageManager.PlayerDeath().Forget();
        }
    }
}
