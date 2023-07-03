using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uncontrolled.Entities;
using UnityEngine;

namespace Uncontrolled.Systems
{
	public class ScienceSystem : ShipSystemBase
	{
		private PLComputerSystem SciSys;
        public int PositiveShieldBoost = 0;
        public int NegativeShieldBoost = 0;

        public ScienceSystem(UnSystem system) : base(system)
		{
			SciSys = system.MyShip.ComputerSystem;
		}

		public override Vector3 Position => SciSys._transform.position;

		public override void Register(IEntity entity)
		{
            var id = UnityEngine.Random.Range(0, 2);
            GetState(id)++;
            entity.Data = id;
            PulsarModLoader.Utilities.Messaging.Echo(PhotonTargets.All, "Science Damage : " + (int)id);
        }

		public override void Unregister(IEntity entity)
		{
            GetState(entity.Data)--;
        }

        private ref int GetState(long state)
        {
            switch (state)
            {
                case 0: return ref PositiveShieldBoost;
                case 1: return ref NegativeShieldBoost;
            }

            throw new ArgumentException();
        }
    }
    [HarmonyPatch]
    class SciSysPatches
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PLGlobal), "GetShieldFreqPosAmt")]
        static int LessPositiveShieldBoost(int __result, PLShipInfoBase ship)
        {
            var sys = UnSystem.Get(ship as PLShipInfo);
            if (sys == null) return __result;
            if (sys.SciSys.PositiveShieldBoost == 0) return __result;
            __result -= sys.SciSys.PositiveShieldBoost * 5;
            return __result;
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PLGlobal), "GetShieldFreqNegAmt")]
        static int MoreNegativeShieldBoost(int __result, PLShipInfoBase ship)
        {
            var sys = UnSystem.Get(ship as PLShipInfo);
            if (sys == null) return __result;
            if (sys.SciSys.NegativeShieldBoost == 0) return __result;
            __result += sys.SciSys.NegativeShieldBoost * 5;
            return __result;
        }
    }
}
