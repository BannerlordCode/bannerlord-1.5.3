using System;
using System.Collections.Generic;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace SandBox.View.Missions
{
	// Token: 0x0200001A RID: 26
	public class MissionCustomCameraView : MissionView
	{
		// Token: 0x060000A6 RID: 166 RVA: 0x00005ECC File Offset: 0x000040CC
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			foreach (GameEntity gameEntity in base.Mission.Scene.FindEntitiesWithTag(this.tag))
			{
				Camera camera = Camera.CreateCamera();
				gameEntity.GetCameraParamsFromCameraScript(camera, ref this._dofParams);
				this._cameras.Add(camera);
			}
			base.MissionScreen.CustomCamera = this._cameras[0];
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00005F5C File Offset: 0x0000415C
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (base.DebugInput.IsHotKeyReleased("CustomCameraMissionViewHotkeyIncreaseCustomCameraIndex"))
			{
				this._currentCameraIndex++;
				if (this._currentCameraIndex >= this._cameras.Count)
				{
					this._currentCameraIndex = 0;
				}
				base.MissionScreen.CustomCamera = this._cameras[this._currentCameraIndex];
			}
		}

		// Token: 0x04000034 RID: 52
		public string tag = "customcamera";

		// Token: 0x04000035 RID: 53
		private readonly List<Camera> _cameras = new List<Camera>();

		// Token: 0x04000036 RID: 54
		public Vec3 _dofParams;

		// Token: 0x04000037 RID: 55
		private int _currentCameraIndex;
	}
}
