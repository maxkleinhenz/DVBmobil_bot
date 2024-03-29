﻿using DVB_Bot.Shared.Model;
using System.Collections.Generic;

namespace DVB_Bot.Shared.Results
{
    public class FavoriteStopResult
    {
        public bool IsSuccessful => ResultType == FavoriteStopResultTypes.Ok;

        public FavoriteStopResultTypes ResultType { get; set; }
        public IFavoriteStop FavoriteStop { get; set; }
        public List<IFavoriteStop> AllFavoriteStops { get; set; }
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

