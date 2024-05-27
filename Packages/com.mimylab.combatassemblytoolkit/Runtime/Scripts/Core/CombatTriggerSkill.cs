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

    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class CombatTriggerSkill : CombatSkill
    {
        [Header("General Settings")]
        [SerializeField, Min(0.5f), Tooltip("sec")]
        private float _coolDownTime = 3.0f;
        [SerializeField, Min(0.0f), Tooltip("sec")]
        private float _warmUpTime = 0.0f;

        [UdonSynced]
        private bool sync_flagDoAction;
        private bool _doAction;

        private float _remainingCoolDownTime = 0.0f;
        private float _remainingWarmUpTime = 0.0f;

        public float RemainingCoolDownTime { get => _remainingCoolDownTime; }
        public bool IsCooling { get => _remainingCoolDownTime > 0.0f; }
        public float RemainingWarmUpTime { get => _remainingWarmUpTime; }

        [UdonSynced, FieldChangeCallback(nameof(IsWarming))]
        private bool _isWarming;
        public bool IsWarming
        {
            get => _isWarming;
            set
            {
                if (_isWarming == value) { return; }

                ToggleWarning(value);

                _isWarming = value;
                RequestSerialization();
            }
        }

        public override void OnPostSerialization(SerializationResult result)
        {
            if (sync_flagDoAction)
            {
                sync_flagDoAction = false;
                RequestSerialization();
            }
        }

        public override void OnDeserialization()
        {
            if (sync_flagDoAction)
            {
                DoAction();
                sync_flagDoAction = false;
            }
        }

        public override void Action()
        {
            if (IsCooling) { return; }
            if (IsWarming) { return; }
            if (!Networking.LocalPlayer.IsOwner(this.gameObject)) { return; }

            StartWarmUp();
        }

        public override void Cancel()
        {
            IsWarming = false;
        }

        // 1回実行するスキルをここにオーバーライド
        protected virtual void TriggerAction() { }

        // 詠唱開始/中断・完了時の処理をここにオーバーライド
        protected virtual void ToggleWarning(bool isOn) { }

        private void StartCoolDown()
        {
            if (_coolDownTime > 0.0f)
            {
                _remainingCoolDownTime = _coolDownTime;
                SendCustomEventDelayedFrames(nameof(_CoolingCountdown), 1);
                return;
            }
        }
        public void _CoolingCountdown()
        {
            _remainingCoolDownTime = Mathf.Max(_remainingCoolDownTime - Time.deltaTime, 0.0f);

            if (IsCooling) { SendCustomEventDelayedFrames(nameof(_CoolingCountdown), 1); }
        }

        private void StartWarmUp()
        {
            if (_warmUpTime > 0.0f)
            {
                IsWarming = true;
                _remainingWarmUpTime = _warmUpTime;
                SendCustomEventDelayedFrames(nameof(_WarmingCountdown), 1);
                return;
            }

            DoAction();
            RequestSerialization();
        }
        public void _WarmingCountdown()
        {
            if (!IsWarming) { return; }

            _remainingWarmUpTime = Mathf.Max(_remainingWarmUpTime - Time.deltaTime, 0.0f);

            if (_remainingWarmUpTime > 0.0f)
            {
                SendCustomEventDelayedFrames(nameof(_WarmingCountdown), 1);
                return;
            }

            IsWarming = false;
            DoAction();
            RequestSerialization();
        }

        private void DoAction()
        {
            sync_flagDoAction = true;

            StartCoolDown();
            TriggerAction();
        }
    }
}
