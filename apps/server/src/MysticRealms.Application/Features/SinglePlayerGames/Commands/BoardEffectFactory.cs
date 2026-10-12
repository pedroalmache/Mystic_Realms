namespace MysticRealms.Application.Features.SinglePlayerGames.Commands;

public static class BoardEffectFactory
{
    public static IBoardEffect GetEffect(int cardId)
    {
        return cardId switch
        {
            719 => new PredatorInstinctBoardEffect(),
            722 => new NightWatcherBoardEffect(),
            _ => new DefaultBoardEffect()
        };
    }
}

public class DefaultBoardEffect : IBoardEffect
{
    public void React(Dtos.BoardCardRecord boardCard, Dtos.SavedGameDto savedGame, System.Collections.Generic.List<Dtos.GameAction> playActions)
    {
        // Do nothing
    }
}
