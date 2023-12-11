using System;

// Token: 0x02000015 RID: 21
public class ScreenProjection
{
	// Token: 0x06000068 RID: 104 RVA: 0x000030DC File Offset: 0x000012DC
	public void Init(int width, int height)
	{
		this.Width = width;
		this.Height = height;
		this.ScreenCenterX = this.Width / 2;
		this.ScreenCenterY = this.Height / 2;
	}

	// Token: 0x06000069 RID: 105 RVA: 0x00003108 File Offset: 0x00001308
	public void UpdateData(Matrix inMat)
	{
		Matrix vievProj;
		Matrix.Transpose(ref inMat, out vievProj);
		this.VievProj = vievProj;
	}

	// Token: 0x0600006A RID: 106 RVA: 0x0000312C File Offset: 0x0000132C
	public bool WorldToScreen(Vector3 pos, out Vector2 screen)
	{
		float num = pos.X * this.VievProj.M13 + pos.Y * this.VievProj.M23 + pos.Z * this.VievProj.M33 + this.VievProj.M43;
		if (num < 0f)
		{
			screen = default(Vector2);
			return false;
		}
		float num2 = pos.X * this.VievProj.M11 + pos.Y * this.VievProj.M21 + pos.Z * this.VievProj.M31 + this.VievProj.M41;
		float num3 = pos.X * this.VievProj.M12 + pos.Y * this.VievProj.M22 + pos.Z * this.VievProj.M32 + this.VievProj.M42;
		float num4 = pos.X * this.VievProj.M14 + pos.Y * this.VievProj.M24 + pos.Z * this.VievProj.M34 + this.VievProj.M44;
		num2 /= num4;
		num3 /= num4;
		num /= num4;
		screen.X = (1f + num2) * (float)this.ScreenCenterX;
		screen.Y = (1f - num3) * (float)this.ScreenCenterY;
		return this.IsVisibleOnScreen(screen);
	}

	// Token: 0x0600006B RID: 107 RVA: 0x000032A8 File Offset: 0x000014A8
	public bool IsVisibleOnScreen(Vector2 screenPos)
	{
		return !(screenPos == Vector2.Zero) && (screenPos.X >= 0f && screenPos.X <= (float)this.Width && screenPos.Y >= 0f) && screenPos.Y <= (float)this.Height;
	}

	// Token: 0x0400006B RID: 107
	public int Width;

	// Token: 0x0400006C RID: 108
	public int Height;

	// Token: 0x0400006D RID: 109
	private int ScreenCenterX;

	// Token: 0x0400006E RID: 110
	private int ScreenCenterY;

	// Token: 0x0400006F RID: 111
	private Matrix VievProj;

	// Token: 0x04000070 RID: 112
	public static ScreenProjection Instance;
}
