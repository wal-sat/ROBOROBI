using UnityEngine;
using DG.Tweening;

public class Button : MonoBehaviour
{
    [SerializeField] private ButtonManager _buttonManager;
    [SerializeField] private ButtonView _buttonView;
    [SerializeField] private IsTriggerWithPlayer _isTriggerWithPlayer;
    [SerializeField] private GameObject _movedObject;
    [SerializeField] private Vector2 _movePoint;

    private const float MoveTime = 1.5f;

    private Tween _currentTween;
    private Vector2 _defaultPosition;
    private FirstCallChecker _firstCallChecker = new FirstCallChecker();

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _buttonManager.Register(this);

        _defaultPosition = _movedObject.transform.position;
        _isTriggerWithPlayer.TriggerEnterCallback += TriggerEnter;
    }

    // ----- Public Methods -----

    public void ButtonInitialize()
    {
        _firstCallChecker.Reset();
        _buttonView.SpriteChange(true);

        _currentTween.Kill();
        _movedObject.transform.position = _defaultPosition;
    }

    // ----- Private Methods -----

    private void TriggerEnter()
    {
        if (_firstCallChecker.Check())
        {
            _buttonView.SpriteChange(false);

            Vector2 pos = new Vector2(_defaultPosition.x + _movePoint.x, _defaultPosition.y + _movePoint.y);
            _currentTween = _movedObject.transform.DOMove(pos, MoveTime).SetEase(Ease.OutQuad);

            S_SEManager.Instance.Play("s_button");
            S_SEManager.Instance.Play("s_movable");
        }
    }
}
