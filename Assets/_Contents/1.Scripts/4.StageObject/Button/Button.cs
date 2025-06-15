using UnityEngine;
using DG.Tweening;

public class Button : MonoBehaviour
{
    [SerializeField] private ButtonManager _buttonManager;
    [SerializeField] private ButtonView _buttonView;
    [SerializeField] private GameObject _movedObject;
    [SerializeField] private Vector2 _movePoint;

    private const float MoveTime = 1.5f;

    private Tween _currentTween;
    private Vector2 _defaultPosition;
    private bool _isEnable;

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _buttonManager.Register(this);

        _defaultPosition = _movedObject.transform.position;
    }

    // ----- Public Methods -----

    public void Initialize()
    {
        _isEnable = true;
        _buttonView.SpriteChange(_isEnable);

        _currentTween.Kill();
        _movedObject.transform.position = _defaultPosition;
    }

    // ----- Private Methods -----

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (_isEnable)
            {
                _isEnable = false;
                _buttonView.SpriteChange(_isEnable);

                PressedButton();

                S_SEManager.Instance.Play("s_button");
                S_SEManager.Instance.Play("s_movable");
            }
        }
    }

    private void PressedButton()
    {
        Vector2 pos = new Vector2(_defaultPosition.x + _movePoint.x, _defaultPosition.y + _movePoint.y);
        _currentTween = _movedObject.transform.DOMove(pos, MoveTime).SetEase(Ease.OutQuad);
    }
}
