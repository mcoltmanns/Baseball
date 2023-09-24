using Baseball.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Baseball.Items
{
    public class BallAmmo : ModItem
    {
        public override void SetDefaults()
        {
            Item.ammo = Item.type; // defining ammo
            Item.shoot = ModContent.ProjectileType<Ball>();

            Item.shootSpeed = 16f;
        }
    }
}