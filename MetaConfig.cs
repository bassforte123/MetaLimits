using System.Collections.Generic;
using UnityEngine;
using Gunfiguration;


namespace MetaLimits
{
    public static class MetaConfig
    {
        public static Gunfig _Gunfig = null;

        internal const string PILOTPAST_LABEL = "The Pilot's Affinity";
        private static readonly List<string> PILOTPAST_OPTIONS = new();
        private static readonly List<string> PILOTPAST_DESCRIPTIONS = new();
        internal const string MARINEPAST_LABEL = "The Marine's Affinity";
        private static readonly List<string> MARINEPAST_OPTIONS = new();
        private static readonly List<string> MARINEPAST_DESCRIPTIONS = new();
        internal const string CONVICTPAST_LABEL = "The Convict's Affinity";
        private static readonly List<string> CONVICTPAST_OPTIONS = new();
        private static readonly List<string> CONVICTPAST_DESCRIPTIONS = new();
        internal const string HUNTERPAST_LABEL = "The Hunter's Affinity";
        private static readonly List<string> HUNTERPAST_OPTIONS = new();
        private static readonly List<string> HUNTERPAST_DESCRIPTIONS = new();
        internal const string BULLETPAST_LABEL = "The Bullet's Affinity";
        private static readonly List<string> BULLETPAST_OPTIONS = new();
        private static readonly List<string> BULLETPAST_DESCRIPTIONS = new();
        internal const string ROBOTPAST_LABEL = "The Robot's Affinity";
        private static readonly List<string> ROBOTPAST_OPTIONS = new();
        private static readonly List<string> ROBOTPAST_DESCRIPTIONS = new();
        internal const string PARADOXPAST_LABEL = "The Paradox's Affinity";
        private static readonly List<string> PARADOXPAST_OPTIONS = new();
        private static readonly List<string> PARADOXPAST_DESCRIPTIONS = new();
        internal const string SLINGERPAST_LABEL = "The Slinger's Affinity";
        private static readonly List<string> SLINGERPAST_OPTIONS = new();
        private static readonly List<string> SLINGERPAST_DESCRIPTIONS = new();
        internal const string COOPCHEST_LABEL = "The Cultist's Affinity";
        private static readonly List<string> COOPCHEST_OPTIONS = new();
        private static readonly List<string> COOPCHEST_DESCRIPTIONS = new();

        internal const string STARTCHEST_LABEL = "The Gungeon's Affinity";
        private static readonly List<string> STARTCHEST_OPTIONS = new();
        private static readonly List<string> STARTCHEST_DESCRIPTIONS = new();
        internal const string MULTI_LABEL = "Bowler's Affinity";
        private static readonly List<string> MULTI_OPTIONS = new();
        private static readonly List<string> MULTI_DESCRIPTIONS = new();

        internal const string MAGNIFICENCE_LABEL = "Frifle and Mauser's Affinity";
        private static readonly List<string> MAGNIFICENCE_OPTIONS = new();
        private static readonly List<string> MAGNIFICENCE_DESCRIPTIONS = new();

        internal const string SYNERGYCHEST_LABEL = "Sorceress's Affinity";
        private static readonly List<string> SYNERGYCHEST_OPTIONS = new();
        private static readonly List<string> SYNERGYCHEST_DESCRIPTIONS = new();
        internal const string SYNERGYFACTOR_LABEL = "Tinker's Affinity";
        private static readonly List<string> SYNERGYFACTOR_OPTIONS = new();
        private static readonly List<string> SYNERGYFACTOR_DESCRIPTIONS = new();
        internal const string SYNERGYFUSE_LABEL = "Tonic's Affinity";
        private static readonly List<string> SYNERGYFUSE_OPTIONS = new();
        private static readonly List<string> SYNERGYFUSE_DESCRIPTIONS = new();
        internal const string SEE_LABEL = "Ser Manuel's Affinity";
        private static readonly List<string> SEE_OPTIONS = new();
        private static readonly List<string> SEE_DESCRIPTIONS = new();
        internal const string CURSE_LABEL = "Daisuke's Affinity";
        private static readonly List<string> CURSE_OPTIONS = new();
        private static readonly List<string> CURSE_DESCRIPTIONS = new();
        internal const string BOSSHIT_LABEL = "Gunsling King's Affinity";
        private static readonly List<string> BOSSHIT_OPTIONS = new();
        private static readonly List<string> BOSSHIT_DESCRIPTIONS = new();
        internal const string MAP_LABEL = "The Lost Adventurer's Affinity";
        private static readonly List<string> MAP_OPTIONS = new();
        private static readonly List<string> MAP_DESCRIPTIONS = new();
        internal const string MONEY_LABEL = "Winchester's Affinity";
        private static readonly List<string> MONEY_OPTIONS = new();
        private static readonly List<string> MONEY_DESCRIPTIONS = new();
        internal const string SECRET_LABEL = "Ledge Goblin's Affinity";
        private static readonly List<string> SECRET_OPTIONS = new();
        private static readonly List<string> SECRET_DESCRIPTIONS = new();

