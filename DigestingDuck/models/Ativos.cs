using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Realms;

namespace DigestingDuck.models
{
    public class Ativos : RealmObject
    {
        [PrimaryKey]
        public int Id { get; set; }
        public bool Status { get; set; }
        public string Descricao { get; set; }
        public int Volatilidade { get; set; } // 0 - 1 -2 
    }
}
