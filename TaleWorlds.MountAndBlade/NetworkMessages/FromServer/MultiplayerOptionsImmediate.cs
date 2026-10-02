using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000061 RID: 97
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class MultiplayerOptionsImmediate : GameNetworkMessage
	{
		// Token: 0x06000362 RID: 866 RVA: 0x00006828 File Offset: 0x00004A28
		public MultiplayerOptionsImmediate()
		{
			this._optionList = new List<MultiplayerOptions.MultiplayerOption>();
			for (MultiplayerOptions.OptionType optionType = MultiplayerOptions.OptionType.ServerName; optionType < MultiplayerOptions.OptionType.NumOfSlots; optionType++)
			{
				if (optionType.GetOptionProperty().Replication == MultiplayerOptionsProperty.ReplicationOccurrence.Immediately)
				{
					this._optionList.Add(MultiplayerOptions.Instance.GetOptionFromOptionType(optionType, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
				}
			}
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00006878 File Offset: 0x00004A78
		public MultiplayerOptions.MultiplayerOption GetOption(MultiplayerOptions.OptionType optionType)
		{
			return this._optionList.First<MultiplayerOptions.MultiplayerOption>((MultiplayerOptions.MultiplayerOption x) => x.OptionType == optionType);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x000068AC File Offset: 0x00004AAC
		protected override bool OnRead()
		{
			bool flag = true;
			this._optionList = new List<MultiplayerOptions.MultiplayerOption>();
			for (MultiplayerOptions.OptionType optionType = MultiplayerOptions.OptionType.ServerName; optionType < MultiplayerOptions.OptionType.NumOfSlots; optionType++)
			{
				MultiplayerOptionsProperty optionProperty = optionType.GetOptionProperty();
				if (optionProperty.Replication == MultiplayerOptionsProperty.ReplicationOccurrence.Immediately)
				{
					MultiplayerOptions.MultiplayerOption multiplayerOption = MultiplayerOptions.MultiplayerOption.CreateMultiplayerOption(optionType);
					switch (optionProperty.OptionValueType)
					{
					case MultiplayerOptions.OptionValueType.Bool:
						multiplayerOption.UpdateValue(GameNetworkMessage.ReadBoolFromPacket(ref flag));
						break;
					case MultiplayerOptions.OptionValueType.Integer:
					case MultiplayerOptions.OptionValueType.Enum:
						multiplayerOption.UpdateValue(GameNetworkMessage.ReadIntFromPacket(new CompressionInfo.Integer(optionProperty.BoundsMin, optionProperty.BoundsMax, true), ref flag));
						break;
					case MultiplayerOptions.OptionValueType.String:
						multiplayerOption.UpdateValue(GameNetworkMessage.ReadStringFromPacket(ref flag));
						break;
					}
					this._optionList.Add(multiplayerOption);
				}
			}
			return flag;
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00006960 File Offset: 0x00004B60
		protected override void OnWrite()
		{
			foreach (MultiplayerOptions.MultiplayerOption multiplayerOption in this._optionList)
			{
				MultiplayerOptions.OptionType optionType = multiplayerOption.OptionType;
				MultiplayerOptionsProperty optionProperty = optionType.GetOptionProperty();
				switch (optionProperty.OptionValueType)
				{
				case MultiplayerOptions.OptionValueType.Bool:
					GameNetworkMessage.WriteBoolToPacket(optionType.GetBoolValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
					break;
				case MultiplayerOptions.OptionValueType.Integer:
				case MultiplayerOptions.OptionValueType.Enum:
					GameNetworkMessage.WriteIntToPacket(optionType.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions), new CompressionInfo.Integer(optionProperty.BoundsMin, optionProperty.BoundsMax, true));
					break;
				case MultiplayerOptions.OptionValueType.String:
					GameNetworkMessage.WriteStringToPacket(optionType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
					break;
				}
			}
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00006A14 File Offset: 0x00004C14
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00006A1C File Offset: 0x00004C1C
		protected override string OnGetLogFormat()
		{
			return "Receiving runtime multiplayer options.";
		}

		// Token: 0x040000A6 RID: 166
		private List<MultiplayerOptions.MultiplayerOption> _optionList;
	}
}