        internal const string SYNERGYCHESTSPAWN_LABEL = "Advanced Dragun's Affinity";
        private static readonly List<string> SYNERGYCHESTSPAWN_OPTIONS = new();
        private static readonly List<string> SYNERGYCHESTSPAWN_DESCRIPTIONS = new();

        internal const string CAP_LABEL = "The Bosses' Affinity";
        private static readonly List<string> CAP_OPTIONS = new();
        private static readonly List<string> CAP_DESCRIPTIONS = new();
        internal const string RAT_LABEL = "Resourceful Rat's Affinity";
        private static readonly List<string> RAT_OPTIONS = new();
        private static readonly List<string> RAT_DESCRIPTIONS = new();


        private static void BuildOptions()
        {
            Color Orange = new Color(1f, .4f, 0f);
            Color PaleOrange = new Color(1f, .7f, 4f);
            Color Ammo = new Color(0f, .6f, .4f);
            Color LightBlue = new Color(.5f, .5f, 1f);
            Color Pink = new Color(1f, .5f, .7f);
            Color Teal = new Color(0f, 1f, .8f);
            Color LightPurple = new Color(.5f, .5f, .8f);
            Color RedOrange = new Color(.8f, .4f, 0f);
            Color BrightBlue = new Color(.1f, .3f, 1f);
            Color Flower = new Color(.7f, .9f, .8f);
            Color BrightLightBlue = new Color(.1f, .7f, .8f);
            Color DarkCrimson = new Color(.6f, 0f, 0f);
            Color BrightPink = new Color(1f, .0f, .4f);
            Color CChest = new Color(.3f, .4f, .55f);
            Color BChest = new Color(.3f, .5f, .15f);

            // Gungoneer Block
            if (GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_ROGUE_PAST))
            {
                PILOTPAST_OPTIONS.Add("Vanilla".White());
                PILOTPAST_DESCRIPTIONS.Add("Mimic chests spawn 2.25% of the time. Increased with curse.".White());
                PILOTPAST_OPTIONS.Add("More Mimics".WithColor(Pink));
                PILOTPAST_DESCRIPTIONS.Add("Mimic chests spawn an additive 10% of the time.".WithColor(Pink));
            }
            else
            {
                PILOTPAST_OPTIONS.Add("Locked".Gray());
                PILOTPAST_DESCRIPTIONS.Add("Kill the Pilot's Past.".Gray());
            }

            if (GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_SOLDIER_PAST))
            {
                MARINEPAST_OPTIONS.Add("Vanilla".White());
                MARINEPAST_DESCRIPTIONS.Add("Normal ammo drop chance.".White());
                MARINEPAST_OPTIONS.Add("Ample Ammo".WithColor(Ammo));
                MARINEPAST_DESCRIPTIONS.Add("Increases ammo drop chance by a multiplicative 125%".WithColor(Ammo));
            }
            else
            {
                MARINEPAST_OPTIONS.Add("Locked".Gray());
                MARINEPAST_DESCRIPTIONS.Add("Kill the Marine's Past.".Gray());
            }

