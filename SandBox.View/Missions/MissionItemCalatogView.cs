using System;
using SandBox.Missions.MissionLogics;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace SandBox.View.Missions
{
	// Token: 0x0200001F RID: 31
	public class MissionItemCalatogView : MissionView
	{
		// Token: 0x060000D2 RID: 210 RVA: 0x00009E48 File Offset: 0x00008048
		public override void AfterStart()
		{
			base.AfterStart();
			this._itemCatalogController = base.Mission.GetMissionBehavior<ItemCatalogController>();
			this._itemCatalogController.BeforeCatalogTick += this.OnBeforeCatalogTick;
			this._itemCatalogController.AfterCatalogTick += this.OnAfterCatalogTick;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00009E9A File Offset: 0x0000809A
		private void OnBeforeCatalogTick(int currentItemIndex)
		{
			Utilities.TakeScreenshot("ItemCatalog/" + this._itemCatalogController.AllItems[currentItemIndex - 1].Name + ".bmp");
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00009EC8 File Offset: 0x000080C8
		private void OnAfterCatalogTick()
		{
			MatrixFrame matrixFrame = default(MatrixFrame);
			Vec3 lookDirection = base.Mission.MainAgent.LookDirection;
			matrixFrame.origin = base.Mission.MainAgent.Position + lookDirection * 2f + new Vec3(0f, 0f, 1.273f, -1f);
			matrixFrame.rotation.u = lookDirection;
			matrixFrame.rotation.s = new Vec3(1f, 0f, 0f, -1f);
			matrixFrame.rotation.f = new Vec3(0f, 0f, 1f, -1f);
			matrixFrame.rotation.Orthonormalize();
			base.Mission.SetCameraFrame(ref matrixFrame, 1f);
			Camera camera = Camera.CreateCamera();
			camera.Frame = matrixFrame;
			base.MissionScreen.CustomCamera = camera;
		}

		// Token: 0x0400007A RID: 122
		private ItemCatalogController _itemCatalogController;
	}
}
