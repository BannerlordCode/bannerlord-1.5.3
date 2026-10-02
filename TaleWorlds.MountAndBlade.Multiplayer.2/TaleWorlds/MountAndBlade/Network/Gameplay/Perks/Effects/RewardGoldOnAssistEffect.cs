using System;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x0200003F RID: 63
	public class RewardGoldOnAssistEffect : MPPerkEffect
	{
		// Token: 0x0600023D RID: 573 RVA: 0x0000A1C4 File Offset: 0x000083C4
		protected RewardGoldOnAssistEffect()
		{
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000A1CC File Offset: 0x000083CC
		protected override void Deserialize(XmlNode node)
		{
			string text;
			if (node == null)
			{
				text = null;
			}
			else
			{
				XmlAttributeCollection attributes = node.Attributes;
				if (attributes == null)
				{
					text = null;
				}
				else
				{
					XmlAttribute xmlAttribute = attributes["is_disabled_in_warmup"];
					text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				}
			}
			string text2 = text;
			base.IsDisabledInWarmup = ((text2 != null) ? text2.ToLower() : null) == "true";
			string text3;
			if (node == null)
			{
				text3 = null;
			}
			else
			{
				XmlAttributeCollection attributes2 = node.Attributes;
				if (attributes2 == null)
				{
					text3 = null;
				}
				else
				{
					XmlAttribute xmlAttribute2 = attributes2["value"];
					text3 = ((xmlAttribute2 != null) ? xmlAttribute2.Value : null);
				}
			}
			string text4 = text3;
			if (text4 == null || !int.TryParse(text4, out this._value))
			{
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\RewardGoldOnAssistEffect.cs", "Deserialize", 22);
			}
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000A270 File Offset: 0x00008470
		public override int GetRewardedGoldOnAssist()
		{
			return this._value;
		}

		// Token: 0x040000AB RID: 171
		protected static string StringType = "RewardGoldOnAssist";

		// Token: 0x040000AC RID: 172
		private int _value;
	}
}
