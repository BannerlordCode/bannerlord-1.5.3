using System;
using TaleWorlds.Core;

namespace SandBox.Objects
{
	// Token: 0x02000035 RID: 53
	public class DefaultMusicInstrumentData
	{
		// Token: 0x060001F0 RID: 496 RVA: 0x0000CB10 File Offset: 0x0000AD10
		public DefaultMusicInstrumentData()
		{
			InstrumentData instrumentData = Game.Current.ObjectManager.RegisterPresumedObject<InstrumentData>(new InstrumentData("cheerful"));
			instrumentData.InitializeInstrumentData("act_musician_idle_sitting_cheerful", "act_musician_idle_stand_cheerful", true);
			instrumentData.AfterInitialized();
			InstrumentData instrumentData2 = Game.Current.ObjectManager.RegisterPresumedObject<InstrumentData>(new InstrumentData("active"));
			instrumentData2.InitializeInstrumentData("act_musician_idle_sitting_active", "act_musician_idle_stand_active", true);
			instrumentData2.AfterInitialized();
			InstrumentData instrumentData3 = Game.Current.ObjectManager.RegisterPresumedObject<InstrumentData>(new InstrumentData("calm"));
			instrumentData3.InitializeInstrumentData("act_musician_idle_sitting_calm", "act_musician_idle_stand_calm", true);
			instrumentData3.AfterInitialized();
		}
	}
}
