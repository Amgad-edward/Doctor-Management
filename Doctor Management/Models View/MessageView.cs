using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Doctor_Management.Models_View
{
    public class MessageView
    {
        public string Message { get; set; }

        public string From { get; set; }

        public DateTime date { get; set; }

        public bool ISRead { get; set; }
    }
}
