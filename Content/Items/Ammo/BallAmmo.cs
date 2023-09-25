using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Baseball.Content.Items.Ammo
{
    public class BallAmmo : ModItem
    {
        public override void SetDefaults()
        {
            Item.ammo = Item.type; // defining ammo
            Item.shoot = ModContent.ProjectileType<Projectiles.Ball>();

            Item.shootSpeed = 16f;
        }
    }
}