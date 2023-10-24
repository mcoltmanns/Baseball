using Terraria.ID;
using Terraria.ModLoader;

namespace Baseball.Content.Projectiles
{
    public class Locust : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.arrow = true;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
        }
    }
}