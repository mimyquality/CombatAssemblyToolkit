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
    //using VRC.Udon;

    public abstract class CombatUnit : UdonSharpBehaviour
    {
        [HideInInspector]
        public int unitId;

        [Header("General Settings")]
        [SerializeField]
        protected CombatLife _life;
        [SerializeField]
        protected CombatSkill[] _holdSkills = new CombatSkill[0];

        private bool _initialized = false;
        private void Initialize()
        {
            if (_initialized) { return; }

            if (_life) { _life.unit = this; }
            for (int i = 0; i < _holdSkills.Length; i++)
            {
                if (_holdSkills[i]) { _holdSkills[i].holder = this; }
            }

            _initialized = true;
        }
        protected virtual void Start()
        {
            Initialize();
        }

        /******************************
         CombatUnitのイベント
        ******************************/
        public virtual void OnUnitBirth() { }
        public virtual void OnUnitDead() { }

        /******************************
         CombatLifeからのイベント
        ******************************/
        public virtual void OnResourceChanged(int index, int newValue) { }
        public virtual void OnConditionChanged(int index, int newValue) { }
        public virtual void OnAbilityChanged(int index, float newValue) { }

        /******************************
         CombatSkillからのイベント
        ******************************/
        public virtual void OnSkillHit(CombatSkill hitSkill) { }

        /******************************
         publicメソッド
        ******************************/
        public virtual bool SetUnitOwner(VRCPlayerApi player)
        {
            if (!Utilities.IsValid(player)) { return false; }

            if (_life) { Networking.SetOwner(player, _life.gameObject); }
            for (int i = 0; i < _holdSkills.Length; i++)
            {
                if (_holdSkills[i])
                {
                    Networking.SetOwner(player, _holdSkills[i].gameObject);
                }
            }

            return true;
        }

        public virtual void SetSkill(CombatSkill skill, int index) { }
        public abstract void Operation(int index);
    }
}
