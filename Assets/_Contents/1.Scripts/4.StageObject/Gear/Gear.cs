using UnityEngine;

public class Gear : MonoBehaviour
{
    [SerializeField] private GearManager _gearManager;
    [SerializeField] private GearView _gearView;
    [SerializeField] private IsTriggerWithPlayer _isTriggerWithPlayer;
    [SerializeField] private ParticleSystem _gearDefaultParticle;
    [SerializeField] private ParticleSystem _gearBurstParticle;

    [SerializeField] public int GearIndex;
    [HideInInspector] public GearStatus GearStatus { get; set; }

    // ----- Life Cycle Methods -----

    private void Awake()
    {
        _gearManager.Register(this);

        _isTriggerWithPlayer.TriggerEnterCallback += TriggerEnter;
    }

    // ----- Public Methods -----

    public void GearInitialize(GearStatus gearStatus)
    {
        GearStatus = gearStatus;

        if (GearStatus == GearStatus.Acquired)
        {
            _gearView.ChangeSprite(false);
        }
        else if (GearStatus == GearStatus.NotAcquired)
        {
            _gearView.ChangeSprite(true);
            _gearDefaultParticle.Play();
        }
    }

    // ----- Private Methods -----

    private void TriggerEnter()
    {
        if (GearStatus == GearStatus.NotAcquired)
        {
            _gearView.ChangeSprite(false);
            GearStatus = GearStatus.TemporaryAcquired;
            _gearManager.OnAcquired();

            _gearDefaultParticle.Stop();
            _gearBurstParticle.Play();

            S_SEManager.Instance.Play("s_gear");
        }
    }
}
