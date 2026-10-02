using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core.ViewModelCollection.Information
{
	// Token: 0x0200001A RID: 26
	public class TooltipProperty : ViewModel, ISerializableObject
	{
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00005019 File Offset: 0x00003219
		// (set) Token: 0x06000169 RID: 361 RVA: 0x00005021 File Offset: 0x00003221
		public bool OnlyShowWhenExtended { get; set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600016A RID: 362 RVA: 0x0000502A File Offset: 0x0000322A
		// (set) Token: 0x0600016B RID: 363 RVA: 0x00005032 File Offset: 0x00003232
		public bool OnlyShowWhenNotExtended { get; set; }

		// Token: 0x0600016C RID: 364 RVA: 0x0000503B File Offset: 0x0000323B
		public TooltipProperty()
		{
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00005064 File Offset: 0x00003264
		public void RefreshValue()
		{
			if (this.valueFunc != null)
			{
				string text = this.valueFunc();
				if (text != null)
				{
					this.ValueLabel = text;
				}
			}
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000508F File Offset: 0x0000328F
		public void RefreshDefinition()
		{
			if (this.definitionFunc != null)
			{
				this.DefinitionLabel = this.definitionFunc();
			}
		}

		// Token: 0x0600016F RID: 367 RVA: 0x000050AC File Offset: 0x000032AC
		public TooltipProperty(string definition, string value, int textHeight, bool onlyShowWhenExtended = false, TooltipProperty.TooltipPropertyFlags modifier = TooltipProperty.TooltipPropertyFlags.None)
		{
			this.TextHeight = textHeight;
			this.DefinitionLabel = definition;
			this.ValueLabel = value;
			this.OnlyShowWhenExtended = onlyShowWhenExtended;
			this.PropertyModifier = (int)modifier;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00005104 File Offset: 0x00003304
		public TooltipProperty(string definition, Func<string> _valueFunc, int textHeight, bool onlyShowWhenExtended = false, TooltipProperty.TooltipPropertyFlags modifier = TooltipProperty.TooltipPropertyFlags.None)
		{
			this.valueFunc = _valueFunc;
			this.TextHeight = textHeight;
			this.DefinitionLabel = definition;
			this.OnlyShowWhenExtended = onlyShowWhenExtended;
			this.PropertyModifier = (int)modifier;
			this.RefreshValue();
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00005164 File Offset: 0x00003364
		public TooltipProperty(Func<string> _definitionFunc, Func<string> _valueFunc, int textHeight, bool onlyShowWhenExtended = false, TooltipProperty.TooltipPropertyFlags modifier = TooltipProperty.TooltipPropertyFlags.None)
		{
			this.valueFunc = _valueFunc;
			this.TextHeight = textHeight;
			this.definitionFunc = _definitionFunc;
			this.OnlyShowWhenExtended = onlyShowWhenExtended;
			this.PropertyModifier = (int)modifier;
			this.RefreshDefinition();
			this.RefreshValue();
		}

		// Token: 0x06000172 RID: 370 RVA: 0x000051C8 File Offset: 0x000033C8
		public TooltipProperty(Func<string> _definitionFunc, Func<string> _valueFunc, object[] valueArgs, int textHeight, bool onlyShowWhenExtended = false, TooltipProperty.TooltipPropertyFlags modifier = TooltipProperty.TooltipPropertyFlags.None)
		{
			this.valueFunc = _valueFunc;
			this.TextHeight = textHeight;
			this.definitionFunc = _definitionFunc;
			this.OnlyShowWhenExtended = onlyShowWhenExtended;
			this.PropertyModifier = (int)modifier;
			this.RefreshDefinition();
			this.RefreshValue();
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000522C File Offset: 0x0000342C
		public TooltipProperty(string definition, string value, int textHeight, Color color, bool onlyShowWhenExtended = false, TooltipProperty.TooltipPropertyFlags modifier = TooltipProperty.TooltipPropertyFlags.None)
		{
			this.TextHeight = textHeight;
			this.TextColor = color;
			this.DefinitionLabel = definition;
			this.ValueLabel = value;
			this.OnlyShowWhenExtended = onlyShowWhenExtended;
			this.PropertyModifier = (int)modifier;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000528C File Offset: 0x0000348C
		public TooltipProperty(string definition, Func<string> _valueFunc, int textHeight, Color color, bool onlyShowWhenExtended = false, TooltipProperty.TooltipPropertyFlags modifier = TooltipProperty.TooltipPropertyFlags.None)
		{
			this.valueFunc = _valueFunc;
			this.TextHeight = textHeight;
			this.TextColor = color;
			this.DefinitionLabel = definition;
			this.OnlyShowWhenExtended = onlyShowWhenExtended;
			this.PropertyModifier = (int)modifier;
			this.RefreshValue();
		}

		// Token: 0x06000175 RID: 373 RVA: 0x000052F4 File Offset: 0x000034F4
		public TooltipProperty(Func<string> _definitionFunc, Func<string> _valueFunc, int textHeight, Color color, bool onlyShowWhenExtended = false, TooltipProperty.TooltipPropertyFlags modifier = TooltipProperty.TooltipPropertyFlags.None)
		{
			this.valueFunc = _valueFunc;
			this.definitionFunc = _definitionFunc;
			this.TextHeight = textHeight;
			this.TextColor = color;
			this.OnlyShowWhenExtended = onlyShowWhenExtended;
			this.PropertyModifier = (int)modifier;
			this.RefreshDefinition();
			this.RefreshValue();
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00005360 File Offset: 0x00003560
		public TooltipProperty(TooltipProperty property)
		{
			this.TextHeight = property.TextHeight;
			this.TextColor = property.TextColor;
			this.DefinitionLabel = property.DefinitionLabel;
			this.ValueLabel = property.ValueLabel;
			this.OnlyShowWhenExtended = property.OnlyShowWhenExtended;
			this.PropertyModifier = property.PropertyModifier;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x000053DA File Offset: 0x000035DA
		public void DeserializeFrom(IReader reader)
		{
			this.TextHeight = reader.ReadInt();
			this.TextColor = reader.ReadColor();
			this.DefinitionLabel = reader.ReadString();
			this.ValueLabel = reader.ReadString();
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000540C File Offset: 0x0000360C
		public void SerializeTo(IWriter writer)
		{
			writer.WriteInt(this.TextHeight);
			writer.WriteColor(this.TextColor);
			writer.WriteString(this.DefinitionLabel);
			writer.WriteString(this.ValueLabel);
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000179 RID: 377 RVA: 0x0000543E File Offset: 0x0000363E
		// (set) Token: 0x0600017A RID: 378 RVA: 0x00005446 File Offset: 0x00003646
		public int TextHeight
		{
			get
			{
				return this._textHeight;
			}
			set
			{
				if (value != this._textHeight)
				{
					this._textHeight = value;
					base.OnPropertyChangedWithValue(value, "TextHeight");
				}
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00005464 File Offset: 0x00003664
		// (set) Token: 0x0600017C RID: 380 RVA: 0x0000546C File Offset: 0x0000366C
		public Color TextColor
		{
			get
			{
				return this._textColor;
			}
			set
			{
				if (value != this._textColor)
				{
					this._textColor = value;
					base.OnPropertyChangedWithValue(value, "TextColor");
				}
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600017D RID: 381 RVA: 0x0000548F File Offset: 0x0000368F
		// (set) Token: 0x0600017E RID: 382 RVA: 0x00005497 File Offset: 0x00003697
		public string DefinitionLabel
		{
			get
			{
				return this._definitionLabel;
			}
			set
			{
				if (value != this._definitionLabel)
				{
					this._definitionLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "DefinitionLabel");
				}
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600017F RID: 383 RVA: 0x000054BA File Offset: 0x000036BA
		// (set) Token: 0x06000180 RID: 384 RVA: 0x000054C2 File Offset: 0x000036C2
		public string ValueLabel
		{
			get
			{
				return this._valueLabel;
			}
			set
			{
				if (value != this._valueLabel)
				{
					this._valueLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "ValueLabel");
				}
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000181 RID: 385 RVA: 0x000054E5 File Offset: 0x000036E5
		// (set) Token: 0x06000182 RID: 386 RVA: 0x000054ED File Offset: 0x000036ED
		public int PropertyModifier
		{
			get
			{
				return this._propertyModifier;
			}
			set
			{
				if (value != this._propertyModifier)
				{
					this._propertyModifier = value;
					base.OnPropertyChangedWithValue(value, "PropertyModifier");
				}
			}
		}

		// Token: 0x04000093 RID: 147
		private Func<string> valueFunc;

		// Token: 0x04000094 RID: 148
		private Func<string> definitionFunc;

		// Token: 0x04000095 RID: 149
		private string _definitionLabel;

		// Token: 0x04000096 RID: 150
		private string _valueLabel;

		// Token: 0x04000097 RID: 151
		private Color _textColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x04000098 RID: 152
		private int _textHeight;

		// Token: 0x04000099 RID: 153
		private int _propertyModifier;

		// Token: 0x02000038 RID: 56
		[Flags]
		public enum TooltipPropertyFlags
		{
			// Token: 0x040000E9 RID: 233
			None = 0,
			// Token: 0x040000EA RID: 234
			MultiLine = 1,
			// Token: 0x040000EB RID: 235
			BattleMode = 2,
			// Token: 0x040000EC RID: 236
			BattleModeOver = 4,
			// Token: 0x040000ED RID: 237
			WarFirstEnemy = 8,
			// Token: 0x040000EE RID: 238
			WarFirstAlly = 16,
			// Token: 0x040000EF RID: 239
			WarFirstNeutral = 32,
			// Token: 0x040000F0 RID: 240
			WarSecondEnemy = 64,
			// Token: 0x040000F1 RID: 241
			WarSecondAlly = 128,
			// Token: 0x040000F2 RID: 242
			WarSecondNeutral = 256,
			// Token: 0x040000F3 RID: 243
			RundownSeperator = 512,
			// Token: 0x040000F4 RID: 244
			DefaultSeperator = 1024,
			// Token: 0x040000F5 RID: 245
			Cost = 2048,
			// Token: 0x040000F6 RID: 246
			Title = 4096,
			// Token: 0x040000F7 RID: 247
			RundownResult = 8192
		}
	}
}
