using System;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x0200003E RID: 62
	public class RangedHeadShotDamageEffect : MPPerkEffect
	{
		// Token: 0x06000239 RID: 569 RVA: 0x0000A101 File Offset: 0x00008301
		protected RangedHeadShotDamageEffect()
		{
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000A10C File Offset: 0x0000830C
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
			if (text4 == null || !float.TryParse(text4, out this._value))
			{
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\RangedHeadShotDamageEffect.cs", "Deserialize", 22);
			}
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000A1B0 File Offset: 0x000083B0
		public override float GetRangedHeadShotDamage()
		{
			return this._value;
		}

		// Token: 0x040000A9 RID: 169
		protected static string StringType = "RangedHeadShotDamage";

		// Token: 0x040000AA RID: 170
		private float _value;
	}
}
