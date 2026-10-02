using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200024F RID: 591
	public interface IFaceGeneratorHandler
	{
		// Token: 0x06002211 RID: 8721
		void ChangeToBodyCamera();

		// Token: 0x06002212 RID: 8722
		void ChangeToEyeCamera();

		// Token: 0x06002213 RID: 8723
		void ChangeToNoseCamera();

		// Token: 0x06002214 RID: 8724
		void ChangeToMouthCamera();

		// Token: 0x06002215 RID: 8725
		void ChangeToFaceCamera();

		// Token: 0x06002216 RID: 8726
		void ChangeToHairCamera();

		// Token: 0x06002217 RID: 8727
		void RefreshCharacterEntity();

		// Token: 0x06002218 RID: 8728
		void MakeVoice();

		// Token: 0x06002219 RID: 8729
		void MakeVoiceDelayed();

		// Token: 0x0600221A RID: 8730
		void SetFacialAnimation(string faceAnimation, bool loop);

		// Token: 0x0600221B RID: 8731
		void Done();

		// Token: 0x0600221C RID: 8732
		void Cancel();

		// Token: 0x0600221D RID: 8733
		void UndressCharacterEntity();

		// Token: 0x0600221E RID: 8734
		void DressCharacterEntity();

		// Token: 0x0600221F RID: 8735
		void DefaultFace();
	}
}
