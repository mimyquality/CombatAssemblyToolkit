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

    [RequireComponent(typeof(ParticleSystem))]
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class DragonBreathInvolver : CombatInvolver
    {
        private ParticleSystem _breathEffect;

        private void OnParticleCollision(GameObject other)
        {
            if (!Utilities.IsValid(other)) { return; }

        }

        public override void Generate()
        {
            if (!_breathEffect) _breathEffect = GetComponent<ParticleSystem>();
            
            _breathEffect.Emit(1);
        }

        public override void Involve(CombatUnit unit)
        {

        }
    }
}
