using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImageAddUpdateRemoveWebApplicationUI.Models.Entity
{
    public class BaseEntity
    {
        public int Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }

#nullable enable

        public DateTime? UpdatedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

#nullable disable
    }
}
