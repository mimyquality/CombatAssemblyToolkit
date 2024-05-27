/*
Copyright (c) 2024 Mimy Quality
Released under the MIT license
https://opensource.org/licenses/mit-license.php
*/

namespace MimyLab.CombatAssemblyToolit
{
    using UdonSharp;
    using UnityEngine;
    //using VRC.SDKBase;
    //using VRC.Udon;

    public abstract class CombatSkill : UdonSharpBehaviour
    {
        [HideInInspector]
        public CombatUnit holder;

        public abstract void Action();
        public abstract void Cancel();
    }
}
