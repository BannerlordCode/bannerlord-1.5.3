using System;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x02000032 RID: 50
	public class GoldGainOnAssistEffect : MPPerkEffect
	{
		// Token: 0x06000203 RID: 515 RVA: 0x00009352 File Offset: 0x00007552
		protected GoldGainOnAssistEffect()
		{
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000935C File Offset: 0x0000755C
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
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\GoldGainOnAssistEffect.cs", "Deserialize", 22);
			}
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00009400 File Offset: 0x00007600
		public override int GetGoldOnAssist()
		{
			return this._value;
		}

		// Token: 0x0400008D RID: 141
		protected static string StringType = "GoldGainOnAssist";

		// Token: 0x0400008E RID: 142
		private int _value;
	}
}
