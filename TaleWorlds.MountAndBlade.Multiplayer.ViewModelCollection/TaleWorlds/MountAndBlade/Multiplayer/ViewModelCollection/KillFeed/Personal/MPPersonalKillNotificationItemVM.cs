using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.KillFeed.Personal
{
	// Token: 0x0200008B RID: 139
	public class MPPersonalKillNotificationItemVM : ViewModel
	{
		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06000D98 RID: 3480 RVA: 0x00029BE3 File Offset: 0x00027DE3
		// (set) Token: 0x06000D99 RID: 3481 RVA: 0x00029BEB File Offset: 0x00027DEB
		private MPPersonalKillNotificationItemVM.ItemTypes ItemTypeAsEnum
		{
			get
			{
				return this._itemTypeAsEnum;
			}
			set
			{
				this._itemType = (int)value;
				this._itemTypeAsEnum = value;
			}
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x00029BFC File Offset: 0x00027DFC
		public MPPersonalKillNotificationItemVM(int amount, bool isFatal, bool isMountDamage, bool isFriendlyFire, bool isHeadshot, string killedAgentName, Action<MPPersonalKillNotificationItemVM> onRemoveItem)
		{
			this._onRemoveItem = onRemoveItem;
			this.Amount = amount;
			if (isFriendlyFire)
			{
				this.ItemTypeAsEnum = (isFatal ? MPPersonalKillNotificationItemVM.ItemTypes.FriendlyFireKill : MPPersonalKillNotificationItemVM.ItemTypes.FriendlyFireDamage);
				this.Message = killedAgentName;
				return;
			}
			if (isMountDamage)
			{
				this.ItemTypeAsEnum = MPPersonalKillNotificationItemVM.ItemTypes.MountDamage;
				this.Message = GameTexts.FindText("str_damage_delivered_message", null).ToString();
				return;
			}
			if (isFatal)
			{
				this.ItemTypeAsEnum = (isHeadshot ? MPPersonalKillNotificationItemVM.ItemTypes.HeadshotKill : MPPersonalKillNotificationItemVM.ItemTypes.NormalKill);
				this.Message = killedAgentName;
				return;
			}
			this.ItemTypeAsEnum = MPPersonalKillNotificationItemVM.ItemTypes.NormalDamage;
			this.Message = GameTexts.FindText("str_damage_delivered_message", null).ToString();
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x00029C90 File Offset: 0x00027E90
		public MPPersonalKillNotificationItemVM(int amount, GoldGainFlags reasonType, Action<MPPersonalKillNotificationItemVM> onRemoveItem)
		{
			this._onRemoveItem = onRemoveItem;
			this.ItemTypeAsEnum = MPPersonalKillNotificationItemVM.ItemTypes.GoldChange;
			if (reasonType <= GoldGainFlags.TenthKill)
			{
				if (reasonType <= GoldGainFlags.SecondAssist)
				{
					switch (reasonType)
					{
					case GoldGainFlags.FirstRangedKill:
						this.Message = GameTexts.FindText("str_gold_gain_first_ranged_kill", null).ToString();
						goto IL_0200;
					case GoldGainFlags.FirstMeleeKill:
						this.Message = GameTexts.FindText("str_gold_gain_first_melee_kill", null).ToString();
						goto IL_0200;
					case GoldGainFlags.FirstRangedKill | GoldGainFlags.FirstMeleeKill:
						break;
					case GoldGainFlags.FirstAssist:
						this.Message = GameTexts.FindText("str_gold_gain_first_assist", null).ToString();
						goto IL_0200;
					default:
						if (reasonType == GoldGainFlags.SecondAssist)
						{
							this.Message = GameTexts.FindText("str_gold_gain_second_assist", null).ToString();
							goto IL_0200;
						}
						break;
					}
				}
				else
				{
					if (reasonType == GoldGainFlags.ThirdAssist)
					{
						this.Message = GameTexts.FindText("str_gold_gain_third_assist", null).ToString();
						goto IL_0200;
					}
					if (reasonType == GoldGainFlags.FifthKill)
					{
						this.Message = GameTexts.FindText("str_gold_gain_fifth_kill", null).ToString();
						goto IL_0200;
					}
					if (reasonType == GoldGainFlags.TenthKill)
					{
						this.Message = GameTexts.FindText("str_gold_gain_tenth_kill", null).ToString();
						goto IL_0200;
					}
				}
			}
			else if (reasonType <= GoldGainFlags.DefaultAssist)
			{
				if (reasonType == GoldGainFlags.DefaultKill)
				{
					this.Message = GameTexts.FindText("str_gold_gain_default_kill", null).ToString();
					goto IL_0200;
				}
				if (reasonType == GoldGainFlags.DefaultAssist)
				{
					this.Message = GameTexts.FindText("str_gold_gain_default_assist", null).ToString();
					goto IL_0200;
				}
			}
			else
			{
				if (reasonType == GoldGainFlags.ObjectiveCompleted)
				{
					this.Message = GameTexts.FindText("str_gold_gain_objective_completed", null).ToString();
					goto IL_0200;
				}
				if (reasonType == GoldGainFlags.ObjectiveDestroyed)
				{
					this.Message = GameTexts.FindText("str_gold_gain_objective_destroyed", null).ToString();
					goto IL_0200;
				}
				if (reasonType == GoldGainFlags.PerkBonus)
				{
					this.Message = GameTexts.FindText("str_gold_gain_perk_bonus", null).ToString();
					goto IL_0200;
				}
			}
			Debug.FailedAssert("Undefined gold change type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\KillFeed\\Personal\\MPPersonalKillNotificationItemVM.cs", ".ctor", 117);
			this.Message = "";
			IL_0200:
			this.Amount = amount;
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x00029EA4 File Offset: 0x000280A4
		public MPPersonalKillNotificationItemVM(string victimAgentName, Action<MPPersonalKillNotificationItemVM> onRemoveItem)
		{
			this._onRemoveItem = onRemoveItem;
			this.Amount = -1;
			this.Message = victimAgentName;
			this.ItemTypeAsEnum = MPPersonalKillNotificationItemVM.ItemTypes.Assist;
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x00029EC8 File Offset: 0x000280C8
		public void ExecuteRemove()
		{
			this._onRemoveItem(this);
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06000D9E RID: 3486 RVA: 0x00029ED6 File Offset: 0x000280D6
		// (set) Token: 0x06000D9F RID: 3487 RVA: 0x00029EDE File Offset: 0x000280DE
		[DataSourceProperty]
		public string Message
		{
			get
			{
				return this._message;
			}
			set
			{
				if (value != this._message)
				{
					this._message = value;
					base.OnPropertyChangedWithValue<string>(value, "Message");
				}
			}
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06000DA0 RID: 3488 RVA: 0x00029F01 File Offset: 0x00028101
		// (set) Token: 0x06000DA1 RID: 3489 RVA: 0x00029F09 File Offset: 0x00028109
		[DataSourceProperty]
		public int ItemType
		{
			get
			{
				return this._itemType;
			}
			set
			{
				if (value != this._itemType)
				{
					this._itemType = value;
					base.OnPropertyChangedWithValue(value, "ItemType");
				}
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06000DA2 RID: 3490 RVA: 0x00029F27 File Offset: 0x00028127
		// (set) Token: 0x06000DA3 RID: 3491 RVA: 0x00029F2F File Offset: 0x0002812F
		[DataSourceProperty]
		public int Amount
		{
			get
			{
				return this._amount;
			}
			set
			{
				if (value != this._amount)
				{
					this._amount = value;
					base.OnPropertyChangedWithValue(value, "Amount");
				}
			}
		}

		// Token: 0x04000632 RID: 1586
		private Action<MPPersonalKillNotificationItemVM> _onRemoveItem;

		// Token: 0x04000633 RID: 1587
		private MPPersonalKillNotificationItemVM.ItemTypes _itemTypeAsEnum;

		// Token: 0x04000634 RID: 1588
		private string _message;

		// Token: 0x04000635 RID: 1589
		private int _amount;

		// Token: 0x04000636 RID: 1590
		private int _itemType;

		// Token: 0x02000185 RID: 389
		private enum ItemTypes
		{
			// Token: 0x04000A9D RID: 2717
			NormalDamage,
			// Token: 0x04000A9E RID: 2718
			FriendlyFireDamage,
			// Token: 0x04000A9F RID: 2719
			FriendlyFireKill,
			// Token: 0x04000AA0 RID: 2720
			MountDamage,
			// Token: 0x04000AA1 RID: 2721
			NormalKill,
			// Token: 0x04000AA2 RID: 2722
			Assist,
			// Token: 0x04000AA3 RID: 2723
			GoldChange,
			// Token: 0x04000AA4 RID: 2724
			HeadshotKill
		}
	}
}
