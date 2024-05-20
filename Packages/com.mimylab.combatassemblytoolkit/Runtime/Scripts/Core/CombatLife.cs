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
    //using VRC.Udon;

    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class CombatLife : UdonSharpBehaviour
    {
        [HideInInspector]
        public CombatUnit unit;

        [UdonSynced] private int[] sync_resources = new int[0];
        [UdonSynced] private int[] sync_conditions = new int[0];
        [UdonSynced] private float[] sync_abilities = new float[0];

        private int[] _resources = new int[0];
        private int[] _conditions = new int[0];
        private float[] _abilities = new float[0];

        private bool _initialized = false;
        public void Initialize(int resourceCount, int conditionCount, int abilityCount)
        {
            if (_initialized) { return; }

            _resources = new int[resourceCount];
            _conditions = new int[conditionCount];
            _abilities = new float[abilityCount];

            sync_resources = new int[resourceCount];
            sync_conditions = new int[conditionCount];
            sync_abilities = new float[abilityCount];

            _initialized = true;
        }

        public override void OnPreSerialization()
        {
            if (_resources.Length > 0)
            {
                Array.Copy(_resources, sync_resources, sync_resources.Length);
            }

            if (_conditions.Length > 0)
            {
                Array.Copy(_conditions, sync_conditions, sync_conditions.Length);
            }

            if (_abilities.Length > 0)
            {
                Array.Copy(_abilities, sync_abilities, sync_abilities.Length);
            }
        }

        public override void OnDeserialization()
        {
            for (int i = 0; i < _resources.Length; i++)
            {
                if (_resources[i] != sync_resources[i])
                {
                    unit.OnResourceChanged(i, sync_resources[i]);
                    _resources[i] = sync_resources[i];
                }
            }
            for (int i = 0; i < _conditions.Length; i++)
            {
                if (_conditions[i] != sync_conditions[i])
                {
                    unit.OnConditionChanged(i, sync_conditions[i]);
                    _conditions[i] = sync_conditions[i];
                }
            }
            for (int i = 0; i < _abilities.Length; i++)
            {
                if (_abilities[i] != sync_abilities[i])
                {
                    unit.OnAbilityChanged(i, sync_abilities[i]);
                    _abilities[i] = sync_abilities[i];
                }
            }
        }

        /******************************
         リソース関係の処理
         ******************************/
        public int GetResourcesCount()
        {
            return _resources.Length;
        }

        public int GetResource(int index)
        {
            return _resources[index];
        }

        public void SetResource(int value, int index)
        {
            if (!Networking.LocalPlayer.IsOwner(this.gameObject)) { return; }

            if (_resources[index] != value)
            {
                unit.OnResourceChanged(index, value);

                _resources[index] = value;
                RequestSerialization();
            }
        }

        public bool IsFatal(int damage, int index)
        {
            return _resources[index] <= damage;
        }

        public bool IsDeficient(int cost, int index)
        {
            return _resources[index] < cost;
        }

        /******************************
         状態関係の処理
         ******************************/
        public int GetConditionsCount()
        {
            return _conditions.Length;
        }

        public int GetCondition(int index)
        {
            return _conditions[index];
        }

        public void SetCondition(int value, int index)
        {
            if (!Networking.LocalPlayer.IsOwner(this.gameObject)) { return; }

            if (_conditions[index] != value)
            {
                unit.OnConditionChanged(index, value);

                _conditions[index] = value;
                RequestSerialization();
            }
        }

        public bool HasCondition(int effect, int index)
        {
            return _conditions[index] == (_conditions[index] | effect);
        }

        public void AddCondition(int effect, int index)
        {
            SetCondition(_conditions[index] | effect, index);
        }

        public void ClearBuff(int effect, int index)
        {
            SetCondition(_conditions[index] & ~effect, index);
        }

        /******************************
         能力値の処理
         ******************************/
        public int GetAbilitiesCount()
        {
            return _abilities.Length;
        }

        public float GetAbility(int index)
        {
            return _abilities[index];
        }

        public void SetAbility(float value, int index)
        {
            _abilities[index] = value;
            if (!Networking.LocalPlayer.IsOwner(this.gameObject)) { return; }

            if (_abilities[index] != value)
            {
                unit.OnAbilityChanged(index, value);

                _abilities[index] = value;
                RequestSerialization();
            }
        }
    }
}
