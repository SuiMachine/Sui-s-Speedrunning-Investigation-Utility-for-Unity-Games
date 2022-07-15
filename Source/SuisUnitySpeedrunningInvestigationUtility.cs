using MelonLoader;

namespace SuisUnitySpeedrunningInvestigationUtility
{
	public class SuisUnitySpeedrunningInvestigationUtility : MelonMod
	{
		public static MelonLogger.Instance loggerInst { get; private set; }

		public override void OnApplicationStart()
		{
			loggerInst = LoggerInstance;
			LoggerInstance.Msg("Loading Sui's Hack loaded");


			base.OnApplicationStart();
		}

		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			base.OnSceneWasLoaded(buildIndex, sceneName);
			HackGuiThing.Initialize();
		}
	}
}
