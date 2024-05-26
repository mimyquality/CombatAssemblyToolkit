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

    public abstract class CombatInvolver : UdonSharpBehaviour
    {
        public abstract void Generate();
        public abstract void Involve(CombatUnit unit);
    }
}
