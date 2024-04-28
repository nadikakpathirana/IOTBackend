using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTBackend.Domain.DbEntities.BaseEntities
{
    public class ModelBase
    {
        [Key]
        public Guid Id { get; set; }
    }
}
