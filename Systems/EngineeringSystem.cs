using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uncontrolled.Entities;
using UnityEngine;

namespace Uncontrolled.Systems
{
	public class EngineeringSystem : ShipSystemBase
	{
		private PLEngineeringSystem EngSys;
		public int ReactorScreenDisabled = 0;
		public int ShieldPowerDisabled = 0;

		public EngineeringSystem(UnSystem system) : base(system)
		{
			EngSys = system.MyShip.EngineeringSystem;
		}

		public override Vector3 Position => EngSys._transform.position;

		public override void Register(IEntity entity)
		{
			var id = UnityEngine.Random.Range(0, 1);
			GetState(id)++;
			entity.Data = id;
		}

		public override void Unregister(IEntity entity)
		{
			GetState(entity.Data)--;
		}

		private ref int GetState(long state)
		{
			switch(state)
			{
				case 0: return ref ReactorScreenDisabled;
				case 1: return ref ShieldPowerDisabled;
			}

			throw new ArgumentException();
		}
	}

	[HarmonyPatch]
	class EngSysPatches
	{
		[HarmonyPrefix]
		[HarmonyPatch(typeof(PLEngineerReactorScreen), "OnEditValue")]
		static bool ChangeReactorBar(PLEngineerReactorScreen __instance) 
			=> UnSystem.Get(__instance.MyScreenHubBase.OptionalShipInfo)?.EngSys.ReactorScreenDisabled == 0;

		[HarmonyPrefix]
		[HarmonyPatch(typeof(PLShieldGenerator), "Tick")]
		static void NoPowerForShields(PLShieldGenerator __instance)
		{
			var sys = UnSystem.Get(__instance.ShipStats.Ship as PLShipInfo);
			if (sys == null) return;
			if (sys.EngSys.ShieldPowerDisabled == 0) return;
			__instance.m_InputPower_Watts = 0;
		}
	}
}
