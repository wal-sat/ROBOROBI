using UnityEngine;

public abstract class StageAreaBase : MonoBehaviour
{
    [HideInInspector] public Vector2 MinPosition;
    [HideInInspector] public Vector2 MaxPosition;

    // ----- Life Cycle Methods -----

    protected virtual void Awake()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Bounds bounds = sr.bounds;

        MinPosition = new Vector2(bounds.min.x, bounds.min.y);
        MaxPosition = new Vector2(bounds.max.x, bounds.max.y);
        sr.sprite = null;
    }
}
