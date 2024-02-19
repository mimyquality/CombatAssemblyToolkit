/*
Copyright (c) 2024 Mimy Quality
Released under the MIT license
https://opensource.org/licenses/mit-license.php
*/

namespace MimyLab.CombatAssemblyKit
{
    using System;
    using UdonSharp;
    using UnityEngine;
    using VRC.SDKBase;
    using VRC.Udon;
    using VRC.SDK3.Components;

    [Flags]
    public enum CombatElement
    {
        None = 0,
        Fire = 1 << 0,
        Earth = 1 << 1,
        Water = 1 << 2,
        Air = 1 << 3,
    }
    [Flags]
    public enum CombatUnitBuff
    {
        None = 0,
        Dummy = 1 << 0,
    }
    [Flags]
    public enum CombatUnitDebuff
    {
        None = 0,
        Dummy = 1 << 0,
    }

    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class CombatUnit : UdonSharpBehaviour
    {
        public CombatLife life;
        [SerializeField]
        protected CombatSkill[] holdSkills = new CombatSkill[0];

        protected bool _initialized = false;
        protected virtual void Initialize()
        {
            if (_initialized) { return; }

            for (int i = 0; i < holdSkills.Length; i++)
            {
                if (holdSkills[i]) { holdSkills[i].holder = this; }
            }

            _initialized = true;
        }
        private void Start()
        {
            Initialize();
        }

        public virtual void Action(int index)
        {
            if (index >= holdSkills.Length) { return; }
            if (!holdSkills[index]) { return; }

            if (life.IsDead) { return; }
            var cost = holdSkills[index].Cost;
            if (!life.HasResource(cost)) { return; }

            life.Consume(cost);
            holdSkills[index].Action();
        }

        public virtual void OnSkillHit(CombatSkill hitSkill)
        {
            var damage = hitSkill.Power;
            for (int i = 0; i < holdSkills.Length; i++)
            {
                if (holdSkills[i])
                {
                    damage = holdSkills[i].Reaction(damage);
                }
            }

            // SPをシールド値として扱うならここで換算？

            // 最終的に通るダメージで死亡判定
            if (life.IsDead = life.IsFatal(damage.x))
            {
                DeadAction();
                return;
            }
            life.Consume(damage);
        }

        public virtual void DeadAction() { }
    }
}
