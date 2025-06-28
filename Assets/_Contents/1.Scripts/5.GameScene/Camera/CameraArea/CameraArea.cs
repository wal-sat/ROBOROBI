using System;
using UnityEngine;

public class CameraArea : MonoBehaviour
{
    [SerializeField] private CameraAreaManager _cameraAreaManager;
    [SerializeField] public int CameraAreaPriority;
    [SerializeField] public float CameraSize;

    [HideInInspector] public Collider2D Collider2D;
    [HideInInspector] public Vector2 BottomLeftPos;
    [HideInInspector] public Vector2 TopRightPos;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        Collider2D = GetComponent<Collider2D>();

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Bounds bounds = sr.bounds;

        BottomLeftPos = new Vector2(bounds.min.x, bounds.min.y);
        TopRightPos = new Vector2(bounds.max.x, bounds.max.y);
        sr.sprite = null;
    }

    // ----- Private Methods -----

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerStageAreaChecker"))
        {
            _cameraAreaManager.Register(this);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerStageAreaChecker"))
        {
            _cameraAreaManager.Unregister(this);
        }
    }
}
