using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001A6 RID: 422
	public struct MBAnimation
	{
		// Token: 0x060016AE RID: 5806 RVA: 0x00053831 File Offset: 0x00051A31
		public MBAnimation(MBAnimation animation)
		{
			this._index = animation._index;
		}

		// Token: 0x060016AF RID: 5807 RVA: 0x0005383F File Offset: 0x00051A3F
		internal MBAnimation(int i)
		{
			this._index = i;
		}

		// Token: 0x060016B0 RID: 5808 RVA: 0x00053848 File Offset: 0x00051A48
		public bool Equals(MBAnimation a)
		{
			return this._index == a._index;
		}

		// Token: 0x060016B1 RID: 5809 RVA: 0x00053858 File Offset: 0x00051A58
		public override int GetHashCode()
		{
			return this._index;
		}

		// Token: 0x060016B2 RID: 5810 RVA: 0x00053860 File Offset: 0x00051A60
		public static int GetAnimationIndexWithName(string animationName)
		{
			if (string.IsNullOrEmpty(animationName))
			{
				return -1;
			}
			return MBAPI.IMBAnimation.GetIndexWithID(animationName);
		}

		// Token: 0x060016B3 RID: 5811 RVA: 0x00053877 File Offset: 0x00051A77
		public static Agent.ActionCodeType GetActionType(ActionIndexCache actionIndex)
		{
			if (!(actionIndex == ActionIndexCache.act_none))
			{
				return MBAPI.IMBAnimation.GetActionType(actionIndex.Index);
			}
			return Agent.ActionCodeType.Other;
		}

		// Token: 0x060016B4 RID: 5812 RVA: 0x00053899 File Offset: 0x00051A99
		public static void PrefetchAnimationClip(MBActionSet actionSet, ActionIndexCache actionIndexCache)
		{
			MBAPI.IMBAnimation.PrefetchAnimationClip(actionSet.Index, actionIndexCache.Index);
		}

		// Token: 0x060016B5 RID: 5813 RVA: 0x000538B4 File Offset: 0x00051AB4
		public static float GetAnimationDuration(string animationName)
		{
			int indexWithID = MBAPI.IMBAnimation.GetIndexWithID(animationName);
			return MBAPI.IMBAnimation.GetAnimationDuration(indexWithID);
		}

		// Token: 0x060016B6 RID: 5814 RVA: 0x000538D8 File Offset: 0x00051AD8
		public static float GetAnimationDuration(int animationIndex)
		{
			return MBAPI.IMBAnimation.GetAnimationDuration(animationIndex);
		}

		// Token: 0x060016B7 RID: 5815 RVA: 0x000538E8 File Offset: 0x00051AE8
		public static float GetAnimationParameter1(string animationName)
		{
			int indexWithID = MBAPI.IMBAnimation.GetIndexWithID(animationName);
			return MBAPI.IMBAnimation.GetAnimationParameter1(indexWithID);
		}

		// Token: 0x060016B8 RID: 5816 RVA: 0x0005390C File Offset: 0x00051B0C
		public static float GetAnimationParameter1(int animationIndex)
		{
			return MBAPI.IMBAnimation.GetAnimationParameter1(animationIndex);
		}

		// Token: 0x060016B9 RID: 5817 RVA: 0x0005391C File Offset: 0x00051B1C
		public static float GetAnimationParameter2(string animationName)
		{
			int indexWithID = MBAPI.IMBAnimation.GetIndexWithID(animationName);
			return MBAPI.IMBAnimation.GetAnimationParameter2(indexWithID);
		}

		// Token: 0x060016BA RID: 5818 RVA: 0x00053940 File Offset: 0x00051B40
		public static float GetAnimationParameter2(int animationIndex)
		{
			return MBAPI.IMBAnimation.GetAnimationParameter2(animationIndex);
		}

		// Token: 0x060016BB RID: 5819 RVA: 0x00053950 File Offset: 0x00051B50
		public static float GetAnimationParameter3(string animationName)
		{
			int indexWithID = MBAPI.IMBAnimation.GetIndexWithID(animationName);
			return MBAPI.IMBAnimation.GetAnimationParameter3(indexWithID);
		}

		// Token: 0x060016BC RID: 5820 RVA: 0x00053974 File Offset: 0x00051B74
		public static float GetAnimationBlendInPeriod(string animationName)
		{
			int indexWithID = MBAPI.IMBAnimation.GetIndexWithID(animationName);
			return MBAPI.IMBAnimation.GetAnimationBlendInPeriod(indexWithID);
		}

		// Token: 0x060016BD RID: 5821 RVA: 0x00053998 File Offset: 0x00051B98
		public static float GetAnimationBlendInPeriod(int animationIndex)
		{
			return MBAPI.IMBAnimation.GetAnimationBlendInPeriod(animationIndex);
		}

		// Token: 0x060016BE RID: 5822 RVA: 0x000539A8 File Offset: 0x00051BA8
		public static ActionIndexCache GetAnimationBlendsWithActionIndex(string animationName)
		{
			int indexWithID = MBAPI.IMBAnimation.GetIndexWithID(animationName);
			return new ActionIndexCache(MBAPI.IMBAnimation.GetAnimationBlendsWithActionIndex(indexWithID));
		}

		// Token: 0x060016BF RID: 5823 RVA: 0x000539D1 File Offset: 0x00051BD1
		public static ActionIndexCache GetAnimationBlendsWithActionIndex(int animationIndex)
		{
			return new ActionIndexCache(MBAPI.IMBAnimation.GetAnimationBlendsWithActionIndex(animationIndex));
		}

		// Token: 0x060016C0 RID: 5824 RVA: 0x000539E4 File Offset: 0x00051BE4
		public static Vec3 GetAnimationDisplacementAtProgress(string animationName, float progress)
		{
			int indexWithID = MBAPI.IMBAnimation.GetIndexWithID(animationName);
			return MBAPI.IMBAnimation.GetAnimationDisplacementAtProgress(indexWithID, progress);
		}

		// Token: 0x060016C1 RID: 5825 RVA: 0x00053A09 File Offset: 0x00051C09
		public static Vec3 GetAnimationDisplacementAtProgress(int animationIndex, float progress)
		{
			return MBAPI.IMBAnimation.GetAnimationDisplacementAtProgress(animationIndex, progress);
		}

		// Token: 0x060016C2 RID: 5826 RVA: 0x00053A17 File Offset: 0x00051C17
		public static int GetActionCodeWithName(string name)
		{
			if (!string.IsNullOrEmpty(name))
			{
				return MBAPI.IMBAnimation.GetActionCodeWithName(name);
			}
			return ActionIndexCache.act_none.Index;
		}

		// Token: 0x060016C3 RID: 5827 RVA: 0x00053A37 File Offset: 0x00051C37
		public static int GetNumActionCodes()
		{
			return MBAPI.IMBAnimation.GetNumActionCodes();
		}

		// Token: 0x060016C4 RID: 5828 RVA: 0x00053A43 File Offset: 0x00051C43
		public static int GetNumAnimations()
		{
			return MBAPI.IMBAnimation.GetNumAnimations();
		}

		// Token: 0x060016C5 RID: 5829 RVA: 0x00053A4F File Offset: 0x00051C4F
		public static bool IsAnyAnimationLoadingFromDisk()
		{
			return MBAPI.IMBAnimation.IsAnyAnimationLoadingFromDisk();
		}

		// Token: 0x04000872 RID: 2162
		private readonly int _index;
	}
}
