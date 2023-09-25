using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Baseball.Content.Items.Weapons
{
    public class Bat : ModItem{
        public override void SetDefaults()
        {
            // width/height?

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20; // cooldown
            Item.useAnimation = 20; // animation time
            Item.autoReuse = false;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 50;
            Item.knockBack = 6;
            Item.crit = 6; // crit chance

            Item.value = Item.buyPrice(copper: 50);
            Item.rare = ItemRarityID.Gray;

            Item.shoot = ModContent.ProjectileType<Projectiles.Ball>(); // need to define what gets shot, as well as the ammo used
            Item.useAmmo = ModContent.ItemType<Ammo.BallAmmo>();
        }

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
    }
}