using System;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x02000042 RID: 66
	public class ShieldDamageTakenEffect : MPPerkEffect
	{
		// Token: 0x0600024B RID: 587 RVA: 0x0000A744 File Offset: 0x00008944
		protected ShieldDamageTakenEffect()
		{
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000A74C File Offset: 0x0000894C
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
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\ShieldDamageTakenEffect.cs", "Deserialize", 31);
			}
			string text5;
			if (node == null)
			{
				text5 = null;
			}
			else
			{
				XmlAttributeCollection attributes3 = node.Attributes;
				if (attributes3 == null)
				{
					text5 = null;
				}
				else
				{
					XmlAttribute xmlAttribute3 = attributes3["block_type"];
					text5 = ((xmlAttribute3 != null) ? xmlAttribute3.Value : null);
				}
			}
			string text6 = text5;
			this._blockType = ShieldDamageTakenEffect.BlockType.Any;
			if (text6 != null && !Enum.TryParse<ShieldDamageTakenEffect.BlockType>(text6, true, out this._blockType))
			{
				this._blockType = ShieldDamageTakenEffect.BlockType.Any;
				Debug.FailedAssert("provided 'block_type' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\ShieldDamageTakenEffect.cs", "Deserialize", 39);
			}
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000A850 File Offset: 0x00008A50
		public override float GetShieldDamageTaken(bool isCorrectSideBlock)
		{
			switch (this._blockType)
			{
			case ShieldDamageTakenEffect.BlockType.Any:
				return this._value;
			case ShieldDamageTakenEffect.BlockType.CorrectSide:
				if (!isCorrectSideBlock)
				{
					return 0f;
				}
				return this._value;
			case ShieldDamageTakenEffect.BlockType.WrongSide:
				if (!isCorrectSideBlock)
				{
					return this._value;
				}
				return 0f;
			default:
				return 0f;
			}
		}

		// Token: 0x040000B4 RID: 180
		protected static string StringType = "ShieldDamageTaken";

		// Token: 0x040000B5 RID: 181
		private float _value;

		// Token: 0x040000B6 RID: 182
		private ShieldDamageTakenEffect.BlockType _blockType;

		// Token: 0x020000AA RID: 170
		private enum BlockType
		{
			// Token: 0x040001C2 RID: 450
			Any,
			// Token: 0x040001C3 RID: 451
			CorrectSide,
			// Token: 0x040001C4 RID: 452
			WrongSide
		}
	}
}
