using System;
using System.Collections.Generic;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000339 RID: 825
	public class FakePointLightSpec : ScriptComponentBehavior
	{
		// Token: 0x06002EB4 RID: 11956 RVA: 0x000B4A20 File Offset: 0x000B2C20
		private void CacheManagedLights()
		{
			this._managedLights.Clear();
			this._descendantBuffer.Clear();
			base.GameEntity.GetChildrenRecursive(ref this._descendantBuffer);
			for (int i = 0; i < this._descendantBuffer.Count; i++)
			{
				WeakGameEntity weakGameEntity = this._descendantBuffer[i];
				if (weakGameEntity.HasTag("fake_spec"))
				{
					Light light = weakGameEntity.GetLight();
					if (!(light == null) && light.IsValid)
					{
						light.SetShadowType(Light.ShadowType.NoShadow);
						this._managedLights.Add(light);
					}
				}
			}
		}

		// Token: 0x06002EB5 RID: 11957 RVA: 0x000B4AB4 File Offset: 0x000B2CB4
		private float GetTimeOfDayMultiplier(float hour)
		{
			hour -= (float)MathF.Floor(hour / 24f) * 24f;
			float num = this.noonIntensityMultiplier;
			float num2 = this.dawnDuskIntensityMultiplier;
			float num3 = this.nightIntensityMultiplier;
			if (hour <= 6f)
			{
				return FakePointLightSpec.LerpBetweenPlateaus(hour, 0f, 6f, num3, num2);
			}
			if (hour <= 12f)
			{
				return FakePointLightSpec.LerpBetweenPlateaus(hour, 6f, 12f, num2, num);
			}
			if (hour <= 18f)
			{
				return FakePointLightSpec.LerpBetweenPlateaus(hour, 12f, 18f, num, num2);
			}
			return FakePointLightSpec.LerpBetweenPlateaus(hour, 18f, 24f, num2, num3);
		}

		// Token: 0x06002EB6 RID: 11958 RVA: 0x000B4B50 File Offset: 0x000B2D50
		private static float LerpBetweenPlateaus(float hour, float fromHour, float toHour, float fromValue, float toValue)
		{
			float num = fromHour + 2f;
			float num2 = toHour - 2f;
			if (hour <= num)
			{
				return fromValue;
			}
			if (hour >= num2)
			{
				return toValue;
			}
			float num3 = num2 - num;
			if (num3 <= 0f)
			{
				return (fromValue + toValue) * 0.5f;
			}
			float num4 = (hour - num) / num3;
			return fromValue + (toValue - fromValue) * num4;
		}

		// Token: 0x06002EB7 RID: 11959 RVA: 0x000B4BA0 File Offset: 0x000B2DA0
		private void ApplyIntensity(float intensity)
		{
			for (int i = 0; i < this._managedLights.Count; i++)
			{
				this._managedLights[i].Intensity = intensity;
			}
		}

		// Token: 0x06002EB8 RID: 11960 RVA: 0x000B4BD8 File Offset: 0x000B2DD8
		private void UpdateLights()
		{
			if (this._managedLights.Count == 0)
			{
				return;
			}
			float num = 12f;
			Scene scene = base.GameEntity.Scene;
			if (scene != null)
			{
				num = scene.TimeOfDay;
			}
			float num2 = this.baseIntensity * this.GetTimeOfDayMultiplier(num);
			this.ApplyIntensity(num2);
		}

		// Token: 0x06002EB9 RID: 11961 RVA: 0x000B4C2E File Offset: 0x000B2E2E
		protected internal override void OnInit()
		{
			base.OnInit();
			this.CacheManagedLights();
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06002EBA RID: 11962 RVA: 0x000B4C48 File Offset: 0x000B2E48
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
			this.CacheManagedLights();
		}

		// Token: 0x06002EBB RID: 11963 RVA: 0x000B4C56 File Offset: 0x000B2E56
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
		}

		// Token: 0x06002EBC RID: 11964 RVA: 0x000B4C60 File Offset: 0x000B2E60
		protected internal override void OnTick(float dt)
		{
			this.UpdateLights();
		}

		// Token: 0x06002EBD RID: 11965 RVA: 0x000B4C68 File Offset: 0x000B2E68
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			this.UpdateLights();
		}

		// Token: 0x06002EBE RID: 11966 RVA: 0x000B4C78 File Offset: 0x000B2E78
		protected internal override void OnEditorVariableChanged(string variableName)
		{
			base.OnEditorVariableChanged(variableName);
			if (variableName == "baseIntensity")
			{
				this.baseIntensity = MathF.Max(0f, this.baseIntensity);
				return;
			}
			if (variableName == "noonIntensityMultiplier" || variableName == "dawnDuskIntensityMultiplier" || variableName == "nightIntensityMultiplier")
			{
				this.noonIntensityMultiplier = MathF.Max(0f, this.noonIntensityMultiplier);
				this.dawnDuskIntensityMultiplier = MathF.Max(0f, this.dawnDuskIntensityMultiplier);
				this.nightIntensityMultiplier = MathF.Max(0f, this.nightIntensityMultiplier);
			}
		}

		// Token: 0x0400127D RID: 4733
		public float baseIntensity = 200f;

		// Token: 0x0400127E RID: 4734
		public float noonIntensityMultiplier = 0.6f;

		// Token: 0x0400127F RID: 4735
		public float dawnDuskIntensityMultiplier = 1f;

		// Token: 0x04001280 RID: 4736
		public float nightIntensityMultiplier = 1.5f;

		// Token: 0x04001281 RID: 4737
		private const string ManagedTag = "fake_spec";

		// Token: 0x04001282 RID: 4738
		private const float PlateauHalfWidth = 2f;

		// Token: 0x04001283 RID: 4739
		private readonly List<Light> _managedLights = new List<Light>();

		// Token: 0x04001284 RID: 4740
		private List<WeakGameEntity> _descendantBuffer = new List<WeakGameEntity>();
	}
}
