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

    public abstract class CombatGameMaster : UdonSharpBehaviour
    {
        public CombatUnit[] units = new CombatUnit[0];

        public abstract void GameStart();
        public abstract void GameOver();
        public abstract void Judge();
        public abstract CombatUnit GetUnitById(int id);
    }
}
