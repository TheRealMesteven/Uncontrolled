using PulsarModLoader;
using PulsarModLoader.Chat.Commands.CommandRouter;
using PulsarModLoader.MPModChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Uncontrolled
{
    public class Mod : PulsarMod
    {
        public override string HarmonyIdentifier() => "BadRyuner.Uncontrolled";
        public override string Author => "BadRyuner";
        public override string Version => "1.0";
        public override string Name => "Uncontrolled";
        public override int MPRequirements => (int)MPRequirement.All;

        public Mod() => MyPrefabs.Init();
    }
    public class CreateRepairableDamage : ChatCommand
    {
        public override string[] CommandAliases() => new string[] { "crd" };
        public override string Description() => "Creates a repairable damage prefab";
        public string UsageExample() => $"/{this.CommandAliases()[0]}";
        public override void Execute(string arguments)
        {
            PLShipInfo ship = PLNetworkManager.Instance.LocalPlayer.GetPawn().CurrentShip;
            Uncontrolled.Entities.EntityGenerator.Spawn(ship, MyPrefabs.BurnedWiresPrefab_Name);
        }
    }

}