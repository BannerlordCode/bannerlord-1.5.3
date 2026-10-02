using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001F0 RID: 496
	public class BodyGenerator
	{
		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06001CF4 RID: 7412 RVA: 0x00062990 File Offset: 0x00060B90
		// (set) Token: 0x06001CF5 RID: 7413 RVA: 0x00062998 File Offset: 0x00060B98
		public BasicCharacterObject Character { get; private set; }

		// Token: 0x06001CF6 RID: 7414 RVA: 0x000629A4 File Offset: 0x00060BA4
		public BodyGenerator(BasicCharacterObject troop)
		{
			this.Character = troop;
			MBDebug.Print("FaceGen set character> character face key: " + troop.GetBodyProperties(troop.Equipment, -1), 0, Debug.DebugColor.White, 17592186044416UL);
			this.Race = this.Character.Race;
			this.IsFemale = this.Character.IsFemale;
		}

		// Token: 0x06001CF7 RID: 7415 RVA: 0x00062A10 File Offset: 0x00060C10
		public FaceGenerationParams InitBodyGenerator(bool isDressed)
		{
			this.CurrentBodyProperties = this.Character.GetBodyProperties(this.Character.Equipment, -1);
			FaceGenerationParams faceGenerationParams = FaceGenerationParams.Create();
			faceGenerationParams.CurrentRace = this.Character.Race;
			faceGenerationParams.CurrentGender = (this.Character.IsFemale ? 1 : 0);
			faceGenerationParams.CurrentAge = this.Character.Age;
			MBBodyProperties.GetParamsFromKey(ref faceGenerationParams, this.CurrentBodyProperties, isDressed && this.Character.Equipment.EarsAreHidden, isDressed && this.Character.Equipment.MouthIsHidden);
			faceGenerationParams.SetRaceGenderAndAdjustParams(faceGenerationParams.CurrentRace, faceGenerationParams.CurrentGender, (int)faceGenerationParams.CurrentAge);
			return faceGenerationParams;
		}

		// Token: 0x06001CF8 RID: 7416 RVA: 0x00062AD0 File Offset: 0x00060CD0
		public void RefreshFace(FaceGenerationParams faceGenerationParams, bool hasEquipment)
		{
			MBBodyProperties.ProduceNumericKeyWithParams(faceGenerationParams, hasEquipment && this.Character.Equipment.EarsAreHidden, hasEquipment && this.Character.Equipment.MouthIsHidden, ref this.CurrentBodyProperties);
			this.Race = faceGenerationParams.CurrentRace;
			this.IsFemale = faceGenerationParams.CurrentGender == 1;
		}

		// Token: 0x06001CF9 RID: 7417 RVA: 0x00062B30 File Offset: 0x00060D30
		public void SaveCurrentCharacter()
		{
			this.Character.UpdatePlayerCharacterBodyProperties(this.CurrentBodyProperties, this.Race, this.IsFemale);
		}

		// Token: 0x040009D8 RID: 2520
		public const string FaceGenTeethAnimationName = "facegen_teeth";

		// Token: 0x040009D9 RID: 2521
		public BodyProperties CurrentBodyProperties;

		// Token: 0x040009DA RID: 2522
		public BodyProperties BodyPropertiesMin;

		// Token: 0x040009DB RID: 2523
		public BodyProperties BodyPropertiesMax;

		// Token: 0x040009DC RID: 2524
		public int Race;

		// Token: 0x040009DD RID: 2525
		public bool IsFemale;
	}
}
