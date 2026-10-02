using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.Core
{
	// Token: 0x020000D3 RID: 211
	public class TauntUsageManager
	{
		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06000B2C RID: 2860 RVA: 0x000243D0 File Offset: 0x000225D0
		public static TauntUsageManager Instance
		{
			get
			{
				if (TauntUsageManager._instance == null)
				{
					TauntUsageManager._instance = TauntUsageManager.Initialize();
				}
				return TauntUsageManager._instance;
			}
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x000243E8 File Offset: 0x000225E8
		private TauntUsageManager()
		{
			this._tauntUsageSets = new List<TauntUsageManager.TauntUsageSet>();
			this._tauntUsageSetIndexMap = new Dictionary<string, int>();
			this.Read();
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x0002440C File Offset: 0x0002260C
		public static TauntUsageManager Initialize()
		{
			if (TauntUsageManager._instance == null)
			{
				TauntUsageManager._instance = new TauntUsageManager();
			}
			return TauntUsageManager._instance;
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x00024424 File Offset: 0x00022624
		public void Read()
		{
			foreach (object obj in TauntUsageManager.LoadXmlFile(ModuleHelper.GetModuleFullPath("Native") + "ModuleData/taunt_usage_sets.xml").DocumentElement.SelectNodes("taunt_usage_set"))
			{
				XmlNode xmlNode = (XmlNode)obj;
				string innerText = xmlNode.Attributes["id"].InnerText;
				this._tauntUsageSets.Add(new TauntUsageManager.TauntUsageSet());
				this._tauntUsageSetIndexMap[innerText] = this._tauntUsageSets.Count - 1;
				foreach (object obj2 in xmlNode.SelectNodes("taunt_usage"))
				{
					XmlNode xmlNode2 = (XmlNode)obj2;
					TauntUsageManager.TauntUsage.TauntUsageFlag tauntUsageFlag = TauntUsageManager.TauntUsage.TauntUsageFlag.None;
					XmlAttribute xmlAttribute = xmlNode2.Attributes["requires_bow"];
					if (bool.Parse(((xmlAttribute != null) ? xmlAttribute.Value : null) ?? "False"))
					{
						tauntUsageFlag |= TauntUsageManager.TauntUsage.TauntUsageFlag.RequiresBow;
					}
					XmlAttribute xmlAttribute2 = xmlNode2.Attributes["requires_on_foot"];
					if (bool.Parse(((xmlAttribute2 != null) ? xmlAttribute2.Value : null) ?? "False"))
					{
						tauntUsageFlag |= TauntUsageManager.TauntUsage.TauntUsageFlag.RequiresOnFoot;
					}
					XmlAttribute xmlAttribute3 = xmlNode2.Attributes["requires_shield"];
					if (bool.Parse(((xmlAttribute3 != null) ? xmlAttribute3.Value : null) ?? "False"))
					{
						tauntUsageFlag |= TauntUsageManager.TauntUsage.TauntUsageFlag.RequiresShield;
					}
					XmlAttribute xmlAttribute4 = xmlNode2.Attributes["is_left_stance"];
					if (bool.Parse(((xmlAttribute4 != null) ? xmlAttribute4.Value : null) ?? "False"))
					{
						tauntUsageFlag |= TauntUsageManager.TauntUsage.TauntUsageFlag.IsLeftStance;
					}
					XmlAttribute xmlAttribute5 = xmlNode2.Attributes["unsuitable_for_two_handed"];
					if (bool.Parse(((xmlAttribute5 != null) ? xmlAttribute5.Value : null) ?? "False"))
					{
						tauntUsageFlag |= TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForTwoHanded;
					}
					XmlAttribute xmlAttribute6 = xmlNode2.Attributes["unsuitable_for_one_handed"];
					if (bool.Parse(((xmlAttribute6 != null) ? xmlAttribute6.Value : null) ?? "False"))
					{
						tauntUsageFlag |= TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForOneHanded;
					}
					XmlAttribute xmlAttribute7 = xmlNode2.Attributes["unsuitable_for_shield"];
					if (bool.Parse(((xmlAttribute7 != null) ? xmlAttribute7.Value : null) ?? "False"))
					{
						tauntUsageFlag |= TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForShield;
					}
					XmlAttribute xmlAttribute8 = xmlNode2.Attributes["unsuitable_for_bow"];
					if (bool.Parse(((xmlAttribute8 != null) ? xmlAttribute8.Value : null) ?? "False"))
					{
						tauntUsageFlag |= TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForBow;
					}
					XmlAttribute xmlAttribute9 = xmlNode2.Attributes["unsuitable_for_crossbow"];
					if (bool.Parse(((xmlAttribute9 != null) ? xmlAttribute9.Value : null) ?? "False"))
					{
						tauntUsageFlag |= TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForCrossbow;
					}
					XmlAttribute xmlAttribute10 = xmlNode2.Attributes["unsuitable_for_empty"];
					if (bool.Parse(((xmlAttribute10 != null) ? xmlAttribute10.Value : null) ?? "False"))
					{
						tauntUsageFlag |= TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForEmpty;
					}
					string value = xmlNode2.Attributes["action"].Value;
					this._tauntUsageSets.Last<TauntUsageManager.TauntUsageSet>().AddUsage(new TauntUsageManager.TauntUsage(tauntUsageFlag, value));
				}
			}
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x00024764 File Offset: 0x00022964
		public TauntUsageManager.TauntUsageSet GetUsageSet(string id)
		{
			int num;
			if (this._tauntUsageSetIndexMap.TryGetValue(id, out num) && num >= 0 && num < this._tauntUsageSets.Count)
			{
				return this._tauntUsageSets[num];
			}
			return null;
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x000247A4 File Offset: 0x000229A4
		public string GetAction(int index, bool isLeftStance, bool onFoot, WeaponComponentData mainHandWeapon, WeaponComponentData offhandWeapon)
		{
			string text = null;
			foreach (TauntUsageManager.TauntUsage tauntUsage in this._tauntUsageSets[index].GetUsages())
			{
				if (tauntUsage.IsSuitable(isLeftStance, onFoot, mainHandWeapon, offhandWeapon))
				{
					text = tauntUsage.GetAction();
					break;
				}
			}
			return text;
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x00024818 File Offset: 0x00022A18
		private static TextObject GetHintTextFromReasons(List<TextObject> reasons)
		{
			TextObject textObject = null;
			for (int i = 0; i < reasons.Count; i++)
			{
				if (i >= 1)
				{
					GameTexts.SetVariable("STR1", textObject.ToString());
					GameTexts.SetVariable("STR2", reasons[i]);
					textObject = GameTexts.FindText("str_string_newline_string", null);
				}
				else
				{
					textObject = reasons[i];
				}
			}
			return textObject;
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x00024874 File Offset: 0x00022A74
		public static string GetActionDisabledReasonText(TauntUsageManager.TauntUsage.TauntUsageFlag disabledReasonFlag)
		{
			List<TextObject> list = new List<TextObject>();
			if (disabledReasonFlag.HasAllFlags(TauntUsageManager.TauntUsage.TauntUsageFlag.RequiresBow))
			{
				list.Add(new TextObject("{=2GE0in0u}Requires Bow.", null));
			}
			if (disabledReasonFlag.HasAllFlags(TauntUsageManager.TauntUsage.TauntUsageFlag.RequiresShield))
			{
				list.Add(new TextObject("{=6Tw6BLXI}Requires Shield.", null));
			}
			if (disabledReasonFlag.HasAllFlags(TauntUsageManager.TauntUsage.TauntUsageFlag.RequiresOnFoot))
			{
				list.Add(new TextObject("{=GHQMM8Df}Can't be used while mounted.", null));
			}
			if (disabledReasonFlag.HasAllFlags(TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForTwoHanded))
			{
				list.Add(new TextObject("{=EhK4Q6S4}Can't be used with Two Handed weapons.", null));
			}
			if (disabledReasonFlag.HasAllFlags(TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForOneHanded))
			{
				list.Add(new TextObject("{=wJbkXP98}Can't be used with One Handed weapons.", null));
			}
			if (disabledReasonFlag.HasAllFlags(TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForShield))
			{
				list.Add(new TextObject("{=bJMUTZ00}Can't be used with Shields.", null));
			}
			if (disabledReasonFlag.HasAllFlags(TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForBow))
			{
				list.Add(new TextObject("{=B9Gp7pIf}Can't be used with Bows.", null));
			}
			if (disabledReasonFlag.HasAllFlags(TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForCrossbow))
			{
				list.Add(new TextObject("{=kkzKtP78}Can't be used with Crossbows.", null));
			}
			if (disabledReasonFlag.HasAllFlags(TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForEmpty))
			{
				list.Add(new TextObject("{=F59nAr9s}Can't be used without a weapon.", null));
			}
			if (list.Count > 0)
			{
				return TauntUsageManager.GetHintTextFromReasons(list).ToString();
			}
			if (disabledReasonFlag.HasAllFlags(TauntUsageManager.TauntUsage.TauntUsageFlag.IsLeftStance))
			{
				return string.Empty;
			}
			return null;
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x000249A8 File Offset: 0x00022BA8
		public TauntUsageManager.TauntUsage.TauntUsageFlag GetIsActionNotSuitableReason(int index, bool isLeftStance, bool onFoot, WeaponComponentData mainHandWeapon, WeaponComponentData offhandWeapon)
		{
			MBReadOnlyList<TauntUsageManager.TauntUsage> usages = this._tauntUsageSets[index].GetUsages();
			if (usages.Count == 0)
			{
				Debug.FailedAssert("Taunt usages are empty", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.Core\\TauntUsageManager.cs", "GetIsActionNotSuitableReason", 238);
				return TauntUsageManager.TauntUsage.TauntUsageFlag.None;
			}
			TauntUsageManager.TauntUsage.TauntUsageFlag[] array = new TauntUsageManager.TauntUsage.TauntUsageFlag[usages.Count];
			for (int i = 0; i < usages.Count; i++)
			{
				TauntUsageManager.TauntUsage.TauntUsageFlag isNotSuitableReason = usages[i].GetIsNotSuitableReason(isLeftStance, onFoot, mainHandWeapon, offhandWeapon);
				if (isNotSuitableReason == TauntUsageManager.TauntUsage.TauntUsageFlag.None)
				{
					return TauntUsageManager.TauntUsage.TauntUsageFlag.None;
				}
				array[i] = isNotSuitableReason;
			}
			Array.Sort<TauntUsageManager.TauntUsage.TauntUsageFlag>(array, new TauntUsageManager.TauntUsageFlagComparer());
			return array[0];
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x00024A31 File Offset: 0x00022C31
		public int GetTauntItemCount()
		{
			return this._tauntUsageSets.Count;
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x00024A40 File Offset: 0x00022C40
		public int GetIndexOfAction(string id)
		{
			int num;
			if (this._tauntUsageSetIndexMap.TryGetValue(id, out num))
			{
				return num;
			}
			return -1;
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x00024A60 File Offset: 0x00022C60
		public string GetDefaultAction(int index)
		{
			TauntUsageManager.TauntUsage tauntUsage = this._tauntUsageSets[index].GetUsages().Last<TauntUsageManager.TauntUsage>();
			if (tauntUsage == null)
			{
				return null;
			}
			return tauntUsage.GetAction();
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x00024A84 File Offset: 0x00022C84
		private static XmlDocument LoadXmlFile(string path)
		{
			string text = new StreamReader(path).ReadToEnd();
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(text);
			return xmlDocument;
		}

		// Token: 0x0400063B RID: 1595
		private static TauntUsageManager _instance;

		// Token: 0x0400063C RID: 1596
		private List<TauntUsageManager.TauntUsageSet> _tauntUsageSets;

		// Token: 0x0400063D RID: 1597
		private Dictionary<string, int> _tauntUsageSetIndexMap;

		// Token: 0x0200013B RID: 315
		private class TauntUsageFlagComparer : IComparer<TauntUsageManager.TauntUsage.TauntUsageFlag>
		{
			// Token: 0x06000C4A RID: 3146 RVA: 0x00027200 File Offset: 0x00025400
			public int Compare(TauntUsageManager.TauntUsage.TauntUsageFlag x, TauntUsageManager.TauntUsage.TauntUsageFlag y)
			{
				int num = (int)x;
				return num.CompareTo((int)y);
			}
		}

		// Token: 0x0200013C RID: 316
		public class TauntUsageSet
		{
			// Token: 0x06000C4C RID: 3148 RVA: 0x0002721F File Offset: 0x0002541F
			public TauntUsageSet()
			{
				this._tauntUsages = new MBList<TauntUsageManager.TauntUsage>();
			}

			// Token: 0x06000C4D RID: 3149 RVA: 0x00027232 File Offset: 0x00025432
			public void AddUsage(TauntUsageManager.TauntUsage usage)
			{
				this._tauntUsages.Add(usage);
			}

			// Token: 0x06000C4E RID: 3150 RVA: 0x00027240 File Offset: 0x00025440
			public MBReadOnlyList<TauntUsageManager.TauntUsage> GetUsages()
			{
				return this._tauntUsages;
			}

			// Token: 0x04000821 RID: 2081
			private MBList<TauntUsageManager.TauntUsage> _tauntUsages;
		}

		// Token: 0x0200013D RID: 317
		public class TauntUsage
		{
			// Token: 0x17000414 RID: 1044
			// (get) Token: 0x06000C4F RID: 3151 RVA: 0x00027248 File Offset: 0x00025448
			public TauntUsageManager.TauntUsage.TauntUsageFlag UsageFlag { get; }

			// Token: 0x06000C50 RID: 3152 RVA: 0x00027250 File Offset: 0x00025450
			public TauntUsage(TauntUsageManager.TauntUsage.TauntUsageFlag usageFlag, string actionName)
			{
				this.UsageFlag = usageFlag;
				this._actionName = actionName;
			}

			// Token: 0x06000C51 RID: 3153 RVA: 0x00027266 File Offset: 0x00025466
			public bool IsSuitable(bool isLeftStance, bool isOnFoot, WeaponComponentData mainHandWeapon, WeaponComponentData offhandWeapon)
			{
				return this.GetIsNotSuitableReason(isLeftStance, isOnFoot, mainHandWeapon, offhandWeapon) == TauntUsageManager.TauntUsage.TauntUsageFlag.None;
			}

			// Token: 0x06000C52 RID: 3154 RVA: 0x00027278 File Offset: 0x00025478
			public TauntUsageManager.TauntUsage.TauntUsageFlag GetIsNotSuitableReason(bool isLeftStance, bool isOnFoot, WeaponComponentData mainHandWeapon, WeaponComponentData offhandWeapon)
			{
				TauntUsageManager.TauntUsage.TauntUsageFlag tauntUsageFlag = TauntUsageManager.TauntUsage.TauntUsageFlag.None;
				if (this.UsageFlag.HasAllFlags(TauntUsageManager.TauntUsage.TauntUsageFlag.RequiresBow) && (mainHandWeapon == null || !mainHandWeapon.IsBow))
				{
					tauntUsageFlag |= TauntUsageManager.TauntUsage.TauntUsageFlag.RequiresBow;
				}
				if (this.UsageFlag.HasAllFlags(TauntUsageManager.TauntUsage.TauntUsageFlag.RequiresShield) && (offhandWeapon == null || !offhandWeapon.IsShield))
				{
					tauntUsageFlag |= TauntUsageManager.TauntUsage.TauntUsageFlag.RequiresShield;
				}
				if (this.UsageFlag.HasAllFlags(TauntUsageManager.TauntUsage.TauntUsageFlag.RequiresOnFoot) && !isOnFoot)
				{
					tauntUsageFlag |= TauntUsageManager.TauntUsage.TauntUsageFlag.RequiresOnFoot;
				}
				if (this.UsageFlag.HasAllFlags(TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForTwoHanded) && mainHandWeapon != null && mainHandWeapon.IsTwoHanded)
				{
					tauntUsageFlag |= TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForTwoHanded;
				}
				if (this.UsageFlag.HasAllFlags(TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForOneHanded) && mainHandWeapon != null && mainHandWeapon.IsOneHanded)
				{
					tauntUsageFlag |= TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForOneHanded;
				}
				if (this.UsageFlag.HasAllFlags(TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForShield) && offhandWeapon != null && offhandWeapon.IsShield)
				{
					tauntUsageFlag |= TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForShield;
				}
				if (this.UsageFlag.HasAllFlags(TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForBow) && mainHandWeapon != null && mainHandWeapon.IsBow)
				{
					tauntUsageFlag |= TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForBow;
				}
				if (this.UsageFlag.HasAllFlags(TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForCrossbow) && mainHandWeapon != null && mainHandWeapon.IsCrossBow)
				{
					tauntUsageFlag |= TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForCrossbow;
				}
				if (this.UsageFlag.HasAllFlags(TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForEmpty) && mainHandWeapon == null && offhandWeapon == null)
				{
					tauntUsageFlag |= TauntUsageManager.TauntUsage.TauntUsageFlag.UnsuitableForEmpty;
				}
				if (this.UsageFlag.HasAllFlags(TauntUsageManager.TauntUsage.TauntUsageFlag.IsLeftStance) != isLeftStance)
				{
					tauntUsageFlag |= TauntUsageManager.TauntUsage.TauntUsageFlag.IsLeftStance;
				}
				return tauntUsageFlag;
			}

			// Token: 0x06000C53 RID: 3155 RVA: 0x000273B6 File Offset: 0x000255B6
			public string GetAction()
			{
				return this._actionName;
			}

			// Token: 0x04000823 RID: 2083
			private string _actionName;

			// Token: 0x02000143 RID: 323
			[Flags]
			public enum TauntUsageFlag
			{
				// Token: 0x04000840 RID: 2112
				None = 0,
				// Token: 0x04000841 RID: 2113
				RequiresBow = 1,
				// Token: 0x04000842 RID: 2114
				RequiresShield = 2,
				// Token: 0x04000843 RID: 2115
				IsLeftStance = 4,
				// Token: 0x04000844 RID: 2116
				RequiresOnFoot = 8,
				// Token: 0x04000845 RID: 2117
				UnsuitableForTwoHanded = 16,
				// Token: 0x04000846 RID: 2118
				UnsuitableForOneHanded = 32,
				// Token: 0x04000847 RID: 2119
				UnsuitableForShield = 64,
				// Token: 0x04000848 RID: 2120
				UnsuitableForBow = 128,
				// Token: 0x04000849 RID: 2121
				UnsuitableForCrossbow = 256,
				// Token: 0x0400084A RID: 2122
				UnsuitableForEmpty = 512
			}
		}
	}
}
