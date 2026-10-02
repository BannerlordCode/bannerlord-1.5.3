using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200019D RID: 413
	public static class BannerlordConfig
	{
		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x060015B0 RID: 5552 RVA: 0x00051680 File Offset: 0x0004F880
		public static int MinBattleSize
		{
			get
			{
				return BannerlordConfig._battleSizes[0];
			}
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x060015B1 RID: 5553 RVA: 0x00051689 File Offset: 0x0004F889
		public static int MaxBattleSize
		{
			get
			{
				return BannerlordConfig._battleSizes[BannerlordConfig._battleSizes.Length - 1];
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x060015B2 RID: 5554 RVA: 0x0005169A File Offset: 0x0004F89A
		public static int MinReinforcementWaveCount
		{
			get
			{
				return BannerlordConfig._reinforcementWaveCounts[0];
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x060015B3 RID: 5555 RVA: 0x000516A3 File Offset: 0x0004F8A3
		public static int MaxReinforcementWaveCount
		{
			get
			{
				return BannerlordConfig._reinforcementWaveCounts[BannerlordConfig._reinforcementWaveCounts.Length - 1];
			}
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x000516B4 File Offset: 0x0004F8B4
		public static void Initialize()
		{
			string text = Utilities.LoadBannerlordConfigFile();
			if (string.IsNullOrEmpty(text))
			{
				BannerlordConfig.Save();
			}
			else
			{
				bool flag = false;
				string[] array = text.Split(new char[] { '\n' });
				for (int i = 0; i < array.Length; i++)
				{
					string[] array2 = array[i].Split(new char[] { '=' });
					PropertyInfo property = typeof(BannerlordConfig).GetProperty(array2[0]);
					if (property == null)
					{
						flag = true;
					}
					else
					{
						string text2 = array2[1];
						try
						{
							if (property.PropertyType == typeof(string))
							{
								string text3 = Regex.Replace(text2, "\\r", "");
								property.SetValue(null, text3);
							}
							else if (property.PropertyType == typeof(float))
							{
								float num;
								if (float.TryParse(text2, out num))
								{
									property.SetValue(null, num);
								}
								else
								{
									flag = true;
								}
							}
							else if (property.PropertyType == typeof(int))
							{
								int num2;
								if (int.TryParse(text2, out num2))
								{
									BannerlordConfig.ConfigPropertyInt customAttribute = property.GetCustomAttribute<BannerlordConfig.ConfigPropertyInt>();
									if (customAttribute == null || customAttribute.IsValidValue(num2))
									{
										property.SetValue(null, num2);
									}
									else
									{
										flag = true;
									}
								}
								else
								{
									flag = true;
								}
							}
							else if (property.PropertyType == typeof(bool))
							{
								bool flag2;
								if (bool.TryParse(text2, out flag2))
								{
									property.SetValue(null, flag2);
								}
								else
								{
									flag = true;
								}
							}
							else
							{
								flag = true;
								Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\BannerlordConfig.cs", "Initialize", 114);
							}
						}
						catch
						{
							flag = true;
						}
					}
				}
				if (flag)
				{
					BannerlordConfig.Save();
				}
				MBAPI.IMBBannerlordConfig.ValidateOptions();
			}
			MBTextManager.TryChangeVoiceLanguage(BannerlordConfig.VoiceLanguage);
			MBTextManager.ChangeLanguage(BannerlordConfig.Language);
			MBTextManager.LocalizationDebugMode = NativeConfig.LocalizationDebugMode;
		}

		// Token: 0x060015B5 RID: 5557 RVA: 0x000518B8 File Offset: 0x0004FAB8
		public static SaveResult Save()
		{
			Dictionary<PropertyInfo, object> dictionary = new Dictionary<PropertyInfo, object>();
			foreach (PropertyInfo propertyInfo in typeof(BannerlordConfig).GetProperties())
			{
				if (propertyInfo.GetCustomAttribute<BannerlordConfig.ConfigProperty>() != null)
				{
					dictionary.Add(propertyInfo, propertyInfo.GetValue(null, null));
				}
			}
			string text = "";
			foreach (KeyValuePair<PropertyInfo, object> keyValuePair in dictionary)
			{
				text = string.Concat(new string[]
				{
					text,
					keyValuePair.Key.Name,
					"=",
					keyValuePair.Value.ToString(),
					"\n"
				});
			}
			SaveResult saveResult = Utilities.SaveConfigFile(text);
			MBAPI.IMBBannerlordConfig.ValidateOptions();
			return saveResult;
		}

		// Token: 0x060015B6 RID: 5558 RVA: 0x00051998 File Offset: 0x0004FB98
		public static float GetDamageToPlayerMultiplier()
		{
			switch (BannerlordConfig.PlayerReceivedDamageDifficulty)
			{
			case 0:
				return 0.25f;
			case 1:
				return 0.5f;
			case 2:
				return 1f;
			default:
				return 1f;
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x060015B7 RID: 5559 RVA: 0x000519D6 File Offset: 0x0004FBD6
		public static string DefaultLanguage
		{
			get
			{
				return BannerlordConfig.GetDefaultLanguage();
			}
		}

		// Token: 0x060015B8 RID: 5560 RVA: 0x000519DD File Offset: 0x0004FBDD
		public static int GetRealBattleSize()
		{
			return BannerlordConfig._battleSizes[BannerlordConfig.BattleSize];
		}

		// Token: 0x060015B9 RID: 5561 RVA: 0x000519EA File Offset: 0x0004FBEA
		public static int GetRealBattleSizeForSiege()
		{
			return BannerlordConfig._siegeBattleSizes[BannerlordConfig.BattleSize];
		}

		// Token: 0x060015BA RID: 5562 RVA: 0x000519F7 File Offset: 0x0004FBF7
		public static int GetRealBattleSizeForNaval()
		{
			return BannerlordConfig._battleSizes[BannerlordConfig.BattleSize];
		}

		// Token: 0x060015BB RID: 5563 RVA: 0x00051A04 File Offset: 0x0004FC04
		public static int GetReinforcementWaveCount()
		{
			return BannerlordConfig._reinforcementWaveCounts[BannerlordConfig.ReinforcementWaveCount];
		}

		// Token: 0x060015BC RID: 5564 RVA: 0x00051A11 File Offset: 0x0004FC11
		public static int GetRealBattleSizeForSallyOut()
		{
			return BannerlordConfig._sallyOutBattleSizes[BannerlordConfig.BattleSize];
		}

		// Token: 0x060015BD RID: 5565 RVA: 0x00051A1E File Offset: 0x0004FC1E
		private static string GetDefaultLanguage()
		{
			return LocalizedTextManager.GetLocalizationCodeOfISOLanguageCode(Utilities.GetSystemLanguage());
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x060015BE RID: 5566 RVA: 0x00051A2A File Offset: 0x0004FC2A
		// (set) Token: 0x060015BF RID: 5567 RVA: 0x00051A34 File Offset: 0x0004FC34
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static string Language
		{
			get
			{
				return BannerlordConfig._language;
			}
			set
			{
				if (BannerlordConfig._language != value)
				{
					if (MBTextManager.LanguageExistsInCurrentConfiguration(value, NativeConfig.IsDevelopmentMode) && MBTextManager.ChangeLanguage(value))
					{
						BannerlordConfig._language = value;
					}
					else if (MBTextManager.ChangeLanguage("English"))
					{
						BannerlordConfig._language = "English";
					}
					else
					{
						Debug.FailedAssert("Language cannot be set!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\BannerlordConfig.cs", "Language", 391);
					}
					MBTextManager.LocalizationDebugMode = NativeConfig.LocalizationDebugMode;
				}
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x060015C0 RID: 5568 RVA: 0x00051AA6 File Offset: 0x0004FCA6
		// (set) Token: 0x060015C1 RID: 5569 RVA: 0x00051AB0 File Offset: 0x0004FCB0
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static string VoiceLanguage
		{
			get
			{
				return BannerlordConfig._voiceLanguage;
			}
			set
			{
				if (BannerlordConfig._voiceLanguage != value)
				{
					if (MBTextManager.LanguageExistsInCurrentConfiguration(value, NativeConfig.IsDevelopmentMode) && MBTextManager.TryChangeVoiceLanguage(value))
					{
						BannerlordConfig._voiceLanguage = value;
						return;
					}
					if (MBTextManager.TryChangeVoiceLanguage("English"))
					{
						BannerlordConfig._voiceLanguage = "English";
						return;
					}
					Debug.FailedAssert("Voice Language cannot be set!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\BannerlordConfig.cs", "VoiceLanguage", 418);
				}
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x060015C2 RID: 5570 RVA: 0x00051B16 File Offset: 0x0004FD16
		// (set) Token: 0x060015C3 RID: 5571 RVA: 0x00051B1D File Offset: 0x0004FD1D
		[BannerlordConfig.ConfigPropertyInt(new int[] { 0, 1, 2 }, false)]
		public static int MapDoubleClickBehavior { get; set; } = BannerlordConfig.MapDoubleClickBehavior;

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x060015C4 RID: 5572 RVA: 0x00051B25 File Offset: 0x0004FD25
		// (set) Token: 0x060015C5 RID: 5573 RVA: 0x00051B2C File Offset: 0x0004FD2C
		[BannerlordConfig.ConfigPropertyInt(new int[] { 0, 1, 2 }, false)]
		public static int PlayerReceivedDamageDifficulty { get; set; } = 0;

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x060015C6 RID: 5574 RVA: 0x00051B34 File Offset: 0x0004FD34
		// (set) Token: 0x060015C7 RID: 5575 RVA: 0x00051B3B File Offset: 0x0004FD3B
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool GyroOverrideForAttackDefend { get; set; } = false;

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x060015C8 RID: 5576 RVA: 0x00051B43 File Offset: 0x0004FD43
		// (set) Token: 0x060015C9 RID: 5577 RVA: 0x00051B4A File Offset: 0x0004FD4A
		[BannerlordConfig.ConfigPropertyInt(new int[] { 0, 1, 2 }, false)]
		public static int AttackDirectionControl { get; set; } = 1;

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x060015CA RID: 5578 RVA: 0x00051B52 File Offset: 0x0004FD52
		// (set) Token: 0x060015CB RID: 5579 RVA: 0x00051B59 File Offset: 0x0004FD59
		[BannerlordConfig.ConfigPropertyInt(new int[] { 0, 1, 2 }, false)]
		public static int DefendDirectionControl { get; set; } = 0;

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x060015CC RID: 5580 RVA: 0x00051B61 File Offset: 0x0004FD61
		// (set) Token: 0x060015CD RID: 5581 RVA: 0x00051B68 File Offset: 0x0004FD68
		[BannerlordConfig.ConfigPropertyInt(new int[] { 0, 1, 2, 3, 4, 5 }, false)]
		public static int NumberOfCorpses
		{
			get
			{
				return BannerlordConfig._numberOfCorpses;
			}
			set
			{
				BannerlordConfig._numberOfCorpses = value;
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x060015CE RID: 5582 RVA: 0x00051B70 File Offset: 0x0004FD70
		// (set) Token: 0x060015CF RID: 5583 RVA: 0x00051B77 File Offset: 0x0004FD77
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool ShowBlood { get; set; } = true;

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x060015D0 RID: 5584 RVA: 0x00051B7F File Offset: 0x0004FD7F
		// (set) Token: 0x060015D1 RID: 5585 RVA: 0x00051B86 File Offset: 0x0004FD86
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool DisplayAttackDirection { get; set; } = true;

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x060015D2 RID: 5586 RVA: 0x00051B8E File Offset: 0x0004FD8E
		// (set) Token: 0x060015D3 RID: 5587 RVA: 0x00051B95 File Offset: 0x0004FD95
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool DisplayTargetingReticule { get; set; } = true;

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x060015D4 RID: 5588 RVA: 0x00051B9D File Offset: 0x0004FD9D
		// (set) Token: 0x060015D5 RID: 5589 RVA: 0x00051BA4 File Offset: 0x0004FDA4
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool ForceVSyncInMenus { get; set; } = true;

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x060015D6 RID: 5590 RVA: 0x00051BAC File Offset: 0x0004FDAC
		// (set) Token: 0x060015D7 RID: 5591 RVA: 0x00051BB3 File Offset: 0x0004FDB3
		[BannerlordConfig.ConfigPropertyInt(new int[] { 0, 1, 2, 3, 4, 5, 6 }, false)]
		public static int BattleSize
		{
			get
			{
				return BannerlordConfig._battleSize;
			}
			set
			{
				BannerlordConfig._battleSize = value;
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x060015D8 RID: 5592 RVA: 0x00051BBB File Offset: 0x0004FDBB
		// (set) Token: 0x060015D9 RID: 5593 RVA: 0x00051BC2 File Offset: 0x0004FDC2
		[BannerlordConfig.ConfigPropertyInt(new int[] { 0, 1, 2, 3 }, false)]
		public static int ReinforcementWaveCount { get; set; } = 3;

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x060015DA RID: 5594 RVA: 0x00051BCA File Offset: 0x0004FDCA
		public static float CivilianAgentCount
		{
			get
			{
				return (float)BannerlordConfig.GetRealBattleSize() * 0.5f;
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x060015DB RID: 5595 RVA: 0x00051BD8 File Offset: 0x0004FDD8
		// (set) Token: 0x060015DC RID: 5596 RVA: 0x00051BDF File Offset: 0x0004FDDF
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static float FirstPersonFov { get; set; } = 65f;

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x060015DD RID: 5597 RVA: 0x00051BE7 File Offset: 0x0004FDE7
		// (set) Token: 0x060015DE RID: 5598 RVA: 0x00051BEE File Offset: 0x0004FDEE
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static float UIScale { get; set; } = 1f;

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x060015DF RID: 5599 RVA: 0x00051BF6 File Offset: 0x0004FDF6
		// (set) Token: 0x060015E0 RID: 5600 RVA: 0x00051BFD File Offset: 0x0004FDFD
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static float CombatCameraDistance { get; set; } = 1f;

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x060015E1 RID: 5601 RVA: 0x00051C05 File Offset: 0x0004FE05
		// (set) Token: 0x060015E2 RID: 5602 RVA: 0x00051C0C File Offset: 0x0004FE0C
		[BannerlordConfig.ConfigPropertyInt(new int[] { 0, 1, 2, 3 }, false)]
		public static int TurnCameraWithHorseInFirstPerson { get; set; } = 2;

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x060015E3 RID: 5603 RVA: 0x00051C14 File Offset: 0x0004FE14
		// (set) Token: 0x060015E4 RID: 5604 RVA: 0x00051C1B File Offset: 0x0004FE1B
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool ReportDamage { get; set; } = true;

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x060015E5 RID: 5605 RVA: 0x00051C23 File Offset: 0x0004FE23
		// (set) Token: 0x060015E6 RID: 5606 RVA: 0x00051C2A File Offset: 0x0004FE2A
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool ReportBark { get; set; } = true;

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x060015E7 RID: 5607 RVA: 0x00051C32 File Offset: 0x0004FE32
		// (set) Token: 0x060015E8 RID: 5608 RVA: 0x00051C39 File Offset: 0x0004FE39
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool LockTarget { get; set; } = false;

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x060015E9 RID: 5609 RVA: 0x00051C41 File Offset: 0x0004FE41
		// (set) Token: 0x060015EA RID: 5610 RVA: 0x00051C48 File Offset: 0x0004FE48
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool EnableTutorialHints { get; set; } = true;

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x060015EB RID: 5611 RVA: 0x00051C50 File Offset: 0x0004FE50
		// (set) Token: 0x060015EC RID: 5612 RVA: 0x00051C57 File Offset: 0x0004FE57
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static int AutoSaveInterval
		{
			get
			{
				return BannerlordConfig._autoSaveInterval;
			}
			set
			{
				if (value == 4)
				{
					BannerlordConfig._autoSaveInterval = -1;
					return;
				}
				BannerlordConfig._autoSaveInterval = value;
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x060015ED RID: 5613 RVA: 0x00051C6A File Offset: 0x0004FE6A
		// (set) Token: 0x060015EE RID: 5614 RVA: 0x00051C71 File Offset: 0x0004FE71
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static float FriendlyTroopsBannerOpacity { get; set; } = 1f;

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x060015EF RID: 5615 RVA: 0x00051C79 File Offset: 0x0004FE79
		// (set) Token: 0x060015F0 RID: 5616 RVA: 0x00051C80 File Offset: 0x0004FE80
		[BannerlordConfig.ConfigPropertyInt(new int[] { 0, 1, 2 }, false)]
		public static int AlwaysShowFriendlyTroopBannersType { get; set; } = 1;

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x060015F1 RID: 5617 RVA: 0x00051C88 File Offset: 0x0004FE88
		// (set) Token: 0x060015F2 RID: 5618 RVA: 0x00051C8F File Offset: 0x0004FE8F
		[BannerlordConfig.ConfigPropertyInt(new int[] { 0, 1, 2 }, false)]
		public static int KillFeedVisualType { get; set; } = 1;

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x060015F3 RID: 5619 RVA: 0x00051C97 File Offset: 0x0004FE97
		// (set) Token: 0x060015F4 RID: 5620 RVA: 0x00051C9E File Offset: 0x0004FE9E
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool ShowFormationDistances { get; set; } = false;

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x060015F5 RID: 5621 RVA: 0x00051CA6 File Offset: 0x0004FEA6
		// (set) Token: 0x060015F6 RID: 5622 RVA: 0x00051CAD File Offset: 0x0004FEAD
		[BannerlordConfig.ConfigPropertyInt(new int[] { 0, 1, 2 }, false)]
		public static int AutoTrackAttackedSettlements { get; set; } = 0;

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x060015F7 RID: 5623 RVA: 0x00051CB5 File Offset: 0x0004FEB5
		// (set) Token: 0x060015F8 RID: 5624 RVA: 0x00051CBC File Offset: 0x0004FEBC
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool ReportPersonalDamage { get; set; } = true;

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x060015F9 RID: 5625 RVA: 0x00051CC4 File Offset: 0x0004FEC4
		// (set) Token: 0x060015FA RID: 5626 RVA: 0x00051CCB File Offset: 0x0004FECB
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool SlowDownOnOrder { get; set; } = true;

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x060015FB RID: 5627 RVA: 0x00051CD3 File Offset: 0x0004FED3
		// (set) Token: 0x060015FC RID: 5628 RVA: 0x00051CDA File Offset: 0x0004FEDA
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool StopGameOnFocusLost
		{
			get
			{
				return BannerlordConfig._stopGameOnFocusLost;
			}
			set
			{
				BannerlordConfig._stopGameOnFocusLost = value;
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x060015FD RID: 5629 RVA: 0x00051CE2 File Offset: 0x0004FEE2
		// (set) Token: 0x060015FE RID: 5630 RVA: 0x00051CE9 File Offset: 0x0004FEE9
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool ReportExperience { get; set; } = true;

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x060015FF RID: 5631 RVA: 0x00051CF1 File Offset: 0x0004FEF1
		// (set) Token: 0x06001600 RID: 5632 RVA: 0x00051CF8 File Offset: 0x0004FEF8
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool EnableDamageTakenVisuals { get; set; } = true;

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06001601 RID: 5633 RVA: 0x00051D00 File Offset: 0x0004FF00
		// (set) Token: 0x06001602 RID: 5634 RVA: 0x00051D07 File Offset: 0x0004FF07
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool EnableVerticalAimCorrection { get; set; } = true;

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06001603 RID: 5635 RVA: 0x00051D0F File Offset: 0x0004FF0F
		// (set) Token: 0x06001604 RID: 5636 RVA: 0x00051D16 File Offset: 0x0004FF16
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static float ZoomSensitivityModifier { get; set; } = 0.66666f;

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06001605 RID: 5637 RVA: 0x00051D1E File Offset: 0x0004FF1E
		// (set) Token: 0x06001606 RID: 5638 RVA: 0x00051D25 File Offset: 0x0004FF25
		[BannerlordConfig.ConfigPropertyInt(new int[] { 0, 1 }, false)]
		public static int CrosshairType { get; set; } = 0;

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06001607 RID: 5639 RVA: 0x00051D2D File Offset: 0x0004FF2D
		// (set) Token: 0x06001608 RID: 5640 RVA: 0x00051D34 File Offset: 0x0004FF34
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool EnableGenericAvatars { get; set; } = false;

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06001609 RID: 5641 RVA: 0x00051D3C File Offset: 0x0004FF3C
		// (set) Token: 0x0600160A RID: 5642 RVA: 0x00051D43 File Offset: 0x0004FF43
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool EnableGenericNames { get; set; } = false;

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x0600160B RID: 5643 RVA: 0x00051D4B File Offset: 0x0004FF4B
		// (set) Token: 0x0600160C RID: 5644 RVA: 0x00051D52 File Offset: 0x0004FF52
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool HideFullServers { get; set; } = false;

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x0600160D RID: 5645 RVA: 0x00051D5A File Offset: 0x0004FF5A
		// (set) Token: 0x0600160E RID: 5646 RVA: 0x00051D61 File Offset: 0x0004FF61
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool HideEmptyServers { get; set; } = false;

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x0600160F RID: 5647 RVA: 0x00051D69 File Offset: 0x0004FF69
		// (set) Token: 0x06001610 RID: 5648 RVA: 0x00051D70 File Offset: 0x0004FF70
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool HidePasswordProtectedServers { get; set; } = false;

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x06001611 RID: 5649 RVA: 0x00051D78 File Offset: 0x0004FF78
		// (set) Token: 0x06001612 RID: 5650 RVA: 0x00051D7F File Offset: 0x0004FF7F
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool HideUnofficialServers { get; set; } = false;

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06001613 RID: 5651 RVA: 0x00051D87 File Offset: 0x0004FF87
		// (set) Token: 0x06001614 RID: 5652 RVA: 0x00051D8E File Offset: 0x0004FF8E
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool HideModuleIncompatibleServers { get; set; } = false;

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06001615 RID: 5653 RVA: 0x00051D96 File Offset: 0x0004FF96
		// (set) Token: 0x06001616 RID: 5654 RVA: 0x00051D9D File Offset: 0x0004FF9D
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool ShowOnlyFavoriteServers { get; set; } = false;

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06001617 RID: 5655 RVA: 0x00051DA5 File Offset: 0x0004FFA5
		// (set) Token: 0x06001618 RID: 5656 RVA: 0x00051DAC File Offset: 0x0004FFAC
		[BannerlordConfig.ConfigPropertyInt(new int[] { 0, 1 }, false)]
		public static int OrderType
		{
			get
			{
				return BannerlordConfig._orderType;
			}
			set
			{
				BannerlordConfig._orderType = value;
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06001619 RID: 5657 RVA: 0x00051DB4 File Offset: 0x0004FFB4
		// (set) Token: 0x0600161A RID: 5658 RVA: 0x00051DBB File Offset: 0x0004FFBB
		[BannerlordConfig.ConfigPropertyInt(new int[] { 0, 1 }, false)]
		public static int OrderLayoutType
		{
			get
			{
				return BannerlordConfig._orderLayoutType;
			}
			set
			{
				BannerlordConfig._orderLayoutType = value;
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x0600161B RID: 5659 RVA: 0x00051DC3 File Offset: 0x0004FFC3
		// (set) Token: 0x0600161C RID: 5660 RVA: 0x00051DCA File Offset: 0x0004FFCA
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool EnableVoiceChat { get; set; } = true;

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x0600161D RID: 5661 RVA: 0x00051DD2 File Offset: 0x0004FFD2
		// (set) Token: 0x0600161E RID: 5662 RVA: 0x00051DD9 File Offset: 0x0004FFD9
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool EnableDeathIcon { get; set; } = true;

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x0600161F RID: 5663 RVA: 0x00051DE1 File Offset: 0x0004FFE1
		// (set) Token: 0x06001620 RID: 5664 RVA: 0x00051DE8 File Offset: 0x0004FFE8
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool EnableNetworkAlertIcons { get; set; } = true;

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06001621 RID: 5665 RVA: 0x00051DF0 File Offset: 0x0004FFF0
		// (set) Token: 0x06001622 RID: 5666 RVA: 0x00051DF7 File Offset: 0x0004FFF7
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool EnableSingleplayerChatBox { get; set; } = true;

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06001623 RID: 5667 RVA: 0x00051DFF File Offset: 0x0004FFFF
		// (set) Token: 0x06001624 RID: 5668 RVA: 0x00051E06 File Offset: 0x00050006
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool EnableMultiplayerChatBox { get; set; } = true;

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06001625 RID: 5669 RVA: 0x00051E0E File Offset: 0x0005000E
		// (set) Token: 0x06001626 RID: 5670 RVA: 0x00051E15 File Offset: 0x00050015
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static float ChatBoxSizeX { get; set; } = 495f;

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06001627 RID: 5671 RVA: 0x00051E1D File Offset: 0x0005001D
		// (set) Token: 0x06001628 RID: 5672 RVA: 0x00051E24 File Offset: 0x00050024
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static float ChatBoxSizeY { get; set; } = 340f;

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06001629 RID: 5673 RVA: 0x00051E2C File Offset: 0x0005002C
		// (set) Token: 0x0600162A RID: 5674 RVA: 0x00051E33 File Offset: 0x00050033
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static string LatestSaveGameName { get; set; } = string.Empty;

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x0600162B RID: 5675 RVA: 0x00051E3B File Offset: 0x0005003B
		// (set) Token: 0x0600162C RID: 5676 RVA: 0x00051E42 File Offset: 0x00050042
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool HideBattleUI { get; set; } = false;

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x0600162D RID: 5677 RVA: 0x00051E4A File Offset: 0x0005004A
		// (set) Token: 0x0600162E RID: 5678 RVA: 0x00051E51 File Offset: 0x00050051
		[BannerlordConfig.ConfigPropertyInt(new int[] { 0, 1, 2, 3 }, false)]
		public static int UnitSpawnPrioritization { get; set; } = 0;

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x0600162F RID: 5679 RVA: 0x00051E59 File Offset: 0x00050059
		// (set) Token: 0x06001630 RID: 5680 RVA: 0x00051E60 File Offset: 0x00050060
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool IAPNoticeConfirmed { get; set; } = false;

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06001631 RID: 5681 RVA: 0x00051E68 File Offset: 0x00050068
		// (set) Token: 0x06001632 RID: 5682 RVA: 0x00051E6F File Offset: 0x0005006F
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool CompletedKingPlaythrough { get; set; } = false;

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06001633 RID: 5683 RVA: 0x00051E77 File Offset: 0x00050077
		// (set) Token: 0x06001634 RID: 5684 RVA: 0x00051E7E File Offset: 0x0005007E
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool CompletedVassalPlaythrough { get; set; } = false;

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06001635 RID: 5685 RVA: 0x00051E86 File Offset: 0x00050086
		// (set) Token: 0x06001636 RID: 5686 RVA: 0x00051E8D File Offset: 0x0005008D
		[BannerlordConfig.ConfigPropertyUnbounded]
		public static bool CompletedMercenaryPlaythrough { get; set; } = false;

		// Token: 0x040006B2 RID: 1714
		private static int[] _battleSizes = new int[] { 200, 300, 400, 500, 600, 800, 1000 };

		// Token: 0x040006B3 RID: 1715
		private static int[] _siegeBattleSizes = new int[] { 150, 230, 320, 425, 540, 625, 1000 };

		// Token: 0x040006B4 RID: 1716
		private static int[] _sallyOutBattleSizes = new int[] { 150, 200, 240, 280, 320, 360, 400 };

		// Token: 0x040006B5 RID: 1717
		private static int[] _reinforcementWaveCounts = new int[] { 3, 4, 5, 0 };

		// Token: 0x040006B6 RID: 1718
		public const int MaxCorpseCount = 1021;

		// Token: 0x040006B7 RID: 1719
		public static double SiegeBattleSizeMultiplier = 0.8;

		// Token: 0x040006B8 RID: 1720
		public const int DefaultMapDoubleClickBehavior = 0;

		// Token: 0x040006B9 RID: 1721
		public const int DefaultPlayerReceviedDamageDifficulty = 0;

		// Token: 0x040006BA RID: 1722
		public const bool DefaultGyroOverrideForAttackDefend = false;

		// Token: 0x040006BB RID: 1723
		public const int DefaultAttackDirectionControl = 1;

		// Token: 0x040006BC RID: 1724
		public const int DefaultDefendDirectionControl = 0;

		// Token: 0x040006BD RID: 1725
		public const int DefaultNumberOfCorpses = 3;

		// Token: 0x040006BE RID: 1726
		public const bool DefaultShowBlood = true;

		// Token: 0x040006BF RID: 1727
		public const bool DefaultDisplayAttackDirection = true;

		// Token: 0x040006C0 RID: 1728
		public const bool DefaultDisplayTargetingReticule = true;

		// Token: 0x040006C1 RID: 1729
		public const bool DefaultForceVSyncInMenus = true;

		// Token: 0x040006C2 RID: 1730
		public const int DefaultBattleSize = 2;

		// Token: 0x040006C3 RID: 1731
		public const int DefaultReinforcementWaveCount = 3;

		// Token: 0x040006C4 RID: 1732
		public const float DefaultBattleSizeMultiplier = 0.5f;

		// Token: 0x040006C5 RID: 1733
		public const float DefaultFirstPersonFov = 65f;

		// Token: 0x040006C6 RID: 1734
		public const float DefaultUIScale = 1f;

		// Token: 0x040006C7 RID: 1735
		public const float DefaultCombatCameraDistance = 1f;

		// Token: 0x040006C8 RID: 1736
		public const int DefaultCombatAI = 0;

		// Token: 0x040006C9 RID: 1737
		public const int DefaultTurnCameraWithHorseInFirstPerson = 2;

		// Token: 0x040006CA RID: 1738
		public const int DefaultAutoSaveInterval = 30;

		// Token: 0x040006CB RID: 1739
		public const float DefaultFriendlyTroopsBannerOpacity = 1f;

		// Token: 0x040006CC RID: 1740
		public const int DefaultAlwaysShowFriendlyTroopBannersType = 1;

		// Token: 0x040006CD RID: 1741
		public const bool DefaultShowFormationDistances = false;

		// Token: 0x040006CE RID: 1742
		public const bool DefaultReportDamage = true;

		// Token: 0x040006CF RID: 1743
		public const bool DefaultReportBark = true;

		// Token: 0x040006D0 RID: 1744
		public const bool DefaultEnableTutorialHints = true;

		// Token: 0x040006D1 RID: 1745
		public const int DefaultKillFeedVisualType = 1;

		// Token: 0x040006D2 RID: 1746
		public const int DefaultAutoTrackAttackedSettlements = 0;

		// Token: 0x040006D3 RID: 1747
		public const bool DefaultReportPersonalDamage = true;

		// Token: 0x040006D4 RID: 1748
		public const bool DefaultStopGameOnFocusLost = true;

		// Token: 0x040006D5 RID: 1749
		public const bool DefaultSlowDownOnOrder = true;

		// Token: 0x040006D6 RID: 1750
		public const bool DefaultReportExperience = true;

		// Token: 0x040006D7 RID: 1751
		public const bool DefaultEnableDamageTakenVisuals = true;

		// Token: 0x040006D8 RID: 1752
		public const bool DefaultEnableVoiceChat = true;

		// Token: 0x040006D9 RID: 1753
		public const bool DefaultEnableDeathIcon = true;

		// Token: 0x040006DA RID: 1754
		public const bool DefaultEnableNetworkAlertIcons = true;

		// Token: 0x040006DB RID: 1755
		public const bool DefaultEnableVerticalAimCorrection = true;

		// Token: 0x040006DC RID: 1756
		public const float DefaultZoomSensitivityModifier = 0.66666f;

		// Token: 0x040006DD RID: 1757
		public const bool DefaultSingleplayerEnableChatBox = true;

		// Token: 0x040006DE RID: 1758
		public const bool DefaultMultiplayerEnableChatBox = true;

		// Token: 0x040006DF RID: 1759
		public const float DefaultChatBoxSizeX = 495f;

		// Token: 0x040006E0 RID: 1760
		public const float DefaultChatBoxSizeY = 340f;

		// Token: 0x040006E1 RID: 1761
		public const int DefaultCrosshairType = 0;

		// Token: 0x040006E2 RID: 1762
		public const bool DefaultEnableGenericAvatars = false;

		// Token: 0x040006E3 RID: 1763
		public const bool DefaultEnableGenericNames = false;

		// Token: 0x040006E4 RID: 1764
		public const bool DefaultHideFullServers = false;

		// Token: 0x040006E5 RID: 1765
		public const bool DefaultHideEmptyServers = false;

		// Token: 0x040006E6 RID: 1766
		public const bool DefaultHidePasswordProtectedServers = false;

		// Token: 0x040006E7 RID: 1767
		public const bool DefaultHideUnofficialServers = false;

		// Token: 0x040006E8 RID: 1768
		public const bool DefaultHideModuleIncompatibleServers = false;

		// Token: 0x040006E9 RID: 1769
		public const bool DefaultShowOnlyFavoriteServers = false;

		// Token: 0x040006EA RID: 1770
		public const int DefaultOrderLayoutType = 0;

		// Token: 0x040006EB RID: 1771
		public const bool DefaultHideBattleUI = false;

		// Token: 0x040006EC RID: 1772
		public const int DefaultUnitSpawnPrioritization = 0;

		// Token: 0x040006ED RID: 1773
		public const int DefaultOrderType = 0;

		// Token: 0x040006EE RID: 1774
		public const bool DefaultLockTarget = false;

		// Token: 0x040006EF RID: 1775
		private static string _language = BannerlordConfig.DefaultLanguage;

		// Token: 0x040006F0 RID: 1776
		private static string _voiceLanguage = BannerlordConfig.DefaultLanguage;

		// Token: 0x040006F6 RID: 1782
		private static int _numberOfCorpses = 3;

		// Token: 0x040006FB RID: 1787
		private static int _battleSize = 2;

		// Token: 0x04000705 RID: 1797
		private static int _autoSaveInterval = 30;

		// Token: 0x0400070D RID: 1805
		private static bool _stopGameOnFocusLost = true;

		// Token: 0x0400071B RID: 1819
		private static int _orderType = 0;

		// Token: 0x0400071C RID: 1820
		private static int _orderLayoutType = 0;

		// Token: 0x020004EE RID: 1262
		private interface IConfigPropertyBoundChecker<T>
		{
		}

		// Token: 0x020004EF RID: 1263
		private abstract class ConfigProperty : Attribute
		{
		}

		// Token: 0x020004F0 RID: 1264
		private sealed class ConfigPropertyInt : BannerlordConfig.ConfigProperty
		{
			// Token: 0x06003C0E RID: 15374 RVA: 0x000F1CFC File Offset: 0x000EFEFC
			public ConfigPropertyInt(int[] possibleValues, bool isRange = false)
			{
				this._possibleValues = possibleValues;
				this._isRange = isRange;
				bool isRange2 = this._isRange;
			}

			// Token: 0x06003C0F RID: 15375 RVA: 0x000F1D1C File Offset: 0x000EFF1C
			public bool IsValidValue(int value)
			{
				if (this._isRange)
				{
					return value >= this._possibleValues[0] && value <= this._possibleValues[1];
				}
				int[] possibleValues = this._possibleValues;
				for (int i = 0; i < possibleValues.Length; i++)
				{
					if (possibleValues[i] == value)
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x04001CC6 RID: 7366
			private int[] _possibleValues;

			// Token: 0x04001CC7 RID: 7367
			private bool _isRange;
		}

		// Token: 0x020004F1 RID: 1265
		private sealed class ConfigPropertyUnbounded : BannerlordConfig.ConfigProperty
		{
		}
	}
}
