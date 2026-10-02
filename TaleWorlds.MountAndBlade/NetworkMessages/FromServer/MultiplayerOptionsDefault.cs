using System;
using System.Collections.Generic;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromServer
{
	// Token: 0x02000060 RID: 96
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromServer)]
	public sealed class MultiplayerOptionsDefault : GameNetworkMessage
	{
		// Token: 0x0600035D RID: 861 RVA: 0x00006694 File Offset: 0x00004894
		public MultiplayerOptionsDefault()
		{
			this._optionList = new List<MultiplayerOptions.OptionType>();
			for (MultiplayerOptions.OptionType optionType = MultiplayerOptions.OptionType.ServerName; optionType < MultiplayerOptions.OptionType.NumOfSlots; optionType++)
			{
				if (optionType.GetOptionProperty().Replication != MultiplayerOptionsProperty.ReplicationOccurrence.Never)
				{
					this._optionList.Add(optionType);
				}
			}
		}

		// Token: 0x0600035E RID: 862 RVA: 0x000066D8 File Offset: 0x000048D8
		protected override bool OnRead()
		{
			bool flag = true;
			for (int i = 0; i < this._optionList.Count; i++)
			{
				MultiplayerOptions.OptionType optionType = this._optionList[i];
				MultiplayerOptionsProperty optionProperty = optionType.GetOptionProperty();
				switch (optionProperty.OptionValueType)
				{
				case MultiplayerOptions.OptionValueType.Bool:
				{
					bool flag2 = GameNetworkMessage.ReadBoolFromPacket(ref flag);
					optionType.SetValue(flag2, MultiplayerOptions.MultiplayerOptionsAccessMode.DefaultMapOptions);
					break;
				}
				case MultiplayerOptions.OptionValueType.Integer:
				case MultiplayerOptions.OptionValueType.Enum:
				{
					int num = GameNetworkMessage.ReadIntFromPacket(new CompressionInfo.Integer(optionProperty.BoundsMin, optionProperty.BoundsMax, true), ref flag);
					optionType.SetValue(num, MultiplayerOptions.MultiplayerOptionsAccessMode.DefaultMapOptions);
					break;
				}
				case MultiplayerOptions.OptionValueType.String:
				{
					string text = GameNetworkMessage.ReadStringFromPacket(ref flag);
					optionType.SetValue(text, MultiplayerOptions.MultiplayerOptionsAccessMode.DefaultMapOptions);
					break;
				}
				}
			}
			return flag;
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00006788 File Offset: 0x00004988
		protected override void OnWrite()
		{
			for (int i = 0; i < this._optionList.Count; i++)
			{
				MultiplayerOptions.OptionType optionType = this._optionList[i];
				MultiplayerOptionsProperty optionProperty = optionType.GetOptionProperty();
				switch (optionProperty.OptionValueType)
				{
				case MultiplayerOptions.OptionValueType.Bool:
					GameNetworkMessage.WriteBoolToPacket(optionType.GetBoolValue(MultiplayerOptions.MultiplayerOptionsAccessMode.DefaultMapOptions));
					break;
				case MultiplayerOptions.OptionValueType.Integer:
				case MultiplayerOptions.OptionValueType.Enum:
					GameNetworkMessage.WriteIntToPacket(optionType.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.DefaultMapOptions), new CompressionInfo.Integer(optionProperty.BoundsMin, optionProperty.BoundsMax, true));
					break;
				case MultiplayerOptions.OptionValueType.String:
					GameNetworkMessage.WriteStringToPacket(optionType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.DefaultMapOptions));
					break;
				}
			}
		}

		// Token: 0x06000360 RID: 864 RVA: 0x00006818 File Offset: 0x00004A18
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00006820 File Offset: 0x00004A20
		protected override string OnGetLogFormat()
		{
			return "Receiving default multiplayer options.";
		}

		// Token: 0x040000A5 RID: 165
		private readonly List<MultiplayerOptions.OptionType> _optionList;
	}
}
