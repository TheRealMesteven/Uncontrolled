using System;
using System.IO;
using UnityEngine;
using HarmonyLib;

namespace Uncontrolled
{
	[HarmonyPatch(typeof(NetworkingPeer), "DoInstantiate")]
	public static class MyPrefabs
	{
		public static GameObject BurnedWiresPrefab;
		public static readonly string BurnedWiresPrefab_Name = "UNSYSBWP";

		internal static void Init()
		{
			var bundle = AssetBundle.LoadFromFile(Path.Combine(typeof(MyPrefabs).Assembly.Location.Replace(".dll", ".bundle")));
			BurnedWiresPrefab = bundle.LoadAsset<GameObject>("BurnedWiresPrefab");
			PhotonNetwork.PrefabCache.Add(BurnedWiresPrefab_Name, BurnedWiresPrefab);
		}

		static void Prefix(ExitGames.Client.Photon.Hashtable evData, PhotonPlayer photonPlayer, ref GameObject resourceGameObject)
		{
			if (resourceGameObject == null)
			{
				string text = (string)evData[0];
				PhotonNetwork.PrefabCache.TryGetValue(text, out resourceGameObject);
			}
		}
	}
}
