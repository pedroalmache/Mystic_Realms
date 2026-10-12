using MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Warrior;
using MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Elementalist;
using MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Assassin;
using MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Druid;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects;

public static class CardEffectFactory
{
    public static ICardEffect GetEffect(int cardId)
    {
        return cardId switch
        {
            // Basic Attack (Rarity 0) — physical classes
            101 or 201 or 301 or 801 or 901 => new BasicAttackEffect(),

            // Basic Attack (Rarity 0) — magic classes
            401 or 501 or 601 or 701 => new BasicMagicAttackEffect(),

            // Basic Defense (Rarity 0) — physical classes
            2 or 102 or 202 or 302 or 802 or 902 => new BasicDefenseEffect(),

            // Basic Defense (Rarity 0) — magic classes
            402 or 502 or 602 or 702 => new BasicMagicDefenseEffect(),

            // Warrior-specific cards
            1 => new Card1Effect(),
            3 => new Card3Effect(),
            7 => new Card7Effect(),
            8 => new Card8Effect(),
            9 => new Card9Effect(),
            10 => new Card10Effect(),
            11 => new Card11Effect(),
            12 => new Card12Effect(),
            13 => new Card13Effect(),
            14 => new Card14Effect(),
            15 => new Card15Effect(),
            16 => new Card16Effect(),


            // Druid-specific cards
            703 => new Card703Effect(),
            704 => new Card704Effect(),
            705 => new Card705Effect(),
            706 => new Card706Effect(),
            707 => new Card707Effect(),
            708 => new Card708Effect(),
            709 => new Card709Effect(),
            710 => new Card710Effect(),
            711 => new Card711Effect(),
            712 => new Card712Effect(),
            713 => new Card713Effect(),
            714 => new Card714Effect(),
            715 => new Card715Effect(),
            716 => new Card716Effect(),
            717 => new Card717Effect(),
            718 => new Card718Effect(),
            719 => new Card719Effect(),
            720 => new Card720Effect(),
            721 => new Card721Effect(),
            722 => new Card722Effect(),
            723 => new Card723Effect(),

            // Elementalist-specific cards
            603 => new Card603Effect(),
            604 => new Card604Effect(),
            605 => new Card605Effect(),
            606 => new Card606Effect(),
            607 => new Card607Effect(),
            608 => new Card608Effect(),
            609 => new Card609Effect(),
            610 => new Card610Effect(),
            611 => new Card611Effect(),
            612 => new Card612Effect(),
            613 => new Card613Effect(),
            614 => new Card614Effect(),
            615 => new Card615Effect(),
            616 => new Card616Effect(),
            617 => new Card617Effect(),
            618 => new Card618Effect(),
            619 => new Card619Effect(),
            // Assassin-specific cards
            103 => new Card103Effect(),

            // Assassin-specific cards
            104 => new Card104Effect(),

            _ => new DefaultCardEffect()
        };
    }
}
