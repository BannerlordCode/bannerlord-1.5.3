using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001D9 RID: 473
	public static class MusicParameters
	{
		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06001C3D RID: 7229 RVA: 0x0006173E File Offset: 0x0005F93E
		public static int SmallBattleTreshold
		{
			get
			{
				return (int)MusicParameters._parameters[0];
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x06001C3E RID: 7230 RVA: 0x00061748 File Offset: 0x0005F948
		public static int MediumBattleTreshold
		{
			get
			{
				return (int)MusicParameters._parameters[1];
			}
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06001C3F RID: 7231 RVA: 0x00061752 File Offset: 0x0005F952
		public static int LargeBattleTreshold
		{
			get
			{
				return (int)MusicParameters._parameters[2];
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06001C40 RID: 7232 RVA: 0x0006175C File Offset: 0x0005F95C
		public static float SmallBattleDistanceTreshold
		{
			get
			{
				return MusicParameters._parameters[3];
			}
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06001C41 RID: 7233 RVA: 0x00061765 File Offset: 0x0005F965
		public static float MediumBattleDistanceTreshold
		{
			get
			{
				return MusicParameters._parameters[4];
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06001C42 RID: 7234 RVA: 0x0006176E File Offset: 0x0005F96E
		public static float LargeBattleDistanceTreshold
		{
			get
			{
				return MusicParameters._parameters[5];
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06001C43 RID: 7235 RVA: 0x00061777 File Offset: 0x0005F977
		public static float MaxBattleDistanceTreshold
		{
			get
			{
				return MusicParameters._parameters[6];
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06001C44 RID: 7236 RVA: 0x00061780 File Offset: 0x0005F980
		public static float MinIntensity
		{
			get
			{
				return MusicParameters._parameters[7];
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06001C45 RID: 7237 RVA: 0x00061789 File Offset: 0x0005F989
		public static float DefaultStartIntensity
		{
			get
			{
				return MusicParameters._parameters[8];
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06001C46 RID: 7238 RVA: 0x00061792 File Offset: 0x0005F992
		public static float PlayerChargeEffectMultiplierOnIntensity
		{
			get
			{
				return MusicParameters._parameters[9];
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06001C47 RID: 7239 RVA: 0x0006179C File Offset: 0x0005F99C
		public static float BattleSizeEffectOnStartIntensity
		{
			get
			{
				return MusicParameters._parameters[10];
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06001C48 RID: 7240 RVA: 0x000617A6 File Offset: 0x0005F9A6
		public static float RandomEffectMultiplierOnStartIntensity
		{
			get
			{
				return MusicParameters._parameters[11];
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06001C49 RID: 7241 RVA: 0x000617B0 File Offset: 0x0005F9B0
		public static float FriendlyTroopDeadEffectOnIntensity
		{
			get
			{
				return MusicParameters._parameters[12];
			}
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06001C4A RID: 7242 RVA: 0x000617BA File Offset: 0x0005F9BA
		public static float EnemyTroopDeadEffectOnIntensity
		{
			get
			{
				return MusicParameters._parameters[13];
			}
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06001C4B RID: 7243 RVA: 0x000617C4 File Offset: 0x0005F9C4
		public static float PlayerTroopDeadEffectMultiplierOnIntensity
		{
			get
			{
				return MusicParameters._parameters[14];
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06001C4C RID: 7244 RVA: 0x000617CE File Offset: 0x0005F9CE
		public static float BattleRatioTresholdOnIntensity
		{
			get
			{
				return MusicParameters._parameters[15];
			}
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06001C4D RID: 7245 RVA: 0x000617D8 File Offset: 0x0005F9D8
		public static float BattleTurnsOneSideCooldown
		{
			get
			{
				return MusicParameters._parameters[16];
			}
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06001C4E RID: 7246 RVA: 0x000617E2 File Offset: 0x0005F9E2
		public static float CampaignDarkModeThreshold
		{
			get
			{
				return MusicParameters._parameters[17];
			}
		}

		// Token: 0x06001C4F RID: 7247 RVA: 0x000617EC File Offset: 0x0005F9EC
		public static void LoadFromXml()
		{
			MusicParameters._parameters = new float[18];
			string text = ModuleHelper.GetModuleFullPath("Native") + "ModuleData/music_parameters.xml";
			XmlDocument xmlDocument = new XmlDocument();
			StreamReader streamReader = new StreamReader(text);
			string text2 = streamReader.ReadToEnd();
			xmlDocument.LoadXml(text2);
			streamReader.Close();
			foreach (object obj in xmlDocument.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.NodeType == XmlNodeType.Element && xmlNode.Name == "music_parameters")
				{
					using (IEnumerator enumerator2 = xmlNode.ChildNodes.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							XmlNode xmlNode2 = (XmlNode)obj2;
							if (xmlNode2.NodeType == XmlNodeType.Element)
							{
								MusicParameters.MusicParametersEnum musicParametersEnum = (MusicParameters.MusicParametersEnum)Enum.Parse(typeof(MusicParameters.MusicParametersEnum), xmlNode2.Attributes["id"].Value);
								float num = float.Parse(xmlNode2.Attributes["value"].Value, CultureInfo.InvariantCulture);
								MusicParameters._parameters[(int)musicParametersEnum] = num;
							}
						}
						break;
					}
				}
			}
			Debug.Print("MusicParameters have been resetted.", 0, Debug.DebugColor.Green, 281474976710656UL);
		}

		// Token: 0x04000976 RID: 2422
		private static float[] _parameters;

		// Token: 0x04000977 RID: 2423
		public const float ZeroIntensity = 0f;

		// Token: 0x02000515 RID: 1301
		private enum MusicParametersEnum
		{
			// Token: 0x04001D48 RID: 7496
			SmallBattleTreshold,
			// Token: 0x04001D49 RID: 7497
			MediumBattleTreshold,
			// Token: 0x04001D4A RID: 7498
			LargeBattleTreshold,
			// Token: 0x04001D4B RID: 7499
			SmallBattleDistanceTreshold,
			// Token: 0x04001D4C RID: 7500
			MediumBattleDistanceTreshold,
			// Token: 0x04001D4D RID: 7501
			LargeBattleDistanceTreshold,
			// Token: 0x04001D4E RID: 7502
			MaxBattleDistanceTreshold,
			// Token: 0x04001D4F RID: 7503
			MinIntensity,
			// Token: 0x04001D50 RID: 7504
			DefaultStartIntensity,
			// Token: 0x04001D51 RID: 7505
			PlayerChargeEffectMultiplierOnIntensity,
			// Token: 0x04001D52 RID: 7506
			BattleSizeEffectOnStartIntensity,
			// Token: 0x04001D53 RID: 7507
			RandomEffectMultiplierOnStartIntensity,
			// Token: 0x04001D54 RID: 7508
			FriendlyTroopDeadEffectOnIntensity,
			// Token: 0x04001D55 RID: 7509
			EnemyTroopDeadEffectOnIntensity,
			// Token: 0x04001D56 RID: 7510
			PlayerTroopDeadEffectMultiplierOnIntensity,
			// Token: 0x04001D57 RID: 7511
			BattleRatioTresholdOnIntensity,
			// Token: 0x04001D58 RID: 7512
			BattleTurnsOneSideCooldown,
			// Token: 0x04001D59 RID: 7513
			CampaignDarkModeThreshold,
			// Token: 0x04001D5A RID: 7514
			Count
		}
	}
}
