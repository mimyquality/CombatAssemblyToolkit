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
    using VRC.Udon.Common;

    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class DragonCommand : UdonSharpBehaviour
    {
        private DragonExistence unit;

        private void Start()
        {
            unit = GetComponent<DragonExistence>();
        }

        public override void InputUse(bool value, UdonInputEventArgs args)
        {
            if (!unit) { return; }

            if (value)
            {
                switch (args.handType)
                {
                    case HandType.RIGHT: unit.Operation((int)DragonExistenceOperationType.RightHandUseDown); break;
                    case HandType.LEFT: unit.Operation((int)DragonExistenceOperationType.LeftHandUseDown); break;
                }
            }
            else
            {
                switch (args.handType)
                {
                    case HandType.RIGHT: unit.Operation((int)DragonExistenceOperationType.RightHandUseUp); break;
                    case HandType.LEFT: unit.Operation((int)DragonExistenceOperationType.LeftHandUseUp); break;
                }
            }
        }
    }
}
