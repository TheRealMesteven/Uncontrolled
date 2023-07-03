using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;

namespace Uncontrolled.Entities;

[HarmonyPatch(typeof(PLShipInfo), "TakeDamage")]
public static class EntityGenerator
{
	public static List<Action<PLShipInfo, EDamageType>> Generators = new(4);

	static EntityGenerator()
	{
		Generators.Add(MyGenerator);
	}

	public static void Spawn(PLShipInfo ship, string prefab)
	{
		var nodes = ship.ShipPathNetwork.AllPathNodes;
		var node = nodes[Rand.Range(0, nodes.Count-1)];
		var direction = new Vector3(Rand.Range(-1f,1f), Rand.Range(-1f, 1f), Rand.Range(-1f, 1f));
		var ray = Physics.Raycast(new Ray(node._transform.position + (Vector3.up*2), direction), out var hit, 500f, ~(256 + 32768));

		var spawned = PhotonNetwork.Instantiate(prefab, hit.point, Quaternion.FromToRotation(hit.collider.transform.up, hit.normal), 0, new object[] { ship.ShipID });
		PulsarModLoader.Utilities.Messaging.Echo(PLNetworkManager.Instance.LocalPlayer, spawned.transform.position.ToString());
	}

	private static void MyGenerator(PLShipInfo ship, EDamageType dmgType)
	{
		Spawn(ship, MyPrefabs.BurnedWiresPrefab_Name); // todo: add more prefabs for each dmg type
	}

	private static void Postfix(PLShipInfo __instance, EDamageType dmgType)
	{
        if (!PhotonNetwork.isMasterClient) return;
        if (!__instance.GetIsPlayerShip()) return;
		if (__instance.ShieldIsActive) return;

		if (Generators.Count == 1)
			Generators[0](__instance, dmgType);
		else
			Generators[Rand.Range(0, Generators.Count - 1)](__instance, dmgType);
	}
}
