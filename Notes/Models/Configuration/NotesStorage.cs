﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Models.Configuration
{
    public class NotesStorage
    {
        public string AccountName { get; set; }
        public string AccountKey { get; set; }
        public string TableName { get; set; }
    }
}
