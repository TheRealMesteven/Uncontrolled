using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uncontrolled.Entities;
using UnityEngine;

namespace Uncontrolled
{
	public abstract class ShipSystemBase
	{
		public UnSystem MyUnSystem;

		public ShipSystemBase(UnSystem system) 
		{ 
			MyUnSystem = system;
		}

		public abstract void Register(IEntity entity);
		public abstract void Unregister(IEntity entity);

		public abstract Vector3 Position { get; }
	}
}
