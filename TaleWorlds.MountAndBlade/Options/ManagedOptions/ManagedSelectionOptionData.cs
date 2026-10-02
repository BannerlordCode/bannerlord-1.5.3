using System;
using System.Collections.Generic;
using TaleWorlds.Engine;
using TaleWorlds.Engine.Options;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Options.ManagedOptions
{
	// Token: 0x020003A6 RID: 934
	public class ManagedSelectionOptionData : ManagedOptionData, ISelectionOptionData, IOptionData
	{
		// Token: 0x06003596 RID: 13718 RVA: 0x000DD6E4 File Offset: 0x000DB8E4
		public ManagedSelectionOptionData(ManagedOptions.ManagedOptionsType type)
			: base(type)
		{
			this._selectableOptionsLimit = ManagedSelectionOptionData.GetOptionsLimit(type);
			this._selectableOptionNames = ManagedSelectionOptionData.GetOptionNames(type);
		}

		// Token: 0x06003597 RID: 13719 RVA: 0x000DD705 File Offset: 0x000DB905
		public int GetSelectableOptionsLimit()
		{
			return this._selectableOptionsLimit;
		}

		// Token: 0x06003598 RID: 13720 RVA: 0x000DD70D File Offset: 0x000DB90D
		public IEnumerable<SelectionData> GetSelectableOptionNames()
		{
			return this._selectableOptionNames;
		}

		// Token: 0x06003599 RID: 13721 RVA: 0x000DD718 File Offset: 0x000DB918
		public static int GetOptionsLimit(ManagedOptions.ManagedOptionsType optionType)
		{
			if (optionType <= ManagedOptions.ManagedOptionsType.ReportCasualtiesType)
			{
				switch (optionType)
				{
				case ManagedOptions.ManagedOptionsType.Language:
					return LocalizedTextManager.GetLanguageIds(NativeConfig.IsDevelopmentMode).Count;
				case ManagedOptions.ManagedOptionsType.GyroOverrideForAttackDefend:
				case ManagedOptions.ManagedOptionsType.ShowBlood:
				case ManagedOptions.ManagedOptionsType.ShowAttackDirection:
				case ManagedOptions.ManagedOptionsType.ShowTargetingReticle:
				case ManagedOptions.ManagedOptionsType.AutoSaveInterval:
				case ManagedOptions.ManagedOptionsType.FriendlyTroopsBannerOpacity:
					break;
				case ManagedOptions.ManagedOptionsType.ControlBlockDirection:
					return 3;
				case ManagedOptions.ManagedOptionsType.ControlAttackDirection:
					return 3;
				case ManagedOptions.ManagedOptionsType.NumberOfCorpses:
					return 6;
				case ManagedOptions.ManagedOptionsType.BattleSize:
					return 7;
				case ManagedOptions.ManagedOptionsType.ReinforcementWaveCount:
					return 4;
				case ManagedOptions.ManagedOptionsType.TurnCameraWithHorseInFirstPerson:
					return 4;
				case ManagedOptions.ManagedOptionsType.AlwaysShowFriendlyTroopBannersType:
					return 3;
				default:
					if (optionType == ManagedOptions.ManagedOptionsType.ReportCasualtiesType)
					{
						return 3;
					}
					break;
				}
			}
			else
			{
				switch (optionType)
				{
				case ManagedOptions.ManagedOptionsType.CrosshairType:
					return 2;
				case ManagedOptions.ManagedOptionsType.EnableGenericAvatars:
				case ManagedOptions.ManagedOptionsType.EnableGenericNames:
					break;
				case ManagedOptions.ManagedOptionsType.OrderType:
					return 2;
				case ManagedOptions.ManagedOptionsType.OrderLayoutType:
					return 2;
				case ManagedOptions.ManagedOptionsType.AutoTrackAttackedSettlements:
					return 3;
				default:
					switch (optionType)
					{
					case ManagedOptions.ManagedOptionsType.UnitSpawnPrioritization:
						return 4;
					case ManagedOptions.ManagedOptionsType.VoiceLanguage:
						return LocalizedVoiceManager.GetVoiceLanguageIds().Count;
					case ManagedOptions.ManagedOptionsType.PlayerReceivedDamageDifficulty:
						return 3;
					case ManagedOptions.ManagedOptionsType.MapDoubleClickBehavior:
						return 3;
					}
					break;
				}
			}
			return 0;
		}

		// Token: 0x0600359A RID: 13722 RVA: 0x000DD7ED File Offset: 0x000DB9ED
		private static IEnumerable<SelectionData> GetOptionNames(ManagedOptions.ManagedOptionsType type)
		{
			if (type == ManagedOptions.ManagedOptionsType.Language)
			{
				List<string> languageIds = LocalizedTextManager.GetLanguageIds(NativeConfig.IsDevelopmentMode);
				int num;
				for (int i = 0; i < languageIds.Count; i = num + 1)
				{
					yield return new SelectionData(false, LocalizedTextManager.GetLanguageTitle(languageIds[i]));
					num = i;
				}
				languageIds = null;
			}
			else if (type == ManagedOptions.ManagedOptionsType.VoiceLanguage)
			{
				List<string> languageIds = LocalizedVoiceManager.GetVoiceLanguageIds();
				int num;
				for (int i = 0; i < languageIds.Count; i = num + 1)
				{
					yield return new SelectionData(false, LocalizedTextManager.GetLanguageTitle(languageIds[i]));
					num = i;
				}
				languageIds = null;
			}
			else
			{
				int i = ManagedSelectionOptionData.GetOptionsLimit(type);
				string typeName = type.ToString();
				int num;
				for (int j = 0; j < i; j = num + 1)
				{
					yield return new SelectionData(true, "str_options_type_" + typeName + "_" + j.ToString());
					num = j;
				}
				typeName = null;
			}
			yield break;
		}

		// Token: 0x040016D7 RID: 5847
		private readonly int _selectableOptionsLimit;

		// Token: 0x040016D8 RID: 5848
		private readonly IEnumerable<SelectionData> _selectableOptionNames;
	}
}
