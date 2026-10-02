using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.Naval
{
	// Token: 0x0200023C RID: 572
	public class DefaultFigureheads
	{
		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x060022CA RID: 8906 RVA: 0x0009997B File Offset: 0x00097B7B
		public static DefaultFigureheads Instance
		{
			get
			{
				return Campaign.Current.DefaultFigureheads;
			}
		}

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x060022CB RID: 8907 RVA: 0x00099987 File Offset: 0x00097B87
		public static Figurehead Hawk
		{
			get
			{
				return DefaultFigureheads.Instance._hawk;
			}
		}

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x060022CC RID: 8908 RVA: 0x00099993 File Offset: 0x00097B93
		public static Figurehead Lion
		{
			get
			{
				return DefaultFigureheads.Instance._lion;
			}
		}

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x060022CD RID: 8909 RVA: 0x0009999F File Offset: 0x00097B9F
		public static Figurehead Dragon
		{
			get
			{
				return DefaultFigureheads.Instance._dragon;
			}
		}

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x060022CE RID: 8910 RVA: 0x000999AB File Offset: 0x00097BAB
		public static Figurehead WingsOfVictory
		{
			get
			{
				return DefaultFigureheads.Instance._wingsOfVictory;
			}
		}

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x060022CF RID: 8911 RVA: 0x000999B7 File Offset: 0x00097BB7
		public static Figurehead Ram
		{
			get
			{
				return DefaultFigureheads.Instance._ram;
			}
		}

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x060022D0 RID: 8912 RVA: 0x000999C3 File Offset: 0x00097BC3
		public static Figurehead SeaSerpent
		{
			get
			{
				return DefaultFigureheads.Instance._seaSerpent;
			}
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x060022D1 RID: 8913 RVA: 0x000999CF File Offset: 0x00097BCF
		public static Figurehead Viper
		{
			get
			{
				return DefaultFigureheads.Instance._viper;
			}
		}

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x060022D2 RID: 8914 RVA: 0x000999DB File Offset: 0x00097BDB
		public static Figurehead SaberToothTiger
		{
			get
			{
				return DefaultFigureheads.Instance._saberToothTiger;
			}
		}

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x060022D3 RID: 8915 RVA: 0x000999E7 File Offset: 0x00097BE7
		public static Figurehead Siren
		{
			get
			{
				return DefaultFigureheads.Instance._siren;
			}
		}

		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x060022D4 RID: 8916 RVA: 0x000999F3 File Offset: 0x00097BF3
		public static Figurehead Horse
		{
			get
			{
				return DefaultFigureheads.Instance._horse;
			}
		}

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x060022D5 RID: 8917 RVA: 0x000999FF File Offset: 0x00097BFF
		public static Figurehead Turtle
		{
			get
			{
				return DefaultFigureheads.Instance._turtle;
			}
		}

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x060022D6 RID: 8918 RVA: 0x00099A0B File Offset: 0x00097C0B
		public static Figurehead Boar
		{
			get
			{
				return DefaultFigureheads.Instance._boar;
			}
		}

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x060022D7 RID: 8919 RVA: 0x00099A17 File Offset: 0x00097C17
		public static Figurehead Oxen
		{
			get
			{
				return DefaultFigureheads.Instance._oxen;
			}
		}

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x060022D8 RID: 8920 RVA: 0x00099A23 File Offset: 0x00097C23
		public static Figurehead Swan
		{
			get
			{
				return DefaultFigureheads.Instance._swan;
			}
		}

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x060022D9 RID: 8921 RVA: 0x00099A2F File Offset: 0x00097C2F
		public static Figurehead Deer
		{
			get
			{
				return DefaultFigureheads.Instance._deer;
			}
		}

		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x060022DA RID: 8922 RVA: 0x00099A3B File Offset: 0x00097C3B
		public static Figurehead Raven
		{
			get
			{
				return DefaultFigureheads.Instance._raven;
			}
		}

		// Token: 0x060022DB RID: 8923 RVA: 0x00099A47 File Offset: 0x00097C47
		public DefaultFigureheads()
		{
			this.RegisterAll();
		}

		// Token: 0x060022DC RID: 8924 RVA: 0x00099A58 File Offset: 0x00097C58
		private void RegisterAll()
		{
			this._hawk = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("hawk"));
			this._lion = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("lion"));
			this._dragon = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("dragon"));
			this._wingsOfVictory = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("wings_of_victory"));
			this._ram = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("ram"));
			this._seaSerpent = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("sea_serpent"));
			this._viper = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("viper"));
			this._saberToothTiger = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("saber_tooth_tiger"));
			this._siren = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("siren"));
			this._horse = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("horse"));
			this._turtle = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("turtle"));
			this._boar = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("boar"));
			this._oxen = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("oxen"));
			this._swan = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("swan"));
			this._deer = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("deer"));
			this._raven = MBObjectManager.Instance.RegisterPresumedObject<Figurehead>(new Figurehead("raven"));
			this.InitializeAll();
		}

		// Token: 0x060022DD RID: 8925 RVA: 0x00099C0C File Offset: 0x00097E0C
		private void InitializeAll()
		{
			this._hawk.Initialize(new TextObject("{=VKFTub9a}Hawk", null), new TextObject("{=ku2DXiY9}Crew ranged accuracy {EFFECT_AMOUNT}%", null), 0.15f, MBObjectManager.Instance.GetObject<CultureObject>("aserai"), EffectIncrementType.AddFactor);
			this._lion.Initialize(new TextObject("{=D0SX1cFQ}Lion", null), new TextObject("{=EjjAmdXp}Crew battle morale {EFFECT_AMOUNT}", null), 10f, MBObjectManager.Instance.GetObject<CultureObject>("vlandia"), EffectIncrementType.Add);
			this._dragon.Initialize(new TextObject("{=GkvX7z6Y}Dragon", null), new TextObject("{=4Ok7GnHs}Boarded enemy crew morale {EFFECT_AMOUNT}", null), -5f, MBObjectManager.Instance.GetObject<CultureObject>("nord"), EffectIncrementType.Add);
			this._wingsOfVictory.Initialize(new TextObject("{=ci0npfYB}Wings Of Victory", null), new TextObject("{=mQuaMNVb}Party battle experience {EFFECT_AMOUNT}%", null), 0.15f, MBObjectManager.Instance.GetObject<CultureObject>("empire"), EffectIncrementType.AddFactor);
			this._ram.Initialize(new TextObject("{=shipFigureheadRam}Ram", null), new TextObject("{=eJ4MC1KO}Ramming ship and morale damage {EFFECT_AMOUNT}%.", null), 0.2f, MBObjectManager.Instance.GetObject<CultureObject>("empire"), EffectIncrementType.AddFactor);
			this._seaSerpent.Initialize(new TextObject("{=fsb5EEbg}Sea Serpent", null), new TextObject("{=OraB7RjB}Fire damage resistance {EFFECT_AMOUNT}%", null), 0.4f, MBObjectManager.Instance.GetObject<CultureObject>("nord"), EffectIncrementType.AddFactor);
			this._viper.Initialize(new TextObject("{=LTOaBiw3}Viper", null), new TextObject("{=NxIUg152}Ballista reload speed {EFFECT_AMOUNT}", null), 0.25f, MBObjectManager.Instance.GetObject<CultureObject>("aserai"), EffectIncrementType.AddFactor);
			this._saberToothTiger.Initialize(new TextObject("{=113F2KC5}Saber Tooth Tiger", null), new TextObject("{=qWDM0Oa1}Archer armor penetration {EFFECT_AMOUNT}", null), 0.1f, MBObjectManager.Instance.GetObject<CultureObject>("vlandia"), EffectIncrementType.AddFactor);
			this._siren.Initialize(new TextObject("{=wrwdRGkW}Siren", null), new TextObject("{=iBPMtWzZ}Boarded enemy crew melee damage {EFFECT_AMOUNT}%", null), -0.1f, MBObjectManager.Instance.GetObject<CultureObject>("sturgia"), EffectIncrementType.AddFactor);
			this._horse.Initialize(new TextObject("{=LwfILaRH}Horse", null), new TextObject("{=sMCpa5Sk}Ship travel speed {EFFECT_AMOUNT}%", null), 0.1f, MBObjectManager.Instance.GetObject<CultureObject>("khuzait"), EffectIncrementType.AddFactor);
			this._turtle.Initialize(new TextObject("{=Ni8CSaxD}Turtle", null), new TextObject("{=bAWHXCsb}Crew shield hitpoints {EFFECT_AMOUNT}%", null), 0.4f, MBObjectManager.Instance.GetObject<CultureObject>("sturgia"), EffectIncrementType.AddFactor);
			this._boar.Initialize(new TextObject("{=0OrIliBh}Boar", null), new TextObject("{=FPZ9QOGl}Crew armor {EFFECT_AMOUNT}%", null), 0.1f, MBObjectManager.Instance.GetObject<CultureObject>("battania"), EffectIncrementType.AddFactor);
			this._oxen.Initialize(new TextObject("{=mGy1EcUd}Oxen", null), new TextObject("{=D2ZA2XT6}Crew hitpoints {EFFECT_AMOUNT}", null), 10f, MBObjectManager.Instance.GetObject<CultureObject>("battania"), EffectIncrementType.Add);
			this._swan.Initialize(new TextObject("{=ZSA1mySL}Swan", null), new TextObject("{=JJTWn3zs}Sail force {EFFECT_AMOUNT}%", null), 0.1f, MBObjectManager.Instance.GetObject<CultureObject>("empire"), EffectIncrementType.AddFactor);
			this._deer.Initialize(new TextObject("{=XbNVQdZN}Deer", null), new TextObject("{=foC3qNav}Oar force {EFFECT_AMOUNT}%", null), 0.15f, MBObjectManager.Instance.GetObject<CultureObject>("sturgia"), EffectIncrementType.AddFactor);
			this._raven.Initialize(new TextObject("{=NVKwvl1G}Raven", null), new TextObject("{=QsR8WTpA}Crew throwing weapon damage {EFFECT_AMOUNT}%", null), 0.1f, MBObjectManager.Instance.GetObject<CultureObject>("nord"), EffectIncrementType.AddFactor);
		}

		// Token: 0x04000A0E RID: 2574
		private Figurehead _hawk;

		// Token: 0x04000A0F RID: 2575
		private Figurehead _lion;

		// Token: 0x04000A10 RID: 2576
		private Figurehead _dragon;

		// Token: 0x04000A11 RID: 2577
		private Figurehead _wingsOfVictory;

		// Token: 0x04000A12 RID: 2578
		private Figurehead _ram;

		// Token: 0x04000A13 RID: 2579
		private Figurehead _seaSerpent;

		// Token: 0x04000A14 RID: 2580
		private Figurehead _viper;

		// Token: 0x04000A15 RID: 2581
		private Figurehead _saberToothTiger;

		// Token: 0x04000A16 RID: 2582
		private Figurehead _siren;

		// Token: 0x04000A17 RID: 2583
		private Figurehead _horse;

		// Token: 0x04000A18 RID: 2584
		private Figurehead _turtle;

		// Token: 0x04000A19 RID: 2585
		private Figurehead _boar;

		// Token: 0x04000A1A RID: 2586
		private Figurehead _oxen;

		// Token: 0x04000A1B RID: 2587
		private Figurehead _swan;

		// Token: 0x04000A1C RID: 2588
		private Figurehead _deer;

		// Token: 0x04000A1D RID: 2589
		private Figurehead _raven;
	}
}
