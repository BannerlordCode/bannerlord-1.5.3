using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges
{
	// Token: 0x0200016A RID: 362
	public static class BadgeManager
	{
		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000A20 RID: 2592 RVA: 0x0000FCDF File Offset: 0x0000DEDF
		// (set) Token: 0x06000A21 RID: 2593 RVA: 0x0000FCE6 File Offset: 0x0000DEE6
		public static List<Badge> Badges { get; private set; }

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000A22 RID: 2594 RVA: 0x0000FCEE File Offset: 0x0000DEEE
		// (set) Token: 0x06000A23 RID: 2595 RVA: 0x0000FCF5 File Offset: 0x0000DEF5
		public static bool IsInitialized { get; private set; }

		// Token: 0x06000A24 RID: 2596 RVA: 0x0000FCFD File Offset: 0x0000DEFD
		public static void InitializeWithXML(string xmlPath)
		{
			Debug.Print("BadgeManager::InitializeWithXML", 0, Debug.DebugColor.White, 17592186044416UL);
			if (BadgeManager.IsInitialized)
			{
				return;
			}
			BadgeManager.LoadFromXml(xmlPath);
			BadgeManager.IsInitialized = true;
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x0000FD2C File Offset: 0x0000DF2C
		public static void OnFinalize()
		{
			Debug.Print("BadgeManager::OnFinalize", 0, Debug.DebugColor.White, 17592186044416UL);
			if (!BadgeManager.IsInitialized)
			{
				return;
			}
			BadgeManager._badgesById.Clear();
			BadgeManager._badgesByType.Clear();
			BadgeManager.Badges.Clear();
			BadgeManager._badgesById = null;
			BadgeManager._badgesByType = null;
			BadgeManager.Badges = null;
			BadgeManager.IsInitialized = false;
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x0000FD90 File Offset: 0x0000DF90
		private static void LoadFromXml(string path)
		{
			XmlDocument xmlDocument = new XmlDocument();
			using (StreamReader streamReader = new StreamReader(path))
			{
				string text = streamReader.ReadToEnd();
				xmlDocument.LoadXml(text);
				streamReader.Close();
			}
			BadgeManager._badgesById = new Dictionary<string, Badge>();
			BadgeManager._badgesByType = new Dictionary<BadgeType, List<Badge>>();
			BadgeManager.Badges = new List<Badge>();
			foreach (object obj in xmlDocument.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name == "Badges")
				{
					foreach (object obj2 in xmlNode.ChildNodes)
					{
						XmlNode xmlNode2 = (XmlNode)obj2;
						if (xmlNode2.Name == "Badge")
						{
							BadgeType badgeType = BadgeType.Custom;
							if (!Enum.TryParse<BadgeType>(xmlNode2.Attributes["type"].Value, true, out badgeType))
							{
								Debug.FailedAssert("No 'type' was provided for a badge", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\MultiplayerBadges\\BadgeManager.cs", "LoadFromXml", 82);
							}
							Badge badge = null;
							if (badgeType > BadgeType.OnLogin)
							{
								if (badgeType == BadgeType.Conditional)
								{
									badge = new ConditionalBadge(BadgeManager.Badges.Count, badgeType);
								}
							}
							else
							{
								badge = new Badge(BadgeManager.Badges.Count, badgeType);
							}
							badge.Deserialize(xmlNode2);
							BadgeManager._badgesById[badge.StringId] = badge;
							BadgeManager.Badges.Add(badge);
							List<Badge> list;
							if (!BadgeManager._badgesByType.TryGetValue(badgeType, out list))
							{
								list = new List<Badge>();
								BadgeManager._badgesByType.Add(badgeType, list);
							}
							list.Add(badge);
						}
					}
				}
			}
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x0000FFAC File Offset: 0x0000E1AC
		public static Badge GetByIndex(int index)
		{
			if (index == -1 || BadgeManager.Badges == null || BadgeManager.Badges.Count <= index || index < 0)
			{
				return null;
			}
			return BadgeManager.Badges[index];
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x0000FFD8 File Offset: 0x0000E1D8
		public static Badge GetById(string id)
		{
			Badge badge;
			if (id == null || !BadgeManager._badgesById.TryGetValue(id, out badge))
			{
				return null;
			}
			return badge;
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x0000FFFC File Offset: 0x0000E1FC
		public static List<Badge> GetByType(BadgeType type)
		{
			List<Badge> list;
			if (!BadgeManager._badgesByType.TryGetValue(type, out list))
			{
				list = new List<Badge>();
				BadgeManager._badgesByType.Add(type, list);
			}
			return list;
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x0001002C File Offset: 0x0000E22C
		public static string GetBadgeConditionValue(this PlayerData playerData, BadgeCondition condition)
		{
			if (playerData == null)
			{
				Debug.FailedAssert("PlayerData is null on get value", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\MultiplayerBadges\\BadgeManager.cs", "GetBadgeConditionValue", 143);
				return "";
			}
			string text;
			if (!condition.Parameters.TryGetValue("property", out text))
			{
				Debug.FailedAssert("Condition with type PlayerData does not have Property parameter", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\MultiplayerBadges\\BadgeManager.cs", "GetBadgeConditionValue", 150);
				return "";
			}
			if (text == "ShownBadgeId")
			{
				return playerData.ShownBadgeId;
			}
			return "";
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x000100A8 File Offset: 0x0000E2A8
		public static int GetBadgeConditionNumericValue(this PlayerData playerData, BadgeCondition condition)
		{
			if (playerData == null)
			{
				Debug.FailedAssert("PlayerData is null on get value", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\MultiplayerBadges\\BadgeManager.cs", "GetBadgeConditionNumericValue", 167);
				return 0;
			}
			string text;
			if (!condition.Parameters.TryGetValue("property", out text))
			{
				Debug.FailedAssert("Condition with type PlayerDataNumeric does not have Property parameter", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\MultiplayerBadges\\BadgeManager.cs", "GetBadgeConditionNumericValue", 174);
				return 0;
			}
			int num = 0;
			string[] array = text.Split(new char[] { '.' });
			string text2 = array[0];
			uint num2 = <PrivateImplementationDetails>.ComputeStringHash(text2);
			if (num2 <= 1096112509U)
			{
				if (num2 <= 267161228U)
				{
					if (num2 != 192547213U)
					{
						if (num2 == 267161228U)
						{
							if (text2 == "Stats")
							{
								if (array.Length == 3 && playerData.Stats != null)
								{
									string text3 = array[1].Trim().ToLower();
									PlayerStatsBase[] stats = playerData.Stats;
									int i = 0;
									while (i < stats.Length)
									{
										PlayerStatsBase playerStatsBase = stats[i];
										if (playerStatsBase.GameType.Trim().ToLower() == text3)
										{
											text2 = array[2];
											if (text2 == "KillCount")
											{
												num = playerStatsBase.KillCount;
												break;
											}
											if (text2 == "DeathCount")
											{
												num = playerStatsBase.DeathCount;
												break;
											}
											if (text2 == "AssistCount")
											{
												num = playerStatsBase.AssistCount;
												break;
											}
											if (text2 == "WinCount")
											{
												num = playerStatsBase.WinCount;
												break;
											}
											if (!(text2 == "LoseCount"))
											{
												break;
											}
											num = playerStatsBase.LoseCount;
											break;
										}
										else
										{
											i++;
										}
									}
								}
							}
						}
					}
					else if (text2 == "AssistCount")
					{
						num = playerData.AssistCount;
					}
				}
				else if (num2 != 1093842208U)
				{
					if (num2 == 1096112509U)
					{
						if (text2 == "Level")
						{
							num = playerData.Level;
						}
					}
				}
				else if (text2 == "WinCount")
				{
					num = playerData.WinCount;
				}
			}
			else if (num2 <= 2667250970U)
			{
				if (num2 != 1128891543U)
				{
					if (num2 == 2667250970U)
					{
						if (text2 == "Playtime")
						{
							num = playerData.Playtime;
						}
					}
				}
				else if (text2 == "LoseCount")
				{
					num = playerData.LoseCount;
				}
			}
			else if (num2 != 3945868512U)
			{
				if (num2 == 4058818476U)
				{
					if (text2 == "DeathCount")
					{
						num = playerData.DeathCount;
					}
				}
			}
			else if (text2 == "KillCount")
			{
				num = playerData.KillCount;
			}
			return num;
		}

		// Token: 0x04000510 RID: 1296
		public const string PropertyParameterName = "property";

		// Token: 0x04000511 RID: 1297
		public const string ValueParameterName = "value";

		// Token: 0x04000512 RID: 1298
		public const string MinValueParameterName = "min_value";

		// Token: 0x04000513 RID: 1299
		public const string MaxValueParameterName = "max_value";

		// Token: 0x04000514 RID: 1300
		public const string IsBestParameterName = "is_best";

		// Token: 0x04000517 RID: 1303
		private static Dictionary<string, Badge> _badgesById;

		// Token: 0x04000518 RID: 1304
		private static Dictionary<BadgeType, List<Badge>> _badgesByType;
	}
}
