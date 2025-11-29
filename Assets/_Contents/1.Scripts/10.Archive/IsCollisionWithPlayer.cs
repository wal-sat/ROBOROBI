using System;
using UnityEngine;

public class IsCollisionWithPlayer : MonoBehaviour
{
    public Action<Collision2D> CollisionEnterCallback;
    public Action<Collision2D> CollisionExitCallback;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CollisionEnterCallback?.Invoke(collision);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CollisionExitCallback?.Invoke(collision);
        }
    }
}
