using UnityEngine;

public class Thorn : MonoBehaviour
{
    [SerializeField] private StageManager _stageManager;

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _stageManager.PlayerDeath();
        }
    }
}
