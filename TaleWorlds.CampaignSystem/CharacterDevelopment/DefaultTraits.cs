using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CharacterDevelopment
{
	// Token: 0x020003C4 RID: 964
	public class DefaultTraits
	{
		// Token: 0x17000D34 RID: 3380
		// (get) Token: 0x0600385C RID: 14428 RVA: 0x000EA0B1 File Offset: 0x000E82B1
		private static DefaultTraits Instance
		{
			get
			{
				return Campaign.Current.DefaultTraits;
			}
		}

		// Token: 0x17000D35 RID: 3381
		// (get) Token: 0x0600385D RID: 14429 RVA: 0x000EA0BD File Offset: 0x000E82BD
		public static TraitObject Frequency
		{
			get
			{
				return DefaultTraits.Instance._traitFrequency;
			}
		}

		// Token: 0x17000D36 RID: 3382
		// (get) Token: 0x0600385E RID: 14430 RVA: 0x000EA0C9 File Offset: 0x000E82C9
		public static TraitObject Mercy
		{
			get
			{
				return DefaultTraits.Instance._traitMercy;
			}
		}

		// Token: 0x17000D37 RID: 3383
		// (get) Token: 0x0600385F RID: 14431 RVA: 0x000EA0D5 File Offset: 0x000E82D5
		public static TraitObject Valor
		{
			get
			{
				return DefaultTraits.Instance._traitValor;
			}
		}

		// Token: 0x17000D38 RID: 3384
		// (get) Token: 0x06003860 RID: 14432 RVA: 0x000EA0E1 File Offset: 0x000E82E1
		public static TraitObject Honor
		{
			get
			{
				return DefaultTraits.Instance._traitHonor;
			}
		}

		// Token: 0x17000D39 RID: 3385
		// (get) Token: 0x06003861 RID: 14433 RVA: 0x000EA0ED File Offset: 0x000E82ED
		public static TraitObject Generosity
		{
			get
			{
				return DefaultTraits.Instance._traitGenerosity;
			}
		}

		// Token: 0x17000D3A RID: 3386
		// (get) Token: 0x06003862 RID: 14434 RVA: 0x000EA0F9 File Offset: 0x000E82F9
		public static TraitObject Calculating
		{
			get
			{
				return DefaultTraits.Instance._traitCalculating;
			}
		}

		// Token: 0x17000D3B RID: 3387
		// (get) Token: 0x06003863 RID: 14435 RVA: 0x000EA105 File Offset: 0x000E8305
		public static TraitObject PersonaCurt
		{
			get
			{
				return DefaultTraits.Instance._traitPersonaCurt;
			}
		}

		// Token: 0x17000D3C RID: 3388
		// (get) Token: 0x06003864 RID: 14436 RVA: 0x000EA111 File Offset: 0x000E8311
		public static TraitObject PersonaEarnest
		{
			get
			{
				return DefaultTraits.Instance._traitPersonaEarnest;
			}
		}

		// Token: 0x17000D3D RID: 3389
		// (get) Token: 0x06003865 RID: 14437 RVA: 0x000EA11D File Offset: 0x000E831D
		public static TraitObject PersonaIronic
		{
			get
			{
				return DefaultTraits.Instance._traitPersonaIronic;
			}
		}

		// Token: 0x17000D3E RID: 3390
		// (get) Token: 0x06003866 RID: 14438 RVA: 0x000EA129 File Offset: 0x000E8329
		public static TraitObject PersonaSoftspoken
		{
			get
			{
				return DefaultTraits.Instance._traitPersonaSoftspoken;
			}
		}

		// Token: 0x17000D3F RID: 3391
		// (get) Token: 0x06003867 RID: 14439 RVA: 0x000EA135 File Offset: 0x000E8335
		public static TraitObject Surgery
		{
			get
			{
				return DefaultTraits.Instance._traitSurgery;
			}
		}

		// Token: 0x17000D40 RID: 3392
		// (get) Token: 0x06003868 RID: 14440 RVA: 0x000EA141 File Offset: 0x000E8341
		public static TraitObject SergeantCommandSkills
		{
			get
			{
				return DefaultTraits.Instance._traitSergeantCommandSkills;
			}
		}

		// Token: 0x17000D41 RID: 3393
		// (get) Token: 0x06003869 RID: 14441 RVA: 0x000EA14D File Offset: 0x000E834D
		public static TraitObject RogueSkills
		{
			get
			{
				return DefaultTraits.Instance._traitRogueSkills;
			}
		}

		// Token: 0x17000D42 RID: 3394
		// (get) Token: 0x0600386A RID: 14442 RVA: 0x000EA159 File Offset: 0x000E8359
		public static TraitObject Siegecraft
		{
			get
			{
				return DefaultTraits.Instance._traitEngineerSkills;
			}
		}

		// Token: 0x17000D43 RID: 3395
		// (get) Token: 0x0600386B RID: 14443 RVA: 0x000EA165 File Offset: 0x000E8365
		public static TraitObject ScoutSkills
		{
			get
			{
				return DefaultTraits.Instance._traitScoutSkills;
			}
		}

		// Token: 0x17000D44 RID: 3396
		// (get) Token: 0x0600386C RID: 14444 RVA: 0x000EA171 File Offset: 0x000E8371
		public static TraitObject Blacksmith
		{
			get
			{
				return DefaultTraits.Instance._traitBlacksmith;
			}
		}

		// Token: 0x17000D45 RID: 3397
		// (get) Token: 0x0600386D RID: 14445 RVA: 0x000EA17D File Offset: 0x000E837D
		public static TraitObject Commander
		{
			get
			{
				return DefaultTraits.Instance._traitCommander;
			}
		}

		// Token: 0x17000D46 RID: 3398
		// (get) Token: 0x0600386E RID: 14446 RVA: 0x000EA189 File Offset: 0x000E8389
		public static TraitObject Trader
		{
			get
			{
				return DefaultTraits.Instance._traitTraderSkills;
			}
		}

		// Token: 0x17000D47 RID: 3399
		// (get) Token: 0x0600386F RID: 14447 RVA: 0x000EA195 File Offset: 0x000E8395
		public static TraitObject Thug
		{
			get
			{
				return DefaultTraits.Instance._traitThug;
			}
		}

		// Token: 0x17000D48 RID: 3400
		// (get) Token: 0x06003870 RID: 14448 RVA: 0x000EA1A1 File Offset: 0x000E83A1
		public static TraitObject Smuggler
		{
			get
			{
				return DefaultTraits.Instance._traitSmuggler;
			}
		}

		// Token: 0x17000D49 RID: 3401
		// (get) Token: 0x06003871 RID: 14449 RVA: 0x000EA1AD File Offset: 0x000E83AD
		public static TraitObject Egalitarian
		{
			get
			{
				return DefaultTraits.Instance._traitEgalitarian;
			}
		}

		// Token: 0x17000D4A RID: 3402
		// (get) Token: 0x06003872 RID: 14450 RVA: 0x000EA1B9 File Offset: 0x000E83B9
		public static TraitObject Oligarchic
		{
			get
			{
				return DefaultTraits.Instance._traitOligarchic;
			}
		}

		// Token: 0x17000D4B RID: 3403
		// (get) Token: 0x06003873 RID: 14451 RVA: 0x000EA1C5 File Offset: 0x000E83C5
		public static TraitObject Authoritarian
		{
			get
			{
				return DefaultTraits.Instance._traitAuthoritarian;
			}
		}

		// Token: 0x17000D4C RID: 3404
		// (get) Token: 0x06003874 RID: 14452 RVA: 0x000EA1D1 File Offset: 0x000E83D1
		public static TraitObject NavalSoldier
		{
			get
			{
				return DefaultTraits.Instance._traitNavalSoldier;
			}
		}

		// Token: 0x17000D4D RID: 3405
		// (get) Token: 0x06003875 RID: 14453 RVA: 0x000EA1DD File Offset: 0x000E83DD
		public static IEnumerable<TraitObject> Personality
		{
			get
			{
				return DefaultTraits.Instance._personality;
			}
		}

		// Token: 0x06003876 RID: 14454 RVA: 0x000EA1EC File Offset: 0x000E83EC
		public DefaultTraits()
		{
			this.RegisterAll();
			this._personality = new TraitObject[] { this._traitMercy, this._traitValor, this._traitHonor, this._traitGenerosity, this._traitCalculating };
		}

		// Token: 0x06003877 RID: 14455 RVA: 0x000EA240 File Offset: 0x000E8440
		public void RegisterAll()
		{
			this._traitFrequency = this.Create("Frequency");
			this._traitMercy = this.Create("Mercy");
			this._traitValor = this.Create("Valor");
			this._traitHonor = this.Create("Honor");
			this._traitGenerosity = this.Create("Generosity");
			this._traitCalculating = this.Create("Calculating");
			this._traitPersonaCurt = this.Create("curt");
			this._traitPersonaIronic = this.Create("ironic");
			this._traitPersonaEarnest = this.Create("earnest");
			this._traitPersonaSoftspoken = this.Create("softspoken");
			this._traitCommander = this.Create("Commander");
			this._traitTraderSkills = this.Create("Trader");
			this._traitSurgery = this.Create("Surgeon");
			this._traitTracking = this.Create("Tracking");
			this._traitBlacksmith = this.Create("Blacksmith");
			this._traitSergeantCommandSkills = this.Create("SergeantCommandSkills");
			this._traitEngineerSkills = this.Create("EngineerSkills");
			this._traitRogueSkills = this.Create("RogueSkills");
			this._traitScoutSkills = this.Create("ScoutSkills");
			this._traitThug = this.Create("Thug");
			this._traitSmuggler = this.Create("Smuggler");
			this._traitEgalitarian = this.Create("Egalitarian");
			this._traitOligarchic = this.Create("Oligarchic");
			this._traitAuthoritarian = this.Create("Authoritarian");
			this._traitNavalSoldier = this.Create("NavalSoldier");
			this.InitializeAll();
		}

		// Token: 0x06003878 RID: 14456 RVA: 0x000EA3FC File Offset: 0x000E85FC
		private TraitObject Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<TraitObject>(new TraitObject(stringId));
		}

		// Token: 0x06003879 RID: 14457 RVA: 0x000EA414 File Offset: 0x000E8614
		private void InitializeAll()
		{
			this._traitFrequency.Initialize(new TextObject("{=vsoyhPnl}Frequency", null), new TextObject("{=!}Frequency Description", null), true, 0, 20);
			this._traitMercy.Initialize(new TextObject("{=2I2uKJlw}Mercy", null), new TextObject("{=Au7VCWTa}Mercy represents your general aversion to suffering and your willingness to help strangers or even enemies.", null), false, -2, 2);
			this._traitValor.Initialize(new TextObject("{=toQLHG6x}Valor", null), new TextObject("{=Ugm9nO49}Valor represents your reputation for risking your life to win glory or wealth or advance your cause.", null), false, -2, 2);
			this._traitHonor.Initialize(new TextObject("{=0oGz5rVx}Honor", null), new TextObject("{=1vYgkaaK}Honor represents your reputation for respecting your formal commitments, like keeping your word and obeying the law.", null), false, -2, 2);
			this._traitGenerosity.Initialize(new TextObject("{=IuWu5Bu7}Generosity", null), new TextObject("{=IKzqzPDS}Generosity represents your loyalty to your kin and those who serve you, and your gratitude to those who have done you a favor.", null), false, -2, 2);
			this._traitCalculating.Initialize(new TextObject("{=5sMBbn7y}Calculating", null), new TextObject("{=QKjF5gTR}Calculating represents your ability to control your emotions for the sake of your long-term interests.", null), false, -2, 2);
			this._traitPersonaCurt.Initialize(new TextObject("{=!}PersonaCurt", null), new TextObject("{=!}PersonaCurt Description", null), false, -2, 2);
			this._traitPersonaIronic.Initialize(new TextObject("{=!}PersonaIronic", null), new TextObject("{=!}PersonaIronic Description", null), false, -2, 2);
			this._traitPersonaEarnest.Initialize(new TextObject("{=!}PersonaEarnest", null), new TextObject("{=!}PersonaEarnest Description", null), false, -2, 2);
			this._traitPersonaSoftspoken.Initialize(new TextObject("{=!}PersonaSoftspoken", null), new TextObject("{=!}PersonaSoftspoken Description", null), false, -2, 2);
			this._traitCommander.Initialize(new TextObject("{=RvKwdXWs}Commander", null), new TextObject("{=!}Commander Description", null), true, 0, 20);
			this._traitSurgery.Initialize(new TextObject("{=QBPrRdQJ}Surgeon", null), new TextObject("{=!}Surgeon Description", null), true, 0, 20);
			this._traitTracking.Initialize(new TextObject("{=dx0hmeH6}Tracking", null), new TextObject("{=!}Tracking Description", null), true, 0, 20);
			this._traitBlacksmith.Initialize(new TextObject("{=bNnQt4jN}Blacksmith", null), new TextObject("{=!}Blacksmith Description", null), true, 0, 20);
			this._traitSergeantCommandSkills.Initialize(new TextObject("{=!}SergeantCommandSkills", null), new TextObject("{=!}SergeantCommandSkills Description", null), true, 0, 20);
			this._traitEngineerSkills.Initialize(new TextObject("{=!}EngineerSkills", null), new TextObject("{=!}EngineerSkills Description", null), true, 0, 20);
			this._traitRogueSkills.Initialize(new TextObject("{=!}RogueSkills", null), new TextObject("{=!}RogueSkills Description", null), true, 0, 20);
			this._traitScoutSkills.Initialize(new TextObject("{=!}ScoutSkills", null), new TextObject("{=!}ScoutSkills Description", null), true, 0, 20);
			this._traitTraderSkills.Initialize(new TextObject("{=!}TraderSkills", null), new TextObject("{=!}Trader Description", null), true, 0, 20);
			this._traitThug.Initialize(new TextObject("{=thugtrait}Thug", null), new TextObject("{=Fjnw9ooa}Indicates a gang member specialized in extortion", null), true, 0, 20);
			this._traitSmuggler.Initialize(new TextObject("{=eeWx1yYd}Smuggler", null), new TextObject("{=87c7IhkZ}Indicates a gang member specialized in smuggling", null), true, 0, 20);
			this._traitEgalitarian.Initialize(new TextObject("{=HMFb1gaq}Egalitarian", null), new TextObject("{=!}Egalitarian Description", null), false, 0, 20);
			this._traitOligarchic.Initialize(new TextObject("{=hR6Zo6pD}Oligarchic", null), new TextObject("{=!}Oligarchic Description", null), false, 0, 20);
			this._traitAuthoritarian.Initialize(new TextObject("{=NaMPa4ML}Authoritarian", null), new TextObject("{=!}Authoritarian Description", null), false, 0, 20);
			this._traitNavalSoldier.Initialize(new TextObject("{=rGUOr2wg}Naval Soldier", null), new TextObject("{=!}Naval Soldier Description", null), true, 0, 20);
		}

		// Token: 0x04001154 RID: 4436
		private const int MaxPersonalityTraitValue = 2;

		// Token: 0x04001155 RID: 4437
		private const int MinPersonalityTraitValue = -2;

		// Token: 0x04001156 RID: 4438
		private const int MaxHiddenTraitValue = 20;

		// Token: 0x04001157 RID: 4439
		private const int MinHiddenTraitValue = 0;

		// Token: 0x04001158 RID: 4440
		private TraitObject _traitMercy;

		// Token: 0x04001159 RID: 4441
		private TraitObject _traitValor;

		// Token: 0x0400115A RID: 4442
		private TraitObject _traitHonor;

		// Token: 0x0400115B RID: 4443
		private TraitObject _traitGenerosity;

		// Token: 0x0400115C RID: 4444
		private TraitObject _traitCalculating;

		// Token: 0x0400115D RID: 4445
		private TraitObject _traitPersonaCurt;

		// Token: 0x0400115E RID: 4446
		private TraitObject _traitPersonaEarnest;

		// Token: 0x0400115F RID: 4447
		private TraitObject _traitPersonaIronic;

		// Token: 0x04001160 RID: 4448
		private TraitObject _traitPersonaSoftspoken;

		// Token: 0x04001161 RID: 4449
		private TraitObject _traitEgalitarian;

		// Token: 0x04001162 RID: 4450
		private TraitObject _traitOligarchic;

		// Token: 0x04001163 RID: 4451
		private TraitObject _traitAuthoritarian;

		// Token: 0x04001164 RID: 4452
		private TraitObject _traitSurgery;

		// Token: 0x04001165 RID: 4453
		private TraitObject _traitTracking;

		// Token: 0x04001166 RID: 4454
		private TraitObject _traitSergeantCommandSkills;

		// Token: 0x04001167 RID: 4455
		private TraitObject _traitRogueSkills;

		// Token: 0x04001168 RID: 4456
		private TraitObject _traitEngineerSkills;

		// Token: 0x04001169 RID: 4457
		private TraitObject _traitBlacksmith;

		// Token: 0x0400116A RID: 4458
		private TraitObject _traitScoutSkills;

		// Token: 0x0400116B RID: 4459
		private TraitObject _traitTraderSkills;

		// Token: 0x0400116C RID: 4460
		private TraitObject _traitFrequency;

		// Token: 0x0400116D RID: 4461
		private TraitObject _traitCommander;

		// Token: 0x0400116E RID: 4462
		private TraitObject _traitThug;

		// Token: 0x0400116F RID: 4463
		private TraitObject _traitSmuggler;

		// Token: 0x04001170 RID: 4464
		private TraitObject _traitNavalSoldier;

		// Token: 0x04001171 RID: 4465
		private readonly TraitObject[] _personality;
	}
}
