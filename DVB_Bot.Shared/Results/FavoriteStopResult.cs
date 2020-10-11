using System.Collections.Generic;
using DVB_Bot.Shared.Model;

namespace DVB_Bot.Shared.Results
{
    public class FavoriteStopResult
    {
        public bool IsSuccessful => ResultType == FavoriteStopResultTypes.Ok;

        public FavoriteStopResultTypes ResultType { get; set; }
        public FavoriteStop FavoriteStop { get; set; }
        public List<Stop> AllFavoriteStops { get; set; }
    }

    public enum FavoriteStopResultTypes
    {
        Ok,
        StopAlreadyAdded,
        StopIsntFavorite,
        StopNotFound,
        TooManyFavoriteStops
    }
}

