using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Uncontrolled.Entities
{
	public class BurnedWires : PLDamageablePlanetObject, IEntity
	{
		public ShipSystemBase AssociatedSys { get; set; }

		public long Data { get; set; }

		public Vector3 Position => this.transform.position;

		public PLShipInfo MyShip;

		public override void Start()
		{
			base.Start();
			this.hideFlags |= HideFlags.HideAndDontSave;
			this.transform.localScale = new Vector3(5f,5f,5f);
			this.Repairable = true;
			this.Destroyable = false;
			this.Damageable = false;
			this.Health = 0;
			this.MaxHealth = 10;
			var shipid = (int)photonView.instantiationData[0];
			MyShip = (PLShipInfo)PLEncounterManager.Instance.GetShipFromID((int)base.photonView.instantiationData[0]);
			UnSystem.Get(MyShip).Register(this);
			this.MyCurrentTLI = MyShip.MyTLI;
		}

		public override void Update()
		{
			base.Update();
			if (this.Health >= this.MaxHealth * 0.99f)
			{
				if (base.photonView.isMine)
				{
					this.transform.parent = null;
					UnSystem.Get(MyShip).Unregister(this);
					if (PhotonNetwork.isMasterClient)
						PhotonNetwork.Destroy(base.photonView);
					return;
				}
			}
		}

		public override string GetName() => "Burned Wires";

		//public override bool ShouldShowInHUD() => PLNetworkManager.Instance.LocalPlayer.ClassID.GetDecrypted() is 0 or 4;
	}

	[HarmonyPatch(typeof(PLGlobal), "LineOfSight", new[] { typeof(Vector3), typeof(Vector3), typeof(bool), typeof(bool), typeof(float) })]
	class patch
	{
		static void Prefix(ref bool throughDoors, ref float radius)
		{
			if (radius == 0.05f)
			{
				throughDoors = true;
				radius = 2f;
			}
		}
	}
}
