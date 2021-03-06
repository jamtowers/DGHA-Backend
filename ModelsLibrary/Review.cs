﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModelsLibrary
{
    public class Review
    {
        public string UserID { get; set; }
        public string PlaceID { get; set; }

        public DateTime TimeAdded { get; set; }
        public byte OverallRating { get; set; }
        public byte LocationRating { get; set; }
        public byte AmentitiesRating { get; set; }
        public byte ServiceRating { get; set; }
        public string Comment { get; set; }
    }
}
