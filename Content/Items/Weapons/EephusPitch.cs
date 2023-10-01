using System;
using System.Numerics;
using Baseball.Content.Projectiles;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace Baseball.Content.Items.Weapons
{
	public class EephusPitch : ModItem
	{
		public override void SetDefaults() {
			//Item.DefaultToMagicWeapon(ModContent.ProjectileType<EephusBall>(), 20, 1);
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Magic;
            Item.noMelee = true;
            Item.autoReuse = false;
            Item.width = 34;
			Item.height = 40;
            Item.mana = 10;
			Item.UseSound = SoundID.Item71;


			Item.SetShopValues(ItemRarityColor.LightRed4, 10000);
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.DirtBlock, 10)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
        
        public override void OnConsumeMana(Player player, int manaConsumed)
        {
            float x1 = player.Center.X;
            float y1 = -player.Center.Y;
            float x2 = Main.MouseWorld.X;
            float y2 = -Main.MouseWorld.Y;
            float g = 0.4f;
            float yVel = (float) Math.Sqrt(g * 2.4 * Math.Max(Math.Abs(x2-x1)/2, y2-y1));
            float t = (yVel / g) + (float) Math.Sqrt((2 / g) * (y1 - y2 + yVel * yVel / (2 * g)));
            float xVel =  (x2 - x1) / t;
            Projectile.NewProjectile(player.GetSource_FromThis(), player.Center.X, player.Center.Y, xVel, -yVel, ModContent.ProjectileType<EephusBall>(), 10, 10, player.whoAmI);
        }
    }
}