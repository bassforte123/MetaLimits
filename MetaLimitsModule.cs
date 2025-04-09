using BepInEx;
using System;
using System.Collections.Generic;
using UnityEngine;
using Dungeonator;
using HarmonyLib;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using Alexandria.Misc;
using MonoMod.RuntimeDetour;
using System.Reflection;




namespace MetaLimits
{
    [BepInDependency(Alexandria.Alexandria.GUID)] // this mod depends on the Alexandria API: https://enter-the-gungeon.thunderstore.io/package/Alexandria/Alexandria/
    [BepInDependency(ETGModMainBehaviour.GUID)]
    [BepInDependency(Gunfiguration.C.MOD_GUID)]
    [BepInPlugin(GUID, NAME, VERSION)]
    public class MetaLimitsModule : BaseUnityPlugin
    {
        public const string GUID = "bassforte.etg.metalimits";
        public const string NAME = "MetaLimits";
        public const string VERSION = "2.1.0";
        public const string TEXT_COLOR = "#00FFFF";

        internal static float currMagnificence = 0;
        internal static bool configStarted = false;
        internal static int curseCure1 = 0;
        internal static int curseCure2 = 0;
        internal static bool CostsFlag = true;
        public static bool startChest = true;
        internal static int boastEnabled = 1;


        public void Start()
        {
            ETGModMainBehaviour.WaitForGameManagerStart(GMStart);
        }
        public void GMStart(GameManager g)
        {
            Log($"{NAME} v{VERSION} started successfully.", TEXT_COLOR);
            MetaConfig.Init();
            InitQoLHooks();
            configStarted = true;

            CustomActions.OnRunStart += initialTonic;
            CustomActions.OnRunStart += initialStats;
            CustomActions.OnAnyPlayerCollectedHealth += CureCurse;
        }


        internal void Awake()
        {
            var harmony = new Harmony("bassforte.etg.metalimits");
            harmony.PatchAll();
        }

        private static void InitQoLHooks()
        {
            //Set PlayerController Hook (From Gunfig QoL)
            new Hook(
              typeof(PlayerController).GetMethod("Awake", BindingFlags.Instance | BindingFlags.Public),
              typeof(MetaLimitsModule).GetMethod("OnPlayerAwake", BindingFlags.Static | BindingFlags.NonPublic)
              );
        }

        [HarmonyPatch(typeof(SharedDungeonSettings), nameof(SharedDungeonSettings.RandomShouldBecomeMimic))]
        private class SharedDungeonSettingsPatch
        {
            [HarmonyILManipulator]
            private static void RandomShouldBecomeMimicIL(ILContext il)
            {
                ILCursor cursor = new ILCursor(il);

                if (!cursor.TryGotoNext(MoveType.After, instr => instr.MatchMul(), instr => instr.MatchAdd()))
                    return;

                cursor.Emit(OpCodes.Call, typeof(MetaLimitsModule).GetMethod("MimicChestRate"));
            }
        }

        public static float MimicChestRate(float curr)
        {
            if (MetaConfig._Gunfig.Value(MetaConfig.PILOTPAST_LABEL) == "More Mimics")
                return curr += 0.1f; //added mimic chest chance
            return curr;
        }

        [HarmonyPatch(typeof(LootEngine), nameof(LootEngine.DoAmmoClipCheck), new Type[] { typeof(float) })]
        private class DoAmmoClipCheckPatch
        {
            [HarmonyILManipulator]
            private static void GenerateContentsIL(ILContext il)
            {
                ILCursor cursor = new ILCursor(il);

                if (!cursor.TryGotoNext(MoveType.After, instr => instr.MatchLdarg(0)))
                    return;

                cursor.Emit(OpCodes.Call, typeof(MetaLimitsModule).GetMethod("AmmoMultValue"));
            }
        }

        public static float AmmoMultValue(float curr)
        {
            if (MetaConfig._Gunfig.Value(MetaConfig.MARINEPAST_LABEL) == "Ample Ammo") curr *= 1.25f; //Multiply ammo drop rate
            return curr;
        }

        [HarmonyPatch(typeof(AIActor), nameof(AIActor.HandleLootPinata))]
        private class HandleLootPinataPatch
        {
            [HarmonyILManipulator]
            private static void LootPinataIL(ILContext il)
            {
                ILCursor cursor = new ILCursor(il);

                if (!cursor.TryGotoNext(MoveType.After, instr => instr.MatchLdarg(1)))
                    return;

                cursor.Emit(OpCodes.Call, typeof(MetaLimitsModule).GetMethod("BossHege"));
            }
        }

        public static int BossHege(int curr)
        {
            if (MetaConfig._Gunfig.Value(MetaConfig.CONVICTPAST_LABEL) == "Hearty Hegemony")
                curr += 1; //Add boss/subboss Hege amount
            return curr;
        }

