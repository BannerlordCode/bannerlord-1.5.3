using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Objects;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD.Compass;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.HUDExtensions
{
	// Token: 0x02000092 RID: 146
	public class CapturePointVM : CompassTargetVM
	{
		// Token: 0x06000E1E RID: 3614 RVA: 0x0002B77C File Offset: 0x0002997C
		public CapturePointVM(FlagCapturePoint target, TargetIconType iconType)
			: base(iconType, 0U, 0U, null, false, false)
		{
			this.Target = target;
			foreach (string text in this.Target.GameEntity.Tags)
			{
				if (text.StartsWith("enable_") || text.StartsWith("disable_"))
				{
					this.IsSpawnAffectorFlag = true;
				}
			}
			if (this.Target.GameEntity.HasTag("keep_capture_point"))
			{
				this.IsKeepFlag = true;
			}
			this.ResetFlag();
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x0002B811 File Offset: 0x00029A11
		public override void Refresh(float circleX, float x, float distance)
		{
			base.Refresh(circleX, x, distance);
			this.FlagProgress = this.Target.GetFlagProgress();
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x0002B830 File Offset: 0x00029A30
		public void OnOwnerChanged(Team newTeam)
		{
			uint num = ((newTeam != null) ? newTeam.Color : 4284111450U);
			uint num2 = ((newTeam != null) ? newTeam.Color2 : uint.MaxValue);
			base.RefreshColor(num, num2);
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x0002B863 File Offset: 0x00029A63
		public void ResetFlag()
		{
			this.OnOwnerChanged(null);
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x0002B86C File Offset: 0x00029A6C
		internal void OnRemainingMoraleChanged(int remainingMorale)
		{
			if (this.RemainingRemovalTime != remainingMorale && remainingMorale != 90)
			{
				this.RemainingRemovalTime = (int)((float)remainingMorale / 1f);
			}
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06000E23 RID: 3619 RVA: 0x0002B88B File Offset: 0x00029A8B
		// (set) Token: 0x06000E24 RID: 3620 RVA: 0x0002B893 File Offset: 0x00029A93
		[DataSourceProperty]
		public float FlagProgress
		{
			get
			{
				return this._flagProgress;
			}
			set
			{
				if (value != this._flagProgress)
				{
					this._flagProgress = value;
					base.OnPropertyChangedWithValue(value, "FlagProgress");
				}
			}
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06000E25 RID: 3621 RVA: 0x0002B8B1 File Offset: 0x00029AB1
		// (set) Token: 0x06000E26 RID: 3622 RVA: 0x0002B8B9 File Offset: 0x00029AB9
		[DataSourceProperty]
		public bool IsSpawnAffectorFlag
		{
			get
			{
				return this._isSpawnAffectorFlag;
			}
			set
			{
				if (value != this._isSpawnAffectorFlag)
				{
					this._isSpawnAffectorFlag = value;
					base.OnPropertyChangedWithValue(value, "IsSpawnAffectorFlag");
				}
			}
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06000E27 RID: 3623 RVA: 0x0002B8D7 File Offset: 0x00029AD7
		// (set) Token: 0x06000E28 RID: 3624 RVA: 0x0002B8DF File Offset: 0x00029ADF
		[DataSourceProperty]
		public bool IsKeepFlag
		{
			get
			{
				return this._isKeepFlag;
			}
			set
			{
				if (value != this._isKeepFlag)
				{
					this._isKeepFlag = value;
					base.OnPropertyChangedWithValue(value, "IsKeepFlag");
				}
			}
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x06000E29 RID: 3625 RVA: 0x0002B8FD File Offset: 0x00029AFD
		// (set) Token: 0x06000E2A RID: 3626 RVA: 0x0002B905 File Offset: 0x00029B05
		[DataSourceProperty]
		public int RemainingRemovalTime
		{
			get
			{
				return this._remainingRemovalTime;
			}
			set
			{
				if (value != this._remainingRemovalTime)
				{
					this._remainingRemovalTime = value;
					base.OnPropertyChangedWithValue(value, "RemainingRemovalTime");
				}
			}
		}

		// Token: 0x04000673 RID: 1651
		public readonly FlagCapturePoint Target;

		// Token: 0x04000674 RID: 1652
		private float _flagProgress;

		// Token: 0x04000675 RID: 1653
		private int _remainingRemovalTime = -1;

		// Token: 0x04000676 RID: 1654
		private bool _isKeepFlag;

		// Token: 0x04000677 RID: 1655
		private bool _isSpawnAffectorFlag;
	}
}
