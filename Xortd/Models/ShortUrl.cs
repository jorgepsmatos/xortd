using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Xortd.Models
{
    public class ShortUrl
    {
        [Required]
        public string Url { get; set; }

        [Key]
        public string Slug { get; set; }
    }
}