        [HarmonyPatch(typeof(Dungeon), nameof(Dungeon.InformRoomCleared))]
        private class InformRoomClearedPatch
        {
            static void Postfix()
            {
                if (MetaConfig._Gunfig.Value(MetaConfig.HUNTERPAST_LABEL) == "Refreshed Rewards")
                    GameManager.Instance.PrimaryPlayer.AdditionalChestSpawnChance += .02f; //added chance for rewards incremented each non reward room clear


                if (MetaConfig._Gunfig.Value(MetaConfig.MAP_LABEL) == "Memorized Maps" && UnityEngine.Random.value < 0.01)
                {
                    PickupObject byId = PickupObjectDatabase.GetById(137);
                    LootEngine.SpawnItem(byId.gameObject, GameManager.Instance.BestActivePlayer.CenterPosition, Vector2.zero, 0f, doDefaultItemPoof: true);
                }
            }
        }


        private static void initialStats(PlayerController arg1, PlayerController arg2, GameManager.GameMode arg3)
        {
            int curecount = 0;
            if (MetaConfig._Gunfig.Value(MetaConfig.CURSE_LABEL) == "Curse Cure") curecount = 1;
            if (MetaConfig._Gunfig.Value(MetaConfig.CURSE_LABEL) == "Master Misfortune") curecount = 2;

            curseCure1 = curecount; //Sets the initial curse cure count;
            curseCure2 = curecount;

            if (MetaConfig._Gunfig.Value(MetaConfig.SLINGERPAST_LABEL) == "Create Coolness")
            {
                StatModifier statModifier = new StatModifier();
                statModifier.amount = 2f; //Sets initial Coolness ammount
                statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
                statModifier.statToBoost = PlayerStats.StatType.Coolness;
                GameManager.Instance.PrimaryPlayer.ownerlessStatModifiers.Add(statModifier);
                GameManager.Instance.PrimaryPlayer.stats.RecalculateStats(GameManager.Instance.PrimaryPlayer);

                if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
                {
                    GameManager.Instance.SecondaryPlayer.ownerlessStatModifiers.Add(statModifier);
                    GameManager.Instance.SecondaryPlayer.stats.RecalculateStats(GameManager.Instance.SecondaryPlayer);
                }
            }

            if (MetaConfig._Gunfig.Value(MetaConfig.ROBOTPAST_LABEL) == "Additional Armor")
            {
                GameManager.Instance.PrimaryPlayer.healthHaver.Armor += 1; //Grants extra initial armor
                if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER) GameManager.Instance.SecondaryPlayer.healthHaver.Armor += 1;
            }

            if (MetaConfig._Gunfig.Value(MetaConfig.MONEY_LABEL) == "Persistent Prizes")
            {
                LootEngine.SpawnCurrency(GameManager.Instance.BestActivePlayer.CenterPosition, 15);
            }

            if (MetaConfig._Gunfig.Value(MetaConfig.BULLETPAST_LABEL) == "Bonus Blank")
            {
                StatModifier blankModifier = new StatModifier();
                blankModifier.amount = 1f; //Sets Additional Blank Per Floor ammount
                blankModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
                blankModifier.statToBoost = PlayerStats.StatType.AdditionalBlanksPerFloor;
                GameManager.Instance.PrimaryPlayer.ownerlessStatModifiers.Add(blankModifier);
                GameManager.Instance.PrimaryPlayer.stats.RecalculateStats(GameManager.Instance.PrimaryPlayer);

                if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
                {
                    GameManager.Instance.SecondaryPlayer.ownerlessStatModifiers.Add(blankModifier);
                    GameManager.Instance.SecondaryPlayer.stats.RecalculateStats(GameManager.Instance.SecondaryPlayer);
                }
            }
        }

        //Heal a point of Curse once per run by picking up health/armor.
        private static void CureCurse(HealthPickup arg1, PlayerController player)
        {
            if (player.stats.GetStatValue(PlayerStats.StatType.Curse) >= 1) //only activate if the player has curse on them
            {
                if (curseCure1 > 0 && player == GameManager.Instance.PrimaryPlayer)
                {
                    curseCure1 -= 1; //resets on Run Start, makes it so Cure only happens once per run

                    StatModifier statModifier = new StatModifier();
                    statModifier.amount = -1f;
                    statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
                    statModifier.statToBoost = PlayerStats.StatType.Curse;
                    player.ownerlessStatModifiers.Add(statModifier);
                    player.stats.RecalculateStats(player);

                    AkSoundEngine.PostEvent("Play_OBJ_power_up_01", arg1.gameObject);
                }

                if (curseCure2 > 0 && player == GameManager.Instance.SecondaryPlayer)
                {
                    curseCure2 -= 1; //resets on Run Start, makes it so Cure only happens once per run

                    StatModifier statModifier = new StatModifier();
                    statModifier.amount = -1f;
                    statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
                    statModifier.statToBoost = PlayerStats.StatType.Curse;
                    player.ownerlessStatModifiers.Add(statModifier);
                    player.stats.RecalculateStats(player);

                    AkSoundEngine.PostEvent("Play_OBJ_power_up_01", arg1.gameObject);
                }
            }
        }

