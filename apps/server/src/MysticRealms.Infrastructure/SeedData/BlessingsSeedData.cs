using MysticRealms.Domain.Dungeons;

namespace MysticRealms.Infrastructure.SeedData;

public static class BlessingsSeedData
{
    public static object[] Get() =>
    [
        new
        {
            Id = 1,
            Type = RewardType.Health,
            Name = "Vital Bloom",
            Level_1_Description = "Heal 1 HP at the end of every turn.",
            Level_2_Description = "Heal 2 HP at the end of every turn.",
            Level_3_Description = "Heal 3 HP at the end of every turn.",
            Level_4_Description = "Heal 5 HP at the end of every turn."
        },
        new
        {
            Id = 2,
            Type = RewardType.Health,
            Name = "Overflowing Vigor",
            Level_1_Description = "Whenever you heal, gain 2 extra HP.",
            Level_2_Description = "Whenever you heal, gain 3 extra HP.",
            Level_3_Description = "Whenever you heal, gain 4 extra HP.",
            Level_4_Description = "Whenever you heal, gain 5 extra HP."
        },
        new
        {
            Id = 3,
            Type = RewardType.Health,
            Name = "Second Wind",
            Level_1_Description = "When your HP drops below 25% for the first time each combat, heal 10 HP.",
            Level_2_Description = "When your HP drops below 25% for the first time each combat, heal 15 HP.",
            Level_3_Description = "When your HP drops below 30% for the first time each combat, heal 15 HP.",
            Level_4_Description = "When your HP drops below 30% for the first time each combat, heal 20 HP."
        },
        new
        {
            Id = 4,
            Type = RewardType.Health,
            Name = "Sanctified Veil",
            Level_1_Description = "Start each combat with a buffer that prevents the first instance of Physical Damage.",
            Level_2_Description = "Start each combat with a buffer that prevents the first instance of Magic Damage.",
            Level_3_Description = "Start each combat with a buffer that prevents the first instance of HP loss.",
            Level_4_Description = "Start each combat with a buffer that heals instead of taking the first instance of Damage."
        },
        new
        {
            Id = 5,
            Type = RewardType.Health,
            Name = "Nature's Embrace",
            Level_1_Description = "Every time you play 5 cards in a turn, heal 2 HP.",
            Level_2_Description = "Every time you play 4 cards in a turn, heal 2 HP.",
            Level_3_Description = "Every time you play 3 cards in a turn, heal 2 HP.",
            Level_4_Description = "Every time you play 3 cards in a turn, heal 5 HP."
        },
        new
        {
            Id = 6,
            Type = RewardType.Health,
            Name = "Radiant Core",
            Level_1_Description = "Gain 5 max HP when you acquire this blessing.",
            Level_2_Description = "Gain 8 max HP when you acquire this blessing.",
            Level_3_Description = "Gain 12 max HP when you acquire this blessing.",
            Level_4_Description = "Gain 15 max HP when you acquire this blessing."
        },
        new
        {
            Id = 7,
            Type = RewardType.Health,
            Name = "Healing Pulse",
            Level_1_Description = "After defeating an enemy, restore 3 HP.",
            Level_2_Description = "After defeating an enemy, restore 4 HP.",
            Level_3_Description = "After defeating an enemy, restore 5 HP.",
            Level_4_Description = "After defeating an enemy, restore 7 HP."
        },
        new
        {
            Id = 8,
            Type = RewardType.Health,
            Name = "Preserved Essence",
            Level_1_Description = "At the end of each combat, heal 3 HP.",
            Level_2_Description = "At the end of each combat, heal 4 HP.",
            Level_3_Description = "At the end of each combat, heal 6 HP.",
            Level_4_Description = "At the end of each combat, heal 8 HP."
        },
        new
        {
            Id = 9,
            Type = RewardType.Health,
            Name = "Bloodbond Pact",
            Level_1_Description = "Heal by 15% of max HP before a boss room.",
            Level_2_Description = "Heal by 18% of max HP before a boss room.",
            Level_3_Description = "Heal by 20% of max HP before a boss room.",
            Level_4_Description = "Heal by 25% of max HP before a boss room."
        },
        new
        {
            Id = 10,
            Type = RewardType.Health,
            Name = "Blessed Resolve",
            Level_1_Description = "Gain 5 max HP when you acquire this blessing.",
            Level_2_Description = "Gain 8 max HP when you acquire this blessing.",
            Level_3_Description = "Gain 12 max HP when you acquire this blessing.",
            Level_4_Description = "Gain 15 max HP when you acquire this blessing."
        },
        new
        {
            Id = 11,
            Type = RewardType.Offensive,
            Name = "Razor Instinct",
            Level_1_Description = "Your first attack each turn deals +2 damage.",
            Level_2_Description = "Your first attack each turn deals +4 damage.",
            Level_3_Description = "Your first attack each turn deals +6 damage.",
            Level_4_Description = "Your first attack each turn deals +8 damage."
        },
        new
        {
            Id = 12,
            Type = RewardType.Offensive,
            Name = "Crimson Burst",
            Level_1_Description = "Every 5th attack deals double damage.",
            Level_2_Description = "Every 4th attack deals double damage.",
            Level_3_Description = "Every 4th attack deals 150% damage.",
            Level_4_Description = "Every 3rd attack deals 150% damage."
        },
        new
        {
            Id = 13,
            Type = RewardType.Offensive,
            Name = "Combustion Surge",
            Level_1_Description = "First Attack each turn applies 1 Poison.",
            Level_2_Description = "First Attack each turn applies 2 Poison.",
            Level_3_Description = "First Attack each turn applies 3 Poison.",
            Level_4_Description = "First Attack each turn applies 5 Poison."
        },
        new
        {
            Id = 14,
            Type = RewardType.Offensive,
            Name = "Momentum Drive",
            Level_1_Description = "Every 5th turn gain 1 Power.",
            Level_2_Description = "Every 4th turn gain 1 Power.",
            Level_3_Description = "Every 4th turn gain 2 Power.",
            Level_4_Description = "Every 3rd turn gain 2 Power."
        },
        new
        {
            Id = 15,
            Type = RewardType.Offensive,
            Name = "Executioner's Edge",
            Level_1_Description = "Deal 25% more damage to enemies below 25% HP.",
            Level_2_Description = "Deal 30% more damage to enemies below 30% HP.",
            Level_3_Description = "Deal 35% more damage to enemies below 30% HP.",
            Level_4_Description = "Deal 40% more damage to enemies below 35% HP."
        },
        new
        {
            Id = 16,
            Type = RewardType.Offensive,
            Name = "Precision Strike",
            Level_1_Description = "Critical hits deal 25% more damage.",
            Level_2_Description = "Critical hits deal 35% more damage.",
            Level_3_Description = "Critical hits deal 50% more damage.",
            Level_4_Description = "Critical hits deal 75% more damage."
        },
        new
        {
            Id = 17,
            Type = RewardType.Offensive,
            Name = "Relentless Assault",
            Level_1_Description = "Whenever you kill an enemy, gain +2 Power next turn.",
            Level_2_Description = "Whenever you kill an enemy, gain +3 Power next turn.",
            Level_3_Description = "Whenever you kill an enemy, gain +4 Power next turn.",
            Level_4_Description = "Whenever you kill an enemy, gain +5 Power next turn."
        },
        new
        {
            Id = 18,
            Type = RewardType.Offensive,
            Name = "Arcane Penetration",
            Level_1_Description = "Attacks ignore 2 Block.",
            Level_2_Description = "Attacks ignore 4 Block.",
            Level_3_Description = "Attacks ignore 6 Block.",
            Level_4_Description = "Attacks ignore all Block."
        },
        new
        {
            Id = 19,
            Type = RewardType.Offensive,
            Name = "Blood Price",
            Level_1_Description = "Whenever you lose HP, deal 1 damage back.",
            Level_2_Description = "Whenever you lose HP, deal 2 damage back.",
            Level_3_Description = "Whenever you lose HP, deal 3 damage back.",
            Level_4_Description = "Whenever you lose HP, deal 3 damage back to all enemies."
        },
        new
        {
            Id = 20,
            Type = RewardType.Offensive,
            Name = "Storm Flurry",
            Level_1_Description = "Every 7th attack this combat hits all enemies.",
            Level_2_Description = "Every 6th attack this combat hits all enemies.",
            Level_3_Description = "Every 5th attack this combat hits all enemies.",
            Level_4_Description = "Every 4th attack this combat hits all enemies."
        },
        new
        {
            Id = 21,
            Type = RewardType.Defensive,
            Name = "Ironhide",
            Level_1_Description = "Start each Combat with 4 Physical Defense.",
            Level_2_Description = "Start each Combat with 6 Physical Defense.",
            Level_3_Description = "Start each Combat with 8 Physical Defense.",
            Level_4_Description = "Start each Combat with 14 Physical Defense."
        },
        new
        {
            Id = 22,
            Type = RewardType.Defensive,
            Name = "Mystic Shroud",
            Level_1_Description = "Start each Combat with 4 Magic Defense.",
            Level_2_Description = "Start each Combat with 6 Magic Defense.",
            Level_3_Description = "Start each Combat with 8 Magic Defense.",
            Level_4_Description = "Start each Combat with 14 Magic Defense."
        },
        new
        {
            Id = 23,
            Type = RewardType.Defensive,
            Name = "Stoneform",
            Level_1_Description = "Gain +1 Armor.",
            Level_2_Description = "Gain +2 Armor.",
            Level_3_Description = "Gain +3 Armor.",
            Level_4_Description = "Gain +4 Armor."
        },
        new
        {
            Id = 24,
            Type = RewardType.Defensive,
            Name = "Reactive Plating",
            Level_1_Description = "When you take Physical Damage, gain +2 Physical Defense next turn.",
            Level_2_Description = "When you take Physical Damage, gain +3 Physical Defense next turn.",
            Level_3_Description = "When you take any damage, gain +3 Physical Defense next turn.",
            Level_4_Description = "When you take any damage, gain +4 Physical Defense next turn."
        },
        new
        {
            Id = 25,
            Type = RewardType.Defensive,
            Name = "Warding Aura",
            Level_1_Description = "When you take Magic Damage, gain +2 Magic Defense next turn.",
            Level_2_Description = "When you take Magic Damage, gain +3 Magic Defense next turn.",
            Level_3_Description = "When you take Magic Damage, gain +3 Magic Defense and +1 Armor next turn.",
            Level_4_Description = "When you take Magic Damage, gain +4 Magic Defense and +2 Armor next turn."
        },
        new
        {
            Id = 26,
            Type = RewardType.Defensive,
            Name = "Bulwark's Embrace",
            Level_1_Description = "Start combat with +2 Physical Defense for your first turn.",
            Level_2_Description = "Start combat with +3 Physical Defense and +1 Armor.",
            Level_3_Description = "Start combat with +4 Physical Defense and +1 Armor.",
            Level_4_Description = "Start combat with +5 Physical Defense and +2 Armor."
        },
        new
        {
            Id = 27,
            Type = RewardType.Defensive,
            Name = "Arcane Shell",
            Level_1_Description = "Start combat with +2 Magic Defense for your first turn.",
            Level_2_Description = "Start combat with +3 Magic Defense and +1 Armor.",
            Level_3_Description = "Start combat with +4 Magic Defense and +1 Armor.",
            Level_4_Description = "Start combat with +5 Magic Defense and +2 Armor."
        },
        new
        {
            Id = 28,
            Type = RewardType.Defensive,
            Name = "Guardian's Resolve",
            Level_1_Description = "If your HP is below 50%, gain +1 Armor until end of combat.",
            Level_2_Description = "If your HP is below 50%, gain +2 Armor until end of combat.",
            Level_3_Description = "If your HP is below 40%, gain +3 Armor until end of combat.",
            Level_4_Description = "If your HP is below 40%, gain +4 Armor until end of combat."
        },
        new
        {
            Id = 29,
            Type = RewardType.Defensive,
            Name = "Aegis Flow",
            Level_1_Description = "When you gain Physical Defense, gain +1 Magic Defense (1 turn).",
            Level_2_Description = "When you gain Physical Defense, gain +1 Magic Defense and +1 Armor.",
            Level_3_Description = "When you gain Physical Defense, gain +2 Magic Defense and +1 Armor.",
            Level_4_Description = "When you gain Physical Defense, gain +2 Magic Defense and +2 Armor."
        },
        new
        {
            Id = 30,
            Type = RewardType.Defensive,
            Name = "Tempered Fortitude",
            Level_1_Description = "Blocking 5+ damage in a turn grants +1 Armor (rest of combat).",
            Level_2_Description = "Blocking 4+ damage in a turn grants +1 Armor (rest of combat).",
            Level_3_Description = "Blocking 4+ damage in a turn grants +2 Armor (rest of combat).",
            Level_4_Description = "Blocking 3+ damage in a turn grants +2 Armor (rest of combat)."
        },
        new
        {
            Id = 31,
            Type = RewardType.Mana,
            Name = "Mana Spring",
            Level_1_Description = "Gain +1 mana every 3rd turn.",
            Level_2_Description = "Gain +1 mana every 2nd turn.",
            Level_3_Description = "Gain +1 mana every turn.",
            Level_4_Description = "Gain +1 mana every turn and +1 on the first turn."
        },
        new
        {
            Id = 32,
            Type = RewardType.Mana,
            Name = "Efficient Flow",
            Level_1_Description = "The first card you play each turn costs 1 less mana.",
            Level_2_Description = "The first two cards you play each turn cost 1 less mana.",
            Level_3_Description = "The first card you play each turn costs 2 less mana.",
            Level_4_Description = "The first two cards you play each turn cost 2 less mana."
        },
        new
        {
            Id = 33,
            Type = RewardType.Mana,
            Name = "Arcane Overflow",
            Level_1_Description = "Unspent mana at the end of your turn carries over 1 point.",
            Level_2_Description = "Unspent mana carries over 2 points.",
            Level_3_Description = "Unspent mana carries over 3 points.",
            Level_4_Description = "All unspent mana carries over to the next turn."
        },
        new
        {
            Id = 34,
            Type = RewardType.Mana,
            Name = "Mana Infusion",
            Level_1_Description = "Restore 1 mana whenever you play 4 Common cards in a turn.",
            Level_2_Description = "Restore 1 mana after playing 3 Common cards in a turn.",
            Level_3_Description = "Restore 1 mana after playing 2 Common cards in a turn.",
            Level_4_Description = "Restore 1 mana after playing any 3 cards."
        },
        new
        {
            Id = 35,
            Type = RewardType.Mana,
            Name = "Surge Catalyst",
            Level_1_Description = "Gain 1 extra mana on the first turn of combat.",
            Level_2_Description = "Gain 2 mana on the first turn of combat.",
            Level_3_Description = "Gain 1 extra mana for the first 2 turns of combat.",
            Level_4_Description = "Gain 2 extra mana for the first 2 turns of combat."
        },
        new
        {
            Id = 36,
            Type = RewardType.Mana,
            Name = "Mystic Channeling",
            Level_1_Description = "Whenever you draw a card, 10% chance to gain 1 mana.",
            Level_2_Description = "15% chance to gain 1 mana when drawing a card.",
            Level_3_Description = "20% chance to gain 1 mana when drawing a card.",
            Level_4_Description = "25% chance to gain 1 mana when drawing a card."
        },
        new
        {
            Id = 37,
            Type = RewardType.Mana,
            Name = "Arcane Meditation",
            Level_1_Description = "If you play 0 cards this turn, gain 1 mana next turn.",
            Level_2_Description = "If you play 1 or fewer cards, gain 1 mana next turn.",
            Level_3_Description = "If you play 1 or fewer cards, gain 2 mana next turn.",
            Level_4_Description = "If you play 1 or fewer cards, gain 3 mana next turn."
        },
        new
        {
            Id = 38,
            Type = RewardType.Mana,
            Name = "Resonant Core",
            Level_1_Description = "Start each combat with +1 max mana.",
            Level_2_Description = "Start each combat with +2 max mana.",
            Level_3_Description = "Start each combat with +3 max mana.",
            Level_4_Description = "Start each combat with +4 max mana."
        },
        new
        {
            Id = 39,
            Type = RewardType.Mana,
            Name = "Mana Armor",
            Level_1_Description = "Gain 1 Armor when you gain mana.",
            Level_2_Description = "Gain 2 Armor when you gain mana.",
            Level_3_Description = "Gain 2 Armor and +1 Physical Defense (1 turn) when you gain mana.",
            Level_4_Description = "Gain 3 Armor and +1 Physical & Magic Defense (1 turn) when you gain mana."
        },
        new
        {
            Id = 40,
            Type = RewardType.Mana,
            Name = "Spell Eater",
            Level_1_Description = "Whenever you play 3 cards in a turn, gain 1 mana.",
            Level_2_Description = "Whenever you play 2 cards in a turn, gain 1 mana.",
            Level_3_Description = "Whenever you play 2 cards in a turn, gain 2 mana.",
            Level_4_Description = "Whenever you play 2 cards in a turn, gain 2 mana and draw a card."
        },
        new
        {
            Id = 41,
            Type = RewardType.Speed,
            Name = "Quickstep",
            Level_1_Description = "Gain +2 Initiative at the start of each combat.",
            Level_2_Description = "Gain +3 Initiative at the start of each combat.",
            Level_3_Description = "Gain +5 Initiative at the start of each combat.",
            Level_4_Description = "Gain +7 Initiative at the start of each combat."
        },
        new
        {
            Id = 42,
            Type = RewardType.Speed,
            Name = "Lightning Focus",
            Level_1_Description = "If you play 3 or more cards in a turn, gain +1 Initiative next turn.",
            Level_2_Description = "If you play 3 or more cards, gain +2 Initiative next turn.",
            Level_3_Description = "If you play 2 or more cards, gain +2 Initiative next turn.",
            Level_4_Description = "If you play 2 or more cards, gain +3 Initiative next turn."
        },
        new
        {
            Id = 43,
            Type = RewardType.Speed,
            Name = "Tempo Surge",
            Level_1_Description = "Every 4th card played in a turn gives +1 Initiative next turn.",
            Level_2_Description = "Every 3rd card gives +1 Initiative next turn.",
            Level_3_Description = "Every 2nd card gives +1 Initiative next turn.",
            Level_4_Description = "Every 2nd card gives +2 Initiative next turn."
        },
        new
        {
            Id = 44,
            Type = RewardType.Speed,
            Name = "Reflexive Rhythm",
            Level_1_Description = "Start turn with +1 Initiative if your hand has 6 or more cards.",
            Level_2_Description = "Start turn with +2 Initiative if your hand has 6 or more cards.",
            Level_3_Description = "Start turn with +2 Initiative if your hand has 5 or more cards.",
            Level_4_Description = "Start turn with +3 Initiative if your hand has 5 or more cards."
        },
        new
        {
            Id = 45,
            Type = RewardType.Speed,
            Name = "Blurred Motion",
            Level_1_Description = "At the start of each combat, gain +2 Initiative and draw 1 extra card.",
            Level_2_Description = "Gain +3 Initiative and draw 1 extra card.",
            Level_3_Description = "Gain +4 Initiative and draw 2 extra cards.",
            Level_4_Description = "Gain +5 Initiative and draw 2 extra cards."
        },
        new
        {
            Id = 46,
            Type = RewardType.Speed,
            Name = "Fleet Edge",
            Level_1_Description = "When you play the last card in your hand, gain +1 Initiative next turn.",
            Level_2_Description = "Gain +2 Initiative next turn.",
            Level_3_Description = "Also draw 1 card.",
            Level_4_Description = "Gain +2 Initiative and draw 1 card."
        },
        new
        {
            Id = 47,
            Type = RewardType.Speed,
            Name = "Slipstream",
            Level_1_Description = "At the end of each turn, gain +1 Initiative if you played 4 cards.",
            Level_2_Description = "Gain +1 Initiative for 3+ cards.",
            Level_3_Description = "Gain +2 Initiative for 3+ cards.",
            Level_4_Description = "Gain +2 Initiative and draw 1 card for 3+ cards played."
        },
        new
        {
            Id = 48,
            Type = RewardType.Speed,
            Name = "Fast Hands",
            Level_1_Description = "Your first card played each turn costs 1 less if you have higher Initiative.",
            Level_2_Description = "First two cards cost 1 less if you have higher Initiative.",
            Level_3_Description = "First card costs 2 less if you have higher Initiative.",
            Level_4_Description = "First two cards cost 2 less if you have higher Initiative."
        },
        new
        {
            Id = 49,
            Type = RewardType.Speed,
            Name = "Momentum Charge",
            Level_1_Description = "Gain +1 Initiative the first time you play 2 cards in a single turn.",
            Level_2_Description = "Gain +1 Initiative and draw 1 card.",
            Level_3_Description = "Gain +2 Initiative and draw 1 card.",
            Level_4_Description = "Gain +2 Initiative and draw 2 cards."
        },
        new
        {
            Id = 50,
            Type = RewardType.Speed,
            Name = "Accelerated Will",
            Level_1_Description = "Whenever your Initiative increases, gain 1 Armor (once per turn).",
            Level_2_Description = "Gain 2 Armor instead.",
            Level_3_Description = "Gain 2 Armor and +1 Physical Defense (1 turn).",
            Level_4_Description = "Gain 3 Armor and +1 Physical & Magic Defense (1 turn)."
        },
        new
        {
            Id = 51,
            Type = RewardType.Gold,
            Name = "Greedy Heart",
            Level_1_Description = "Gain 10% more gold from all encounters.",
            Level_2_Description = "Gain 15% more gold from all encounters.",
            Level_3_Description = "Gain 20% more gold from all encounters.",
            Level_4_Description = "Gain 25% more gold from all encounters."
        },
        new
        {
            Id = 52,
            Type = RewardType.Gold,
            Name = "Treasure Seeker",
            Level_1_Description = "Gain 10% more gold from treasure rooms.",
            Level_2_Description = "Gain 20% more gold from treasure rooms.",
            Level_3_Description = "Gain 30% more gold from treasure rooms.",
            Level_4_Description = "Gain 50% more gold from treasure rooms."
        },
        new
        {
            Id = 53,
            Type = RewardType.Gold,
            Name = "Elite Tax",
            Level_1_Description = "Gain 15 extra gold from elite enemies.",
            Level_2_Description = "Gain 25 extra gold from elite enemies.",
            Level_3_Description = "Gain 40 extra gold from elite enemies.",
            Level_4_Description = "Gain 60 extra gold from elite enemies."
        },
        new
        {
            Id = 54,
            Type = RewardType.Gold,
            Name = "Boss Tribute",
            Level_1_Description = "Gain 25 extra gold from bosses.",
            Level_2_Description = "Gain 50 extra gold from bosses.",
            Level_3_Description = "Gain 75 extra gold from bosses.",
            Level_4_Description = "Gain 100 extra gold from bosses."
        },
        new
        {
            Id = 55,
            Type = RewardType.Gold,
            Name = "Golden Opportunity",
            Level_1_Description = "At the start of each combat, gain 5 gold.",
            Level_2_Description = "Gain 8 gold at the start of combat.",
            Level_3_Description = "Gain 12 gold at the start of combat.",
            Level_4_Description = "Gain 15 gold at the start of combat."
        },
        new
        {
            Id = 56,
            Type = RewardType.Gold,
            Name = "Merchant's Favor",
            Level_1_Description = "Store prices are reduced by 5%.",
            Level_2_Description = "Store prices are reduced by 10%.",
            Level_3_Description = "Store prices are reduced by 15%.",
            Level_4_Description = "Store prices are reduced by 20%."
        },
        new
        {
            Id = 57,
            Type = RewardType.Gold,
            Name = "Refund Ritual",
            Level_1_Description = "Gain 5 gold back after each purchase.",
            Level_2_Description = "Gain 10 gold back after each purchase.",
            Level_3_Description = "Gain 15 gold back after each purchase.",
            Level_4_Description = "Gain 20 gold back after each purchase."
        },
        new
        {
            Id = 58,
            Type = RewardType.Gold,
            Name = "Golden Idol",
            Level_1_Description = "Gain 100 Gold.",
            Level_2_Description = "Gain 125 Gold.",
            Level_3_Description = "Gain 150 Gold.",
            Level_4_Description = "Gain 300 Gold."
        },
        new
        {
            Id = 59,
            Type = RewardType.Gold,
            Name = "Glittering Strikes",
            Level_1_Description = "Each time you win a combat without taking damage, gain 10 bonus gold.",
            Level_2_Description = "Gain 15 bonus gold for flawless victories.",
            Level_3_Description = "Gain 20 gold for flawless victories.",
            Level_4_Description = "Gain 25 gold and +1 Initiative for flawless victories."
        },
        new
        {
            Id = 60,
            Type = RewardType.Gold,
            Name = "Investment Plan",
            Level_1_Description = "Every 3 rooms, gain interest: +10% of your current gold.",
            Level_2_Description = "Every 2 rooms, gain +10% gold interest.",
            Level_3_Description = "Every 2 rooms, gain +15% interest.",
            Level_4_Description = "Every rooms, gain +15% gold interest."
        },
        new
        {
            Id = 61,
            Type = RewardType.Misc,
            Name = "Wanderer's Luck",
            Level_1_Description = "10% chance after combat to receive a random bonus reward.",
            Level_2_Description = "15% chance after combat to receive a random bonus reward.",
            Level_3_Description = "20% chance after combat to receive a random bonus reward.",
            Level_4_Description = "25% chance after combat to receive a random bonus reward."
        },
        new
        {
            Id = 62,
            Type = RewardType.Misc,
            Name = "Explorer's Curiosity",
            Level_1_Description = "When choosing cards after combat, choose from 4 options instead of 3.",
            Level_2_Description = "Choose from 5 options instead of 3.",
            Level_3_Description = "Choose from 5 options and reroll once per map.",
            Level_4_Description = "Choose from 6 options and reroll once per combat."
        },
        new
        {
            Id = 63,
            Type = RewardType.Misc,
            Name = "Unstable Artifact",
            Level_1_Description = "Gain 1 random temporary blessing at the start of each combat.",
            Level_2_Description = "Gain 2 random temporary blessings.",
            Level_3_Description = "Gain 2 random temporary blessings (can repeat).",
            Level_4_Description = "Gain 3 random temporary blessings."
        },
        new
        {
            Id = 64,
            Type = RewardType.Misc,
            Name = "Whispers of the Unknown",
            Level_1_Description = "10% chance to discover a hidden room when exploring.",
            Level_2_Description = "15% chance to discover a hidden room.",
            Level_3_Description = "20% chance to discover a hidden room and gain a random reward.",
            Level_4_Description = "25% chance to discover a hidden room with double rewards."
        },
        new
        {
            Id = 65,
            Type = RewardType.Misc,
            Name = "Deckbuilder's Delight",
            Level_1_Description = "After every 3 combats, gain 1 bonus card from a random rarity.",
            Level_2_Description = "After every 3 combats, gain 2 bonus cards (random rarity).",
            Level_3_Description = "After every 3 combats, gain 2 bonus cards and 1 random blessing.",
            Level_4_Description = "After every 3 combats, gain 3 cards and 1 random upgraded blessing."
        },
        new
        {
            Id = 66,
            Type = RewardType.Misc,
            Name = "Rushed Planning",
            Level_1_Description = "Draw +1 extra card on your first turn each combat.",
            Level_2_Description = "Draw +2 extra cards on your first turn each combat.",
            Level_3_Description = "Draw +1 extra card on your first two turns each combat.",
            Level_4_Description = "Draw +2 extra cards on your first two turns each combat."
        },
        new
        {
            Id = 67,
            Type = RewardType.Misc,
            Name = "Fate Twister",
            Level_1_Description = "Once per map, reroll your post-combat rewards.",
            Level_2_Description = "Twice per map, reroll post-combat rewards.",
            Level_3_Description = "Once per combat, reroll card choices.",
            Level_4_Description = "Once per combat, reroll card and gold rewards."
        },
        new
        {
            Id = 68,
            Type = RewardType.Misc,
            Name = "Traveler's Pack",
            Level_1_Description = "After every 3 combats, gain 1 random card and 10 gold.",
            Level_2_Description = "Gain 1 card and 20 gold.",
            Level_3_Description = "Gain 1 card, 30 gold, and 1 random item (if implemented).",
            Level_4_Description = "Gain 2 cards, 50 gold, and 1 random item (if implemented)."
        },
        new
        {
            Id = 69,
            Type = RewardType.Misc,
            Name = "Deal with Destiny",
            Level_1_Description = "When entering a new map region, choose from 2 random blessings.",
            Level_2_Description = "Choose from 3 random blessings.",
            Level_3_Description = "Choose from 3 and keep 1 permanent and 1 temporary.",
            Level_4_Description = "Choose 2 permanent blessings from 4 options."
        },
        new
        {
            Id = 70,
            Type = RewardType.Misc,
            Name = "Vision of Victory",
            Level_1_Description = "Each combat, the first time you draw 5 or more cards in a turn, gain +1 mana.",
            Level_2_Description = "Gain +1 mana and draw 1 card.",
            Level_3_Description = "Gain +1 mana and draw 2 cards.",
            Level_4_Description = "Gain +2 mana and draw 2 cards."
        }
    ];
}
