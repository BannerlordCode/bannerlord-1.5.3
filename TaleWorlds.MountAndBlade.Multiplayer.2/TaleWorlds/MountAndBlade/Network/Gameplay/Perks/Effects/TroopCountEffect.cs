using System;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x02000046 RID: 70
	public class TroopCountEffect : MPOnSpawnPerkEffect
	{
		// Token: 0x0600025B RID: 603 RVA: 0x0000AB54 File Offset: 0x00008D54
		protected TroopCountEffect()
		{
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000AB5C File Offset: 0x00008D5C
		protected override void Deserialize(XmlNode node)
		{
			base.Deserialize(node);
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
					XmlAttribute xmlAttribute = attributes["value"];
					text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				}
			}
			string text2 = text;
			if (text2 == null || !int.TryParse(text2, out this._value))
			{
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\TroopCountEffect.cs", "Deserialize", 20);
			}
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000ABC1 File Offset: 0x00008DC1
		public override int GetExtraTroopCount()
		{
			return this._value;
		}

		// Token: 0x040000BD RID: 189
		protected static string StringType = "TroopCountOnSpawn";

		// Token: 0x040000BE RID: 190
		private int _value;
	}
}
