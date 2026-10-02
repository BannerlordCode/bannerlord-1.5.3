using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD.FormationMarker
{
	// Token: 0x02000062 RID: 98
	public class MissionFormationMarkerVM : ViewModel
	{
		// Token: 0x060007C8 RID: 1992 RVA: 0x0001B045 File Offset: 0x00019245
		public MissionFormationMarkerVM(Mission mission)
			: this(mission, false)
		{
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x0001B050 File Offset: 0x00019250
		public MissionFormationMarkerVM(Mission mission, bool isMultiplayer)
		{
			this._mission = mission;
			this._isMultiplayer = isMultiplayer;
			this._comparer = new MissionFormationMarkerVM.FormationMarkerDistanceComparer();
			this.Targets = new MBBindingList<MissionFormationMarkerTargetVM>();
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x0001B0A8 File Offset: 0x000192A8
		public void SetMarkerDistanceConfig(float farDistanceCutoff, float farAlphaTarget, float alwaysOnDistance)
		{
			this._overrideFarDistanceCutoff = farDistanceCutoff;
			this._overrideFarAlphaTarget = farAlphaTarget;
			this._overrideAlwaysOnDistance = alwaysOnDistance;
			float num;
			float num2;
			float num3;
			float num4;
			float num5;
			this.GetMarkerDistanceConfig(out num, out num2, out num3, out num4, out num5);
			foreach (MissionFormationMarkerTargetVM missionFormationMarkerTargetVM in this.Targets)
			{
				MissionFormationMarkerVM.ApplyMarkerDistanceConfig(missionFormationMarkerTargetVM, num, num2, num3, num4, num5);
			}
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x0001B124 File Offset: 0x00019324
		private void GetMarkerDistanceConfig(out float farAlphaTarget, out float farDistanceCutoff, out float closeDistanceCutoff, out float closestFadeoutRange, out float alwaysOnDistance)
		{
			closeDistanceCutoff = (this._isMultiplayer ? 5f : 10f);
			closestFadeoutRange = (this._isMultiplayer ? 1f : 5f);
			float num = (this._isMultiplayer ? 0.35f : 0.7f);
			float num2 = (this._isMultiplayer ? 350f : 500f);
			float num3 = (this._isMultiplayer ? 25f : 25f);
			farAlphaTarget = ((this._overrideFarAlphaTarget >= 0f) ? this._overrideFarAlphaTarget : num);
			farDistanceCutoff = ((this._overrideFarDistanceCutoff >= 0f) ? this._overrideFarDistanceCutoff : num2);
			alwaysOnDistance = ((this._overrideAlwaysOnDistance >= 0f) ? this._overrideAlwaysOnDistance : num3);
			if (farDistanceCutoff <= closeDistanceCutoff)
			{
				farDistanceCutoff = num2;
			}
			if (farAlphaTarget <= 0f && this._overrideFarDistanceCutoff < 0f)
			{
				farAlphaTarget = num;
			}
			if (alwaysOnDistance >= farDistanceCutoff)
			{
				alwaysOnDistance = num3;
			}
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x0001B213 File Offset: 0x00019413
		private static void ApplyMarkerDistanceConfig(MissionFormationMarkerTargetVM target, float farAlphaTarget, float farDistanceCutoff, float closeDistanceCutoff, float closestFadeoutRange, float alwaysOnDistance)
		{
			target.FarAlphaTarget = farAlphaTarget;
			target.FarDistanceCutoff = farDistanceCutoff;
			target.CloseDistanceCutoff = closeDistanceCutoff;
			target.ClosestFadeoutRange = closestFadeoutRange;
			target.AlwaysOnDistance = alwaysOnDistance;
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x0001B23C File Offset: 0x0001943C
		public void RefreshFormationMarkers()
		{
			IEnumerable<Formation> formationList = this._mission.Teams.SelectMany<Team, Formation>((Team t) => t.FormationsIncludingEmpty.WhereQ<Formation>((Formation f) => f.CountOfUnits > 0));
			float num;
			float num2;
			float num3;
			float num4;
			float num5;
			this.GetMarkerDistanceConfig(out num, out num2, out num3, out num4, out num5);
			using (IEnumerator<Formation> enumerator = formationList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Formation formation = enumerator.Current;
					if (this.Targets.All<MissionFormationMarkerTargetVM>((MissionFormationMarkerTargetVM t) => t.Formation != formation))
					{
						MissionFormationMarkerTargetVM missionFormationMarkerTargetVM = new MissionFormationMarkerTargetVM(formation);
						this.Targets.Add(missionFormationMarkerTargetVM);
						missionFormationMarkerTargetVM.IsEnabled = this.IsEnabled;
						missionFormationMarkerTargetVM.IsFormationTargetRelevant = this.IsFormationTargetRelevant;
						missionFormationMarkerTargetVM.ShowDistanceTexts = this.ShowDistanceTexts;
						MissionFormationMarkerVM.ApplyMarkerDistanceConfig(missionFormationMarkerTargetVM, num, num2, num3, num4, num5);
					}
				}
			}
			if (formationList.CountQ<Formation>() < this.Targets.Count)
			{
				foreach (MissionFormationMarkerTargetVM missionFormationMarkerTargetVM2 in this.Targets.WhereQ<MissionFormationMarkerTargetVM>((MissionFormationMarkerTargetVM t) => !formationList.Contains(t.Formation)).ToList<MissionFormationMarkerTargetVM>())
				{
					this.Targets.Remove(missionFormationMarkerTargetVM2);
				}
			}
			this.Targets.Sort(this._comparer);
			foreach (MissionFormationMarkerTargetVM missionFormationMarkerTargetVM3 in this.Targets)
			{
				missionFormationMarkerTargetVM3.Refresh();
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x060007CE RID: 1998 RVA: 0x0001B414 File Offset: 0x00019614
		// (set) Token: 0x060007CF RID: 1999 RVA: 0x0001B41C File Offset: 0x0001961C
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
					for (int i = 0; i < this.Targets.Count; i++)
					{
						this.Targets[i].IsEnabled = value;
					}
				}
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x060007D0 RID: 2000 RVA: 0x0001B46D File Offset: 0x0001966D
		// (set) Token: 0x060007D1 RID: 2001 RVA: 0x0001B478 File Offset: 0x00019678
		[DataSourceProperty]
		public bool IsFormationTargetRelevant
		{
			get
			{
				return this._isFormationTargetRelevant;
			}
			set
			{
				if (value != this._isFormationTargetRelevant)
				{
					this._isFormationTargetRelevant = value;
					base.OnPropertyChangedWithValue(value, "IsFormationTargetRelevant");
					for (int i = 0; i < this.Targets.Count; i++)
					{
						this.Targets[i].IsFormationTargetRelevant = value;
					}
				}
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x060007D2 RID: 2002 RVA: 0x0001B4C9 File Offset: 0x000196C9
		// (set) Token: 0x060007D3 RID: 2003 RVA: 0x0001B4D4 File Offset: 0x000196D4
		[DataSourceProperty]
		public bool ShowDistanceTexts
		{
			get
			{
				return this._showDistanceTexts;
			}
			set
			{
				if (this._showDistanceTexts != value)
				{
					this._showDistanceTexts = value;
					base.OnPropertyChangedWithValue(value, "ShowDistanceTexts");
					for (int i = 0; i < this.Targets.Count; i++)
					{
						this.Targets[i].ShowDistanceTexts = value;
					}
				}
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x060007D4 RID: 2004 RVA: 0x0001B525 File Offset: 0x00019725
		// (set) Token: 0x060007D5 RID: 2005 RVA: 0x0001B52D File Offset: 0x0001972D
		[DataSourceProperty]
		public MBBindingList<MissionFormationMarkerTargetVM> Targets
		{
			get
			{
				return this._targets;
			}
			set
			{
				if (value != this._targets)
				{
					this._targets = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionFormationMarkerTargetVM>>(value, "Targets");
				}
			}
		}

		// Token: 0x04000378 RID: 888
		private readonly Mission _mission;

		// Token: 0x04000379 RID: 889
		private readonly MissionFormationMarkerVM.FormationMarkerDistanceComparer _comparer;

		// Token: 0x0400037A RID: 890
		private readonly bool _isMultiplayer;

		// Token: 0x0400037B RID: 891
		private const float MultiplayerFarAlphaTarget = 0.35f;

		// Token: 0x0400037C RID: 892
		private const float MultiplayerFarDistanceCutoff = 350f;

		// Token: 0x0400037D RID: 893
		private const float MultiplayerCloseDistanceCutoff = 5f;

		// Token: 0x0400037E RID: 894
		private const float MultiplayerClosestFadeoutRange = 1f;

		// Token: 0x0400037F RID: 895
		private const float MultiplayerAlwaysOnDistance = 25f;

		// Token: 0x04000380 RID: 896
		private const float SingleplayerFarAlphaTarget = 0.7f;

		// Token: 0x04000381 RID: 897
		private const float SingleplayerFarDistanceCutoff = 500f;

		// Token: 0x04000382 RID: 898
		private const float SingleplayerCloseDistanceCutoff = 10f;

		// Token: 0x04000383 RID: 899
		private const float SingleplayerClosestFadeoutRange = 5f;

		// Token: 0x04000384 RID: 900
		private const float SingleplayerAlwaysOnDistance = 25f;

		// Token: 0x04000385 RID: 901
		private const float NoOverride = -1f;

		// Token: 0x04000386 RID: 902
		private float _overrideFarAlphaTarget = -1f;

		// Token: 0x04000387 RID: 903
		private float _overrideFarDistanceCutoff = -1f;

		// Token: 0x04000388 RID: 904
		private float _overrideAlwaysOnDistance = -1f;

		// Token: 0x04000389 RID: 905
		private bool _isEnabled;

		// Token: 0x0400038A RID: 906
		private bool _isFormationTargetRelevant;

		// Token: 0x0400038B RID: 907
		private bool _showDistanceTexts;

		// Token: 0x0400038C RID: 908
		private MBBindingList<MissionFormationMarkerTargetVM> _targets;

		// Token: 0x020000F2 RID: 242
		public class FormationMarkerDistanceComparer : IComparer<MissionFormationMarkerTargetVM>
		{
			// Token: 0x06000D19 RID: 3353 RVA: 0x00029F20 File Offset: 0x00028120
			public int Compare(MissionFormationMarkerTargetVM x, MissionFormationMarkerTargetVM y)
			{
				return y.Distance.CompareTo(x.Distance);
			}
		}
	}
}