        [HarmonyPatch(typeof(Chest), nameof(Chest.Initialize))]
        private class InitializePatch
        {
            [HarmonyILManipulator]
            private static void InitializeIL(ILContext il)
            {
                ILCursor cursor = new ILCursor(il);

                if (!cursor.TryGotoNext(MoveType.After, instr => instr.MatchLdcR4(0.001f)))
                    return;

                cursor.Emit(OpCodes.Call, typeof(MetaLimitsModule).GetMethod("GlitchChestRate"));
            }
        }

        public static float GlitchChestRate(float curr)
        {
            if (MetaConfig._Gunfig.Value(MetaConfig.PARADOXPAST_LABEL) == "Gather Glitches")
                return .006f; //glitch chest chance
            return curr;
        }

        /* Depricated feature

        //affects Hegemony taken on character select screen for the Paradox
        [HarmonyPatch(typeof(FoyerCharacterSelectFlag), nameof(FoyerCharacterSelectFlag.OnSelectedCharacterCallback))]
        private class OnSelectedCharacterCallbackPatch
        {
            [HarmonyILManipulator]
            private static void GenerateContentsIL(ILContext il)
            {
                ILCursor cursor = new ILCursor(il);

                if (!cursor.TryGotoNext(MoveType.After, instr => instr.MatchLdcR4(-5)))
                    return;

                cursor.Emit(OpCodes.Call, typeof(MetaLimitsModule).GetMethod("CharPayUpdate"));
            }
        }

        //affects Hegemony taken on character select screen for the Slinger
        [HarmonyPatch(typeof(FoyerCharacterSelectFlag), nameof(FoyerCharacterSelectFlag.OnSelectedCharacterCallback))]
        private class OnSelectedCharacterCallback2Patch
        {
            [HarmonyILManipulator]
            private static void GenerateContentsIL(ILContext il)
            {
                ILCursor cursor = new ILCursor(il);

                if (!cursor.TryGotoNext(MoveType.After, instr => instr.MatchLdcR4(-7)))
                    return;

                cursor.Emit(OpCodes.Call, typeof(MetaLimitsModule).GetMethod("CharPayUpdate"));
            }
        }

        public static float CharPayUpdate(float curr)
        {
            if (MetaConfig._Gunfig.Value(MetaConfig.SLINGERPAST_LABEL) == "Fast Fingers")
            {
                if (curr == -5f) return -2f;
                if (curr == -7f) return -3f;
                }
            return curr; //do nothing if flag is off
        }
        */

        public static void initialTonic(PlayerController arg1, PlayerController arg2, GameManager.GameMode arg3)
        {
            if (MetaConfig._Gunfig.Value(MetaConfig.SYNERGYFUSE_LABEL) == "Better than Bonus Stages")
                GameStatsManager.Instance.SetFlag(GungeonFlags.TONIC_IS_LOADED, true);
        }


        [HarmonyPatch(typeof(Dungeon), nameof(Dungeon.FloorReached))]
        private class FloorReachedPatch
        {
            //Spawns chests after the floor has been loaded
            static void Postfix()
            {
                GameManager metaInstance = GameManager.Instance;

                //resets the Boss Boast flag every floor
                if (metaInstance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER) boastEnabled = 2;
                else boastEnabled = 1;
                

                if (!(metaInstance.CurrentLevelOverrideState == GameManager.LevelOverrideState.CHARACTER_PAST || metaInstance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER || metaInstance.CurrentLevelOverrideState == GameManager.LevelOverrideState.TUTORIAL)
                && !GameStatsManager.Instance.rainbowRunToggled && startChest)
                {
                    if (GameStatsManager.Instance.GetSessionStatValue(TrackedStats.TIME_PLAYED) < 0.1f) SpawnInitialChest();
                    if (GameStatsManager.Instance.GetSessionStatValue(TrackedStats.TIME_PLAYED) < 0.1f && metaInstance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && MetaConfig._Gunfig.Value(MetaConfig.COOPCHEST_LABEL) == "The Power of Friendship") SpawnInitialChest();
                }
                if (GameStatsManager.Instance.IsRainbowRun && metaInstance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && MetaConfig._Gunfig.Value(MetaConfig.COOPCHEST_LABEL) == "The Power of Friendship") SpawnCoopRainbow();
            }
        }

        //spawns a second rainbow chest during Rainbow Run Mode during Coop
        static void SpawnCoopRainbow()
        {
            bool success;

            Chest chest = Chest.Spawn(GameManager.Instance.RewardManager.A_Chest, GameManager.Instance.Dungeon.data.Entrance.GetCenteredVisibleClearSpot(2, 2, out success));
            chest.m_isMimic = false;
            chest.IsRainbowChest = true;
            chest.BecomeRainbowChest();
        }

