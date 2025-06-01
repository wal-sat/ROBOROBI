using UnityEngine;

public class DisplayInEditorOnly : MonoBehaviour
{
    [SerializeField] private bool _isDestroy;

    private void Start()
    {
        if (this.gameObject.activeSelf) this.gameObject.SetActive(false);

        if (_isDestroy) Destroy(this.gameObject);
        else Destroy(this);
    }
}
