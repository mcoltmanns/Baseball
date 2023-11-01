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
	public class CurveBallPitch : ModItem
	{
		public override void SetDefaults() {
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Magic;
            Item.noMelee = true;
            Item.autoReuse = false;
            Item.width = 34;
			Item.height = 40;
            Item.mana = 10;
			Item.UseSound = SoundID.Item71;
            Item.shootSpeed = 8;
            Item.damage = 10;

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
            Item.shoot = ModContent.ProjectileType<CurveBall>();
            base.OnConsumeMana(player, manaConsumed);
        }
    }
}