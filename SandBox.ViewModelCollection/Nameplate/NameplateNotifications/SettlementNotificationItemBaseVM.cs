using System;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Nameplate.NameplateNotifications
{
	// Token: 0x02000022 RID: 34
	public class SettlementNotificationItemBaseVM : ViewModel
	{
		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000354 RID: 852 RVA: 0x0000EA7E File Offset: 0x0000CC7E
		// (set) Token: 0x06000355 RID: 853 RVA: 0x0000EA86 File Offset: 0x0000CC86
		public int CreatedTick { get; set; }

		// Token: 0x06000356 RID: 854 RVA: 0x0000EA8F File Offset: 0x0000CC8F
		public SettlementNotificationItemBaseVM(Action<SettlementNotificationItemBaseVM> onRemove, int createdTick)
		{
			this._onRemove = onRemove;
			this.RelationType = 0;
			this.CreatedTick = createdTick;
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000EAAC File Offset: 0x0000CCAC
		public void ExecuteRemove()
		{
			this._onRemove(this);
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000358 RID: 856 RVA: 0x0000EABA File Offset: 0x0000CCBA
		// (set) Token: 0x06000359 RID: 857 RVA: 0x0000EAC2 File Offset: 0x0000CCC2
		public string CharacterName
		{
			get
			{
				return this._characterName;
			}
			set
			{
				if (value != this._characterName)
				{
					this._characterName = value;
					base.OnPropertyChangedWithValue<string>(value, "CharacterName");
				}
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600035A RID: 858 RVA: 0x0000EAE5 File Offset: 0x0000CCE5
		// (set) Token: 0x0600035B RID: 859 RVA: 0x0000EAED File Offset: 0x0000CCED
		public int RelationType
		{
			get
			{
				return this._relationType;
			}
			set
			{
				if (value != this._relationType)
				{
					this._relationType = value;
					base.OnPropertyChangedWithValue(value, "RelationType");
				}
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600035C RID: 860 RVA: 0x0000EB0B File Offset: 0x0000CD0B
		// (set) Token: 0x0600035D RID: 861 RVA: 0x0000EB13 File Offset: 0x0000CD13
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				if (value != this._text)
				{
					this._text = value;
					base.OnPropertyChangedWithValue<string>(value, "Text");
				}
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600035E RID: 862 RVA: 0x0000EB36 File Offset: 0x0000CD36
		// (set) Token: 0x0600035F RID: 863 RVA: 0x0000EB3E File Offset: 0x0000CD3E
		public CharacterImageIdentifierVM CharacterVisual
		{
			get
			{
				return this._characterVisual;
			}
			set
			{
				if (value != this._characterVisual)
				{
					this._characterVisual = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "CharacterVisual");
				}
			}
		}

		// Token: 0x040001B5 RID: 437
		private readonly Action<SettlementNotificationItemBaseVM> _onRemove;

		// Token: 0x040001B7 RID: 439
		private CharacterImageIdentifierVM _characterVisual;

		// Token: 0x040001B8 RID: 440
		private string _text;

		// Token: 0x040001B9 RID: 441
		private string _characterName;

		// Token: 0x040001BA RID: 442
		private int _relationType;
	}
}
