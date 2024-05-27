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

    [RequireComponent(typeof(ParticleSystem))]
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class DragonBreathInvolver : CombatInvolver
    {
        [HideInInspector]
        public ParticleSystem breathEffect;

        private void Start()
        {
            breathEffect = GetComponent<ParticleSystem>();
        }

        private void OnParticleCollision(GameObject other)
        {
            if (!Utilities.IsValid(other)) { return; }

        }

        public override void Generate()
        {
            breathEffect.Emit(1);
        }

        public override void Involve(CombatUnit unit)
        {

        }
    }
}
