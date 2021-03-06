﻿using System.Collections.Generic;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CA2227 // Collection properties should be read only
#pragma warning disable CA1034 // Nested types should not be visible
#pragma warning disable CA1052 // Static holder types should be Static or NotInheritable
#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable IDE1006 // Naming Styles

namespace API.Models
{
    public class Coords
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Viewport
    {
        public Coords northeast { get; set; }
        public Coords southwest { get; set; }
    }

    public class Geometry
    {
        public Coords location { get; set; }
        public Viewport viewport { get; set; }
    }

    public class OpeningHours
    {
        public bool open_now { get; set; }
    }

    public class Photo
    {
        public int height { get; set; }
        public List<string> html_attributions { get; set; }
        public string photo_reference { get; set; }
        public int width { get; set; }
    }

    public class PlusCode
    {
        public string compound_code { get; set; }
        public string global_code { get; set; }
    }

    public class SearchPlaceResult
    {
        public string formatted_address { get; set; }
        public Geometry geometry { get; set; }
        public string icon { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public OpeningHours opening_hours { get; set; }
        public List<Photo> photos { get; set; }
        public string place_id { get; set; }
        public PlusCode plus_code { get; set; }
        public int price_level { get; set; }
        public double rating { get; set; }
        public string reference { get; set; }
        public List<string> types { get; set; }
        public int user_ratings_total { get; set; }
    }

    public class PlaceAPIQueryResponse
    {
        public List<object> html_attributions { get; set; }
        public string next_page_token { get; set; }
        public List<SearchPlaceResult> results { get; set; }
        public string status { get; set; }
    }
}
