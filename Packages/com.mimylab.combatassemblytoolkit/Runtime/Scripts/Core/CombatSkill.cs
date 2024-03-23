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
    public class CombatSkill : UdonSharpBehaviour
    {
        public CombatUnit holder;

        [SerializeField, Tooltip("X : HP, Y : MP, Z : SP")]
        private Vector3Int _cost;
        [SerializeField, Tooltip("X : HP, Y : MP, Z : SP")]
        private Vector3Int _power;
        [SerializeField, Min(0.0f), Tooltip("sec")]
        private float _coolDownTime = 1.0f;
        [SerializeField, Min(0.0f), Tooltip("sec")]
        private float _warmUpTime = 0.0f;

        private float _remainingCoolDownTime = 0.0f;
        private float _remainingWarmUpTime = 0.0f;

        public virtual Vector3Int Cost { get => _cost; }
        public virtual Vector3Int Power { get => _power; }
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

        [UdonSynced, FieldChangeCallback(nameof(InAction))]
        private bool _inAction;
        public bool InAction
        {
            get => _inAction;
            set
            {
                if (_inAction == value) { return; }

                if (value) { StartCoolDown(); }
                ToggleAction(value);

                _inAction = value;
                RequestSerialization();
            }
        }

        [UdonSynced, FieldChangeCallback(nameof(OnAction))]
        private bool _onAction;
        private bool OnAction
        {
            get => _onAction;
            set
            {
                if (_onAction == value) { return; }

                if (value)
                {
                    StartCoolDown();
                    SingleAction();
                }

                _onAction = value;
                RequestSerialization();
            }
        }

        public override void OnPostSerialization(SerializationResult result)
        {
            OnAction = false;
        }

        public override void OnDeserialization()
        {
            OnAction = false;
        }

        // スキルを1回実行
        public void ShotAction()
        {
            if (IsCooling) { return; }
            if (IsWarming) { return; }

            WarmUpSingleAction();
        }

        // 持続的なスキルを実行
        public void StartAction()
        {
            if (IsCooling) { return; }
            if (IsWarming) { return; }

            WarmUpToggleAction();
        }
        public void StopAction()
        {
            InAction = false;
        }

        // スキル詠唱の中断
        public void CancelAction()
        {
            IsWarming = false;
        }

        // 着弾時の処理をここにオーバーライド
        public virtual void OnUnitHit(CombatUnit unit) { }

        // 被弾時に実行するスキルをここにオーバーライド
        public virtual Vector3Int Reaction(Vector3Int effect) { return effect; }

        // 1回実行するスキルをここにオーバーライド
        protected virtual void SingleAction() { }

        // 持続実行するスキルをここにオーバーライド
        protected virtual void ToggleAction(bool isOn) { }

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

        private void WarmUpSingleAction()
        {
            if (_warmUpTime > 0.0f)
            {
                IsWarming = true;
                _remainingWarmUpTime = _warmUpTime;
                SendCustomEventDelayedFrames(nameof(_WarmingCountdownSingle), 1);
                return;
            }

            OnAction = true;
        }
        public void _WarmingCountdownSingle()
        {
            if (!IsWarming) { return; }

            _remainingWarmUpTime = Mathf.Max(_remainingWarmUpTime - Time.deltaTime, 0.0f);

            if (_remainingWarmUpTime > 0.0f) { SendCustomEventDelayedFrames(nameof(_WarmingCountdownSingle), 1); }
            else { OnAction = true; }
        }

        private void WarmUpToggleAction()
        {
            if (_warmUpTime > 0.0f)
            {
                IsWarming = true;
                _remainingWarmUpTime = _warmUpTime;
                SendCustomEventDelayedFrames(nameof(_WarmingCountdownToggle), 1);
                return;
            }

            InAction = true;
        }
        public void _WarmingCountdownToggle()
        {
            if (!IsWarming) { return; }

            _remainingWarmUpTime = Mathf.Max(_remainingWarmUpTime - Time.deltaTime, 0.0f);

            if (_remainingWarmUpTime > 0.0f) { SendCustomEventDelayedFrames(nameof(_WarmingCountdownToggle), 1); }
            else { InAction = true; }
        }
    }
}
