using UnityEngine;

public interface IOverlapRope
{
    public void Register(Rope rope);
    public void Unregister(Rope rope);
}
