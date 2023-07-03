using PulsarModLoader;
using PulsarModLoader.MPModChecks;
using System;
using System.Collections.Generic;
using System.Linq;

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
}