            if (GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_CONVICT_PAST))
            {
                CONVICTPAST_OPTIONS.Add("Vanilla".White());
                CONVICTPAST_DESCRIPTIONS.Add("Normal boss Hegemony drops.".White());
                CONVICTPAST_OPTIONS.Add("Hearty Hegemony".Green());
                CONVICTPAST_DESCRIPTIONS.Add("Bosses drop 1 additional base Hegemony.".Green());
            }
            else
            {
                CONVICTPAST_OPTIONS.Add("Locked".Gray());
                CONVICTPAST_DESCRIPTIONS.Add("Kill the Convict's Past.".Gray());
            }

            if (GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_GUIDE_PAST))
            {
                HUNTERPAST_OPTIONS.Add("Vanilla".White());
                HUNTERPAST_DESCRIPTIONS.Add("Room rewards are about 9% likely every room clear without getting a reward.".White());
                HUNTERPAST_OPTIONS.Add("Refreshed Rewards".WithColor(Orange));
                HUNTERPAST_DESCRIPTIONS.Add("Incremental room reward chance increased by an additive 2%".WithColor(Orange));
            }
            else
            {
                HUNTERPAST_OPTIONS.Add("Locked".Gray());
                HUNTERPAST_DESCRIPTIONS.Add("Kill the Hunter's Past.".Gray());
            }

            if (GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_BULLET_PAST))
            {
                BULLETPAST_OPTIONS.Add("Vanilla".White());
                BULLETPAST_DESCRIPTIONS.Add("Start with at least 2 blanks per floor.".White());
                BULLETPAST_OPTIONS.Add("Bonus Blank".Cyan());
                BULLETPAST_DESCRIPTIONS.Add("Start with at least 3 blanks per floor.".Cyan());
            }
            else
            {
                BULLETPAST_OPTIONS.Add("Locked".Gray());
                BULLETPAST_DESCRIPTIONS.Add("Kill the Bullet's Past.".Gray());
            }

            if (GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_ROBOT_PAST))
            {
                ROBOTPAST_OPTIONS.Add("Vanilla".White());
                ROBOTPAST_DESCRIPTIONS.Add("Base starting armor.".White());
                ROBOTPAST_OPTIONS.Add("Additional Armor".Blue());
                ROBOTPAST_DESCRIPTIONS.Add("Start with 1 additional point of armor.".Blue());
            }
            else
            {
                ROBOTPAST_OPTIONS.Add("Locked".Gray());
                ROBOTPAST_DESCRIPTIONS.Add("Kill the Robot's Past.".Gray());
            }

            if (GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_DRAGUN_PARADOX))
            {
                PARADOXPAST_OPTIONS.Add("Vanilla".White());
                PARADOXPAST_DESCRIPTIONS.Add("Glitch chest appearance rate of 0.1%".White());
                PARADOXPAST_OPTIONS.Add("Gather Glitches".Magenta());
                PARADOXPAST_DESCRIPTIONS.Add("Glitch chest appearance rate increased to 0.6%".Magenta());
            }
            else
            {
                PARADOXPAST_OPTIONS.Add("Locked".Gray());
                PARADOXPAST_DESCRIPTIONS.Add("Kill the Paradox's Past.".Gray());
            }

            if (GameStatsManager.Instance.GetFlag(GungeonFlags.GUNSLINGER_PAST_KILLED))
            {
                SLINGERPAST_OPTIONS.Add("Vanilla".White());
                SLINGERPAST_DESCRIPTIONS.Add("Start with 0 coolness.".White());
                SLINGERPAST_OPTIONS.Add("Create Coolness".WithColor(Orange));
                SLINGERPAST_DESCRIPTIONS.Add("Start with 2 coolness.".WithColor(Orange));
            }
            else
            {
                SLINGERPAST_OPTIONS.Add("Locked".Gray());
                SLINGERPAST_DESCRIPTIONS.Add("Kill the Slinger's Past.".Gray());
            }

            if (GameStatsManager.Instance.GetFlag(GungeonFlags.COOP_PAST_REACHED))
            {
                COOPCHEST_OPTIONS.Add("Vanilla".White());
                COOPCHEST_DESCRIPTIONS.Add("Only spawn one copy of initial chests.".White());
                COOPCHEST_OPTIONS.Add("The Power of Friendship".WithColor(BrightPink));
                COOPCHEST_DESCRIPTIONS.Add("Spawn a duplicate chest at the start of Co-op Mode.".WithColor(BrightPink));
            }
            else
            {
                COOPCHEST_OPTIONS.Add("Locked".Gray());
                COOPCHEST_DESCRIPTIONS.Add("Complete a Co-op Run.".Gray());
            }

            //Chest Block
            if (GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_DRAGUN))
            {
                STARTCHEST_OPTIONS.Add("Vanilla".White());
                STARTCHEST_DESCRIPTIONS.Add("No starting chest.".White());
                STARTCHEST_OPTIONS.Add("Starter Chest".WithColor(Orange));
                STARTCHEST_DESCRIPTIONS.Add("Start with a D rank chest.".White());

                if (GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_ROGUE_PAST) ||
                    GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_SOLDIER_PAST) ||
                    GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_CONVICT_PAST) ||
                    GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_GUIDE_PAST) ||
                    GameStatsManager.Instance.GetFlag(GungeonFlags.COOP_PAST_REACHED) ||
                    GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_ROBOT_PAST))
                {
                    STARTCHEST_OPTIONS.Add("Evolved Chest".WithColor(CChest));
                    STARTCHEST_DESCRIPTIONS.Add("Start with a C rank chest.".White());

                    if (GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_LICH))
                    {
                        STARTCHEST_OPTIONS.Add("Final Chest".WithColor(BChest));
                        STARTCHEST_DESCRIPTIONS.Add("Start with a B rank chest.".WithColor(BChest));
                    }
                    else
                    {
                        STARTCHEST_OPTIONS.Add("Locked".Gray());
                        STARTCHEST_DESCRIPTIONS.Add("Kill the Lich.".Gray());
                    }
                }
                else
                {
                    STARTCHEST_OPTIONS.Add("Locked".Gray());
                    STARTCHEST_DESCRIPTIONS.Add("Kill any vanilla Past.".Gray());
                }
            }
            else
            {
                STARTCHEST_OPTIONS.Add("Locked".Gray());
                STARTCHEST_DESCRIPTIONS.Add("Kill the Dragun.".Gray());
            }

            if (GameStatsManager.Instance.GetFlag(GungeonFlags.BOWLER_RAINBOW_RUN_COMPLETE))
            {
                MULTI_OPTIONS.Add("Vanilla".White());
                MULTI_DESCRIPTIONS.Add("Your initial chest drops one item.".White());
                MULTI_OPTIONS.Add("Rainbow Blessing".Magenta());
                MULTI_DESCRIPTIONS.Add("Your initial chest has multiple options.".Magenta());
            }
            else
            {
                MULTI_OPTIONS.Add("Locked".Gray());
                MULTI_DESCRIPTIONS.Add("Complete a Rainbow Run.".Gray());
            }

            //Magnificence
            if (GameStatsManager.Instance.GetFlag(GungeonFlags.FRIFLE_MONSTERHUNT_01_COMPLETE))
            {
                MAGNIFICENCE_OPTIONS.Add("Vanilla".White());
                MAGNIFICENCE_DESCRIPTIONS.Add("Chances of rerolling A/S items after the first are 80%/95%/98.6%/99%".White());
                MAGNIFICENCE_OPTIONS.Add("Magnificence I".WithColor(LightBlue));
                MAGNIFICENCE_DESCRIPTIONS.Add("Chances of rerolling after the first are 70%/95%/98.6%/99%".White());

                if (GameStatsManager.Instance.GetFlag(GungeonFlags.FRIFLE_MONSTERHUNT_02_COMPLETE))
                {
                    MAGNIFICENCE_OPTIONS.Add("Magnificence II Dawn of Souls".WithColor(Pink));
                    MAGNIFICENCE_DESCRIPTIONS.Add("Chances of rerolling after the first are 60%/95%/98.6%/99%".White());

                    if (GameStatsManager.Instance.GetFlag(GungeonFlags.FRIFLE_MONSTERHUNT_03_COMPLETE))
                    {
                        MAGNIFICENCE_OPTIONS.Add("Magnificence III 3D".WithColor(Teal));
                        MAGNIFICENCE_DESCRIPTIONS.Add("Chances of rerolling after the first are 60%/80%/98.6%/99%".White());

                        if (GameStatsManager.Instance.GetFlag(GungeonFlags.FRIFLE_MONSTERHUNT_04_COMPLETE))
                        {
                            MAGNIFICENCE_OPTIONS.Add("Magnificence IV The After Years".Blue());
                            MAGNIFICENCE_DESCRIPTIONS.Add("Chances of rerolling after the first are 60%/80%/90%/99%".White());

                            if (GameStatsManager.Instance.GetFlag(GungeonFlags.FRIFLE_MONSTERHUNT_05_COMPLETE))
                            {
                                MAGNIFICENCE_OPTIONS.Add("Magnificence V Advance".WithColor(LightPurple));
                                MAGNIFICENCE_DESCRIPTIONS.Add("Chances of rerolling after the first are 50%/80%/90%/99%".White());

                                if (GameStatsManager.Instance.GetFlag(GungeonFlags.FRIFLE_MONSTERHUNT_06_COMPLETE))
                                {
                                    MAGNIFICENCE_OPTIONS.Add("Magnificence VI Pixel Remaster".Red());
                                    MAGNIFICENCE_DESCRIPTIONS.Add("Chances of rerolling after the first are 50%/70%/90%/99%".White());

                                    if (GameStatsManager.Instance.GetFlag(GungeonFlags.FRIFLE_MONSTERHUNT_07_COMPLETE))
                                    {
                                        MAGNIFICENCE_OPTIONS.Add("Magnificence VII Remake".WithColor(Ammo));
                                        MAGNIFICENCE_DESCRIPTIONS.Add("Chances of rerolling after the first are 50%/70%/80%/99%".White());

                                        if (GameStatsManager.Instance.GetFlag(GungeonFlags.FRIFLE_MONSTERHUNT_08_COMPLETE))
                                        {
                                            MAGNIFICENCE_OPTIONS.Add("Magnificence VIII Remastered".WithColor(RedOrange));
                                            MAGNIFICENCE_DESCRIPTIONS.Add("Chances of rerolling after the first are 50%/70%/80%/90%".White());

                                            if (GameStatsManager.Instance.GetFlag(GungeonFlags.FRIFLE_MONSTERHUNT_09_COMPLETE))
                                            {
                                                MAGNIFICENCE_OPTIONS.Add("Magnificence IX Steam Version".Yellow());
                                                MAGNIFICENCE_DESCRIPTIONS.Add("Chances of rerolling after the first are 40%/70%/80%/90%".White());

                                                if (GameStatsManager.Instance.GetFlag(GungeonFlags.FRIFLE_MONSTERHUNT_10_COMPLETE))
                                                {
                                                    MAGNIFICENCE_OPTIONS.Add("Magnificence X HD".WithColor(BrightBlue));
                                                    MAGNIFICENCE_DESCRIPTIONS.Add("Chances of rerolling after the first are 40%/60%/80%/90%".White());

                                                    if (GameStatsManager.Instance.GetFlag(GungeonFlags.FRIFLE_MONSTERHUNT_11_COMPLETE))
                                                    {
                                                        MAGNIFICENCE_OPTIONS.Add("Magnificence XI Online".WithColor(LightBlue));
                                                        MAGNIFICENCE_DESCRIPTIONS.Add("Chances of rerolling after the first are 40%/60%/70%/90%".White());

                                                        if (GameStatsManager.Instance.GetFlag(GungeonFlags.FRIFLE_MONSTERHUNT_12_COMPLETE))
                                                        {
                                                            MAGNIFICENCE_OPTIONS.Add("Magnificence XII The Zodiac Age".WithColor(PaleOrange));
                                                            MAGNIFICENCE_DESCRIPTIONS.Add("Chances of rerolling after the first are 40%/60%/70%/80%".White());

                                                            if (GameStatsManager.Instance.GetFlag(GungeonFlags.FRIFLE_MONSTERHUNT_13_COMPLETE))
                                                            {
                                                                MAGNIFICENCE_OPTIONS.Add("Magnificence XIII Lightning Returns".WithColor(Flower));
                                                                MAGNIFICENCE_DESCRIPTIONS.Add("Chances of rerolling after the first are 40%/50%/70%/80%".White());

                                                                if (GameStatsManager.Instance.GetFlag(GungeonFlags.FRIFLE_MONSTERHUNT_14_COMPLETE))
                                                                {
                                                                    MAGNIFICENCE_OPTIONS.Add("Magnificence XIV A Realm Reborn".WithColor(BrightLightBlue));
                                                                    MAGNIFICENCE_DESCRIPTIONS.Add("Chances of rerolling after the first are 40%/50%/60%/80%".White());

                                                                    if (GameStatsManager.Instance.GetFlag(GungeonFlags.FRIFLE_CORE_HUNTS_COMPLETE))
                                                                    {
                                                                        MAGNIFICENCE_OPTIONS.Add("Magnificence XV Royal Edition".WithColor(DarkCrimson));
                                                                        MAGNIFICENCE_DESCRIPTIONS.Add("Chances of rerolling after the first are 30%/40%/50%/70%".WithColor(DarkCrimson));
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (!GameStatsManager.Instance.GetFlag(GungeonFlags.FRIFLE_CORE_HUNTS_COMPLETE))
                {
                    MAGNIFICENCE_OPTIONS.Add("Locked".Gray());

                    if (GameStatsManager.Instance.GetFlag(GungeonFlags.FRIFLE_MONSTERHUNT_14_COMPLETE))
                    {
                        MAGNIFICENCE_DESCRIPTIONS.Add("Complete the final hunt.".Gray());
                    }
                    else
                    {
                        MAGNIFICENCE_DESCRIPTIONS.Add("Complete the next hunt.".Gray());
                    }
                }
            }
            else
            {
                MAGNIFICENCE_OPTIONS.Add("Locked".Gray());
                MAGNIFICENCE_DESCRIPTIONS.Add("Complete a hunt.".Gray());
            }

            //NPC Block
            if (GameStatsManager.Instance.GetFlag(GungeonFlags.MONSTERMANUEL_EVER_TALKED))
            {
                SEE_OPTIONS.Add("Vanilla".White());
                SEE_DESCRIPTIONS.Add("Regular enemies have hidden HP.".White());
                SEE_OPTIONS.Add("Stats Studied".Cyan());
                SEE_DESCRIPTIONS.Add("Regular enemies have visible HP bars.".Cyan());
            }
            else
            {
                SEE_OPTIONS.Add("Locked".Gray());
                SEE_DESCRIPTIONS.Add("Find Ser Manuel in the Gungeon.".Gray());
            }

            if (GameStatsManager.Instance.GetFlag(GungeonFlags.DAISUKE_CHALLENGE_COMPLETE))
            {
                CURSE_OPTIONS.Add("Vanilla".White());
                CURSE_DESCRIPTIONS.Add("Natural curse accumulation.".White());
                CURSE_OPTIONS.Add("Curse Cure".Green());
                CURSE_DESCRIPTIONS.Add("Picking up health or armor removes 1 curse point one time per run. (sound plays)".White());
                if (GameStatsManager.Instance.GetFlag(GungeonFlags.DAISUKE_MEGA_CHALLENGE_COMPLETE))
                {
                    CURSE_OPTIONS.Add("Master Misfortune".Yellow());
                    CURSE_DESCRIPTIONS.Add("Picking up health or armor removes 1 curse point two times per run. (sound plays)".Yellow());
                }
                else
                {
                    CURSE_OPTIONS.Add("Locked".Gray());
                    CURSE_DESCRIPTIONS.Add("Complete a Double Challenge Run.".Gray());
                }
            }
            else
            {
                CURSE_OPTIONS.Add("Locked".Gray());
                CURSE_DESCRIPTIONS.Add("Complete a Challenge Run.".Gray());
            }



            if (GameStatsManager.Instance.GetFlag(GungeonFlags.GUNSLING_KING_ACHIEVEMENT_REWARD_GIVEN))
            {
                BOSSHIT_OPTIONS.Add("Vanilla".White());
                BOSSHIT_DESCRIPTIONS.Add("Lose no-damage rewards during boss fights after taking a hit.".White());
                BOSSHIT_OPTIONS.Add("Battle Boast".Red());
                BOSSHIT_DESCRIPTIONS.Add("No-damage boss rewards ignore your first hit. (2 in co-op) \"I meant to do that.\"".Red());
            }
            else
            {
                BOSSHIT_OPTIONS.Add("Locked".Gray());
                BOSSHIT_DESCRIPTIONS.Add("Receive Gunsling King's reward after completing 10 challenges.".Gray());
            }

            if (GameStatsManager.Instance.GetFlag(GungeonFlags.LOST_ADVENTURER_CORE_COMPLETE))
            {
                MAP_OPTIONS.Add("Vanilla".White());
                MAP_DESCRIPTIONS.Add("Maps have a 0.03% chance to spawn as a room pickup reward.".White());
                MAP_OPTIONS.Add("Memorized Maps".Green());
                MAP_DESCRIPTIONS.Add("Maps gain an additional flat 1% chance to spawn per room clear.".Green());
            }
            else
            {
                MAP_OPTIONS.Add("Locked".Gray());
                MAP_DESCRIPTIONS.Add("Help the Lost Adventurer on all five floors.".Gray());
            }

            if (GameStatsManager.Instance.GetFlag(GungeonFlags.WINCHESTER_ACHIEVEMENT_REWARD_GIVEN))
            {
                MONEY_OPTIONS.Add("Vanilla".White());
                MONEY_DESCRIPTIONS.Add("No starting casings.".White());
                MONEY_OPTIONS.Add("Persistent Prizes".Yellow());
                MONEY_DESCRIPTIONS.Add("Start with 15 casings.".Yellow());
            }
            else
            {
                MONEY_OPTIONS.Add("Locked".Gray());
                MONEY_DESCRIPTIONS.Add("Win three of Winchester's games and claim his reward.".Gray());
            }

            if (GameStatsManager.Instance.GetFlag(GungeonFlags.LEDGEGOBLIN_RECEIVED_REWARD))
            {
                SECRET_OPTIONS.Add("Vanilla".White());
                SECRET_DESCRIPTIONS.Add("Secret Rooms cannot be detected using guns with infinite ammo.".White());
                SECRET_OPTIONS.Add("Frustrated Findings".WithColor(Ammo));
                SECRET_DESCRIPTIONS.Add("Infinite ammo guns can crack breakable walls.".WithColor(Ammo));
            }
            else
            {
                SECRET_OPTIONS.Add("Locked".Gray());
                SECRET_DESCRIPTIONS.Add("Receive the reward for tormenting Ledge Goblin.".Gray());
            }

            //Synergy Block
            if (GameStatsManager.Instance.GetFlag(GungeonFlags.SORCERESS_BLESSED_MODE_COMPLETE))
            {
                SYNERGYCHEST_OPTIONS.Add("Vanilla".White());
                SYNERGYCHEST_DESCRIPTIONS.Add("Synergy chests have about a 50% chance of success.".White());
                SYNERGYCHEST_OPTIONS.Add("Blessed Synergy".Red());
                SYNERGYCHEST_DESCRIPTIONS.Add("Synergy chests have an 80% chance of success.".Red());
            }
            else
            {
                SYNERGYCHEST_OPTIONS.Add("Locked".Gray());
                SYNERGYCHEST_DESCRIPTIONS.Add("Complete a Blessed Run.".Gray());
            }

            if (GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_UNLOCK1_COMPLETE))
            {
                SYNERGYFACTOR_OPTIONS.Add("Vanilla".White());
                SYNERGYFACTOR_DESCRIPTIONS.Add("First and second synergies receive boosted find rates.".White());
                SYNERGYFACTOR_OPTIONS.Add("AMPLIFIED".Yellow());
                SYNERGYFACTOR_DESCRIPTIONS.Add("Second synergy rate is raised further.".White());

                if (GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_UNLOCK2_COMPLETE))
                {
                    SYNERGYFACTOR_OPTIONS.Add("Cadence".Red());
                    SYNERGYFACTOR_DESCRIPTIONS.Add("Synergy Factor is doubled along with previous tiers.".White());

                    if (GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_UNLOCK3_COMPLETE))
                    {
                        SYNERGYFACTOR_OPTIONS.Add("SYNCHRONY".WithColor(Teal));
                        SYNERGYFACTOR_DESCRIPTIONS.Add("Third synergy also receives a boosted rate.".White());

                        if (GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_UNLOCK4_COMPLETE))
                        {
                            SYNERGYFACTOR_OPTIONS.Add("Hatsune Miku".Cyan());
                            SYNERGYFACTOR_DESCRIPTIONS.Add("Synergy Factor is tripled along with previous tiers.".Cyan());
                        }
                        else
                        {
                            SYNERGYFACTOR_OPTIONS.Add("Locked".Gray());
                            SYNERGYFACTOR_DESCRIPTIONS.Add("Build the last Elevator Shortcut.".Gray());
                        }
                    }
                    else
                    {
                        SYNERGYFACTOR_OPTIONS.Add("Locked".Gray());
                        SYNERGYFACTOR_DESCRIPTIONS.Add("Build the next Elevator Shortcut.".Gray());
                    }
                }
                else
                {
                    SYNERGYFACTOR_OPTIONS.Add("Locked".Gray());
                    SYNERGYFACTOR_DESCRIPTIONS.Add("Build the next Elevator Shortcut.".Gray());
                }
            }
            else
            {
                SYNERGYFACTOR_OPTIONS.Add("Locked".Gray());
                SYNERGYFACTOR_DESCRIPTIONS.Add("Build an Elevator Shortcut.".Gray());
            }

            if (GameStatsManager.Instance.GetFlag(GungeonFlags.TONIC_TURBO_MODE_COMPLETE))
            {
                SYNERGYFUSE_OPTIONS.Add("Vanilla".White());
                SYNERGYFUSE_DESCRIPTIONS.Add("Natural synergy chests always have a fuse.".White());
                SYNERGYFUSE_OPTIONS.Add("Faster than Fuses".Blue());
                SYNERGYFUSE_DESCRIPTIONS.Add("Synergy chests have a regular fuse rate. Carries over to next unlock tier.".Blue());
                if(GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.META_CURRENCY) >= 500f || GameStatsManager.Instance.GetFlag(GungeonFlags.TONIC_IS_LOADED))
                {
                    SYNERGYFUSE_OPTIONS.Add("Better than Bonus Stages".Green());
                    SYNERGYFUSE_DESCRIPTIONS.Add("Super Tonic is unlocked.".Yellow());
                }
                else
                {
                    SYNERGYFUSE_OPTIONS.Add("Locked".Gray());
                    SYNERGYFUSE_DESCRIPTIONS.Add("Have at least 500 Hegemony and do a double dodge roll (if you feel like it).".Gray());
                }
            }
            else
            {
                SYNERGYFUSE_OPTIONS.Add("Locked".Gray());
                SYNERGYFUSE_DESCRIPTIONS.Add("Complete a Turbo Run.".Gray());
            }

            if (GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_HIGHDRAGUN))
            {
                SYNERGYCHESTSPAWN_OPTIONS.Add("Vanilla".White());
                SYNERGYCHESTSPAWN_DESCRIPTIONS.Add("Synergy chests have a 5% chance to spawn.".White());
                SYNERGYCHESTSPAWN_OPTIONS.Add("Synergized Synergy".Magenta());
                SYNERGYCHESTSPAWN_DESCRIPTIONS.Add("Synergy chests have an 10% chance to spawn.".Magenta());
            }
            else
            {
                SYNERGYCHESTSPAWN_OPTIONS.Add("Locked".Gray());
                SYNERGYCHESTSPAWN_DESCRIPTIONS.Add("Kill the Advanced Dragun.".Gray());
            }


            //Boss Block
            if (GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_BOSSRUSH))
            {
                CAP_OPTIONS.Add("Vanilla".White());
                CAP_DESCRIPTIONS.Add("Normal Boss DPS Cap.".White());
                CAP_OPTIONS.Add("Bosses Busted".Red());
                CAP_DESCRIPTIONS.Add("Boss DPS Cap increased by 25%".Red());
            }
            else
            {
                CAP_OPTIONS.Add("Locked".Gray());
                CAP_DESCRIPTIONS.Add("Complete a Boss Rush.".Gray());
            }
            
            if (GameStatsManager.Instance.GetFlag(GungeonFlags.RESOURCEFUL_RAT_PUNCHOUT_BEATEN))
            {
                RAT_OPTIONS.Add("Vanilla".White());
                RAT_DESCRIPTIONS.Add("The Resourceful Rat will steal unattended items.".White());
                RAT_OPTIONS.Add("RATDIE".WithColor(Ammo));
                RAT_DESCRIPTIONS.Add("The Resourceful Rat will leave your items alone.".WithColor(Ammo));
            }
            else
            {
                RAT_OPTIONS.Add("Locked".Gray());
                RAT_DESCRIPTIONS.Add("Kill the Resourceful Rat.".Gray());
            }
        }


        internal static void Init()
        {
            _Gunfig = Gunfig.Get(modName: "MetaLimits".Cyan());

            BuildOptions();

            _Gunfig.AddScrollBox(key: PILOTPAST_LABEL, options: PILOTPAST_OPTIONS, info: PILOTPAST_DESCRIPTIONS);
            _Gunfig.AddScrollBox(key: MARINEPAST_LABEL, options: MARINEPAST_OPTIONS, info: MARINEPAST_DESCRIPTIONS);
            _Gunfig.AddScrollBox(key: CONVICTPAST_LABEL, options: CONVICTPAST_OPTIONS, info: CONVICTPAST_DESCRIPTIONS);
            _Gunfig.AddScrollBox(key: HUNTERPAST_LABEL, options: HUNTERPAST_OPTIONS, info: HUNTERPAST_DESCRIPTIONS);
            _Gunfig.AddScrollBox(key: BULLETPAST_LABEL, options: BULLETPAST_OPTIONS, info: BULLETPAST_DESCRIPTIONS);
            _Gunfig.AddScrollBox(key: ROBOTPAST_LABEL, options: ROBOTPAST_OPTIONS, info: ROBOTPAST_DESCRIPTIONS);
            _Gunfig.AddScrollBox(key: PARADOXPAST_LABEL, options: PARADOXPAST_OPTIONS, info: PARADOXPAST_DESCRIPTIONS);
            _Gunfig.AddScrollBox(key: SLINGERPAST_LABEL, options: SLINGERPAST_OPTIONS, info: SLINGERPAST_DESCRIPTIONS);
            _Gunfig.AddScrollBox(key: COOPCHEST_LABEL, options: COOPCHEST_OPTIONS, info: COOPCHEST_DESCRIPTIONS);

            _Gunfig.AddScrollBox(key: STARTCHEST_LABEL, options: STARTCHEST_OPTIONS, info: STARTCHEST_DESCRIPTIONS);
            _Gunfig.AddScrollBox(key: MULTI_LABEL, options: MULTI_OPTIONS, info: MULTI_DESCRIPTIONS);

            _Gunfig.AddScrollBox(key: MAGNIFICENCE_LABEL, options: MAGNIFICENCE_OPTIONS, info: MAGNIFICENCE_DESCRIPTIONS);

            _Gunfig.AddScrollBox(key: SEE_LABEL, options: SEE_OPTIONS, info: SEE_DESCRIPTIONS);
            _Gunfig.AddScrollBox(key: CURSE_LABEL, options: CURSE_OPTIONS, info: CURSE_DESCRIPTIONS);
            _Gunfig.AddScrollBox(key: BOSSHIT_LABEL, options: BOSSHIT_OPTIONS, info: BOSSHIT_DESCRIPTIONS);
            _Gunfig.AddScrollBox(key: MAP_LABEL, options: MAP_OPTIONS, info: MAP_DESCRIPTIONS);
            _Gunfig.AddScrollBox(key: MONEY_LABEL, options: MONEY_OPTIONS, info: MONEY_DESCRIPTIONS);
            _Gunfig.AddScrollBox(key: SECRET_LABEL, options: SECRET_OPTIONS, info: SECRET_DESCRIPTIONS);

            _Gunfig.AddScrollBox(key: SYNERGYFACTOR_LABEL, options: SYNERGYFACTOR_OPTIONS, info: SYNERGYFACTOR_DESCRIPTIONS);
            _Gunfig.AddScrollBox(key: SYNERGYCHEST_LABEL, options: SYNERGYCHEST_OPTIONS, info: SYNERGYCHEST_DESCRIPTIONS);
            _Gunfig.AddScrollBox(key: SYNERGYFUSE_LABEL, options: SYNERGYFUSE_OPTIONS, info: SYNERGYFUSE_DESCRIPTIONS);
            _Gunfig.AddScrollBox(key: SYNERGYCHESTSPAWN_LABEL, options: SYNERGYCHESTSPAWN_OPTIONS, info: SYNERGYCHESTSPAWN_DESCRIPTIONS);


            _Gunfig.AddScrollBox(key: CAP_LABEL, options: CAP_OPTIONS, info: CAP_DESCRIPTIONS);
            _Gunfig.AddScrollBox(key: RAT_LABEL, options: RAT_OPTIONS, info: RAT_DESCRIPTIONS);
        }

    }
}