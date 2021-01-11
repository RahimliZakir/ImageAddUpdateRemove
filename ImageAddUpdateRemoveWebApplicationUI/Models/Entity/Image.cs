using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImageAddUpdateRemoveWebApplicationUI.Models.Entity
{
    public class Image : BaseEntity
    {
        public string ImagePath { get; set; }

        [NotMapped]
        public string FileTemp { get; set; }
    }
}
