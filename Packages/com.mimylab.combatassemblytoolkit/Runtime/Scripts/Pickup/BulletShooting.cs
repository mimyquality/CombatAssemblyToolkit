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

    public class BulletShooting : CombatTriggerSkill
    {
        [SerializeField]
        private ParticleSystem _bullet;

        protected override void TriggerAction()
        {
            _bullet.Emit(1);
        }
    }
}
