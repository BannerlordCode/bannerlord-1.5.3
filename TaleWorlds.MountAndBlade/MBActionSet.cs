using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200019F RID: 415
	[EngineStruct("int", false, null)]
	public struct MBActionSet
	{
		// Token: 0x06001640 RID: 5696 RVA: 0x00052128 File Offset: 0x00050328
		internal MBActionSet(int i)
		{
			this.Index = i;
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06001641 RID: 5697 RVA: 0x00052131 File Offset: 0x00050331
		public bool IsValid
		{
			get
			{
				return this.Index >= 0;
			}
		}

		// Token: 0x06001642 RID: 5698 RVA: 0x0005213F File Offset: 0x0005033F
		public bool Equals(MBActionSet a)
		{
			return this.Index == a.Index;
		}

		// Token: 0x06001643 RID: 5699 RVA: 0x0005214F File Offset: 0x0005034F
		public bool Equals(int index)
		{
			return this.Index == index;
		}

		// Token: 0x06001644 RID: 5700 RVA: 0x0005215A File Offset: 0x0005035A
		public override int GetHashCode()
		{
			return this.Index;
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x00052162 File Offset: 0x00050362
		public string GetName()
		{
			if (!this.IsValid)
			{
				return "Invalid";
			}
			return MBAPI.IMBActionSet.GetNameWithIndex(this.Index);
		}

		// Token: 0x06001646 RID: 5702 RVA: 0x00052182 File Offset: 0x00050382
		public string GetSkeletonName()
		{
			return MBAPI.IMBActionSet.GetSkeletonName(this.Index);
		}

		// Token: 0x06001647 RID: 5703 RVA: 0x00052194 File Offset: 0x00050394
		public string GetAnimationName(in ActionIndexCache actionCode)
		{
			return MBAPI.IMBActionSet.GetAnimationName(this.Index, actionCode.Index);
		}

		// Token: 0x06001648 RID: 5704 RVA: 0x000521AC File Offset: 0x000503AC
		public bool AreActionsAlternatives(in ActionIndexCache actionCode1, in ActionIndexCache actionCode2)
		{
			return MBAPI.IMBActionSet.AreActionsAlternatives(this.Index, actionCode1.Index, actionCode2.Index);
		}

		// Token: 0x06001649 RID: 5705 RVA: 0x000521CA File Offset: 0x000503CA
		public static int GetNumberOfActionSets()
		{
			return MBAPI.IMBActionSet.GetNumberOfActionSets();
		}

		// Token: 0x0600164A RID: 5706 RVA: 0x000521D6 File Offset: 0x000503D6
		public static int GetNumberOfMonsterUsageSets()
		{
			return MBAPI.IMBActionSet.GetNumberOfMonsterUsageSets();
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x000521E2 File Offset: 0x000503E2
		public static MBActionSet GetActionSet(string objectID)
		{
			return MBActionSet.GetActionSetWithIndex(MBAPI.IMBActionSet.GetIndexWithID(objectID));
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x000521F4 File Offset: 0x000503F4
		public static MBActionSet GetActionSetWithIndex(int index)
		{
			return new MBActionSet(index);
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x000521FC File Offset: 0x000503FC
		public static sbyte GetBoneIndexWithId(string actionSetId, string boneId)
		{
			return MBAPI.IMBActionSet.GetBoneIndexWithId(actionSetId, boneId);
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x0005220A File Offset: 0x0005040A
		public static bool GetBoneHasParentBone(string actionSetId, sbyte boneIndex)
		{
			return MBAPI.IMBActionSet.GetBoneHasParentBone(actionSetId, boneIndex);
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x00052218 File Offset: 0x00050418
		public static Vec3 GetActionDisplacementVector(MBActionSet actionSet, in ActionIndexCache actionIndexCache)
		{
			return MBAPI.IMBAnimation.GetDisplacementVector(actionSet.Index, actionIndexCache.Index);
		}

		// Token: 0x06001650 RID: 5712 RVA: 0x00052230 File Offset: 0x00050430
		public static AnimFlags GetActionAnimationFlags(MBActionSet actionSet, in ActionIndexCache actionIndexCache)
		{
			return MBAPI.IMBAnimation.GetAnimationFlags(actionSet.Index, actionIndexCache.Index);
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x00052248 File Offset: 0x00050448
		public static bool CheckActionAnimationClipExists(MBActionSet actionSet, in ActionIndexCache actionIndexCache)
		{
			return MBAPI.IMBAnimation.CheckAnimationClipExists(actionSet.Index, actionIndexCache.Index);
		}

		// Token: 0x06001652 RID: 5714 RVA: 0x00052260 File Offset: 0x00050460
		public static int GetAnimationIndexOfAction(MBActionSet actionSet, in ActionIndexCache actionIndexCache)
		{
			return MBAPI.IMBAnimation.AnimationIndexOfActionCode(actionSet.Index, actionIndexCache.Index);
		}

		// Token: 0x06001653 RID: 5715 RVA: 0x00052278 File Offset: 0x00050478
		public static string GetActionAnimationName(MBActionSet actionSet, in ActionIndexCache actionIndexCache)
		{
			return MBAPI.IMBAnimation.GetAnimationName(actionSet.Index, actionIndexCache.Index);
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x00052290 File Offset: 0x00050490
		public static float GetActionAnimationDuration(MBActionSet actionSet, in ActionIndexCache actionIndexCache)
		{
			return MBAPI.IMBAnimation.GetActionAnimationDuration(actionSet.Index, actionIndexCache.Index);
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x000522A8 File Offset: 0x000504A8
		public static ActionIndexCache GetActionAnimationContinueToAction(MBActionSet actionSet, in ActionIndexCache actionIndexCache)
		{
			return new ActionIndexCache(MBAPI.IMBAnimation.GetAnimationContinueToAction(actionSet.Index, actionIndexCache.Index));
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x000522C8 File Offset: 0x000504C8
		public static float GetTotalAnimationDurationWithContinueToAction(MBActionSet actionSet, ActionIndexCache actionIndexCache)
		{
			float num = 0f;
			while (actionIndexCache != ActionIndexCache.act_none)
			{
				num += MBActionSet.GetActionAnimationDuration(actionSet, in actionIndexCache);
				actionIndexCache = MBActionSet.GetActionAnimationContinueToAction(actionSet, in actionIndexCache);
			}
			return num;
		}

		// Token: 0x06001657 RID: 5719 RVA: 0x00052300 File Offset: 0x00050500
		public static float GetActionBlendOutStartProgress(MBActionSet actionSet, in ActionIndexCache actionIndexCache)
		{
			return MBAPI.IMBAnimation.GetActionBlendOutStartProgress(actionSet.Index, actionIndexCache.Index);
		}

		// Token: 0x0400072E RID: 1838
		[CustomEngineStructMemberData("ignoredMember", true)]
		internal readonly int Index;

		// Token: 0x0400072F RID: 1839
		public static readonly MBActionSet InvalidActionSet = new MBActionSet(-1);
	}
}
