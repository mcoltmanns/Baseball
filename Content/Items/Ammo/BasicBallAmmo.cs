using Terraria;
using Terraria.ModLoader;

namespace Baseball.Content.Items.Ammo
{
    public class BasicBallAmmo : ModItem
    {
        public override void SetDefaults()
        {
            Item.ammo = Item.type; // defining ammo
            Item.shoot = ModContent.ProjectileType<Projectiles.BasicBall>();
            Item.maxStack = Item.CommonMaxStack;
            Item.shootSpeed = 16f;
            Item.consumable = true;
        }
    }
}