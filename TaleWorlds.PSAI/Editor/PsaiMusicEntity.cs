using System;
using System.Collections.Generic;

namespace psai.Editor
{
	// Token: 0x02000007 RID: 7
	[Serializable]
	public abstract class PsaiMusicEntity : ICloneable
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00002DF8 File Offset: 0x00000FF8
		// (set) Token: 0x06000055 RID: 85 RVA: 0x00002E00 File Offset: 0x00001000
		public string Name { get; set; }

		// Token: 0x06000056 RID: 86
		public abstract string GetClassString();

		// Token: 0x06000057 RID: 87
		public abstract CompatibilitySetting GetCompatibilitySetting(PsaiMusicEntity targetEntity);

		// Token: 0x06000058 RID: 88
		public abstract CompatibilityType GetCompatibilityType(PsaiMusicEntity targetEntity, out CompatibilityReason reason);

		// Token: 0x06000059 RID: 89
		public abstract PsaiMusicEntity GetParent();

		// Token: 0x0600005A RID: 90
		public abstract List<PsaiMusicEntity> GetChildren();

		// Token: 0x0600005B RID: 91
		public abstract int GetIndexPositionWithinParentEntity(PsaiProject parentProject);

		// Token: 0x0600005C RID: 92 RVA: 0x00002E09 File Offset: 0x00001009
		public virtual object Clone()
		{
			return base.MemberwiseClone();
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002E11 File Offset: 0x00001011
		public virtual PsaiMusicEntity ShallowCopy()
		{
			return (PsaiMusicEntity)base.MemberwiseClone();
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002E1E File Offset: 0x0000101E
		public virtual bool PropertyDifferencesAffectCompatibilities(PsaiMusicEntity otherEntity)
		{
			return false;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002E24 File Offset: 0x00001024
		public Theme GetTheme()
		{
			PsaiMusicEntity psaiMusicEntity = this;
			Theme theme;
			do
			{
				theme = psaiMusicEntity as Theme;
				psaiMusicEntity = psaiMusicEntity.GetParent();
			}
			while (theme == null && psaiMusicEntity != null);
			return theme;
		}
	}
}