        //Modifies how much each point of Magnificence increases your reroll chance
        [HarmonyPatch(typeof(MagnificenceConstants), nameof(MagnificenceConstants.ModifyQualityByMagnificence))]
        private class MagnificenceConstantsPatch
        {
            [HarmonyILManipulator]
            private static void ModifyQualityByMagnificenceIL(ILContext il)
            {
                ILCursor cursor = new ILCursor(il);

                if (!cursor.TryGotoNext(MoveType.After,
                instr => instr.MatchCallOrCallvirt<UnityEngine.Mathf>("Clamp01")))
                    return;

                cursor.Emit(OpCodes.Call, typeof(MetaLimitsModule).GetMethod("AssignMagnificenceValue"));
            }
        }

        public static float AssignMagnificenceValue(float curr)
        {
            if ((MetaConfig._Gunfig.Value(MetaConfig.MAGNIFICENCE_LABEL) == "Vanilla")
                || (MetaConfig._Gunfig.Value(MetaConfig.MAGNIFICENCE_LABEL) == "Locked"))
                return curr;

            //base Magnificence reroll chances 80/95/98.6/99
            {
                float ma = .8f;
                float mb = .95f;
                float mc = .986f;
                float md = .99f;

                int optioncount = 0;

                if (MetaConfig._Gunfig.Value(MetaConfig.MAGNIFICENCE_LABEL) == "Magnificence I") optioncount = 1;
                if (MetaConfig._Gunfig.Value(MetaConfig.MAGNIFICENCE_LABEL) == "Magnificence II Dawn of Souls") optioncount = 2;
                if (MetaConfig._Gunfig.Value(MetaConfig.MAGNIFICENCE_LABEL) == "Magnificence III 3D") optioncount = 3;
                if (MetaConfig._Gunfig.Value(MetaConfig.MAGNIFICENCE_LABEL) == "Magnificence IV The After Years") optioncount = 4;
                if (MetaConfig._Gunfig.Value(MetaConfig.MAGNIFICENCE_LABEL) == "Magnificence V Advance") optioncount = 5;
                if (MetaConfig._Gunfig.Value(MetaConfig.MAGNIFICENCE_LABEL) == "Magnificence VI Pixel Remaster") optioncount = 6;
                if (MetaConfig._Gunfig.Value(MetaConfig.MAGNIFICENCE_LABEL) == "Magnificence VII Remake") optioncount = 7;
                if (MetaConfig._Gunfig.Value(MetaConfig.MAGNIFICENCE_LABEL) == "Magnificence VIII Remastered") optioncount = 8;
                if (MetaConfig._Gunfig.Value(MetaConfig.MAGNIFICENCE_LABEL) == "Magnificence IX Steam Version") optioncount = 9;
                if (MetaConfig._Gunfig.Value(MetaConfig.MAGNIFICENCE_LABEL) == "Magnificence X HD") optioncount = 10;
                if (MetaConfig._Gunfig.Value(MetaConfig.MAGNIFICENCE_LABEL) == "Magnificence XI Online") optioncount = 11;
                if (MetaConfig._Gunfig.Value(MetaConfig.MAGNIFICENCE_LABEL) == "Magnificence XII The Zodiac Age") optioncount = 12;
                if (MetaConfig._Gunfig.Value(MetaConfig.MAGNIFICENCE_LABEL) == "Magnificence XIII Lightning Returns") optioncount = 13;
                if (MetaConfig._Gunfig.Value(MetaConfig.MAGNIFICENCE_LABEL) == "Magnificence XIV A Realm Reborn") optioncount = 14;
                if (MetaConfig._Gunfig.Value(MetaConfig.MAGNIFICENCE_LABEL) == "Magnificence XV Royal Edition") optioncount = 15;

                /*odds of reroll
                 0   80/95/98.6/99
                 1   70/95/98.6/99
                 2   60/95/98.6/99
                 3   70/80/85/99
                 4   60/80/90/99
                 5   50/80/90/99
                 6   50/70/90/99
                 7   50/70/80/99
                 8   50/70/80/90
                 9   40/70/80/90
                 10  40/60/80/90
                 11  40/60/70/90
                 12  40/60/70/80
                 13  40/50/70/80
                 14  30/50/60/80
                 15  30/40/50/70
                */

                if (optioncount >= 1) ma = .7f;
                if (optioncount >= 2) ma = .6f;
                if (optioncount >= 3) mb = .8f;
                if (optioncount >= 4) mc = .9f;
                if (optioncount >= 5) ma = .5f;
                if (optioncount >= 6) mb = .7f;
                if (optioncount >= 7) mc = .8f;
                if (optioncount >= 8) md = .9f;
                if (optioncount >= 9) ma = .4f;
                if (optioncount >= 10) mb = .6f;
                if (optioncount >= 11) mc = .7f;
                if (optioncount >= 12) md = .8f;
                if (optioncount >= 13) mb = .5f;
                if (optioncount >= 14) mc = .6f;
                if (optioncount >= 15)
                {
                    ma = .3f;
                    mb = .4f;
                    mc = .5f;
                    md = .7f;
                }

                //Log($"Reroll mode: " + MetaConfig._Gunfig.Value(MetaConfig.MAGNIFICENCE_LABEL), TEXT_COLOR);
                //Log($"Reroll chances: " + ma + " / " + mb + " / " + mc + " / " + md, TEXT_COLOR);
                if (currMagnificence == 1)
                    return 1f - ma;
                if (currMagnificence == 2)
                    return 1f - mb;
                if (currMagnificence == 3)
                    return 1f - mc;
                if (currMagnificence == 4)
                    return 1f - md;
                else return 1f - ((1 - md) / 2); //Anything after 4 Magnificence gets half the remain chance left

            }
        }



