﻿using Microsoft.Xna.Framework;

using RoA.Common.Druid;
using RoA.Common.Druid.Claws;
using RoA.Common.Networking;
using RoA.Common.Networking.Packets;
using RoA.Content.Projectiles.Friendly.Druidic;
using RoA.Core;
using RoA.Core.Utility;

using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace RoA.Content.Items.Weapons.Druidic.Claws;

sealed class HorrorPincers : BaseClawsItem {
    protected override void SafeSetDefaults() {
        Item.SetSize(26);
        Item.SetWeaponValues(14, 4f);

        Item.rare = ItemRarityID.Blue;

        Item.value = Item.sellPrice(0, 0, 25, 0);

        Item.SetDefaultToUsable(ItemUseStyleID.Swing, 18, false, autoReuse: true);

        NatureWeaponHandler.SetPotentialDamage(Item, 16);
        NatureWeaponHandler.SetFillingRate(Item, 1f);
    }

    protected override (Color, Color) SlashColors(Player player) => (new Color(112, 75, 140), new Color(130, 100, 210));

    public override void SafeOnUse(Player player, ClawsHandler clawsStats) {
        int offset = 30 * player.direction;
        var position = new Vector2(player.Center.X + offset, player.Center.Y);
        Vector2 pointPosition = player.GetViableMousePosition();
        Vector2 point = Helper.VelocityToPoint(player.Center, pointPosition, 1.2f);
        clawsStats.SetSpecialAttackData<InfectedWave>(new ClawsHandler.AttackSpawnInfoArgs() {
            Owner = Item,
            SpawnPosition = new Vector2(position.X, position.Y - 14f),
            StartVelocity = point,
            PlaySoundStyle = new SoundStyle(ResourceManager.ItemSounds + "ClawsWave") { Volume = 0.75f },
            OnAttack = (player) => {
                if (Main.netMode == NetmodeID.MultiplayerClient) {
                    MultiplayerSystem.SendPacket(new PlayOtherItemSoundPacket(player, 1, player.Center));
                }
            }
        });
    }
}
