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
        [SerializeField]
        private DragonBreath _dragonBreath;
        private ParticleSystem _breathEffect;

        private void Start()
        {
            _breathEffect = GetComponent<ParticleSystem>();
        }

        private void OnParticleCollision(GameObject other)
        {
            if (!Networking.IsOwner(_dragonBreath.gameObject)) { return; }
            if (!Utilities.IsValid(other)) { return; }

            var unit = other.GetComponent<CombatUnit>();
            if (unit) { _dragonBreath.OnUnitHit(unit); }
        }

        public override void Generate()
        {
            _breathEffect.Emit(1);
        }

        public override void Involve(CombatUnit unit)
        {

        }
    }
}
