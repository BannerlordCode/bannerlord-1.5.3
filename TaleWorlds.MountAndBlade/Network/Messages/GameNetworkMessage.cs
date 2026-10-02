using System;
using System.Collections.Generic;
using System.Text;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.Network.Messages
{
	// Token: 0x020003C9 RID: 969
	public abstract class GameNetworkMessage
	{
		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x06003686 RID: 13958 RVA: 0x000E184E File Offset: 0x000DFA4E
		// (set) Token: 0x06003687 RID: 13959 RVA: 0x000E1856 File Offset: 0x000DFA56
		public int MessageId { get; set; }

		// Token: 0x06003688 RID: 13960 RVA: 0x000E1860 File Offset: 0x000DFA60
		internal void Write()
		{
			DebugNetworkEventStatistics.StartEvent(base.GetType().Name, this.MessageId);
			GameNetworkMessage.WriteIntToPacket(this.MessageId, GameNetwork.IsClientOrReplay ? CompressionBasic.NetworkComponentEventTypeFromClientCompressionInfo : CompressionBasic.NetworkComponentEventTypeFromServerCompressionInfo);
			this.OnWrite();
			GameNetworkMessage.WriteIntToPacket(5, GameNetworkMessage.TestValueCompressionInfo);
			DebugNetworkEventStatistics.EndEvent();
		}

		// Token: 0x06003689 RID: 13961
		protected abstract void OnWrite();

		// Token: 0x0600368A RID: 13962 RVA: 0x000E18B8 File Offset: 0x000DFAB8
		internal bool Read()
		{
			bool flag = this.OnRead();
			bool flag2 = true;
			if (GameNetworkMessage.ReadIntFromPacket(GameNetworkMessage.TestValueCompressionInfo, ref flag2) != 5)
			{
				throw new MBNetworkBitException(base.GetType().Name);
			}
			return flag;
		}

		// Token: 0x0600368B RID: 13963
		protected abstract bool OnRead();

		// Token: 0x0600368C RID: 13964 RVA: 0x000E18ED File Offset: 0x000DFAED
		internal MultiplayerMessageFilter GetLogFilter()
		{
			return this.OnGetLogFilter();
		}

		// Token: 0x0600368D RID: 13965
		protected abstract MultiplayerMessageFilter OnGetLogFilter();

		// Token: 0x0600368E RID: 13966 RVA: 0x000E18F5 File Offset: 0x000DFAF5
		internal string GetLogFormat()
		{
			return this.OnGetLogFormat();
		}

		// Token: 0x0600368F RID: 13967
		protected abstract string OnGetLogFormat();

		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x06003690 RID: 13968 RVA: 0x000E18FD File Offset: 0x000DFAFD
		public static bool IsClientMissionOver
		{
			get
			{
				return GameNetwork.IsClient && !NetworkMain.GameClient.IsInGame && !NetworkMain.CommunityClient.IsInGame;
			}
		}

		// Token: 0x06003691 RID: 13969 RVA: 0x000E1924 File Offset: 0x000DFB24
		public static bool ReadBoolFromPacket(ref bool bufferReadValid)
		{
			CompressionInfo.Integer integer = new CompressionInfo.Integer(0, 1);
			int num = 0;
			bufferReadValid = bufferReadValid && MBAPI.IMBNetwork.ReadIntFromPacket(ref integer, out num);
			return num != 0;
		}

		// Token: 0x06003692 RID: 13970 RVA: 0x000E1958 File Offset: 0x000DFB58
		public static void WriteBoolToPacket(bool value)
		{
			CompressionInfo.Integer integer = new CompressionInfo.Integer(0, 1);
			MBAPI.IMBNetwork.WriteIntToPacket(value ? 1 : 0, ref integer);
			DebugNetworkEventStatistics.AddDataToStatistic(integer.GetNumBits());
		}

		// Token: 0x06003693 RID: 13971 RVA: 0x000E1990 File Offset: 0x000DFB90
		public static int ReadIntFromPacket(CompressionInfo.Integer compressionInfo, ref bool bufferReadValid)
		{
			int num = 0;
			bufferReadValid = bufferReadValid && MBAPI.IMBNetwork.ReadIntFromPacket(ref compressionInfo, out num);
			return num;
		}

		// Token: 0x06003694 RID: 13972 RVA: 0x000E19B7 File Offset: 0x000DFBB7
		public static void WriteIntToPacket(int value, CompressionInfo.Integer compressionInfo)
		{
			MBAPI.IMBNetwork.WriteIntToPacket(value, ref compressionInfo);
			DebugNetworkEventStatistics.AddDataToStatistic(compressionInfo.GetNumBits());
		}

		// Token: 0x06003695 RID: 13973 RVA: 0x000E19D4 File Offset: 0x000DFBD4
		public static uint ReadUintFromPacket(CompressionInfo.UnsignedInteger compressionInfo, ref bool bufferReadValid)
		{
			uint num = 0U;
			bufferReadValid = bufferReadValid && MBAPI.IMBNetwork.ReadUintFromPacket(ref compressionInfo, out num);
			return num;
		}

		// Token: 0x06003696 RID: 13974 RVA: 0x000E19FB File Offset: 0x000DFBFB
		public static void WriteUintToPacket(uint value, CompressionInfo.UnsignedInteger compressionInfo)
		{
			MBAPI.IMBNetwork.WriteUintToPacket(value, ref compressionInfo);
			DebugNetworkEventStatistics.AddDataToStatistic(compressionInfo.GetNumBits());
		}

		// Token: 0x06003697 RID: 13975 RVA: 0x000E1A18 File Offset: 0x000DFC18
		public static long ReadLongFromPacket(CompressionInfo.LongInteger compressionInfo, ref bool bufferReadValid)
		{
			long num = 0L;
			bufferReadValid = bufferReadValid && MBAPI.IMBNetwork.ReadLongFromPacket(ref compressionInfo, out num);
			return num;
		}

		// Token: 0x06003698 RID: 13976 RVA: 0x000E1A40 File Offset: 0x000DFC40
		public static void WriteLongToPacket(long value, CompressionInfo.LongInteger compressionInfo)
		{
			MBAPI.IMBNetwork.WriteLongToPacket(value, ref compressionInfo);
			DebugNetworkEventStatistics.AddDataToStatistic(compressionInfo.GetNumBits());
		}

		// Token: 0x06003699 RID: 13977 RVA: 0x000E1A5C File Offset: 0x000DFC5C
		public static ulong ReadUlongFromPacket(CompressionInfo.UnsignedLongInteger compressionInfo, ref bool bufferReadValid)
		{
			ulong num = 0UL;
			bufferReadValid = bufferReadValid && MBAPI.IMBNetwork.ReadUlongFromPacket(ref compressionInfo, out num);
			return num;
		}

		// Token: 0x0600369A RID: 13978 RVA: 0x000E1A84 File Offset: 0x000DFC84
		public static void WriteUlongToPacket(ulong value, CompressionInfo.UnsignedLongInteger compressionInfo)
		{
			MBAPI.IMBNetwork.WriteUlongToPacket(value, ref compressionInfo);
			DebugNetworkEventStatistics.AddDataToStatistic(compressionInfo.GetNumBits());
		}

		// Token: 0x0600369B RID: 13979 RVA: 0x000E1AA0 File Offset: 0x000DFCA0
		public static float ReadFloatFromPacket(CompressionInfo.Float compressionInfo, ref bool bufferReadValid)
		{
			float num = 0f;
			bufferReadValid = bufferReadValid && MBAPI.IMBNetwork.ReadFloatFromPacket(ref compressionInfo, out num);
			return num;
		}

		// Token: 0x0600369C RID: 13980 RVA: 0x000E1ACB File Offset: 0x000DFCCB
		public static void WriteFloatToPacket(float value, CompressionInfo.Float compressionInfo)
		{
			MBAPI.IMBNetwork.WriteFloatToPacket(value, ref compressionInfo);
			DebugNetworkEventStatistics.AddDataToStatistic(compressionInfo.GetNumBits());
		}

		// Token: 0x0600369D RID: 13981 RVA: 0x000E1AE8 File Offset: 0x000DFCE8
		public static string ReadStringFromPacket(ref bool bufferReadValid)
		{
			byte[] array = new byte[1024];
			int num = GameNetworkMessage.ReadByteArrayFromPacket(array, 0, 1024, ref bufferReadValid);
			return GameNetworkMessage.StringEncoding.GetString(array, 0, num);
		}

		// Token: 0x0600369E RID: 13982 RVA: 0x000E1B1C File Offset: 0x000DFD1C
		public static void WriteStringToPacket(string value)
		{
			byte[] array = (string.IsNullOrEmpty(value) ? new byte[0] : GameNetworkMessage.StringEncoding.GetBytes(value));
			GameNetworkMessage.WriteByteArrayToPacket(array, 0, array.Length);
		}

		// Token: 0x0600369F RID: 13983 RVA: 0x000E1B4F File Offset: 0x000DFD4F
		public static int ReadByteArrayFromPacket(byte[] buffer, int offset, int bufferCapacity, ref bool bufferReadValid)
		{
			return MBAPI.IMBNetwork.ReadByteArrayFromPacket(buffer, offset, bufferCapacity, ref bufferReadValid);
		}

		// Token: 0x060036A0 RID: 13984 RVA: 0x000E1B60 File Offset: 0x000DFD60
		public static void WriteBannerCodeToPacket(string bannerCode)
		{
			List<BannerData> list;
			Banner.TryGetBannerDataFromCode(bannerCode, out list);
			GameNetworkMessage.WriteIntToPacket(list.Count, CompressionBasic.BannerDataCountCompressionInfo);
			for (int i = 0; i < list.Count; i++)
			{
				BannerData bannerData = list[i];
				GameNetworkMessage.WriteIntToPacket(bannerData.MeshId, CompressionBasic.BannerDataMeshIdCompressionInfo);
				GameNetworkMessage.WriteIntToPacket(bannerData.ColorId, CompressionBasic.BannerDataColorIndexCompressionInfo);
				GameNetworkMessage.WriteIntToPacket(bannerData.ColorId2, CompressionBasic.BannerDataColorIndexCompressionInfo);
				GameNetworkMessage.WriteIntToPacket((int)bannerData.Size.X, CompressionBasic.BannerDataSizeCompressionInfo);
				GameNetworkMessage.WriteIntToPacket((int)bannerData.Size.Y, CompressionBasic.BannerDataSizeCompressionInfo);
				GameNetworkMessage.WriteIntToPacket((int)bannerData.Position.X, CompressionBasic.BannerDataSizeCompressionInfo);
				GameNetworkMessage.WriteIntToPacket((int)bannerData.Position.Y, CompressionBasic.BannerDataSizeCompressionInfo);
				GameNetworkMessage.WriteBoolToPacket(bannerData.DrawStroke);
				GameNetworkMessage.WriteBoolToPacket(bannerData.Mirror);
				GameNetworkMessage.WriteIntToPacket((int)bannerData.Rotation, CompressionBasic.BannerDataRotationCompressionInfo);
			}
		}

		// Token: 0x060036A1 RID: 13985 RVA: 0x000E1C60 File Offset: 0x000DFE60
		public static string ReadBannerCodeFromPacket(ref bool bufferReadValid)
		{
			int num = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.BannerDataCountCompressionInfo, ref bufferReadValid);
			MBList<BannerData> mblist = new MBList<BannerData>(num);
			for (int i = 0; i < num; i++)
			{
				BannerData bannerData = new BannerData(GameNetworkMessage.ReadIntFromPacket(CompressionBasic.BannerDataMeshIdCompressionInfo, ref bufferReadValid), GameNetworkMessage.ReadIntFromPacket(CompressionBasic.BannerDataColorIndexCompressionInfo, ref bufferReadValid), GameNetworkMessage.ReadIntFromPacket(CompressionBasic.BannerDataColorIndexCompressionInfo, ref bufferReadValid), new Vec2((float)GameNetworkMessage.ReadIntFromPacket(CompressionBasic.BannerDataSizeCompressionInfo, ref bufferReadValid), (float)GameNetworkMessage.ReadIntFromPacket(CompressionBasic.BannerDataSizeCompressionInfo, ref bufferReadValid)), new Vec2((float)GameNetworkMessage.ReadIntFromPacket(CompressionBasic.BannerDataSizeCompressionInfo, ref bufferReadValid), (float)GameNetworkMessage.ReadIntFromPacket(CompressionBasic.BannerDataSizeCompressionInfo, ref bufferReadValid)), GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid), GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid), (float)GameNetworkMessage.ReadIntFromPacket(CompressionBasic.BannerDataRotationCompressionInfo, ref bufferReadValid) * 0.0027777778f);
				mblist.Add(bannerData);
			}
			return Banner.GetBannerCodeFromBannerDataList(mblist);
		}

		// Token: 0x060036A2 RID: 13986 RVA: 0x000E1D1E File Offset: 0x000DFF1E
		public static void WriteByteArrayToPacket(byte[] value, int offset, int size)
		{
			MBAPI.IMBNetwork.WriteByteArrayToPacket(value, offset, size);
			DebugNetworkEventStatistics.AddDataToStatistic(MathF.Min(size, 1024) + 10);
		}

		// Token: 0x060036A3 RID: 13987 RVA: 0x000E1D40 File Offset: 0x000DFF40
		public static MBActionSet ReadActionSetReferenceFromPacket(CompressionInfo.Integer compressionInfo, ref bool bufferReadValid)
		{
			if (bufferReadValid)
			{
				int num;
				bufferReadValid = MBAPI.IMBNetwork.ReadIntFromPacket(ref compressionInfo, out num);
				return new MBActionSet(num);
			}
			return MBActionSet.InvalidActionSet;
		}

		// Token: 0x060036A4 RID: 13988 RVA: 0x000E1D6D File Offset: 0x000DFF6D
		public static void WriteActionSetReferenceToPacket(MBActionSet actionSet, CompressionInfo.Integer compressionInfo)
		{
			MBAPI.IMBNetwork.WriteIntToPacket(actionSet.Index, ref compressionInfo);
			DebugNetworkEventStatistics.AddDataToStatistic(compressionInfo.GetNumBits());
		}

		// Token: 0x060036A5 RID: 13989 RVA: 0x000E1D90 File Offset: 0x000DFF90
		public static int ReadAgentIndexFromPacket(ref bool bufferReadValid)
		{
			CompressionInfo.Integer agentCompressionInfo = CompressionMission.AgentCompressionInfo;
			int num = -1;
			bufferReadValid = bufferReadValid && MBAPI.IMBNetwork.ReadIntFromPacket(ref agentCompressionInfo, out num);
			return num;
		}

		// Token: 0x060036A6 RID: 13990 RVA: 0x000E1DC0 File Offset: 0x000DFFC0
		public static void WriteAgentIndexToPacket(int agentIndex)
		{
			CompressionInfo.Integer agentCompressionInfo = CompressionMission.AgentCompressionInfo;
			MBAPI.IMBNetwork.WriteIntToPacket(agentIndex, ref agentCompressionInfo);
			DebugNetworkEventStatistics.AddDataToStatistic(agentCompressionInfo.GetNumBits());
		}

		// Token: 0x060036A7 RID: 13991 RVA: 0x000E1DEC File Offset: 0x000DFFEC
		public static MBObjectBase ReadObjectReferenceFromPacket(MBObjectManager objectManager, CompressionInfo.UnsignedInteger compressionInfo, ref bool bufferReadValid)
		{
			uint num = GameNetworkMessage.ReadUintFromPacket(compressionInfo, ref bufferReadValid);
			if (bufferReadValid && num > 0U)
			{
				MBGUID mbguid = new MBGUID(num);
				return objectManager.GetObject(mbguid);
			}
			return null;
		}

		// Token: 0x060036A8 RID: 13992 RVA: 0x000E1E1C File Offset: 0x000E001C
		public static void WriteObjectReferenceToPacket(MBObjectBase value, CompressionInfo.UnsignedInteger compressionInfo)
		{
			MBAPI.IMBNetwork.WriteUintToPacket((value != null) ? value.Id.InternalValue : 0U, ref compressionInfo);
			DebugNetworkEventStatistics.AddDataToStatistic(compressionInfo.GetNumBits());
		}

		// Token: 0x060036A9 RID: 13993 RVA: 0x000E1E58 File Offset: 0x000E0058
		public static VirtualPlayer ReadVirtualPlayerReferenceToPacket(ref bool bufferReadValid, bool canReturnNull = false)
		{
			int num = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.PlayerCompressionInfo, ref bufferReadValid);
			bool flag = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
			if ((num >= 0 && !GameNetworkMessage.IsClientMissionOver) & bufferReadValid)
			{
				VirtualPlayer virtualPlayer;
				if (!flag)
				{
					virtualPlayer = GameNetwork.VirtualPlayers[num];
				}
				else
				{
					virtualPlayer = GameNetwork.DisconnectedNetworkPeers[num].VirtualPlayer;
				}
				return virtualPlayer;
			}
			return null;
		}

		// Token: 0x060036AA RID: 13994 RVA: 0x000E1EAD File Offset: 0x000E00AD
		public static NetworkCommunicator ReadNetworkPeerReferenceFromPacket(ref bool bufferReadValid, bool canReturnNull = false)
		{
			VirtualPlayer virtualPlayer = GameNetworkMessage.ReadVirtualPlayerReferenceToPacket(ref bufferReadValid, canReturnNull);
			return ((virtualPlayer != null) ? virtualPlayer.Communicator : null) as NetworkCommunicator;
		}

		// Token: 0x060036AB RID: 13995 RVA: 0x000E1EC8 File Offset: 0x000E00C8
		public static void WriteVirtualPlayerReferenceToPacket(VirtualPlayer virtualPlayer)
		{
			bool flag = false;
			int num = ((virtualPlayer != null) ? virtualPlayer.Index : (-1));
			if (num >= 0 && GameNetwork.VirtualPlayers[num] != virtualPlayer)
			{
				for (int i = 0; i < GameNetwork.DisconnectedNetworkPeers.Count; i++)
				{
					if (GameNetwork.DisconnectedNetworkPeers[i].VirtualPlayer == virtualPlayer)
					{
						num = i;
						flag = true;
						break;
					}
				}
			}
			GameNetworkMessage.WriteIntToPacket(num, CompressionBasic.PlayerCompressionInfo);
			GameNetworkMessage.WriteBoolToPacket(flag);
		}

		// Token: 0x060036AC RID: 13996 RVA: 0x000E1F31 File Offset: 0x000E0131
		public static void WriteNetworkPeerReferenceToPacket(NetworkCommunicator networkCommunicator)
		{
			GameNetworkMessage.WriteVirtualPlayerReferenceToPacket((networkCommunicator != null) ? networkCommunicator.VirtualPlayer : null);
		}

		// Token: 0x060036AD RID: 13997 RVA: 0x000E1F44 File Offset: 0x000E0144
		public static int ReadTeamIndexFromPacket(ref bool bufferReadValid)
		{
			return GameNetworkMessage.ReadIntFromPacket(CompressionMission.TeamCompressionInfo, ref bufferReadValid);
		}

		// Token: 0x060036AE RID: 13998 RVA: 0x000E1F51 File Offset: 0x000E0151
		public static void WriteTeamIndexToPacket(int teamIndex)
		{
			GameNetworkMessage.WriteIntToPacket(teamIndex, CompressionMission.TeamCompressionInfo);
		}

		// Token: 0x060036AF RID: 13999 RVA: 0x000E1F60 File Offset: 0x000E0160
		public static MissionObjectId ReadMissionObjectIdFromPacket(ref bool bufferReadValid)
		{
			bool flag = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
			int num = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.MissionObjectIDCompressionInfo, ref bufferReadValid);
			if (!bufferReadValid || num == -1 || GameNetworkMessage.IsClientMissionOver)
			{
				if (num != -1)
				{
					MBDebug.Print(string.Concat(new object[]
					{
						"Reading null MissionObject because IsClientMissionOver: ",
						GameNetworkMessage.IsClientMissionOver.ToString(),
						" valid read: ",
						bufferReadValid.ToString(),
						" MissionObject ID: ",
						num,
						" runtime: ",
						flag.ToString()
					}), 0, Debug.DebugColor.White, 17592186044416UL);
				}
				return new MissionObjectId(-1, false);
			}
			return new MissionObjectId(num, flag);
		}

		// Token: 0x060036B0 RID: 14000 RVA: 0x000E200A File Offset: 0x000E020A
		public static void WriteMissionObjectIdToPacket(MissionObjectId value)
		{
			GameNetworkMessage.WriteBoolToPacket(value.CreatedAtRuntime);
			GameNetworkMessage.WriteIntToPacket(value.Id, CompressionBasic.MissionObjectIDCompressionInfo);
		}

		// Token: 0x060036B1 RID: 14001 RVA: 0x000E2028 File Offset: 0x000E0228
		public static Vec3 ReadVec3FromPacket(CompressionInfo.Float compressionInfo, ref bool bufferReadValid)
		{
			float num = GameNetworkMessage.ReadFloatFromPacket(compressionInfo, ref bufferReadValid);
			float num2 = GameNetworkMessage.ReadFloatFromPacket(compressionInfo, ref bufferReadValid);
			float num3 = GameNetworkMessage.ReadFloatFromPacket(compressionInfo, ref bufferReadValid);
			return new Vec3(num, num2, num3, -1f);
		}

		// Token: 0x060036B2 RID: 14002 RVA: 0x000E2058 File Offset: 0x000E0258
		public static void WriteVec3ToPacket(Vec3 value, CompressionInfo.Float compressionInfo)
		{
			GameNetworkMessage.WriteFloatToPacket(value.x, compressionInfo);
			GameNetworkMessage.WriteFloatToPacket(value.y, compressionInfo);
			GameNetworkMessage.WriteFloatToPacket(value.z, compressionInfo);
		}

		// Token: 0x060036B3 RID: 14003 RVA: 0x000E2080 File Offset: 0x000E0280
		public static Vec2 ReadVec2FromPacket(CompressionInfo.Float compressionInfo, ref bool bufferReadValid)
		{
			float num = GameNetworkMessage.ReadFloatFromPacket(compressionInfo, ref bufferReadValid);
			float num2 = GameNetworkMessage.ReadFloatFromPacket(compressionInfo, ref bufferReadValid);
			return new Vec2(num, num2);
		}

		// Token: 0x060036B4 RID: 14004 RVA: 0x000E20A2 File Offset: 0x000E02A2
		public static void WriteVec2ToPacket(Vec2 value, CompressionInfo.Float compressionInfo)
		{
			GameNetworkMessage.WriteFloatToPacket(value.x, compressionInfo);
			GameNetworkMessage.WriteFloatToPacket(value.y, compressionInfo);
		}

		// Token: 0x060036B5 RID: 14005 RVA: 0x000E20BC File Offset: 0x000E02BC
		public static Mat3 ReadRotationMatrixFromPacket(ref bool bufferReadValid)
		{
			Vec3 vec = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.UnitVectorCompressionInfo, ref bufferReadValid);
			Vec3 vec2 = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.UnitVectorCompressionInfo, ref bufferReadValid);
			Vec3 vec3 = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.UnitVectorCompressionInfo, ref bufferReadValid);
			return new Mat3(in vec, in vec2, in vec3);
		}

		// Token: 0x060036B6 RID: 14006 RVA: 0x000E20F8 File Offset: 0x000E02F8
		public static void WriteRotationMatrixToPacket(Mat3 value)
		{
			GameNetworkMessage.WriteVec3ToPacket(value.s, CompressionBasic.UnitVectorCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(value.f, CompressionBasic.UnitVectorCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(value.u, CompressionBasic.UnitVectorCompressionInfo);
		}

		// Token: 0x060036B7 RID: 14007 RVA: 0x000E212C File Offset: 0x000E032C
		public static MatrixFrame ReadMatrixFrameFromPacket(ref bool bufferReadValid)
		{
			Vec3 vec = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.PositionCompressionInfo, ref bufferReadValid);
			Vec3 vec2 = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.ScaleCompressionInfo, ref bufferReadValid);
			Mat3 mat = GameNetworkMessage.ReadRotationMatrixFromPacket(ref bufferReadValid);
			MatrixFrame matrixFrame = new MatrixFrame(in mat, in vec);
			matrixFrame.Scale(in vec2);
			return matrixFrame;
		}

		// Token: 0x060036B8 RID: 14008 RVA: 0x000E216C File Offset: 0x000E036C
		public static void WriteMatrixFrameToPacket(MatrixFrame frame)
		{
			Vec3 scaleVector = frame.rotation.GetScaleVector();
			MatrixFrame matrixFrame = frame;
			Vec3 vec = new Vec3(1f / scaleVector.x, 1f / scaleVector.y, 1f / scaleVector.z, -1f);
			matrixFrame.Scale(in vec);
			GameNetworkMessage.WriteVec3ToPacket(matrixFrame.origin, CompressionBasic.PositionCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(scaleVector, CompressionBasic.ScaleCompressionInfo);
			GameNetworkMessage.WriteRotationMatrixToPacket(matrixFrame.rotation);
		}

		// Token: 0x060036B9 RID: 14009 RVA: 0x000E21E8 File Offset: 0x000E03E8
		public static MatrixFrame ReadNonUniformTransformFromPacket(CompressionInfo.Float positionCompressionInfo, CompressionInfo.Float quaternionCompressionInfo, ref bool bufferReadValid)
		{
			MatrixFrame matrixFrame = GameNetworkMessage.ReadUnitTransformFromPacket(positionCompressionInfo, quaternionCompressionInfo, ref bufferReadValid);
			Vec3 vec = GameNetworkMessage.ReadVec3FromPacket(CompressionBasic.ScaleCompressionInfo, ref bufferReadValid);
			matrixFrame.rotation.ApplyScaleLocal(in vec);
			return matrixFrame;
		}

		// Token: 0x060036BA RID: 14010 RVA: 0x000E221C File Offset: 0x000E041C
		public static void WriteNonUniformTransformToPacket(MatrixFrame frame, CompressionInfo.Float positionCompressionInfo, CompressionInfo.Float quaternionCompressionInfo)
		{
			MatrixFrame matrixFrame = frame;
			Vec3 vec = matrixFrame.rotation.MakeUnit();
			GameNetworkMessage.WriteUnitTransformToPacket(matrixFrame, positionCompressionInfo, quaternionCompressionInfo);
			GameNetworkMessage.WriteVec3ToPacket(vec, CompressionBasic.ScaleCompressionInfo);
		}

		// Token: 0x060036BB RID: 14011 RVA: 0x000E224C File Offset: 0x000E044C
		public static MatrixFrame ReadTransformFromPacket(CompressionInfo.Float positionCompressionInfo, CompressionInfo.Float quaternionCompressionInfo, ref bool bufferReadValid)
		{
			MatrixFrame matrixFrame = GameNetworkMessage.ReadUnitTransformFromPacket(positionCompressionInfo, quaternionCompressionInfo, ref bufferReadValid);
			if (GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid))
			{
				float num = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.ScaleCompressionInfo, ref bufferReadValid);
				matrixFrame.rotation.ApplyScaleLocal(num);
			}
			return matrixFrame;
		}

		// Token: 0x060036BC RID: 14012 RVA: 0x000E2284 File Offset: 0x000E0484
		public static void WriteTransformToPacket(MatrixFrame frame, CompressionInfo.Float positionCompressionInfo, CompressionInfo.Float quaternionCompressionInfo)
		{
			MatrixFrame matrixFrame = frame;
			Vec3 vec = matrixFrame.rotation.MakeUnit();
			GameNetworkMessage.WriteUnitTransformToPacket(matrixFrame, positionCompressionInfo, quaternionCompressionInfo);
			bool flag = !vec.x.ApproximatelyEqualsTo(1f, CompressionBasic.ScaleCompressionInfo.GetPrecision());
			GameNetworkMessage.WriteBoolToPacket(flag);
			if (flag)
			{
				GameNetworkMessage.WriteFloatToPacket(vec.x, CompressionBasic.ScaleCompressionInfo);
			}
		}

		// Token: 0x060036BD RID: 14013 RVA: 0x000E22E0 File Offset: 0x000E04E0
		public static MatrixFrame ReadUnitTransformFromPacket(CompressionInfo.Float positionCompressionInfo, CompressionInfo.Float quaternionCompressionInfo, ref bool bufferReadValid)
		{
			return new MatrixFrame
			{
				origin = GameNetworkMessage.ReadVec3FromPacket(positionCompressionInfo, ref bufferReadValid),
				rotation = GameNetworkMessage.ReadQuaternionFromPacket(quaternionCompressionInfo, ref bufferReadValid).ToMat3()
			};
		}

		// Token: 0x060036BE RID: 14014 RVA: 0x000E231A File Offset: 0x000E051A
		public static void WriteUnitTransformToPacket(MatrixFrame frame, CompressionInfo.Float positionCompressionInfo, CompressionInfo.Float quaternionCompressionInfo)
		{
			GameNetworkMessage.WriteVec3ToPacket(frame.origin, positionCompressionInfo);
			GameNetworkMessage.WriteQuaternionToPacket(frame.rotation.ToQuaternion(), quaternionCompressionInfo);
		}

		// Token: 0x060036BF RID: 14015 RVA: 0x000E233C File Offset: 0x000E053C
		public static Quaternion ReadQuaternionFromPacket(CompressionInfo.Float compressionInfo, ref bool bufferReadValid)
		{
			Quaternion quaternion = default(Quaternion);
			float num = 0f;
			int num2 = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.OmittedQuaternionComponentIndexCompressionInfo, ref bufferReadValid);
			for (int i = 0; i < 4; i++)
			{
				if (i != num2)
				{
					quaternion[i] = GameNetworkMessage.ReadFloatFromPacket(compressionInfo, ref bufferReadValid);
					num += quaternion[i] * quaternion[i];
				}
			}
			quaternion[num2] = MathF.Sqrt(1f - num);
			quaternion.SafeNormalize();
			return quaternion;
		}

		// Token: 0x060036C0 RID: 14016 RVA: 0x000E23B4 File Offset: 0x000E05B4
		public static void WriteQuaternionToPacket(Quaternion q, CompressionInfo.Float compressionInfo)
		{
			int num = -1;
			float num2 = 0f;
			Quaternion quaternion = q;
			quaternion.SafeNormalize();
			for (int i = 0; i < 4; i++)
			{
				float num3 = MathF.Abs(quaternion[i]);
				if (num3 > num2)
				{
					num2 = num3;
					num = i;
				}
			}
			if (quaternion[num] < 0f)
			{
				quaternion.Flip();
			}
			GameNetworkMessage.WriteIntToPacket(num, CompressionBasic.OmittedQuaternionComponentIndexCompressionInfo);
			for (int j = 0; j < 4; j++)
			{
				if (j != num)
				{
					GameNetworkMessage.WriteFloatToPacket(quaternion[j], compressionInfo);
				}
			}
		}

		// Token: 0x060036C1 RID: 14017 RVA: 0x000E2440 File Offset: 0x000E0640
		public static void WriteBodyPropertiesToPacket(BodyProperties bodyProperties)
		{
			GameNetworkMessage.WriteFloatToPacket(bodyProperties.Age, CompressionBasic.AgentAgeCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(bodyProperties.Weight, CompressionBasic.FaceKeyDataCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(bodyProperties.Build, CompressionBasic.FaceKeyDataCompressionInfo);
			GameNetworkMessage.WriteUlongToPacket(bodyProperties.KeyPart1, CompressionBasic.DebugULongNonCompressionInfo);
			GameNetworkMessage.WriteUlongToPacket(bodyProperties.KeyPart2, CompressionBasic.DebugULongNonCompressionInfo);
			GameNetworkMessage.WriteUlongToPacket(bodyProperties.KeyPart3, CompressionBasic.DebugULongNonCompressionInfo);
			GameNetworkMessage.WriteUlongToPacket(bodyProperties.KeyPart4, CompressionBasic.DebugULongNonCompressionInfo);
			GameNetworkMessage.WriteUlongToPacket(bodyProperties.KeyPart5, CompressionBasic.DebugULongNonCompressionInfo);
			GameNetworkMessage.WriteUlongToPacket(bodyProperties.KeyPart6, CompressionBasic.DebugULongNonCompressionInfo);
			GameNetworkMessage.WriteUlongToPacket(bodyProperties.KeyPart7, CompressionBasic.DebugULongNonCompressionInfo);
			GameNetworkMessage.WriteUlongToPacket(bodyProperties.KeyPart8, CompressionBasic.DebugULongNonCompressionInfo);
		}

		// Token: 0x060036C2 RID: 14018 RVA: 0x000E2508 File Offset: 0x000E0708
		public static BodyProperties ReadBodyPropertiesFromPacket(ref bool bufferReadValid)
		{
			float num = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.AgentAgeCompressionInfo, ref bufferReadValid);
			float num2 = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.FaceKeyDataCompressionInfo, ref bufferReadValid);
			float num3 = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.FaceKeyDataCompressionInfo, ref bufferReadValid);
			ulong num4 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref bufferReadValid);
			ulong num5 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref bufferReadValid);
			ulong num6 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref bufferReadValid);
			ulong num7 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref bufferReadValid);
			ulong num8 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref bufferReadValid);
			ulong num9 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref bufferReadValid);
			ulong num10 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref bufferReadValid);
			ulong num11 = GameNetworkMessage.ReadUlongFromPacket(CompressionBasic.DebugULongNonCompressionInfo, ref bufferReadValid);
			if (bufferReadValid)
			{
				return new BodyProperties(new DynamicBodyProperties(num, num2, num3), new StaticBodyProperties(num4, num5, num6, num7, num8, num9, num10, num11));
			}
			return default(BodyProperties);
		}

		// Token: 0x0400177F RID: 6015
		private static readonly Encoding StringEncoding = new UTF8Encoding();

		// Token: 0x04001780 RID: 6016
		private static CompressionInfo.Integer TestValueCompressionInfo = new CompressionInfo.Integer(0, 3);

		// Token: 0x04001781 RID: 6017
		private const int ConstTestValue = 5;

		// Token: 0x02000694 RID: 1684
		// (Invoke) Token: 0x0600424B RID: 16971
		public delegate bool ClientMessageHandlerDelegate<T>(NetworkCommunicator peer, T message) where T : GameNetworkMessage;

		// Token: 0x02000695 RID: 1685
		// (Invoke) Token: 0x0600424F RID: 16975
		public delegate void ServerMessageHandlerDelegate<T>(T message) where T : GameNetworkMessage;
	}
}
