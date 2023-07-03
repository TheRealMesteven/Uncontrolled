using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uncontrolled.Entities;
using Uncontrolled.Systems;
using UnityEngine;

namespace Uncontrolled
{
	public class UnSystem
	{
		private static List<UnSystem> Active = new List<UnSystem>(4);

		public static UnSystem Get(PLShipInfo target)
		{
			
			if (target == null) return null;
			if (target.GetIsPlayerShip())
			{
				var sys = Active.FirstOrDefault(s => s.MyShip == target);
				if (sys == null)
				{
					sys = new UnSystem(target);
					Active.Add(sys);
				}
				return sys;
			}
			return null; // Unsupported
		}

		internal static void Free(PLShipInfo target)
		{
			var sys = Active.FirstOrDefault(s => s.MyShip == target);
			if (sys != null)
				Active.Remove(sys);
		}

		private UnSystem(PLShipInfo target) 
		{ 
			MyShip = target;
			SciSys = new ScienceSystem(this);
			EngSys = new EngineeringSystem(this);
			AllSystems.Add(SciSys);
			AllSystems.Add(EngSys);
		}

		public readonly PLShipInfo MyShip;
		public readonly List<ShipSystemBase> AllSystems = new List<ShipSystemBase>(6);
		public readonly ScienceSystem SciSys;
		public readonly EngineeringSystem EngSys;

		public void Register(IEntity entity)
		{
			// todo: add sync id
			var entityPos = entity.Position;
			var nearest = AllSystems.OrderBy(s => Vector3.Distance(s.Position, entityPos)).First();
			entity.AssociatedSys = nearest;
			nearest.Register(entity);
		}

		public void Unregister(IEntity entity)
		{
			entity.AssociatedSys.Unregister(entity);
		}
	}
}
