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
        private int[] sync_hitUnitsId = new int[MaxHitCount];

        private CombatGameMaster _gameMaster;
        private CombatUnit[] _hitUnitQueue = new CombatUnit[MaxHitCount];

        private void Start()
        {
            _gameMaster = GameObject.Find(_gameMasterObjectName).GetComponent<CombatGameMaster>();
        }

        public override void OnPostSerialization(SerializationResult result)
        {
            base.OnPostSerialization(result);

            if (sync_flagSkillHit)
            {
                sync_flagSkillHit = false;
                for (int i = 0; i < sync_hitUnitsId.Length; i++)
                {
                    sync_hitUnitsId[i] = 0;
                }
                RequestSerialization();
            }
        }

        public override void OnDeserialization()
        {
            base.OnDeserialization();

            if (sync_flagSkillHit)
            {
                for (int i = 0; i < sync_hitUnitsId.Length; i++)
                {
                    var unit = _gameMaster.GetUnitById(sync_hitUnitsId[i]);
                    if (unit) { unit.OnSkillHit(this); }
                    sync_hitUnitsId[i] = 0;
                }

                sync_flagSkillHit = false;
            }
        }

        public override void OnUnitHit(CombatUnit unit)
        {
            unit.OnSkillHit(this);

            sync_flagSkillHit = true;
            for (int i = 0; i < sync_hitUnitsId.Length; i++)
            {
                if (sync_hitUnitsId[i] > 0) { continue; }

                sync_hitUnitsId[i] = unit.unitId;
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
            _involver.Generate();
        }

        protected override void ToggleWarning(bool isOn)
        {
            if (isOn) _chargeEffect.SetActive(true);
        }
    }
}
