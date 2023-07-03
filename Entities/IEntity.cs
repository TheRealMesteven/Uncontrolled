using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Uncontrolled.Entities;
public interface IEntity
{
	public ShipSystemBase AssociatedSys { get; set; }

	public Vector3 Position { get; }

	public long Data { get; set; }
}
