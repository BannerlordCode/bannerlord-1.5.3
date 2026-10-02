using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.MissionViews
{
	// Token: 0x02000082 RID: 130
	public class ReplayCaptureLogic : MissionView
	{
		// Token: 0x06000500 RID: 1280 RVA: 0x00025140 File Offset: 0x00023340
		private void CheckFixedDeltaTimeMode()
		{
			if (this.RenderActive && this.SaveScreenshots)
			{
				base.Mission.FixedDeltaTime = 0.016666668f;
				base.Mission.FixedDeltaTimeMode = true;
				return;
			}
			base.Mission.FixedDeltaTime = 0f;
			base.Mission.FixedDeltaTimeMode = false;
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x00025196 File Offset: 0x00023396
		// (set) Token: 0x06000502 RID: 1282 RVA: 0x0002519E File Offset: 0x0002339E
		private bool RenderActive
		{
			get
			{
				return this._renderActive;
			}
			set
			{
				this._renderActive = value;
				this.CheckFixedDeltaTimeMode();
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000503 RID: 1283 RVA: 0x000251AD File Offset: 0x000233AD
		private Camera MissionCamera
		{
			get
			{
				if (base.MissionScreen == null || !(base.MissionScreen.CombatCamera != null))
				{
					return null;
				}
				return base.MissionScreen.CombatCamera;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000504 RID: 1284 RVA: 0x000251D7 File Offset: 0x000233D7
		private float ReplayTime
		{
			get
			{
				return base.Mission.CurrentTime - this._replayTimeDiff;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000505 RID: 1285 RVA: 0x000251EB File Offset: 0x000233EB
		// (set) Token: 0x06000506 RID: 1286 RVA: 0x000251F3 File Offset: 0x000233F3
		private bool SaveScreenshots
		{
			get
			{
				return this._saveScreenshots;
			}
			set
			{
				this._saveScreenshots = value;
				this.CheckFixedDeltaTimeMode();
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000507 RID: 1287 RVA: 0x00025202 File Offset: 0x00023402
		private KeyValuePair<float, MatrixFrame> PreviousKey
		{
			get
			{
				return this.GetPreviousKey();
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000508 RID: 1288 RVA: 0x0002520A File Offset: 0x0002340A
		private KeyValuePair<float, MatrixFrame> NextKey
		{
			get
			{
				return this.GetNextKey();
			}
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x00025214 File Offset: 0x00023414
		private KeyValuePair<float, MatrixFrame> GetPreviousKey()
		{
			KeyValuePair<float, MatrixFrame> keyValuePair = this._invalid;
			if (!this._cameraKeys.Any<KeyValuePair<float, SortedDictionary<int, MatrixFrame>>>())
			{
				return keyValuePair;
			}
			foreach (KeyValuePair<float, SortedDictionary<int, MatrixFrame>> keyValuePair2 in this._cameraKeys)
			{
				if (keyValuePair2.Key <= this.ReplayTime)
				{
					keyValuePair = new KeyValuePair<float, MatrixFrame>(keyValuePair2.Key, keyValuePair2.Value[keyValuePair2.Value.Count - 1]);
				}
			}
			return keyValuePair;
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x000252B0 File Offset: 0x000234B0
		private KeyValuePair<float, MatrixFrame> GetNextKey()
		{
			KeyValuePair<float, MatrixFrame> keyValuePair = this._invalid;
			if (!this._cameraKeys.Any<KeyValuePair<float, SortedDictionary<int, MatrixFrame>>>())
			{
				return keyValuePair;
			}
			foreach (KeyValuePair<float, SortedDictionary<int, MatrixFrame>> keyValuePair2 in this._cameraKeys)
			{
				if (keyValuePair2.Key > this.ReplayTime)
				{
					keyValuePair = new KeyValuePair<float, MatrixFrame>(keyValuePair2.Key, keyValuePair2.Value[0]);
					break;
				}
			}
			return keyValuePair;
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00025340 File Offset: 0x00023540
		public ReplayCaptureLogic()
		{
			this._cameraKeys = new SortedDictionary<float, SortedDictionary<int, MatrixFrame>>();
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00025377 File Offset: 0x00023577
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._replayLogic = base.Mission.GetMissionBehavior<ReplayMissionView>();
			this._replayLogic.OverrideInput(true);
			if (!MBCommon.IsPaused)
			{
				this._replayLogic.Pause();
			}
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x000253B0 File Offset: 0x000235B0
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (this._frameSkip && !MBCommon.IsPaused)
			{
				if (!this._isRendered)
				{
					this._isRendered = true;
					return;
				}
				this._replayLogic.Pause();
				this._frameSkip = false;
			}
			if (this.RenderActive)
			{
				this.SaveScreenshot();
				if (!base.Mission.Recorder.IsEndOfRecord())
				{
					KeyValuePair<float, MatrixFrame> previousKey = this.PreviousKey;
					KeyValuePair<float, MatrixFrame> nextKey = this.NextKey;
					this._replayLogic.Resume();
					if (nextKey.Key >= 0f)
					{
						for (int i = 0; i < this._cameraKeys.Count; i++)
						{
							if (previousKey.Key == this._cameraKeys.ElementAt<KeyValuePair<float, SortedDictionary<int, MatrixFrame>>>(i).Key)
							{
								float num = nextKey.Key - previousKey.Key;
								float num2 = (this.ReplayTime - previousKey.Key) / num;
								int count = this._cameraKeys[previousKey.Key].Count;
								MatrixFrame matrixFrame;
								if (this._lastUsedIndex != i && count > 1)
								{
									matrixFrame = this._cameraKeys[previousKey.Key][count - 1];
								}
								else
								{
									matrixFrame = new MatrixFrame
									{
										origin = this._path.GetHermiteFrameForDt(num2, i).origin
									};
									Vec3 vec = previousKey.Value.rotation.s * (1f - num2) + nextKey.Value.rotation.s * num2;
									Vec3 vec2 = previousKey.Value.rotation.u * (1f - num2) + nextKey.Value.rotation.u * num2;
									Vec3 vec3 = previousKey.Value.rotation.f * (1f - num2) + nextKey.Value.rotation.f * num2;
									matrixFrame.rotation.s = vec;
									matrixFrame.rotation.u = vec2;
									matrixFrame.rotation.f = vec3;
								}
								matrixFrame.rotation.s.Normalize();
								matrixFrame.rotation.u.Normalize();
								matrixFrame.rotation.f.Normalize();
								matrixFrame.rotation.Orthonormalize();
								base.MissionScreen.CustomCamera.Frame = matrixFrame;
								this._lastUsedIndex = i;
								return;
							}
						}
						return;
					}
					if (previousKey.Key >= 0f)
					{
						int count2 = this._cameraKeys[previousKey.Key].Count;
						if (count2 > 1)
						{
							MatrixFrame matrixFrame2 = this._cameraKeys[previousKey.Key][count2 - 1];
							matrixFrame2.rotation.s.Normalize();
							matrixFrame2.rotation.u.Normalize();
							matrixFrame2.rotation.f.Normalize();
							matrixFrame2.rotation.Orthonormalize();
							base.MissionScreen.CustomCamera.Frame = matrixFrame2;
							return;
						}
					}
				}
				else
				{
					MBDebug.Print("All images are saved.", 0, Debug.DebugColor.DarkCyan, 64UL);
					this.RenderActive = false;
					this._replayLogic.ResetReplay();
					this._replayTimeDiff = base.Mission.CurrentTime;
					base.MissionScreen.CustomCamera = null;
					this._replayLogic.Pause();
					this.SaveScreenshots = false;
					this._ssNum = 0;
				}
				return;
			}
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00025750 File Offset: 0x00023950
		private void InsertCamKey()
		{
			float replayTime = this.ReplayTime;
			MatrixFrame frame = this.MissionCamera.Frame;
			int num = 0;
			if (this._cameraKeys.ContainsKey(replayTime))
			{
				num = this._cameraKeys[replayTime].Count;
				this._cameraKeys[replayTime].Add(num, frame);
			}
			else
			{
				this._cameraKeys.Add(replayTime, new SortedDictionary<int, MatrixFrame> { { num, frame } });
			}
			MBDebug.Print(string.Concat(new object[] { "Keyframe to \"", replayTime, "\" has been inserted with the index: ", num, ".\n" }), 0, Debug.DebugColor.Green, 64UL);
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x000257FF File Offset: 0x000239FF
		private void MoveToNextFrame()
		{
			this._replayLogic.FastForward(0.016666668f);
			this._replayLogic.Resume();
			this._frameSkip = true;
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00025824 File Offset: 0x00023A24
		private void GoToKey(float keyTime)
		{
			if (keyTime < 0f || !this._cameraKeys.ContainsKey(keyTime) || keyTime == this.ReplayTime)
			{
				return;
			}
			MatrixFrame matrixFrame;
			if (keyTime < this.ReplayTime)
			{
				matrixFrame = this._cameraKeys[keyTime][this._cameraKeys[keyTime].Count - 1];
				this._replayLogic.Rewind(this.ReplayTime - keyTime);
				this._replayTimeDiff = base.Mission.CurrentTime;
			}
			else
			{
				matrixFrame = this._cameraKeys[keyTime][0];
				this._replayLogic.FastForward(keyTime - this.ReplayTime);
			}
			this.MissionCamera.Frame = matrixFrame;
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x000258D8 File Offset: 0x00023AD8
		private void SetPath()
		{
			if (base.Mission.Scene.GetPathWithName("CameraPath") != null)
			{
				base.Mission.Scene.DeletePathWithName("CameraPath");
			}
			base.Mission.Scene.AddPath("CameraPath");
			foreach (KeyValuePair<float, SortedDictionary<int, MatrixFrame>> keyValuePair in this._cameraKeys)
			{
				base.Mission.Scene.AddPathPoint("CameraPath", keyValuePair.Value[0]);
			}
			this._path = base.Mission.Scene.GetPathWithName("CameraPath");
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x000259A8 File Offset: 0x00023BA8
		private void Render(bool saveScreenshots = false)
		{
			if (!this._cameraKeys.ContainsKey(0f))
			{
				this._cameraKeys.Add(0f, new SortedDictionary<int, MatrixFrame> { 
				{
					0,
					this.MissionCamera.Frame
				} });
			}
			else
			{
				this._cameraKeys[0f] = new SortedDictionary<int, MatrixFrame> { 
				{
					0,
					this.MissionCamera.Frame
				} };
			}
			this._replayLogic.ResetReplay();
			this._replayLogic.Pause();
			this._replayTimeDiff = base.Mission.CurrentTime;
			this.SetPath();
			this.SaveScreenshots = saveScreenshots;
			this.RenderActive = true;
			this._lastUsedIndex = 0;
			base.MissionScreen.CustomCamera = base.MissionScreen.CombatCamera;
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00025A70 File Offset: 0x00023C70
		private void SaveScreenshot()
		{
			if (!this.SaveScreenshots)
			{
				return;
			}
			if (string.IsNullOrEmpty(this._directoryPath.Path))
			{
				PlatformDirectoryPath platformDirectoryPath = new PlatformDirectoryPath(PlatformFileType.User, "Captures");
				string text = "Cap_" + string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
				this._directoryPath = platformDirectoryPath + text;
			}
			Utilities.TakeScreenshot(new PlatformFilePath(this._directoryPath, "time_" + string.Format("{0:000000}", this._ssNum) + ".bmp"));
			this._ssNum++;
		}

		// Token: 0x040002D1 RID: 721
		private ReplayMissionView _replayLogic;

		// Token: 0x040002D2 RID: 722
		private bool _renderActive;

		// Token: 0x040002D3 RID: 723
		public const float CaptureFrameRate = 60f;

		// Token: 0x040002D4 RID: 724
		private float _replayTimeDiff;

		// Token: 0x040002D5 RID: 725
		private bool _frameSkip;

		// Token: 0x040002D6 RID: 726
		private Path _path;

		// Token: 0x040002D7 RID: 727
		private PlatformDirectoryPath _directoryPath;

		// Token: 0x040002D8 RID: 728
		private bool _saveScreenshots;

		// Token: 0x040002D9 RID: 729
		private readonly KeyValuePair<float, MatrixFrame> _invalid = new KeyValuePair<float, MatrixFrame>(-1f, default(MatrixFrame));

		// Token: 0x040002DA RID: 730
		private SortedDictionary<float, SortedDictionary<int, MatrixFrame>> _cameraKeys;

		// Token: 0x040002DB RID: 731
		private bool _isRendered;

		// Token: 0x040002DC RID: 732
		private int _lastUsedIndex;

		// Token: 0x040002DD RID: 733
		private int _ssNum;
	}
}
