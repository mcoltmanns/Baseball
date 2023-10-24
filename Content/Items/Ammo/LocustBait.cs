using Terraria.ModLoader;
using Terraria;

namespace Baseball.Content.Items.Ammo
{
    public class LocustBait : ModItem
    {
        public override void SetDefaults()
        {
            Item.ammo = Item.type;
            Item.shoot = ModContent.ProjectileType<Projectiles.LocustBait>();
            Item.maxStack = Item.CommonMaxStack;
            Item.shootSpeed = 16f;
            Item.consumable = true;
        }
    }
}