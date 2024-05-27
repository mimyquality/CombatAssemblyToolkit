/*
Copyright (c) 2024 Mimy Quality
Released under the MIT license
https://opensource.org/licenses/mit-license.php
*/

namespace MimyLab.CombatAssemblyToolit
{
    using UdonSharp;
    using UnityEngine;
    using VRC.SDKBase;
    using VRC.Udon;

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
