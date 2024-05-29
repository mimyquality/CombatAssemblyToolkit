/*
Copyright (c) 2024 Mimy Quality
Released under the MIT license
https://opensource.org/licenses/mit-license.php
*/

namespace MimyLab.CombatAssemblyToolit
{
    using System;
    using UdonSharp;
    using UnityEngine;
    using VRC.SDKBase;
    using VRC.Udon;

    [Flags]
    public enum CombatElementFlag
    {
        None,
        Fire = 1 << 0,
        Earth = 1 << 1,
        Water = 1 << 2,
        Air = 1 << 3,
    }

    public enum DragonExistenceOperationType
    {
        RightHandUseDown,
        RightHandUseUp,
        LeftHandUseDown,
        LeftHandUseUp,
    }

    [DefaultExecutionOrder(-100)]
    [RequireComponent(typeof(DragonCommand))]
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class DragonExistence : CombatUnit
    {
        //[Header("Dragon Settings")]

        private Collider[] _colliders = new Collider[0];
        private DragonCommand _command;

        protected override void Start()
        {
            _colliders = GetComponentsInChildren<Collider>();
            _command = GetComponent<DragonCommand>();
            _command.enabled = false;
            base.Start();
            _life.Initialize((int)DragonHeartResourceType.Count, (int)DragonHeartConditionType.Count, 0);
        }

        public override void OnResourceChanged(int index, int newValue)
        {

        }

        public override void OnConditionChanged(int index, int newValue)
        {

        }

        public void _OnLocalPlayerRide()
        {
            SetUnitOwner(Networking.LocalPlayer);
            // 戦闘状況を開始

            // Triggerコマンド受付
            _command.enabled = true;
        }

        public void _OnLocalPlayerExit()
        {
            // 戦闘状況を終了

            // Triggerコマンド停止
            _command.enabled = false;
        }

        public override void Operation(int index)
        {
            switch ((DragonExistenceOperationType)index)
            {
                case DragonExistenceOperationType.LeftHandUseDown: _holdSkills[0].Action(); break;
                case DragonExistenceOperationType.LeftHandUseUp: _holdSkills[0].Cancel(); break;
                case DragonExistenceOperationType.RightHandUseDown: _holdSkills[1].Action(); break;
            }
        }

        public void _SetHitBoxLayer(int index)
        {
            for (int i = 0; i < _colliders.Length; i++)
            {
                _colliders[i].gameObject.layer = index;
            }
        }
    }
}
