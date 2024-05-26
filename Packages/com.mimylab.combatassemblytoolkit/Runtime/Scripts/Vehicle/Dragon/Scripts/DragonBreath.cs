
namespace MimyLab
{
    using UdonSharp;
    using UnityEngine;
    using VRC.SDKBase;
    using VRC.Udon;
    using CombatAssemblyToolit;

    public class DragonBreath : CombatTriggerSkill
    {
        [SerializeField]
        private DragonBreathInvolver _involver;

        [SerializeField]
        private GameObject _chargeEffect;
        [SerializeField]
        private float _ownerDelayTime = 0.5f;

        [UdonSynced]
        private Vector3 sync_involvePosition;
        [UdonSynced]
        private Quaternion sync_involveRotation;

        [UdonSynced]
        private bool sync_flagSkillHit;

        public void _GenerateInvolver()
        {
            _involver.gameObject.SetActive(true);
            _involver.Generate();
        }

        protected override void TriggerAction()
        {
            if (Networking.IsOwner(this.gameObject))
            {
                _chargeEffect.SetActive(true);
                SendCustomEventDelayedSeconds(nameof(_GenerateInvolver), _ownerDelayTime);

                sync_involvePosition = _involver.transform.position;
                sync_involveRotation = _involver.transform.rotation;
            }
            else
            {
                _involver.transform.SetPositionAndRotation(sync_involvePosition, sync_involveRotation);
                _GenerateInvolver();
            }
        }

        protected override void ToggleWarning(bool isOn) { }
    }
}
