using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace SandBox.View.Missions
{
	// Token: 0x02000025 RID: 37
	[DefaultView]
	public class MissionSoundParametersView : MissionView
	{
		// Token: 0x060000E5 RID: 229 RVA: 0x0000A91F File Offset: 0x00008B1F
		public override void EarlyStart()
		{
			base.EarlyStart();
			this.InitializeGlobalParameters();
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0000A92D File Offset: 0x00008B2D
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			SoundManager.SetGlobalParameter("MissionCulture", 0f);
			SoundManager.SetGlobalParameter("MissionProsperity", 0f);
			SoundManager.SetGlobalParameter("MissionCombatMode", 0f);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x0000A962 File Offset: 0x00008B62
		public override void OnMissionModeChange(MissionMode oldMissionMode, bool atStart)
		{
			this.InitializeCombatModeParameter();
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000A96A File Offset: 0x00008B6A
		private void InitializeGlobalParameters()
		{
			this.InitializeCultureParameter();
			this.InitializeProsperityParameter();
			this.InitializeCombatModeParameter();
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x0000A980 File Offset: 0x00008B80
		private void InitializeCultureParameter()
		{
			MissionSoundParametersView.SoundParameterMissionCulture soundParameterMissionCulture = MissionSoundParametersView.SoundParameterMissionCulture.None;
			if (Campaign.Current != null)
			{
				Settlement currentSettlement = Settlement.CurrentSettlement;
				if (currentSettlement != null)
				{
					if (currentSettlement.IsHideout)
					{
						soundParameterMissionCulture = MissionSoundParametersView.SoundParameterMissionCulture.Bandit;
					}
					else
					{
						string stringId = currentSettlement.Culture.StringId;
						uint num = <PrivateImplementationDetails>.ComputeStringHash(stringId);
						if (num <= 2848701557U)
						{
							if (num != 744444005U)
							{
								if (num != 1759932477U)
								{
									if (num == 2848701557U)
									{
										if (stringId == "khuzait")
										{
											soundParameterMissionCulture = MissionSoundParametersView.SoundParameterMissionCulture.Khuzait;
										}
									}
								}
								else if (stringId == "battania")
								{
									soundParameterMissionCulture = MissionSoundParametersView.SoundParameterMissionCulture.Battania;
								}
							}
							else if (stringId == "empire")
							{
								soundParameterMissionCulture = MissionSoundParametersView.SoundParameterMissionCulture.Empire;
							}
						}
						else if (num <= 3015521580U)
						{
							if (num != 2894801972U)
							{
								if (num == 3015521580U)
								{
									if (stringId == "aserai")
									{
										soundParameterMissionCulture = MissionSoundParametersView.SoundParameterMissionCulture.Aserai;
									}
								}
							}
							else if (stringId == "nord")
							{
								soundParameterMissionCulture = MissionSoundParametersView.SoundParameterMissionCulture.Nord;
							}
						}
						else if (num != 3311783860U)
						{
							if (num == 4214512470U)
							{
								if (stringId == "vlandia")
								{
									soundParameterMissionCulture = MissionSoundParametersView.SoundParameterMissionCulture.Vlandia;
								}
							}
						}
						else if (stringId == "sturgia")
						{
							soundParameterMissionCulture = MissionSoundParametersView.SoundParameterMissionCulture.Sturgia;
						}
					}
				}
			}
			SoundManager.SetGlobalParameter("MissionCulture", (float)soundParameterMissionCulture);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000AAB4 File Offset: 0x00008CB4
		private void InitializeProsperityParameter()
		{
			MissionSoundParametersView.SoundParameterMissionProsperityLevel soundParameterMissionProsperityLevel = MissionSoundParametersView.SoundParameterMissionProsperityLevel.None;
			if (Campaign.Current != null && Settlement.CurrentSettlement != null)
			{
				switch (Settlement.CurrentSettlement.SettlementComponent.GetProsperityLevel())
				{
				case SettlementComponent.ProsperityLevel.Low:
					soundParameterMissionProsperityLevel = MissionSoundParametersView.SoundParameterMissionProsperityLevel.None;
					break;
				case SettlementComponent.ProsperityLevel.Mid:
					soundParameterMissionProsperityLevel = MissionSoundParametersView.SoundParameterMissionProsperityLevel.Mid;
					break;
				case SettlementComponent.ProsperityLevel.High:
					soundParameterMissionProsperityLevel = MissionSoundParametersView.SoundParameterMissionProsperityLevel.High;
					break;
				}
			}
			SoundManager.SetGlobalParameter("MissionProsperity", (float)soundParameterMissionProsperityLevel);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000AB0C File Offset: 0x00008D0C
		private void InitializeCombatModeParameter()
		{
			bool flag = base.Mission.Mode == MissionMode.Battle || base.Mission.Mode == MissionMode.Duel || base.Mission.Mode == MissionMode.Tournament;
			SoundManager.SetGlobalParameter("MissionCombatMode", (float)(flag ? 1 : 0));
		}

		// Token: 0x0400007E RID: 126
		private const string CultureParameterId = "MissionCulture";

		// Token: 0x0400007F RID: 127
		private const string ProsperityParameterId = "MissionProsperity";

		// Token: 0x04000080 RID: 128
		private const string CombatParameterId = "MissionCombatMode";

		// Token: 0x02000096 RID: 150
		public enum SoundParameterMissionCulture : short
		{
			// Token: 0x040002EE RID: 750
			None,
			// Token: 0x040002EF RID: 751
			Aserai,
			// Token: 0x040002F0 RID: 752
			Battania,
			// Token: 0x040002F1 RID: 753
			Empire,
			// Token: 0x040002F2 RID: 754
			Khuzait,
			// Token: 0x040002F3 RID: 755
			Sturgia,
			// Token: 0x040002F4 RID: 756
			Vlandia,
			// Token: 0x040002F5 RID: 757
			Nord,
			// Token: 0x040002F6 RID: 758
			ReservedA,
			// Token: 0x040002F7 RID: 759
			ReservedB,
			// Token: 0x040002F8 RID: 760
			Bandit
		}

		// Token: 0x02000097 RID: 151
		private enum SoundParameterMissionProsperityLevel : short
		{
			// Token: 0x040002FA RID: 762
			None,
			// Token: 0x040002FB RID: 763
			Low = 0,
			// Token: 0x040002FC RID: 764
			Mid,
			// Token: 0x040002FD RID: 765
			High
		}
	}
}
