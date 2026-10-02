using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200036B RID: 875
	public class SoundPlayer : ScriptComponentBehavior
	{
		// Token: 0x06003257 RID: 12887 RVA: 0x000CE444 File Offset: 0x000CC644
		private void ValidateSoundEvent()
		{
			if ((this.SoundEvent == null || !this.SoundEvent.IsValid) && this.SoundName.Length > 0)
			{
				if (this.SoundCode == -1)
				{
					this.SoundCode = SoundManager.GetEventGlobalIndex(this.SoundName);
				}
				this.SoundEvent = SoundEvent.CreateEvent(this.SoundCode, base.GameEntity.Scene);
			}
		}

		// Token: 0x06003258 RID: 12888 RVA: 0x000CE4AD File Offset: 0x000CC6AD
		public void UpdatePlaying()
		{
			this.Playing = this.SoundEvent != null && this.SoundEvent.IsValid && this.SoundEvent.IsPlaying();
		}

		// Token: 0x06003259 RID: 12889 RVA: 0x000CE4D8 File Offset: 0x000CC6D8
		public void PlaySound()
		{
			if (this.Playing)
			{
				return;
			}
			if (this.SoundEvent != null && this.SoundEvent.IsValid)
			{
				this.SoundEvent.SetPosition(base.GameEntity.GlobalPosition);
				this.SoundEvent.Play();
				this.Playing = true;
			}
		}

		// Token: 0x0600325A RID: 12890 RVA: 0x000CE52F File Offset: 0x000CC72F
		public void ResumeSound()
		{
			if (this.Playing)
			{
				return;
			}
			if (this.SoundEvent != null && this.SoundEvent.IsValid && this.SoundEvent.IsPaused())
			{
				this.SoundEvent.Resume();
				this.Playing = true;
			}
		}

		// Token: 0x0600325B RID: 12891 RVA: 0x000CE56E File Offset: 0x000CC76E
		public void PauseSound()
		{
			if (!this.Playing)
			{
				return;
			}
			if (this.SoundEvent != null && this.SoundEvent.IsValid)
			{
				this.SoundEvent.Pause();
				this.Playing = false;
			}
		}

		// Token: 0x0600325C RID: 12892 RVA: 0x000CE5A0 File Offset: 0x000CC7A0
		public void StopSound()
		{
			if (!this.Playing)
			{
				return;
			}
			if (this.SoundEvent != null && this.SoundEvent.IsValid)
			{
				this.SoundEvent.Stop();
				this.Playing = false;
			}
		}

		// Token: 0x0600325D RID: 12893 RVA: 0x000CE5D2 File Offset: 0x000CC7D2
		protected internal override void OnInit()
		{
			base.OnInit();
			MBDebug.Print("SoundPlayer : OnInit called.", 0, Debug.DebugColor.Yellow, 17592186044416UL);
			this.ValidateSoundEvent();
			if (this.AutoStart)
			{
				this.PlaySound();
			}
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x0600325E RID: 12894 RVA: 0x000CE610 File Offset: 0x000CC810
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
		}

		// Token: 0x0600325F RID: 12895 RVA: 0x000CE61A File Offset: 0x000CC81A
		protected internal override void OnTick(float dt)
		{
			this.UpdatePlaying();
			if (!this.Playing && this.AutoLoop)
			{
				this.ValidateSoundEvent();
				this.PlaySound();
			}
		}

		// Token: 0x06003260 RID: 12896 RVA: 0x000CE63E File Offset: 0x000CC83E
		protected internal override bool MovesEntity()
		{
			return false;
		}

		// Token: 0x04001563 RID: 5475
		private bool Playing;

		// Token: 0x04001564 RID: 5476
		private int SoundCode = -1;

		// Token: 0x04001565 RID: 5477
		private SoundEvent SoundEvent;

		// Token: 0x04001566 RID: 5478
		public bool AutoLoop;

		// Token: 0x04001567 RID: 5479
		public bool AutoStart;

		// Token: 0x04001568 RID: 5480
		public string SoundName;
	}
}
