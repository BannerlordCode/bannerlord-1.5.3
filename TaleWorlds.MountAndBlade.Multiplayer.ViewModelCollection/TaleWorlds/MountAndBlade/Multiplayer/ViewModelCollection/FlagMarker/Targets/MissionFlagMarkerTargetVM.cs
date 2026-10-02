using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Objects;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.FlagMarker.Targets
{
	// Token: 0x0200009D RID: 157
	public class MissionFlagMarkerTargetVM : MissionMarkerTargetVM
	{
		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06000F8C RID: 3980 RVA: 0x000309D7 File Offset: 0x0002EBD7
		// (set) Token: 0x06000F8D RID: 3981 RVA: 0x000309DF File Offset: 0x0002EBDF
		public FlagCapturePoint TargetFlag { get; private set; }

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06000F8E RID: 3982 RVA: 0x000309E8 File Offset: 0x0002EBE8
		public override Vec3 WorldPosition
		{
			get
			{
				if (this.TargetFlag != null)
				{
					return this.TargetFlag.Position;
				}
				Debug.FailedAssert("No target found!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\FlagMarker\\Targets\\MissionFlagMarkerTargetVM.cs", "WorldPosition", 24);
				return Vec3.One;
			}
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06000F8F RID: 3983 RVA: 0x00030A19 File Offset: 0x0002EC19
		protected override float HeightOffset
		{
			get
			{
				return 2f;
			}
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x00030A20 File Offset: 0x0002EC20
		public MissionFlagMarkerTargetVM(FlagCapturePoint flag)
			: base(MissionMarkerType.Flag)
		{
			this.TargetFlag = flag;
			base.Name = Convert.ToChar(flag.FlagChar).ToString();
			foreach (string text in this.TargetFlag.GameEntity.Tags)
			{
				if (text.StartsWith("enable_") || text.StartsWith("disable_"))
				{
					this.IsSpawnAffectorFlag = true;
				}
			}
			if (this.TargetFlag.GameEntity.HasTag("keep_capture_point"))
			{
				this.IsKeepFlag = true;
			}
			this.OnOwnerChanged(null);
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x00030AD0 File Offset: 0x0002ECD0
		private Vec3 Vector3Maxamize(Vec3 vector)
		{
			float num = 0f;
			num = ((vector.x > num) ? vector.x : num);
			num = ((vector.y > num) ? vector.y : num);
			num = ((vector.z > num) ? vector.z : num);
			return vector / num;
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x00030B24 File Offset: 0x0002ED24
		public override void UpdateScreenPosition(Camera missionCamera)
		{
			Vec3 worldPosition = this.WorldPosition;
			worldPosition.z += this.HeightOffset;
			Vec3 vec = missionCamera.WorldPointToViewPortPoint(ref worldPosition);
			vec.y = 1f - vec.y;
			if (vec.z < 0f)
			{
				vec.x = 1f - vec.x;
				vec.y = 1f - vec.y;
				vec.z = 0f;
				vec = this.Vector3Maxamize(vec);
			}
			if (float.IsPositiveInfinity(vec.x))
			{
				vec.x = 1f;
			}
			else if (float.IsNegativeInfinity(vec.x))
			{
				vec.x = 0f;
			}
			if (float.IsPositiveInfinity(vec.y))
			{
				vec.y = 1f;
			}
			else if (float.IsNegativeInfinity(vec.y))
			{
				vec.y = 0f;
			}
			vec.x = MathF.Clamp(vec.x, 0f, 1f) * Screen.RealScreenResolutionWidth;
			vec.y = MathF.Clamp(vec.y, 0f, 1f) * Screen.RealScreenResolutionHeight;
			base.ScreenPosition = new Vec2(vec.x, vec.y);
			this.FlagProgress = this.TargetFlag.GetFlagProgress();
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x00030C80 File Offset: 0x0002EE80
		public void OnOwnerChanged(Team team)
		{
			bool flag = team == null || team.TeamIndex == -1;
			uint num = (flag ? 4284111450U : team.Color);
			uint num2 = (flag ? uint.MaxValue : team.Color2);
			base.RefreshColor(num, num2);
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x00030CC1 File Offset: 0x0002EEC1
		public void OnRemainingMoraleChanged(int remainingMorale)
		{
			if (this.RemainingRemovalTime != remainingMorale && remainingMorale != 90)
			{
				this.RemainingRemovalTime = (int)((float)remainingMorale / 1f);
			}
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06000F95 RID: 3989 RVA: 0x00030CE0 File Offset: 0x0002EEE0
		// (set) Token: 0x06000F96 RID: 3990 RVA: 0x00030CE8 File Offset: 0x0002EEE8
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

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06000F97 RID: 3991 RVA: 0x00030D06 File Offset: 0x0002EF06
		// (set) Token: 0x06000F98 RID: 3992 RVA: 0x00030D0E File Offset: 0x0002EF0E
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

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06000F99 RID: 3993 RVA: 0x00030D2C File Offset: 0x0002EF2C
		// (set) Token: 0x06000F9A RID: 3994 RVA: 0x00030D34 File Offset: 0x0002EF34
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

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06000F9B RID: 3995 RVA: 0x00030D52 File Offset: 0x0002EF52
		// (set) Token: 0x06000F9C RID: 3996 RVA: 0x00030D5A File Offset: 0x0002EF5A
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

		// Token: 0x0400073F RID: 1855
		private bool _isKeepFlag;

		// Token: 0x04000740 RID: 1856
		private bool _isSpawnAffectorFlag;

		// Token: 0x04000741 RID: 1857
		private float _flagProgress;

		// Token: 0x04000742 RID: 1858
		private int _remainingRemovalTime = -1;
	}
}
