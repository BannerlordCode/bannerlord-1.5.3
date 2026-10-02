using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000395 RID: 917
	public static class SkinVoiceManager
	{
		// Token: 0x060034D5 RID: 13525 RVA: 0x000DA6DD File Offset: 0x000D88DD
		public static int GetVoiceDefinitionCountWithMonsterSoundAndCollisionInfoClassName(string className)
		{
			return MBAPI.IMBVoiceManager.GetVoiceDefinitionCountWithMonsterSoundAndCollisionInfoClassName(className);
		}

		// Token: 0x060034D6 RID: 13526 RVA: 0x000DA6EA File Offset: 0x000D88EA
		public static void GetVoiceDefinitionListWithMonsterSoundAndCollisionInfoClassName(string className, int[] definitionIndices)
		{
			MBAPI.IMBVoiceManager.GetVoiceDefinitionListWithMonsterSoundAndCollisionInfoClassName(className, definitionIndices);
		}

		// Token: 0x0200066B RID: 1643
		public enum CombatVoiceNetworkPredictionType
		{
			// Token: 0x04002237 RID: 8759
			Prediction,
			// Token: 0x04002238 RID: 8760
			OwnerPrediction,
			// Token: 0x04002239 RID: 8761
			NoPrediction
		}

		// Token: 0x0200066C RID: 1644
		public struct SkinVoiceType
		{
			// Token: 0x17000AF0 RID: 2800
			// (get) Token: 0x06004156 RID: 16726 RVA: 0x000FD045 File Offset: 0x000FB245
			// (set) Token: 0x06004157 RID: 16727 RVA: 0x000FD04D File Offset: 0x000FB24D
			public string TypeID { get; private set; }

			// Token: 0x17000AF1 RID: 2801
			// (get) Token: 0x06004158 RID: 16728 RVA: 0x000FD056 File Offset: 0x000FB256
			// (set) Token: 0x06004159 RID: 16729 RVA: 0x000FD05E File Offset: 0x000FB25E
			public int Index { get; private set; }

			// Token: 0x0600415A RID: 16730 RVA: 0x000FD067 File Offset: 0x000FB267
			public SkinVoiceType(string typeID)
			{
				this.TypeID = typeID;
				this.Index = MBAPI.IMBVoiceManager.GetVoiceTypeIndex(typeID);
			}

			// Token: 0x0600415B RID: 16731 RVA: 0x000FD081 File Offset: 0x000FB281
			public TextObject GetName()
			{
				return GameTexts.FindText("str_taunt_name", this.TypeID);
			}
		}

		// Token: 0x0200066D RID: 1645
		public static class VoiceType
		{
			// Token: 0x0400223C RID: 8764
			public static readonly SkinVoiceManager.SkinVoiceType Grunt = new SkinVoiceManager.SkinVoiceType("Grunt");

			// Token: 0x0400223D RID: 8765
			public static readonly SkinVoiceManager.SkinVoiceType Jump = new SkinVoiceManager.SkinVoiceType("Jump");

			// Token: 0x0400223E RID: 8766
			public static readonly SkinVoiceManager.SkinVoiceType Yell = new SkinVoiceManager.SkinVoiceType("Yell");

			// Token: 0x0400223F RID: 8767
			public static readonly SkinVoiceManager.SkinVoiceType Pain = new SkinVoiceManager.SkinVoiceType("Pain");

			// Token: 0x04002240 RID: 8768
			public static readonly SkinVoiceManager.SkinVoiceType Death = new SkinVoiceManager.SkinVoiceType("Death");

			// Token: 0x04002241 RID: 8769
			public static readonly SkinVoiceManager.SkinVoiceType Stun = new SkinVoiceManager.SkinVoiceType("Stun");

			// Token: 0x04002242 RID: 8770
			public static readonly SkinVoiceManager.SkinVoiceType Fear = new SkinVoiceManager.SkinVoiceType("Fear");

			// Token: 0x04002243 RID: 8771
			public static readonly SkinVoiceManager.SkinVoiceType Climb = new SkinVoiceManager.SkinVoiceType("Climb");

			// Token: 0x04002244 RID: 8772
			public static readonly SkinVoiceManager.SkinVoiceType Focus = new SkinVoiceManager.SkinVoiceType("Focus");

			// Token: 0x04002245 RID: 8773
			public static readonly SkinVoiceManager.SkinVoiceType Debacle = new SkinVoiceManager.SkinVoiceType("Debacle");

			// Token: 0x04002246 RID: 8774
			public static readonly SkinVoiceManager.SkinVoiceType Victory = new SkinVoiceManager.SkinVoiceType("Victory");

			// Token: 0x04002247 RID: 8775
			public static readonly SkinVoiceManager.SkinVoiceType HorseStop = new SkinVoiceManager.SkinVoiceType("HorseStop");

			// Token: 0x04002248 RID: 8776
			public static readonly SkinVoiceManager.SkinVoiceType HorseRally = new SkinVoiceManager.SkinVoiceType("HorseRally");

			// Token: 0x04002249 RID: 8777
			public static readonly SkinVoiceManager.SkinVoiceType Drown = new SkinVoiceManager.SkinVoiceType("Drown");

			// Token: 0x0400224A RID: 8778
			public static readonly SkinVoiceManager.SkinVoiceType Infantry = new SkinVoiceManager.SkinVoiceType("Infantry");

			// Token: 0x0400224B RID: 8779
			public static readonly SkinVoiceManager.SkinVoiceType Cavalry = new SkinVoiceManager.SkinVoiceType("Cavalry");

			// Token: 0x0400224C RID: 8780
			public static readonly SkinVoiceManager.SkinVoiceType Archers = new SkinVoiceManager.SkinVoiceType("Archers");

			// Token: 0x0400224D RID: 8781
			public static readonly SkinVoiceManager.SkinVoiceType HorseArchers = new SkinVoiceManager.SkinVoiceType("HorseArchers");

			// Token: 0x0400224E RID: 8782
			public static readonly SkinVoiceManager.SkinVoiceType Everyone = new SkinVoiceManager.SkinVoiceType("Everyone");

			// Token: 0x0400224F RID: 8783
			public static readonly SkinVoiceManager.SkinVoiceType MixedFormation = new SkinVoiceManager.SkinVoiceType("Mixed");

			// Token: 0x04002250 RID: 8784
			public static readonly SkinVoiceManager.SkinVoiceType Move = new SkinVoiceManager.SkinVoiceType("Move");

			// Token: 0x04002251 RID: 8785
			public static readonly SkinVoiceManager.SkinVoiceType Follow = new SkinVoiceManager.SkinVoiceType("Follow");

			// Token: 0x04002252 RID: 8786
			public static readonly SkinVoiceManager.SkinVoiceType Charge = new SkinVoiceManager.SkinVoiceType("Charge");

			// Token: 0x04002253 RID: 8787
			public static readonly SkinVoiceManager.SkinVoiceType Advance = new SkinVoiceManager.SkinVoiceType("Advance");

			// Token: 0x04002254 RID: 8788
			public static readonly SkinVoiceManager.SkinVoiceType FallBack = new SkinVoiceManager.SkinVoiceType("FallBack");

			// Token: 0x04002255 RID: 8789
			public static readonly SkinVoiceManager.SkinVoiceType Stop = new SkinVoiceManager.SkinVoiceType("Stop");

			// Token: 0x04002256 RID: 8790
			public static readonly SkinVoiceManager.SkinVoiceType Retreat = new SkinVoiceManager.SkinVoiceType("Retreat");

			// Token: 0x04002257 RID: 8791
			public static readonly SkinVoiceManager.SkinVoiceType Mount = new SkinVoiceManager.SkinVoiceType("Mount");

			// Token: 0x04002258 RID: 8792
			public static readonly SkinVoiceManager.SkinVoiceType Dismount = new SkinVoiceManager.SkinVoiceType("Dismount");

			// Token: 0x04002259 RID: 8793
			public static readonly SkinVoiceManager.SkinVoiceType FireAtWill = new SkinVoiceManager.SkinVoiceType("FireAtWill");

			// Token: 0x0400225A RID: 8794
			public static readonly SkinVoiceManager.SkinVoiceType HoldFire = new SkinVoiceManager.SkinVoiceType("HoldFire");

			// Token: 0x0400225B RID: 8795
			public static readonly SkinVoiceManager.SkinVoiceType PickSpears = new SkinVoiceManager.SkinVoiceType("PickSpears");

			// Token: 0x0400225C RID: 8796
			public static readonly SkinVoiceManager.SkinVoiceType PickDefault = new SkinVoiceManager.SkinVoiceType("PickDefault");

			// Token: 0x0400225D RID: 8797
			public static readonly SkinVoiceManager.SkinVoiceType FaceEnemy = new SkinVoiceManager.SkinVoiceType("FaceEnemy");

			// Token: 0x0400225E RID: 8798
			public static readonly SkinVoiceManager.SkinVoiceType FaceDirection = new SkinVoiceManager.SkinVoiceType("FaceDirection");

			// Token: 0x0400225F RID: 8799
			public static readonly SkinVoiceManager.SkinVoiceType UseSiegeWeapon = new SkinVoiceManager.SkinVoiceType("UseSiegeWeapon");

			// Token: 0x04002260 RID: 8800
			public static readonly SkinVoiceManager.SkinVoiceType UseLadders = new SkinVoiceManager.SkinVoiceType("UseLadders");

			// Token: 0x04002261 RID: 8801
			public static readonly SkinVoiceManager.SkinVoiceType AttackGate = new SkinVoiceManager.SkinVoiceType("AttackGate");

			// Token: 0x04002262 RID: 8802
			public static readonly SkinVoiceManager.SkinVoiceType CommandDelegate = new SkinVoiceManager.SkinVoiceType("CommandDelegate");

			// Token: 0x04002263 RID: 8803
			public static readonly SkinVoiceManager.SkinVoiceType CommandUndelegate = new SkinVoiceManager.SkinVoiceType("CommandUndelegate");

			// Token: 0x04002264 RID: 8804
			public static readonly SkinVoiceManager.SkinVoiceType BoardAtWill = new SkinVoiceManager.SkinVoiceType("BoardAtWill");

			// Token: 0x04002265 RID: 8805
			public static readonly SkinVoiceManager.SkinVoiceType AvoidBoarding = new SkinVoiceManager.SkinVoiceType("AvoidBoarding");

			// Token: 0x04002266 RID: 8806
			public static readonly SkinVoiceManager.SkinVoiceType FormLine = new SkinVoiceManager.SkinVoiceType("FormLine");

			// Token: 0x04002267 RID: 8807
			public static readonly SkinVoiceManager.SkinVoiceType FormShieldWall = new SkinVoiceManager.SkinVoiceType("FormShieldWall");

			// Token: 0x04002268 RID: 8808
			public static readonly SkinVoiceManager.SkinVoiceType FormLoose = new SkinVoiceManager.SkinVoiceType("FormLoose");

			// Token: 0x04002269 RID: 8809
			public static readonly SkinVoiceManager.SkinVoiceType FormCircle = new SkinVoiceManager.SkinVoiceType("FormCircle");

			// Token: 0x0400226A RID: 8810
			public static readonly SkinVoiceManager.SkinVoiceType FormSquare = new SkinVoiceManager.SkinVoiceType("FormSquare");

			// Token: 0x0400226B RID: 8811
			public static readonly SkinVoiceManager.SkinVoiceType FormSkein = new SkinVoiceManager.SkinVoiceType("FormSkein");

			// Token: 0x0400226C RID: 8812
			public static readonly SkinVoiceManager.SkinVoiceType FormColumn = new SkinVoiceManager.SkinVoiceType("FormColumn");

			// Token: 0x0400226D RID: 8813
			public static readonly SkinVoiceManager.SkinVoiceType FormScatter = new SkinVoiceManager.SkinVoiceType("FormScatter");

			// Token: 0x0400226E RID: 8814
			public static readonly SkinVoiceManager.SkinVoiceType[] MpBarks = new SkinVoiceManager.SkinVoiceType[]
			{
				new SkinVoiceManager.SkinVoiceType("MpDefend"),
				new SkinVoiceManager.SkinVoiceType("MpAttack"),
				new SkinVoiceManager.SkinVoiceType("MpHelp"),
				new SkinVoiceManager.SkinVoiceType("MpSpot"),
				new SkinVoiceManager.SkinVoiceType("MpThanks"),
				new SkinVoiceManager.SkinVoiceType("MpSorry"),
				new SkinVoiceManager.SkinVoiceType("MpAffirmative"),
				new SkinVoiceManager.SkinVoiceType("MpNegative"),
				new SkinVoiceManager.SkinVoiceType("MpRegroup")
			};

			// Token: 0x0400226F RID: 8815
			public static readonly SkinVoiceManager.SkinVoiceType MpDefend = SkinVoiceManager.VoiceType.MpBarks[0];

			// Token: 0x04002270 RID: 8816
			public static readonly SkinVoiceManager.SkinVoiceType MpAttack = SkinVoiceManager.VoiceType.MpBarks[1];

			// Token: 0x04002271 RID: 8817
			public static readonly SkinVoiceManager.SkinVoiceType MpHelp = SkinVoiceManager.VoiceType.MpBarks[2];

			// Token: 0x04002272 RID: 8818
			public static readonly SkinVoiceManager.SkinVoiceType MpSpot = SkinVoiceManager.VoiceType.MpBarks[3];

			// Token: 0x04002273 RID: 8819
			public static readonly SkinVoiceManager.SkinVoiceType MpThanks = SkinVoiceManager.VoiceType.MpBarks[4];

			// Token: 0x04002274 RID: 8820
			public static readonly SkinVoiceManager.SkinVoiceType MpSorry = SkinVoiceManager.VoiceType.MpBarks[5];

			// Token: 0x04002275 RID: 8821
			public static readonly SkinVoiceManager.SkinVoiceType MpAffirmative = SkinVoiceManager.VoiceType.MpBarks[6];

			// Token: 0x04002276 RID: 8822
			public static readonly SkinVoiceManager.SkinVoiceType MpNegative = SkinVoiceManager.VoiceType.MpBarks[7];

			// Token: 0x04002277 RID: 8823
			public static readonly SkinVoiceManager.SkinVoiceType MpRegroup = SkinVoiceManager.VoiceType.MpBarks[8];

			// Token: 0x04002278 RID: 8824
			public static readonly SkinVoiceManager.SkinVoiceType Idle = new SkinVoiceManager.SkinVoiceType("Idle");

			// Token: 0x04002279 RID: 8825
			public static readonly SkinVoiceManager.SkinVoiceType Neigh = new SkinVoiceManager.SkinVoiceType("Neigh");

			// Token: 0x0400227A RID: 8826
			public static readonly SkinVoiceManager.SkinVoiceType Collide = new SkinVoiceManager.SkinVoiceType("Collide");
		}
	}
}
