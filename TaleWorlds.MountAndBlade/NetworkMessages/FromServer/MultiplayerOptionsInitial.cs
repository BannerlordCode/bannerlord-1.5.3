using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000062 RID: 98
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class MultiplayerOptionsInitial : GameNetworkMessage
	{
		// Token: 0x06000368 RID: 872 RVA: 0x00006A24 File Offset: 0x00004C24
		public MultiplayerOptionsInitial()
		{
			this._optionList = new List<MultiplayerOptions.MultiplayerOption>();
			for (MultiplayerOptions.OptionType optionType = MultiplayerOptions.OptionType.ServerName; optionType < MultiplayerOptions.OptionType.NumOfSlots; optionType++)
			{
				if (optionType.GetOptionProperty().Replication == MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad)
				{
					this._optionList.Add(MultiplayerOptions.Instance.GetOptionFromOptionType(optionType, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
				}
			}
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00006A74 File Offset: 0x00004C74
		public MultiplayerOptions.MultiplayerOption GetOption(MultiplayerOptions.OptionType optionType)
		{
			return this._optionList.First<MultiplayerOptions.MultiplayerOption>((MultiplayerOptions.MultiplayerOption x) => x.OptionType == optionType);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00006AA8 File Offset: 0x00004CA8
		protected override bool OnRead()
		{
			bool flag = true;
			this._optionList = new List<MultiplayerOptions.MultiplayerOption>();
			for (MultiplayerOptions.OptionType optionType = MultiplayerOptions.OptionType.ServerName; optionType < MultiplayerOptions.OptionType.NumOfSlots; optionType++)
			{
				MultiplayerOptionsProperty optionProperty = optionType.GetOptionProperty();
				if (optionProperty.Replication == MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad)
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

		// Token: 0x0600036B RID: 875 RVA: 0x00006B5C File Offset: 0x00004D5C
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

		// Token: 0x0600036C RID: 876 RVA: 0x00006C10 File Offset: 0x00004E10
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00006C18 File Offset: 0x00004E18
		protected override string OnGetLogFormat()
		{
			return "Receiving initial multiplayer options.";
		}

		// Token: 0x040000A7 RID: 167
		private List<MultiplayerOptions.MultiplayerOption> _optionList;
	}
}
