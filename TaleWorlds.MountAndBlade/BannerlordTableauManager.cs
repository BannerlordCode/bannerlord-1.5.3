using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200019E RID: 414
	public static class BannerlordTableauManager
	{
		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06001638 RID: 5688 RVA: 0x000520A0 File Offset: 0x000502A0
		public static Scene[] TableauCharacterScenes
		{
			get
			{
				return BannerlordTableauManager._tableauCharacterScenes;
			}
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x000520A7 File Offset: 0x000502A7
		public static void RequestCharacterTableauRender(int characterCodeId, string path, GameEntity poseEntity, Camera cameraObject, int tableauType)
		{
			MBAPI.IMBBannerlordTableauManager.RequestCharacterTableauRender(characterCodeId, path, poseEntity.Pointer, cameraObject.Pointer, tableauType);
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x000520C3 File Offset: 0x000502C3
		public static void ClearManager()
		{
			BannerlordTableauManager._tableauCharacterScenes = null;
			BannerlordTableauManager.RequestCallback = null;
			BannerlordTableauManager._isTableauRenderSystemInitialized = false;
		}

		// Token: 0x0600163B RID: 5691 RVA: 0x000520D7 File Offset: 0x000502D7
		public static void InitializeCharacterTableauRenderSystem()
		{
			if (!BannerlordTableauManager._isTableauRenderSystemInitialized)
			{
				MBAPI.IMBBannerlordTableauManager.InitializeCharacterTableauRenderSystem();
				BannerlordTableauManager._isTableauRenderSystemInitialized = true;
			}
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x000520F0 File Offset: 0x000502F0
		public static int GetNumberOfPendingTableauRequests()
		{
			return MBAPI.IMBBannerlordTableauManager.GetNumberOfPendingTableauRequests();
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x000520FC File Offset: 0x000502FC
		[MBCallback(null, false)]
		internal static void RequestCharacterTableauSetup(int characterCodeId, Scene scene, GameEntity poseEntity)
		{
			BannerlordTableauManager.RequestCallback(characterCodeId, scene, poseEntity);
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x0005210B File Offset: 0x0005030B
		[MBCallback(null, false)]
		internal static void RegisterCharacterTableauScene(Scene scene, int type)
		{
			BannerlordTableauManager.TableauCharacterScenes[type] = scene;
		}

		// Token: 0x0400072B RID: 1835
		private static Scene[] _tableauCharacterScenes = new Scene[5];

		// Token: 0x0400072C RID: 1836
		private static bool _isTableauRenderSystemInitialized = false;

		// Token: 0x0400072D RID: 1837
		public static BannerlordTableauManager.RequestCharacterTableauSetupDelegate RequestCallback;

		// Token: 0x020004F2 RID: 1266
		// (Invoke) Token: 0x06003C12 RID: 15378
		public delegate void RequestCharacterTableauSetupDelegate(int characterCodeId, Scene scene, GameEntity poseEntity);
	}
}
