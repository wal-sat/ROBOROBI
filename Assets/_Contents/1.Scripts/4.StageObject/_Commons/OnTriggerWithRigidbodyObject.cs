using System;
using UnityEngine;

public class OnTriggerWithRigidbodyObject : MonoBehaviour
{
    [SerializeField] private bool isOnlyTriggerWithPlayer = true;

    public Action<RigidbodyObject> TriggerEnterCallback;
    public Action<RigidbodyObject> TriggerStayCallback;
    public Action<RigidbodyObject> TriggerExitCallback;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RigidbodyObject rigidbodyObject = collision.gameObject.GetComponent<RigidbodyObject>();
        if (rigidbodyObject != null)
        {
            if (isOnlyTriggerWithPlayer && !rigidbodyObject.gameObject.CompareTag("Player")) return;

            TriggerEnterCallback?.Invoke(rigidbodyObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        RigidbodyObject rigidbodyObject = collision.gameObject.GetComponent<RigidbodyObject>();
        if (rigidbodyObject != null)
        {
            if (isOnlyTriggerWithPlayer && !rigidbodyObject.gameObject.CompareTag("Player")) return;

            TriggerStayCallback?.Invoke(rigidbodyObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        RigidbodyObject rigidbodyObject = collision.gameObject.GetComponent<RigidbodyObject>();
        if (rigidbodyObject != null)
        {
            if (isOnlyTriggerWithPlayer && !rigidbodyObject.gameObject.CompareTag("Player")) return;

            TriggerExitCallback?.Invoke(rigidbodyObject);
        }
    }
}
