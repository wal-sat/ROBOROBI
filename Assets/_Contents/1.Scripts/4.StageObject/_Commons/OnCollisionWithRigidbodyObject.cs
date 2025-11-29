using System;
using UnityEngine;

public class OnCollisionWithRigidbodyObject : MonoBehaviour
{
    [SerializeField] private bool isOnlyCollideWithPlayer = true;

    public Action<RigidbodyObject> CollisionEnterCallback;
    public Action<RigidbodyObject> CollisionStayCallback;
    public Action<RigidbodyObject> CollisionExitCallback;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        RigidbodyObject rigidbodyObject = collision.gameObject.GetComponent<RigidbodyObject>();
        if (rigidbodyObject != null)
        {
            if (isOnlyCollideWithPlayer && !rigidbodyObject.gameObject.CompareTag("Player")) return;

            CollisionEnterCallback?.Invoke(rigidbodyObject);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        RigidbodyObject rigidbodyObject = collision.gameObject.GetComponent<RigidbodyObject>();
        if (rigidbodyObject != null)
        {
            if (isOnlyCollideWithPlayer && !rigidbodyObject.gameObject.CompareTag("Player")) return;

            CollisionStayCallback?.Invoke(rigidbodyObject);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        RigidbodyObject rigidbodyObject = collision.gameObject.GetComponent<RigidbodyObject>();
        if (rigidbodyObject != null)
        {
            if (isOnlyCollideWithPlayer && !rigidbodyObject.gameObject.CompareTag("Player")) return;

            CollisionExitCallback?.Invoke(rigidbodyObject);
        }
    }
}
