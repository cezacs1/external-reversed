using System;

// Token: 0x02000011 RID: 17
public class LocalPlayer : Entity
{
	// Token: 0x0600003F RID: 63 RVA: 0x00002F9C File Offset: 0x0000119C
	public LocalPlayer(long pawnOffset) : base(pawnOffset)
	{
		LocalPlayer.Instance = this;
		base.GetActiveWeaponId();
	}

	// Token: 0x0400004E RID: 78
	public static LocalPlayer Instance;
}
