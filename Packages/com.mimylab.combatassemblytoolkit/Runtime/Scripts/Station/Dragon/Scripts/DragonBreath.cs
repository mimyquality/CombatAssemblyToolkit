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

    public class DragonBreath : CombatTriggerSkill
    {
        [Header("Dragon Settings")]
        [SerializeField]
        private DragonBreathInvolver _involver;

        [SerializeField]
        private GameObject _chargeEffect;

        [UdonSynced]
        private Vector3 sync_involvePosition;
        [UdonSynced]
        private Quaternion sync_involveRotation;

        [UdonSynced]
        private bool sync_flagSkillHit;

        protected override void TriggerAction()
        {
            if (Networking.IsOwner(this.gameObject))
            {
                sync_involvePosition = _involver.transform.position;
                sync_involveRotation = _involver.transform.rotation;

                var collisionModule = _involver.breathEffect.collision;
                collisionModule.collidesWith = collisionModule.collidesWith & ~(1 << 23);
            }
            else
            {
                _involver.transform.SetPositionAndRotation(sync_involvePosition, sync_involveRotation);

                var collisionModule = _involver.breathEffect.collision;
                collisionModule.collidesWith = collisionModule.collidesWith | (1 << 23);
            }

            _involver.gameObject.SetActive(true);
            _involver.Generate();
        }

        protected override void ToggleWarning(bool isOn)
        {
            if (isOn) _chargeEffect.SetActive(true);
        }
    }
}
