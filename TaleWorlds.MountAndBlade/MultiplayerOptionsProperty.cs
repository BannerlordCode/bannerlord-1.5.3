using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200032A RID: 810
	[AttributeUsage(AttributeTargets.Field)]
	public class MultiplayerOptionsProperty : Attribute
	{
		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x06002E45 RID: 11845 RVA: 0x000B32B2 File Offset: 0x000B14B2
		public bool HasBounds
		{
			get
			{
				return this.BoundsMax > this.BoundsMin;
			}
		}

		// Token: 0x06002E46 RID: 11846 RVA: 0x000B32C4 File Offset: 0x000B14C4
		public MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType optionValueType, MultiplayerOptionsProperty.ReplicationOccurrence replicationOccurrence, string description = null, int boundsMin = 0, int boundsMax = 0, string[] validGameModes = null, bool hasMultipleSelections = false, Type enumType = null)
		{
			this.OptionValueType = optionValueType;
			this.Replication = replicationOccurrence;
			this.Description = description;
			this.BoundsMin = boundsMin;
			this.BoundsMax = boundsMax;
			this.ValidGameModes = validGameModes;
			this.HasMultipleSelections = hasMultipleSelections;
			this.EnumType = enumType;
		}

		// Token: 0x0400123E RID: 4670
		public readonly MultiplayerOptions.OptionValueType OptionValueType;

		// Token: 0x0400123F RID: 4671
		public readonly MultiplayerOptionsProperty.ReplicationOccurrence Replication;

		// Token: 0x04001240 RID: 4672
		public readonly string Description;

		// Token: 0x04001241 RID: 4673
		public readonly int BoundsMin;

		// Token: 0x04001242 RID: 4674
		public readonly int BoundsMax;

		// Token: 0x04001243 RID: 4675
		public readonly string[] ValidGameModes;

		// Token: 0x04001244 RID: 4676
		public readonly bool HasMultipleSelections;

		// Token: 0x04001245 RID: 4677
		public readonly Type EnumType;

		// Token: 0x02000611 RID: 1553
		public enum ReplicationOccurrence
		{
			// Token: 0x040020C0 RID: 8384
			Never,
			// Token: 0x040020C1 RID: 8385
			AtMapLoad,
			// Token: 0x040020C2 RID: 8386
			Immediately
		}
	}
}
