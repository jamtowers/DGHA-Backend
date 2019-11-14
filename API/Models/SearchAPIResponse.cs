﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;


#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Styles

namespace API.Models
{
    public class SearchResponse
    {
        public List<Place> results { get; }
        public string nextPageToken { get; set; }

        public SearchResponse()
        {
            results = new List<Place>();
        }
    }
}
