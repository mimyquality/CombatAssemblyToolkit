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
    using VRC.Udon.Common;

    public class DragonBreath : CombatTriggerSkill
    {
        private const int MaxHitCount = 5;

        [SerializeField]
        private string _gameMasterObjectName = "GameMaster";

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
        [UdonSynced]
        private int[] sync_hitUnitQueue = new int[MaxHitCount];

        private CombatGameMaster _gameMaster;
        private int[] _hitUnitQueue = new int[MaxHitCount];

        private void Start()
        {
            _gameMaster = GameObject.Find(_gameMasterObjectName).GetComponent<CombatGameMaster>();
            _involver.gamemaster = _gameMaster;
            _involver.skill = this;
        }

        public override void OnPreSerialization()
        {
            base.OnPreSerialization();

            for (int i = 0; i < _hitUnitQueue.Length; i++)
            {
                if (_hitUnitQueue[i] > 0)
                {
                    for (int j = 0; j < sync_hitUnitQueue.Length; j++)
                    {
                        if (sync_hitUnitQueue[j] > 0) { continue; }

                        sync_hitUnitQueue[j] = _hitUnitQueue[i];
                        _hitUnitQueue[i] = 0;
                        sync_flagSkillHit = true;
                        break;
                    }
                }
            }
        }

        public override void OnPostSerialization(SerializationResult result)
        {
            base.OnPostSerialization(result);

            if (sync_flagSkillHit)
            {
                for (int i = 0; i < sync_hitUnitQueue.Length; i++)
                {
                    sync_hitUnitQueue[i] = 0;
                }

                sync_flagSkillHit = false;
                RequestSerialization();
            }
        }

        public override void OnDeserialization()
        {
            base.OnDeserialization();

            if (sync_flagSkillHit)
            {
                for (int i = 0; i < sync_hitUnitQueue.Length; i++)
                {
                    var unit = _gameMaster.GetUnitById(sync_hitUnitQueue[i]);
                    if (unit) { unit.OnSkillHit(this); }
                    sync_hitUnitQueue[i] = 0;
                }

                sync_flagSkillHit = false;
            }
        }

        public override void OnUnitHit(CombatUnit unit)
        {
            if (!Networking.IsOwner(this.gameObject)) { return; }

            unit.OnSkillHit(this);

            for (int i = 0; i < _hitUnitQueue.Length; i++)
            {
                if (_hitUnitQueue[i] > 0) { continue; }

                _hitUnitQueue[i] = unit.unitId;
                break;
            }
            RequestSerialization();
        }

        protected override void TriggerAction()
        {
            if (Networking.IsOwner(this.gameObject))
            {
                sync_involvePosition = _involver.transform.position;
                sync_involveRotation = _involver.transform.rotation;
            }
            else
            {
                _involver.transform.SetPositionAndRotation(sync_involvePosition, sync_involveRotation);
            }

            _involver.gameObject.SetActive(true);
            _involver.Involve();
        }

        protected override void ToggleWarning(bool isOn)
        {
            if (isOn) _chargeEffect.SetActive(true);
        }
    }
}
