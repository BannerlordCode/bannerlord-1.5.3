using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core
{
	// Token: 0x02000058 RID: 88
	[Serializable]
	public struct DynamicBodyProperties
	{
		// Token: 0x060006FA RID: 1786 RVA: 0x00018436 File Offset: 0x00016636
		public DynamicBodyProperties(float age, float weight, float build)
		{
			this.Age = age;
			this.Weight = weight;
			this.Build = build;
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x00018450 File Offset: 0x00016650
		public static bool operator ==(DynamicBodyProperties a, DynamicBodyProperties b)
		{
			return a == b || (a != null && b != null && (a.Age == b.Age && a.Weight == b.Weight) && a.Build == b.Build);
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x000184AB File Offset: 0x000166AB
		public static bool operator !=(DynamicBodyProperties a, DynamicBodyProperties b)
		{
			return !(a == b);
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x000184B7 File Offset: 0x000166B7
		public bool Equals(DynamicBodyProperties other)
		{
			return this.Age.Equals(other.Age) && this.Weight.Equals(other.Weight) && this.Build.Equals(other.Build);
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x000184F2 File Offset: 0x000166F2
		public override bool Equals(object obj)
		{
			return obj != null && obj is DynamicBodyProperties && this.Equals((DynamicBodyProperties)obj);
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0001850F File Offset: 0x0001670F
		public override int GetHashCode()
		{
			return (((this.Age.GetHashCode() * 397) ^ this.Weight.GetHashCode()) * 397) ^ this.Build.GetHashCode();
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x00018540 File Offset: 0x00016740
		public override string ToString()
		{
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(150, "ToString");
			mbstringBuilder.Append<string>("age=\"");
			mbstringBuilder.Append<string>(this.Age.ToString("0.##"));
			mbstringBuilder.Append<string>("\" weight=\"");
			mbstringBuilder.Append<string>(this.Weight.ToString("0.####"));
			mbstringBuilder.Append<string>("\" build=\"");
			mbstringBuilder.Append<string>(this.Build.ToString("0.####"));
			mbstringBuilder.Append<string>("\" ");
			return mbstringBuilder.ToStringAndRelease();
		}

		// Token: 0x0400037B RID: 891
		public const float MaxAge = 128f;

		// Token: 0x0400037C RID: 892
		public const float MaxAgeTeenager = 21f;

		// Token: 0x0400037D RID: 893
		public float Age;

		// Token: 0x0400037E RID: 894
		public float Weight;

		// Token: 0x0400037F RID: 895
		public float Build;

		// Token: 0x04000380 RID: 896
		public static readonly DynamicBodyProperties Invalid;

		// Token: 0x04000381 RID: 897
		public static readonly DynamicBodyProperties Default = new DynamicBodyProperties(20f, 0.5f, 0.5f);
	}
}
