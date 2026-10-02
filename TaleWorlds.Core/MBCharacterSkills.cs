using System;
using System.Xml;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x0200009D RID: 157
	public class MBCharacterSkills : MBObjectBase
	{
		// Token: 0x1700031D RID: 797
		// (get) Token: 0x060008FB RID: 2299 RVA: 0x0001D8FC File Offset: 0x0001BAFC
		// (set) Token: 0x060008FC RID: 2300 RVA: 0x0001D904 File Offset: 0x0001BB04
		public PropertyOwner<SkillObject> Skills { get; private set; }

		// Token: 0x060008FD RID: 2301 RVA: 0x0001D90D File Offset: 0x0001BB0D
		public MBCharacterSkills()
		{
			this.Skills = new PropertyOwner<SkillObject>();
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x0001D920 File Offset: 0x0001BB20
		public void Init(MBObjectManager objectManager, XmlNode node)
		{
			base.Initialize();
			this.Skills.Deserialize(objectManager, node);
			base.AfterInitialized();
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x0001D93B File Offset: 0x0001BB3B
		public override void Deserialize(MBObjectManager objectManager, XmlNode node)
		{
			base.Deserialize(objectManager, node);
			this.Skills.Deserialize(objectManager, node);
		}
	}
}