        //Spawn a Starter Chest
        static void SpawnInitialChest()
        {
            bool success;
            Chest chest = null;

            //Decide type of chest
            if (MetaConfig._Gunfig.Value(MetaConfig.STARTCHEST_LABEL) == "Starter Chest") chest = Chest.Spawn(GameManager.Instance.RewardManager.D_Chest, GameManager.Instance.Dungeon.data.Entrance.GetCenteredVisibleClearSpot(2, 2, out success));
            else if (MetaConfig._Gunfig.Value(MetaConfig.STARTCHEST_LABEL) == "Evolved Chest") chest = Chest.Spawn(GameManager.Instance.RewardManager.C_Chest, GameManager.Instance.Dungeon.data.Entrance.GetCenteredVisibleClearSpot(2, 2, out success));
            else if (MetaConfig._Gunfig.Value(MetaConfig.STARTCHEST_LABEL) == "Final Chest") chest = Chest.Spawn(GameManager.Instance.RewardManager.B_Chest, GameManager.Instance.Dungeon.data.Entrance.GetCenteredVisibleClearSpot(2, 2, out success));
            else if (MetaConfig._Gunfig.Value(MetaConfig.STARTCHEST_LABEL) == "Mega Chest" || MetaConfig._Gunfig.Value(MetaConfig.STARTCHEST_LABEL) == "Mega Chest (Burgle Bowler)") chest = Chest.Spawn(GameManager.Instance.RewardManager.A_Chest, GameManager.Instance.Dungeon.data.Entrance.GetCenteredVisibleClearSpot(2, 2, out success));

            if (chest != null)
            {
                chest.lootTable.Common_Chance = 0f;
                chest.m_isMimic = false;
                chest.IsLocked = false;

                if (MetaConfig._Gunfig.Value(MetaConfig.MULTI_LABEL) == "Rainbow Blessing" || MetaConfig._Gunfig.Value(MetaConfig.STARTCHEST_LABEL) == "Mega Chest (Burgle Bowler)")
                    BecomeBlessedChest(chest);
            }
        }

