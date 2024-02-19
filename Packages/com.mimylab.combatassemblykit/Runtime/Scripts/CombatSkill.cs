/*
Copyright (c) 2024 Mimy Quality
Released under the MIT license
https://opensource.org/licenses/mit-license.php
*/

namespace MimyLab.CombatAssemblyKit
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

        public virtual Vector3Int Cost { get => _cost; }
        public virtual Vector3Int Power { get => _power; }

        [UdonSynced, FieldChangeCallback(nameof(InAction))]
        private bool _inAction;
        protected bool InAction
        {
            get => _inAction;
            set
            {
                if (_inAction == value) { return; }

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

                if (value) { SingleAction(); }
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
        public void Action() { OnAction = true; }

        // 持続的なスキルを実行
        public void Action(bool isOn) { InAction = isOn; }
        public void StartAction() { Action(true); }
        public void StopAction() { Action(false); }

        // 被弾時に実行するスキルをここにオーバーライド
        public virtual Vector3Int Reaction(Vector3Int effect) { return effect; }

        // 着弾時の処理をここにオーバーライド
        public virtual void OnUnitHit(CombatUnit unit) { }

        // 1回実行するスキルをここにオーバーライド
        protected virtual void SingleAction() { }

        // 持続実行するスキルをここにオーバーライド
        protected virtual void ToggleAction(bool isOn) { }

    }
}
