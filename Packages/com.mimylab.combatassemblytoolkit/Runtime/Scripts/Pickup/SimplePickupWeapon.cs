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
    //using VRC.SDK3.Components;

    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class SimplePickupWeapon : CombatUnit
    {
        public override void OnPickup()
        {
            SetUnitOwner(Networking.LocalPlayer);
        }

        public override void OnPickupUseDown()
        {
            _holdSkills[0].Action();
        }
    }
}
