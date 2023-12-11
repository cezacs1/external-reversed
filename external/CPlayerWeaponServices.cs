using System;
using System.Runtime.InteropServices;

// Token: 0x02000008 RID: 8
[StructLayout(LayoutKind.Explicit)]
public struct CPlayerWeaponServices
{
	// Token: 0x04000038 RID: 56
	[FieldOffset(96)]
	public C_BasePlayerWeapon m_hActiveWeapon;

	// Token: 0x04000039 RID: 57
	[FieldOffset(104)]
	public int m_iAmmo;
}
