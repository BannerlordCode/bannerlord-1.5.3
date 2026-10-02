using System;
using System.Collections.Generic;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001A7 RID: 423
	public static class MBAPI
	{
		// Token: 0x060016C6 RID: 5830 RVA: 0x00053A5C File Offset: 0x00051C5C
		private static T GetObject<T>() where T : class
		{
			object obj;
			if (MBAPI._objects.TryGetValue(typeof(T).FullName, out obj))
			{
				return obj as T;
			}
			return default(T);
		}

		// Token: 0x060016C7 RID: 5831 RVA: 0x00053A9C File Offset: 0x00051C9C
		internal static void SetObjects(Dictionary<string, object> objects)
		{
			MBAPI._objects = objects;
			MBAPI.IMBTestRun = MBAPI.GetObject<IMBTestRun>();
			MBAPI.IMBActionSet = MBAPI.GetObject<IMBActionSet>();
			MBAPI.IMBAgent = MBAPI.GetObject<IMBAgent>();
			MBAPI.IMBAnimation = MBAPI.GetObject<IMBAnimation>();
			MBAPI.IMBDelegate = MBAPI.GetObject<IMBDelegate>();
			MBAPI.IMBItem = MBAPI.GetObject<IMBItem>();
			MBAPI.IMBEditor = MBAPI.GetObject<IMBEditor>();
			MBAPI.IMBMission = MBAPI.GetObject<IMBMission>();
			MBAPI.IMBMultiplayerData = MBAPI.GetObject<IMBMultiplayerData>();
			MBAPI.IMouseManager = MBAPI.GetObject<IMouseManager>();
			MBAPI.IMBNetwork = MBAPI.GetObject<IMBNetwork>();
			MBAPI.IMBPeer = MBAPI.GetObject<IMBPeer>();
			MBAPI.IMBSkeletonExtensions = MBAPI.GetObject<IMBSkeletonExtensions>();
			MBAPI.IMBGameEntityExtensions = MBAPI.GetObject<IMBGameEntityExtensions>();
			MBAPI.IMBScreen = MBAPI.GetObject<IMBScreen>();
			MBAPI.IMBSoundEvent = MBAPI.GetObject<IMBSoundEvent>();
			MBAPI.IMBVoiceManager = MBAPI.GetObject<IMBVoiceManager>();
			MBAPI.IMBTeam = MBAPI.GetObject<IMBTeam>();
			MBAPI.IMBWorld = MBAPI.GetObject<IMBWorld>();
			MBAPI.IInput = MBAPI.GetObject<IInput>();
			MBAPI.IMBMessageManager = MBAPI.GetObject<IMBMessageManager>();
			MBAPI.IMBWindowManager = MBAPI.GetObject<IMBWindowManager>();
			MBAPI.IMBDebugExtensions = MBAPI.GetObject<IMBDebugExtensions>();
			MBAPI.IMBGame = MBAPI.GetObject<IMBGame>();
			MBAPI.IMBFaceGen = MBAPI.GetObject<IMBFaceGen>();
			MBAPI.IMBMapScene = MBAPI.GetObject<IMBMapScene>();
			MBAPI.IMBBannerlordChecker = MBAPI.GetObject<IMBBannerlordChecker>();
			MBAPI.IMBAgentVisuals = MBAPI.GetObject<IMBAgentVisuals>();
			MBAPI.IMBBannerlordTableauManager = MBAPI.GetObject<IMBBannerlordTableauManager>();
			MBAPI.IMBBannerlordConfig = MBAPI.GetObject<IMBBannerlordConfig>();
		}

		// Token: 0x04000873 RID: 2163
		internal static IMBTestRun IMBTestRun;

		// Token: 0x04000874 RID: 2164
		internal static IMBActionSet IMBActionSet;

		// Token: 0x04000875 RID: 2165
		internal static IMBAgent IMBAgent;

		// Token: 0x04000876 RID: 2166
		internal static IMBAgentVisuals IMBAgentVisuals;

		// Token: 0x04000877 RID: 2167
		internal static IMBAnimation IMBAnimation;

		// Token: 0x04000878 RID: 2168
		internal static IMBDelegate IMBDelegate;

		// Token: 0x04000879 RID: 2169
		internal static IMBItem IMBItem;

		// Token: 0x0400087A RID: 2170
		internal static IMBEditor IMBEditor;

		// Token: 0x0400087B RID: 2171
		internal static IMBMission IMBMission;

		// Token: 0x0400087C RID: 2172
		internal static IMBMultiplayerData IMBMultiplayerData;

		// Token: 0x0400087D RID: 2173
		internal static IMouseManager IMouseManager;

		// Token: 0x0400087E RID: 2174
		internal static IMBNetwork IMBNetwork;

		// Token: 0x0400087F RID: 2175
		internal static IMBPeer IMBPeer;

		// Token: 0x04000880 RID: 2176
		internal static IMBSkeletonExtensions IMBSkeletonExtensions;

		// Token: 0x04000881 RID: 2177
		internal static IMBGameEntityExtensions IMBGameEntityExtensions;

		// Token: 0x04000882 RID: 2178
		internal static IMBScreen IMBScreen;

		// Token: 0x04000883 RID: 2179
		internal static IMBSoundEvent IMBSoundEvent;

		// Token: 0x04000884 RID: 2180
		internal static IMBVoiceManager IMBVoiceManager;

		// Token: 0x04000885 RID: 2181
		internal static IMBTeam IMBTeam;

		// Token: 0x04000886 RID: 2182
		internal static IMBWorld IMBWorld;

		// Token: 0x04000887 RID: 2183
		internal static IInput IInput;

		// Token: 0x04000888 RID: 2184
		internal static IMBMessageManager IMBMessageManager;

		// Token: 0x04000889 RID: 2185
		internal static IMBWindowManager IMBWindowManager;

		// Token: 0x0400088A RID: 2186
		internal static IMBDebugExtensions IMBDebugExtensions;

		// Token: 0x0400088B RID: 2187
		internal static IMBGame IMBGame;

		// Token: 0x0400088C RID: 2188
		internal static IMBFaceGen IMBFaceGen;

		// Token: 0x0400088D RID: 2189
		internal static IMBMapScene IMBMapScene;

		// Token: 0x0400088E RID: 2190
		internal static IMBBannerlordChecker IMBBannerlordChecker;

		// Token: 0x0400088F RID: 2191
		internal static IMBBannerlordTableauManager IMBBannerlordTableauManager;

		// Token: 0x04000890 RID: 2192
		internal static IMBBannerlordConfig IMBBannerlordConfig;

		// Token: 0x04000891 RID: 2193
		private static Dictionary<string, object> _objects;
	}
}
