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
    using VRC.SDK3.Components;

    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class BattleRoyalGameMaster : CombatGameMaster
    {
        private bool _initialized = false;
        private void Initialize()
        {
            if (_initialized) { return; }



            _initialized = true;
        }
        private void Start()
        {
            Initialize();


        }

        public override void GameStart() { }

        public override void GameOver() { }

        public override void Judge() { }

        public override CombatUnit GetUnitById(int id)
        {
            if (id <= 0) { return null; }

            for (int i = 0; i < units.Length; i++)
            {
                if (units[i] && units[i].unitId == id)
                {
                    return units[i];
                }
            }

            return null;
        }
    }
}
