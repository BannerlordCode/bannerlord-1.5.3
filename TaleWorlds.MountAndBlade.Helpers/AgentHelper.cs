using System;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000002 RID: 2
	internal static class AgentHelper
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static Vec3 GetAgentPosition(UIntPtr agentPositionPointer)
		{
			Vec3* ptr = (Vec3*)agentPositionPointer.ToPointer();
			return *ptr;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002064 File Offset: 0x00000264
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void SetAgentPosition(UIntPtr agentPositionPointer, ref Vec3 newPos)
		{
			Debug.FailedAssert("Do not use this!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Helpers\\Helper.cs", "SetAgentPosition", 20);
			Vec3* ptr = (Vec3*)agentPositionPointer.ToPointer();
			*ptr = newPos;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000209C File Offset: 0x0000029C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static int GetAgentIndex(UIntPtr indexPtr)
		{
			int* ptr = (int*)indexPtr.ToPointer();
			return *ptr;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020B4 File Offset: 0x000002B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static AgentFlag GetAgentFlags(UIntPtr flagsPtr)
		{
			AgentFlag* ptr = (AgentFlag*)flagsPtr.ToPointer();
			return *ptr;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020CC File Offset: 0x000002CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static AgentState GetAgentState(UIntPtr statePtr)
		{
			AgentState* ptr = (AgentState*)statePtr.ToPointer();
			return *ptr;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020E4 File Offset: 0x000002E4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static AgentMovementMode GetAgentMovementMode(UIntPtr movementModePointer)
		{
			AgentMovementMode* ptr = (AgentMovementMode*)movementModePointer.ToPointer();
			return *ptr;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020FC File Offset: 0x000002FC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static AgentControllerType GetAgentControllerType(UIntPtr controllerTypePointer)
		{
			AgentControllerType* ptr = (AgentControllerType*)controllerTypePointer.ToPointer();
			return *ptr;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002114 File Offset: 0x00000314
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static float GetAgentMovementDirectionAsAngle(UIntPtr movementDirectionPointer)
		{
			float* ptr = (float*)movementDirectionPointer.ToPointer();
			return *ptr;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000212C File Offset: 0x0000032C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static EquipmentIndex GetPrimaryWieldedItemIndex(UIntPtr primaryWieldedItemIndexPointer)
		{
			EquipmentIndex* ptr = (EquipmentIndex*)primaryWieldedItemIndexPointer.ToPointer();
			return *ptr;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002144 File Offset: 0x00000344
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static EquipmentIndex GetOffhandWieldedItemIndex(UIntPtr offhandWieldedItemIndexPointer)
		{
			EquipmentIndex* ptr = (EquipmentIndex*)offhandWieldedItemIndexPointer.ToPointer();
			return *ptr;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000215C File Offset: 0x0000035C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static int GetChannel0CurrentActionIndex(UIntPtr channel0CurrentActionPointer)
		{
			int* ptr = (int*)channel0CurrentActionPointer.ToPointer();
			return *ptr;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002174 File Offset: 0x00000374
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static int GetChannel1CurrentActionIndex(UIntPtr channel1CurrentActionPointer)
		{
			int* ptr = (int*)channel1CurrentActionPointer.ToPointer();
			return *ptr;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000218C File Offset: 0x0000038C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static float GetMaximumForwardUnlimitedSpeed(UIntPtr maximumForwardUnlimitedSpeed)
		{
			float* ptr = (float*)maximumForwardUnlimitedSpeed.ToPointer();
			return *ptr;
		}
	}
}
