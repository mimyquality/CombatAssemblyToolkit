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

    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class CombatLife : UdonSharpBehaviour
    {
        [SerializeField]
        private int _maxHP = 10000;
        [SerializeField]
        private int _maxMP = 10000;
        [SerializeField]
        private int _maxSP = 10000;

        [UdonSynced, FieldChangeCallback(nameof(IsDead))]
        private bool _isDead = false;
        [UdonSynced, FieldChangeCallback(nameof(HP))]
        private int _hp;
        [UdonSynced, FieldChangeCallback(nameof(MP))]
        private int _mp;
        [UdonSynced, FieldChangeCallback(nameof(SP))]
        private int _sp;
        [UdonSynced]
        private int _buff;
        [UdonSynced]
        private int _debuff;

        public int MaxHP { get => _maxHP; }
        public int MaxMP { get => _maxMP; }
        public int MaxSP { get => _maxSP; }

        public bool IsDead
        {
            get => _isDead;
            set
            {
                Initialize();

                if (value) { _hp = 0; }
                _isDead = value;
                RequestSerialization();
            }
        }
        public int HP
        {
            get => _hp;
            set
            {
                _hp = Mathf.Clamp(value, 0, _maxHP);
                RequestSerialization();
            }
        }
        public int MP
        {
            get => _mp;
            set
            {
                _mp = Mathf.Clamp(value, 0, _maxMP);
                RequestSerialization();
            }
        }
        public int SP
        {
            get => _sp;
            set
            {
                _sp = Mathf.Clamp(value, 0, _maxSP);
                RequestSerialization();
            }
        }
        public CombatUnitBuff Buff { get => (CombatUnitBuff)_buff; }
        public CombatUnitDebuff Debuff { get => (CombatUnitDebuff)_debuff; }

        protected bool _initialized = false;
        protected virtual void Initialize()
        {
            if (_initialized) { return; }

            _hp = _maxHP;
            _mp = _maxMP;
            _sp = _maxSP;

            _initialized = true;
        }
        private void Start()
        {
            Initialize();
        }

        /******************************
         リソース関係の処理
         ******************************/
        public bool IsFatal(int damage)
        {
            Initialize();

            return HP + damage < 1;
        }

        public bool IsInsufficient(int cost)
        {
            Initialize();

            return MP + cost < 0;
        }

        public bool IsCrack(int brisance)
        {
            Initialize();

            return SP + brisance < 1;
        }

        public bool HasResource(int damage, int cost, int brisance)
        {
            return !(IsFatal(damage) || IsInsufficient(cost) || IsCrack(brisance));
        }
        public bool HasResource(Vector3Int amount)
        {
            return HasResource(amount.x, amount.y, amount.z);
        }

        public void Consume(int damage, int cost, int brisance)
        {
            Initialize();

            HP += damage;
            MP += cost;
            SP += brisance;
        }
        public void Consume(Vector3Int amount)
        {
            Consume(amount.x, amount.y, amount.z);
        }


        /******************************
         バフ関係の処理
         ******************************/
        public bool HasBuff(CombatUnitBuff effect)
        {
            return _buff == (_buff | (int)effect);
        }

        public void AddBuff(CombatUnitBuff effect)
        {
            _buff |= (int)effect;
            RequestSerialization();
        }

        public void ClearBuff(CombatUnitBuff effect)
        {
            _buff &= ~(int)effect;
            RequestSerialization();
        }

        public void ClearBuffAll()
        {
            _buff = 0;
            RequestSerialization();
        }

        /******************************
         デバフ関係の処理
         ******************************/
        public bool HasDebuff(CombatUnitDebuff effect)
        {
            return _debuff == (_debuff | (int)effect);
        }

        public void AddDebuff(CombatUnitDebuff effect)
        {
            _debuff |= (int)effect;
            RequestSerialization();
        }

        public void ClearDebuff(CombatUnitDebuff effect)
        {
            _debuff &= ~(int)effect;
            RequestSerialization();
        }

        public void ClearDebuffAll()
        {
            _debuff = 0;
            RequestSerialization();
        }
    }
}
