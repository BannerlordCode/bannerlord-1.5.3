using System;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement
{
	// Token: 0x02000121 RID: 289
	public readonly struct ClanCardSelectionItemPropertyInfo
	{
		// Token: 0x06001A62 RID: 6754 RVA: 0x00063E80 File Offset: 0x00062080
		public ClanCardSelectionItemPropertyInfo(TextObject title, TextObject value)
		{
			this.Title = title;
			this.Value = value;
		}

		// Token: 0x06001A63 RID: 6755 RVA: 0x00063EA0 File Offset: 0x000620A0
		public ClanCardSelectionItemPropertyInfo(TextObject value)
		{
			this.Title = null;
			this.Value = value;
		}

		// Token: 0x06001A64 RID: 6756 RVA: 0x00063EBD File Offset: 0x000620BD
		public static TextObject CreateLabeledValueText(TextObject label, TextObject value)
		{
			TextObject textObject = new TextObject("{=!}<span style=\"Label\">{LABEL}</span>: {VALUE}", null);
			textObject.SetTextVariable("LABEL", label);
			textObject.SetTextVariable("VALUE", value);
			return textObject;
		}

		// Token: 0x06001A65 RID: 6757 RVA: 0x00063EE4 File Offset: 0x000620E4
		public static TextObject CreateActionGoldChangeText(int goldChange)
		{
			if (goldChange != 0)
			{
				bool flag = goldChange > 0;
				string text = (flag ? "PositiveChange" : "NegativeChange");
				TextObject textObject = (flag ? new TextObject("{=8N1EdPB3}You will earn {GOLD}{GOLD_ICON}", null) : new TextObject("{=kjaACKUq}This action will cost {GOLD}{GOLD_ICON}", null));
				textObject.SetTextVariable("GOLD", string.Format("<span style=\"{0}\">{1}</span>", text, Math.Abs(goldChange)));
				textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
				return textObject;
			}
			return TextObject.GetEmpty();
		}

		// Token: 0x04000C15 RID: 3093
		public readonly TextObject Title;

		// Token: 0x04000C16 RID: 3094
		public readonly TextObject Value;
	}
}
