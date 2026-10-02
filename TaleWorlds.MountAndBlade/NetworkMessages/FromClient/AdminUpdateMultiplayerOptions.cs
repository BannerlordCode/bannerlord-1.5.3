using System;
using System.Collections.Generic;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace NetworkMessages.FromClient
{
	// Token: 0x02000015 RID: 21
	[DefineGameNetworkMessageType(GameNetworkMessageSendType.FromClient)]
	public sealed class AdminUpdateMultiplayerOptions : GameNetworkMessage
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00002C9A File Offset: 0x00000E9A
		// (set) Token: 0x06000090 RID: 144 RVA: 0x00002CA2 File Offset: 0x00000EA2
		public List<AdminUpdateMultiplayerOptions.AdminMultiplayerOptionInfo> Options { get; private set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00002CAB File Offset: 0x00000EAB
		// (set) Token: 0x06000092 RID: 146 RVA: 0x00002CB3 File Offset: 0x00000EB3
		public int OptionCount { get; private set; }

		// Token: 0x06000093 RID: 147 RVA: 0x00002CBC File Offset: 0x00000EBC
		public AdminUpdateMultiplayerOptions()
		{
			this.Options = new List<AdminUpdateMultiplayerOptions.AdminMultiplayerOptionInfo>();
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00002CCF File Offset: 0x00000ECF
		protected override MultiplayerMessageFilter OnGetLogFilter()
		{
			return MultiplayerMessageFilter.Administration;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00002CD7 File Offset: 0x00000ED7
		protected override string OnGetLogFormat()
		{
			return "Admin requesting update multiplayer options on server";
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00002CE0 File Offset: 0x00000EE0
		protected override bool OnRead()
		{
			bool flag = true;
			this.OptionCount = GameNetworkMessage.ReadIntFromPacket(new CompressionInfo.Integer(0, 53, true), ref flag);
			for (int i = 0; i < this.OptionCount; i++)
			{
				AdminUpdateMultiplayerOptions.AdminMultiplayerOptionInfo adminMultiplayerOptionInfo = this.ReadOptionInfoFromPacket(ref flag);
				this.Options.Add(adminMultiplayerOptionInfo);
			}
			return flag;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00002D2C File Offset: 0x00000F2C
		protected override void OnWrite()
		{
			GameNetworkMessage.WriteIntToPacket(this.Options.Count, new CompressionInfo.Integer(0, 53, true));
			for (int i = 0; i < this.Options.Count; i++)
			{
				this.WriteOptionInfoToPacket(this.Options[i]);
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00002D7C File Offset: 0x00000F7C
		public void AddMultiplayerOption(MultiplayerOptions.OptionType optionType, MultiplayerOptions.MultiplayerOptionsAccessMode accessMode, bool value)
		{
			AdminUpdateMultiplayerOptions.AdminMultiplayerOptionInfo adminMultiplayerOptionInfo = new AdminUpdateMultiplayerOptions.AdminMultiplayerOptionInfo(optionType, accessMode);
			adminMultiplayerOptionInfo.SetValue(value);
			this.Options.Add(adminMultiplayerOptionInfo);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00002DA4 File Offset: 0x00000FA4
		public void AddMultiplayerOption(MultiplayerOptions.OptionType optionType, MultiplayerOptions.MultiplayerOptionsAccessMode accessMode, int value)
		{
			AdminUpdateMultiplayerOptions.AdminMultiplayerOptionInfo adminMultiplayerOptionInfo = new AdminUpdateMultiplayerOptions.AdminMultiplayerOptionInfo(optionType, accessMode);
			adminMultiplayerOptionInfo.SetValue(value);
			this.Options.Add(adminMultiplayerOptionInfo);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00002DCC File Offset: 0x00000FCC
		public void AddMultiplayerOption(MultiplayerOptions.OptionType optionType, MultiplayerOptions.MultiplayerOptionsAccessMode accessMode, string value)
		{
			AdminUpdateMultiplayerOptions.AdminMultiplayerOptionInfo adminMultiplayerOptionInfo = new AdminUpdateMultiplayerOptions.AdminMultiplayerOptionInfo(optionType, accessMode);
			adminMultiplayerOptionInfo.SetValue(value);
			this.Options.Add(adminMultiplayerOptionInfo);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00002DF4 File Offset: 0x00000FF4
		private AdminUpdateMultiplayerOptions.AdminMultiplayerOptionInfo ReadOptionInfoFromPacket(ref bool bufferReadValid)
		{
			int num = GameNetworkMessage.ReadIntFromPacket(new CompressionInfo.Integer(0, 53, true), ref bufferReadValid);
			MultiplayerOptions.MultiplayerOptionsAccessMode multiplayerOptionsAccessMode = (MultiplayerOptions.MultiplayerOptionsAccessMode)GameNetworkMessage.ReadIntFromPacket(new CompressionInfo.Integer(0, 3, true), ref bufferReadValid);
			AdminUpdateMultiplayerOptions.AdminMultiplayerOptionInfo adminMultiplayerOptionInfo = new AdminUpdateMultiplayerOptions.AdminMultiplayerOptionInfo((MultiplayerOptions.OptionType)num, multiplayerOptionsAccessMode);
			MultiplayerOptionsProperty optionProperty = ((MultiplayerOptions.OptionType)num).GetOptionProperty();
			switch (optionProperty.OptionValueType)
			{
			case MultiplayerOptions.OptionValueType.Bool:
			{
				bool flag = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
				adminMultiplayerOptionInfo.SetValue(flag);
				break;
			}
			case MultiplayerOptions.OptionValueType.Integer:
			case MultiplayerOptions.OptionValueType.Enum:
			{
				int num2 = GameNetworkMessage.ReadIntFromPacket(new CompressionInfo.Integer(optionProperty.BoundsMin, optionProperty.BoundsMax, true), ref bufferReadValid);
				adminMultiplayerOptionInfo.SetValue(num2);
				break;
			}
			case MultiplayerOptions.OptionValueType.String:
			{
				string text = GameNetworkMessage.ReadStringFromPacket(ref bufferReadValid);
				adminMultiplayerOptionInfo.SetValue(text);
				break;
			}
			}
			return adminMultiplayerOptionInfo;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00002E94 File Offset: 0x00001094
		private void WriteOptionInfoToPacket(AdminUpdateMultiplayerOptions.AdminMultiplayerOptionInfo optionInfo)
		{
			GameNetworkMessage.WriteIntToPacket((int)optionInfo.OptionType, new CompressionInfo.Integer(0, 53, true));
			GameNetworkMessage.WriteIntToPacket((int)optionInfo.AccessMode, new CompressionInfo.Integer(0, 3, true));
			MultiplayerOptionsProperty optionProperty = optionInfo.OptionType.GetOptionProperty();
			switch (optionProperty.OptionValueType)
			{
			case MultiplayerOptions.OptionValueType.Bool:
				GameNetworkMessage.WriteBoolToPacket(optionInfo.BoolValue);
				return;
			case MultiplayerOptions.OptionValueType.Integer:
			case MultiplayerOptions.OptionValueType.Enum:
				GameNetworkMessage.WriteIntToPacket(optionInfo.IntValue, new CompressionInfo.Integer(optionProperty.BoundsMin, optionProperty.BoundsMax, true));
				return;
			case MultiplayerOptions.OptionValueType.String:
				GameNetworkMessage.WriteStringToPacket(optionInfo.StringValue);
				return;
			default:
				return;
			}
		}

		// Token: 0x02000411 RID: 1041
		public class AdminMultiplayerOptionInfo
		{
			// Token: 0x17000A34 RID: 2612
			// (get) Token: 0x060038B4 RID: 14516 RVA: 0x000E9FE8 File Offset: 0x000E81E8
			public MultiplayerOptions.OptionType OptionType { get; }

			// Token: 0x17000A35 RID: 2613
			// (get) Token: 0x060038B5 RID: 14517 RVA: 0x000E9FF0 File Offset: 0x000E81F0
			public MultiplayerOptions.MultiplayerOptionsAccessMode AccessMode { get; }

			// Token: 0x17000A36 RID: 2614
			// (get) Token: 0x060038B6 RID: 14518 RVA: 0x000E9FF8 File Offset: 0x000E81F8
			// (set) Token: 0x060038B7 RID: 14519 RVA: 0x000EA000 File Offset: 0x000E8200
			public string StringValue { get; private set; }

			// Token: 0x17000A37 RID: 2615
			// (get) Token: 0x060038B8 RID: 14520 RVA: 0x000EA009 File Offset: 0x000E8209
			// (set) Token: 0x060038B9 RID: 14521 RVA: 0x000EA011 File Offset: 0x000E8211
			public bool BoolValue { get; private set; }

			// Token: 0x17000A38 RID: 2616
			// (get) Token: 0x060038BA RID: 14522 RVA: 0x000EA01A File Offset: 0x000E821A
			// (set) Token: 0x060038BB RID: 14523 RVA: 0x000EA022 File Offset: 0x000E8222
			public int IntValue { get; private set; }

			// Token: 0x060038BC RID: 14524 RVA: 0x000EA02B File Offset: 0x000E822B
			public AdminMultiplayerOptionInfo(MultiplayerOptions.OptionType optionType, MultiplayerOptions.MultiplayerOptionsAccessMode accessMode)
			{
				this.OptionType = optionType;
				this.AccessMode = accessMode;
			}

			// Token: 0x060038BD RID: 14525 RVA: 0x000EA041 File Offset: 0x000E8241
			internal void SetValue(string value)
			{
				this.StringValue = value;
			}

			// Token: 0x060038BE RID: 14526 RVA: 0x000EA04A File Offset: 0x000E824A
			internal void SetValue(bool value)
			{
				this.BoolValue = value;
			}

			// Token: 0x060038BF RID: 14527 RVA: 0x000EA053 File Offset: 0x000E8253
			internal void SetValue(int value)
			{
				this.IntValue = value;
			}
		}
	}
}
