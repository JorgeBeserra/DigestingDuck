using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigestingDuck.Models
{
    public class AtivoCategoria : RealmObject
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Descricao { get; set; }
    }
}
