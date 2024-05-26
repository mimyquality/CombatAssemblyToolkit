
namespace MimyLab
{
    using UdonSharp;
    using UnityEngine;
    using VRC.SDKBase;
    using VRC.Udon;
    using CombatAssemblyToolit;

    public enum DragonHeartResourceType
    {
        HP,
        MP,
        Count
    }

    public enum DragonHeartConditionType
    {
        Element,
        Count
    }

    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class DragonHeart : CombatLife
    {

    }
}
