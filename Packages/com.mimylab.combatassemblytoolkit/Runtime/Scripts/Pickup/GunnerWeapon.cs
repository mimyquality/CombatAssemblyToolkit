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
    using VRC.SDK3.Components;

    [RequireComponent(typeof(VRCPickup))]
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class GunnerWeapon : CombatUnit
    {
        public override void OnPickup()
        {
            SetUnitOwner(Networking.LocalPlayer);
        }

        public override void OnPickupUseDown()
        {
            ShotAction(_holdSkills[0]);
        }

        private  void ShotAction(CombatSkill skill)
        {
            if (!skill) { return; }

            var cost = skill.Cost;
            if (!_life.HasResource(cost)) { return; }

            _life.Consume(cost);
            skill.ShotAction();
        }
    }
}
