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

		public ScienceSystem(UnSystem system) : base(system)
		{
			SciSys = system.MyShip.ComputerSystem;
		}

		public override Vector3 Position => SciSys._transform.position;

		public override void Register(IEntity entity)
		{

		}

		public override void Unregister(IEntity entity)
		{

		}
	}
}
