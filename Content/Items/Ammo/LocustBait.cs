using Terraria.ModLoader;

namespace Baseball.Content.Items.Ammo
{
    public class LocustBait : ModItem
    {
        public override void SetDefaults()
        {
            Item.ammo = Item.type;
            Item.shoot = ModContent.ProjectileType<Projectiles.LocustBait>();
        }
    }
}