        public static void BecomeBlessedChest(Chest chest)
        {
            chest.IsRainbowChest = true; //rainbow status so that the rainbowrun first chest rules apply

            chest.lootTable.canDropMultipleItems = true;
            chest.lootTable.multipleItemDropChances = new WeightedIntCollection();
            chest.lootTable.multipleItemDropChances.elements = new WeightedInt[1];
            chest.lootTable.overrideItemLootTables = new List<GenericLootTable>();
            chest.lootTable.lootTable = GameManager.Instance.RewardManager.GunsLootTable;
            chest.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable); //alternates between guns and items
            chest.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
            chest.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
            chest.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
            chest.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
            chest.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
            chest.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
            chest.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);

            WeightedInt weightedInt = new WeightedInt();

            weightedInt.weight = 1f;
            weightedInt.additionalPrerequisites = new DungeonPrerequisite[0];
            chest.lootTable.multipleItemDropChances.elements[0] = weightedInt;
            chest.lootTable.onlyOneGunCanDrop = false;
            chest.sprite.usesOverrideMaterial = true;
            tk2dSpriteAnimationClip clipByName = chest.spriteAnimator.GetClipByName(chest.spawnAnimName);
            chest.sprite.SetSprite(clipByName.frames[clipByName.frames.Length - 1].spriteId);

            //Use Rainbow Shader
            chest.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/Internal/RainbowChestShader");

            //Control Rainbow Shader intensity
            if (chest.spawnAnimName.StartsWith("wood_"))
            {
                chest.sprite.renderer.material.SetFloat("_HueTestValue", -4f);
                weightedInt.value = 3;
            }
            else if (chest.spawnAnimName.StartsWith("silver_"))
            {
                chest.sprite.renderer.material.SetFloat("_HueTestValue", -.2f);
                chest.lootTable.C_Chance = .75f;
                chest.lootTable.D_Chance = .25f;
                weightedInt.value = 4;
                chest.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);

            }
            else if (chest.spawnAnimName.StartsWith("green_"))
            {
                chest.sprite.renderer.material.SetFloat("_HueTestValue", -.6f);
                chest.lootTable.B_Chance = .60f;
                chest.lootTable.C_Chance = .20f;
                chest.lootTable.D_Chance = .20f;
                weightedInt.value = 5;
                chest.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
            }
            else if (chest.spawnAnimName.StartsWith("redgold_") && MetaConfig._Gunfig.Value(MetaConfig.STARTCHEST_LABEL) == "Mega Chest")
            {
                chest.sprite.renderer.material.SetFloat("_HueTestValue", -3f);
                chest.lootTable.A_Chance = 0.6f;
                chest.lootTable.B_Chance = 0.3f;
                chest.lootTable.C_Chance = 0.2f;
                chest.lootTable.D_Chance = 0.1f;
                weightedInt.value = 6;
            }
            else if (chest.spawnAnimName.StartsWith("redgold_") && MetaConfig._Gunfig.Value(MetaConfig.STARTCHEST_LABEL) == "Mega Chest (Burgle Bowler)")
            {
                chest.lootTable.S_Chance = 0.2f;
                chest.lootTable.A_Chance = 0.7f;
                chest.lootTable.B_Chance = 0.4f;
                chest.lootTable.C_Chance = 0.2f;
                chest.lootTable.D_Chance = 0.2f;
                weightedInt.value = 8;
            }
        }


        [HarmonyPatch(typeof(Chest), nameof(Chest.SpewContentsOntoGround))]

        public class SpewContentsOntoGroundPatch
        {
            //Only the first item from an initial Rainbow Run chest can be picked up.
            static bool Prefix(Chest __instance, List<Transform> spawnTransforms)

            {
                if (__instance.IsRainbowChest && (GameStatsManager.Instance.IsRainbowRun || MetaConfig._Gunfig.Value(MetaConfig.MULTI_LABEL) == "Rainbow Blessing" || MetaConfig._Gunfig.Value(MetaConfig.STARTCHEST_LABEL) == "Mega Chest (Burgle Bowler)") && __instance.transform.position.GetAbsoluteRoom() == GameManager.Instance.Dungeon.data.Entrance)
                {
                    List<DebrisObject> list = new List<DebrisObject>();

                    for (int i = 0; i < __instance.contents.Count; i++)
                    {
                        List<GameObject> list2 = new List<GameObject>();
                        list2.Add(__instance.contents[i].gameObject);
                        List<DebrisObject> list3 = LootEngine.SpewLoot(list2, spawnTransforms[i].position);
                        list.AddRange(list3);
                        for (int j = 0; j < list3.Count; j++)
                        {
                            if ((bool)list3[j])
                            {
                                list3[j].PreventFallingInPits = true;
                            }
                            if (!(list3[j].GetComponent<Gun>() != null) && !(list3[j].GetComponent<CurrencyPickup>() != null) && list3[j].specRigidbody != null)
                            {
                                list3[j].specRigidbody.CollideWithOthers = false;
                                DebrisObject debrisObject = list3[j];
                                debrisObject.OnTouchedGround = (Action<DebrisObject>)Delegate.Combine(debrisObject.OnTouchedGround, new Action<DebrisObject>(__instance.BecomeViableItem));
                            }
                        }
                    }
                    GameManager.Instance.Dungeon.StartCoroutine(__instance.HandleRainbowRunLootProcessing(list));
                    return false;
                }
                else { return true; }
            }

        }


        //Synergy Chest success rate
        [HarmonyPatch(typeof(Chest), nameof(Chest.GenerateContents))]
        private class GenerateContentsPatch
        {
            [HarmonyILManipulator]
            private static void GenerateContentsIL(ILContext il)
            {
                ILCursor cursor = new ILCursor(il);

                if (!cursor.TryGotoNext(MoveType.After,
                    instr => instr.MatchCallOrCallvirt<UnityEngine.Mathf>("Clamp")))
                    return;
                cursor.Emit(OpCodes.Call, typeof(MetaLimitsModule).GetMethod("AssignSynergyChestValue"));
            }
        }



        //Sets the Synergy Chest failure rate
        public static float AssignSynergyChestValue(float curr)
        {
            if (MetaConfig._Gunfig.Value(MetaConfig.SYNERGYCHEST_LABEL) == "Blessed Synergy") return 0.20f; //failure rate
            //Log($"SynergyChestChance: " + GameManager.Instance.RewardManager.GlobalSynerchestChance, TEXT_COLOR);
            return curr;
        }


        //Change how much each synergy affects Synergy Factor
        [HarmonyPatch(typeof(SynergyFactorConstants), nameof(SynergyFactorConstants.GetSynergyFactor))]
        private class SynergyFactorPatch
        {
            [HarmonyILManipulator]
            private static void ModifyGetSynergyFactor(ILContext il)
            {
                ILCursor cursor = new ILCursor(il);

                if (!cursor.TryGotoNext(MoveType.After, instr => instr.MatchLdloc(3)))
                    return;

                cursor.Emit(OpCodes.Call, typeof(MetaLimitsModule).GetMethod("MultSynergyFactor"));
            }
        }

        public static float MultSynergyFactor(float curr)
        {
            //Do nothing on Vanilla (Values ignoring starting bonuses = 0.8/0.121/0.026/0.008)
            if (MetaConfig._Gunfig.Value(MetaConfig.SYNERGYFACTOR_LABEL) == "Vanilla") return curr;

            int synNum = GameStatsManager.Instance.GetNumberOfSynergiesEncounteredThisRun();
            float SFMult = 1;
            curr -= 1;
            int mode = 0;

            if (MetaConfig._Gunfig.Value(MetaConfig.SYNERGYFACTOR_LABEL) == "Cadence") mode = 1;
            if (MetaConfig._Gunfig.Value(MetaConfig.SYNERGYFACTOR_LABEL) == "SYNCHRONY") mode = 2;
            if (MetaConfig._Gunfig.Value(MetaConfig.SYNERGYFACTOR_LABEL) == "Hatsune Miku") mode = 3;

            //flat values first
            if (synNum == 1) SFMult = 5f; //Second Synergy is x5 (0.6) on AMPLIFIED and higher (always on when not Vanilla)
            if ((synNum == 2) && (mode > 1)) SFMult = 15f; //third synergy is x14 (0.39) on SYNCHRONY and higher

            if (mode == 1 || mode == 2) SFMult *= 2f;
            if (mode == 3) SFMult *= 3f;

            //Log($"Synergy Factor: " + (1 + curr * SFMult), TEXT_COLOR);
            return (1 + curr * SFMult);
        }


        //Make Synergy Chests have a normal chance of having a fuse
        [HarmonyPatch(typeof(Chest), nameof(Chest.RoomEntered))]
        private class RoomEnteredPatch
        {
            [HarmonyILManipulator]
            private static void GenerateContentsIL(ILContext il)
            {
                ILCursor cursor = new ILCursor(il);

                if (!cursor.TryGotoNext(MoveType.After, instr => instr.MatchLdcR4(1)))
                    return;

                cursor.Emit(OpCodes.Ldloc_0); //load the regular fuse chance
                cursor.Emit(OpCodes.Call, typeof(MetaLimitsModule).GetMethod("SynergyFuseValue"));
                cursor.Index++; //skip over the storage instruction
                cursor.Emit(OpCodes.Pop); //drop the extra stack entry used here
            }
        }

        public static float SynergyFuseValue(float curr)
        {
            if (MetaConfig._Gunfig.Value(MetaConfig.SYNERGYFUSE_LABEL) == "Faster than Fuses" || (MetaConfig._Gunfig.Value(MetaConfig.SYNERGYFUSE_LABEL) == "Better than Bonus Stages")) return curr; //return normal fuse chance
            return 1f; //return always have fuse
        }


        [HarmonyPatch(typeof(FloorRewardData), nameof(FloorRewardData.GetTargetQualityFromChances))]
        private class GetTargetQualityFromChancesPatch
        {
            //Sets the current Magnificence to a variable
            static void Postfix(FloorRewardData __instance)
            {
                if (MetaConfig._Gunfig.Value(MetaConfig.SYNERGYCHESTSPAWN_LABEL) == "Synergized Synergy")
                    GameManager.Instance.RewardManager.GlobalSynerchestChance = 0.1f; //chance of a synergy chest spawning

                //__instance.S_Shop_Chance = 1000f; //sets shop to always spawn natural S items to test magnificence
                currMagnificence = __instance.DetermineCurrentMagnificence();
            }
        }

        //Activates Health bars when enemies take damage
        private static void OnPlayerAwake(Action<PlayerController> orig, PlayerController player)
        {
            orig(player);
            player.OnAnyEnemyReceivedDamage += DoHealthEffects; // health bars (borrowed from Scouter)

        }


        private static GameObject VFXHealthBar = null;
        private static readonly int ScouterId = 821;

        //Sets health bar logic (From Gunfig QoL > from Scouter item)
        private static void DoHealthEffects(float damageAmount, bool fatal, HealthHaver target)
        {
            if (GameManager.Instance.PrimaryPlayer.HasPassiveItem(ScouterId))
                return;
            if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && GameManager.Instance.SecondaryPlayer.HasPassiveItem(ScouterId))
                return;

            VFXHealthBar ??= (PickupObjectDatabase.GetById(ScouterId) as RatchetScouterItem).VFXHealthBar;

            Vector3 worldPosition = target.transform.position;
            float heightOffGround = 1f;

            if (target.GetComponent<SpeculativeRigidbody>() is SpeculativeRigidbody body)
            {
                worldPosition = body.UnitCenter.ToVector3ZisY();
                heightOffGround = worldPosition.y - body.UnitBottomCenter.y;
                if (MetaConfig._Gunfig.Value(MetaConfig.SEE_LABEL) == "Stats Studied" && (bool)body.healthHaver && !body.healthHaver.HasHealthBar && !body.healthHaver.HasRatchetHealthBar && !body.healthHaver.IsBoss)
                {
                    body.healthHaver.HasRatchetHealthBar = true;
                    UnityEngine.Object.Instantiate(VFXHealthBar).GetComponent<SimpleHealthBarController>().Initialize(body, body.healthHaver);
                }
            }
            else if (target.GetComponent<AIActor>() is AIActor actor)
            {
                worldPosition = actor.CenterPosition.ToVector3ZisY();
                if (actor.sprite)
                    heightOffGround = worldPosition.y - actor.sprite.WorldBottomCenter.y;
            }
        }


        //Modify Boss DPS Cap
        [HarmonyPatch(typeof(HealthHaver), nameof(HealthHaver.Start))]
        private class HealthHaverStartPatch
        {
            static void Postfix(HealthHaver __instance)
            {
                float capChange = 1;

                if (MetaConfig._Gunfig.Value(MetaConfig.CAP_LABEL) == "Better Bosses") capChange = .75f; //DPS cap multiplier
                if (MetaConfig._Gunfig.Value(MetaConfig.CAP_LABEL) == "Bosses Busted") capChange = 1.25f; 

                GameLevelDefinition lastLoadedLevelDefinition = GameManager.Instance.GetLastLoadedLevelDefinition();
                if (__instance.IsBoss && !__instance.IsSubboss && lastLoadedLevelDefinition.bossDpsCap > 0f)
                {
                    float num = 1f;
                    if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
                    {
                        num = (GameManager.Instance.COOP_ENEMY_HEALTH_MULTIPLIER + 2f) / 2f;
                    }
                    __instance.m_bossDpsCap = lastLoadedLevelDefinition.bossDpsCap * num * capChange;
                    //Log($"DPS Cap: " + __instance.m_bossDpsCap, TEXT_COLOR);
                }
            }
        }

        //Undoes the damaged flag when being hit in the floor boss room
        [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.Damaged))]
        private class DamagedPlayerPatch
        {
            static void Postfix(PlayerController __instance)
            {
                RoomHandler _CurrentRoom = __instance.CurrentRoom;

                if (MetaConfig._Gunfig.Value(MetaConfig.BOSSHIT_LABEL) == "Battle Boast" && boastEnabled > 0 &&
                    _CurrentRoom.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS && _CurrentRoom.area.PrototypeRoomBossSubcategory == PrototypeDungeonRoom.RoomBossSubCategory.FLOOR_BOSS)
                {
                    __instance.CurrentRoom.PlayerHasTakenDamageInThisRoom = false;
                    boastEnabled -= 1;
                    AkSoundEngine.PostEvent("Play_OBJ_power_up_01", __instance.gameObject);
                }
                //Log($"Has taken damage this room: " + __instance.CurrentRoom.PlayerHasTakenDamageInThisRoom, TEXT_COLOR);
            }
        }


        [HarmonyPatch(typeof(Projectile), nameof(Projectile.OnRigidbodyCollision))]
        private class OnRigidBodyCollisionPatch
        {
            [HarmonyILManipulator]
            private static void GenerateContentsIL(ILContext il)
            {
                ILCursor cursor = new ILCursor(il);

                if (!cursor.TryGotoNext(MoveType.After, instr => instr.MatchCallvirt<Gun>("get_InfiniteAmmo")))
                    return;
                //cursor.MoveBeforeLabels();
                cursor.Emit(OpCodes.Call, typeof(MetaLimitsModule).GetMethod("InfiniteAmmoCheck"));
            }
        }

        public static bool InfiniteAmmoCheck(bool curr)
        {
            if (MetaConfig._Gunfig.Value(MetaConfig.SECRET_LABEL) == "Frustrated Findings") return false; //return never infinite ammo so that infinte guns act like regular guns
            return curr; //normal infinite ammo status
        }


        [HarmonyPatch(typeof(PickupObject), nameof(PickupObject.ShouldBeTakenByRat))]
        private class ShouldBeTakenByRatPatch
        {
            static bool Prefix(ref bool __result)
            {
                if (MetaConfig._Gunfig.Value(MetaConfig.RAT_LABEL) == "RATDIE")
                {
                    __result = false;  //Rat can't steal
                    return false;  //skip stealing code
                }
                return true;
            }
        }

        public static void Log(string text, string color = "#00FFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }
    }
}
