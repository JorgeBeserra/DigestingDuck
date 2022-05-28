using Realms;
using System.Collections.Generic;

namespace DigestingDuck.Models
{
    public class Ativo : RealmObject
    {
        [PrimaryKey]
        public int _id { get; set; }
        public bool Status { get; set; }
        public string Descricao { get; set; }
        public int Volatilidade { get; set; } // 0 - 1 -2 
        public IList<SinaisSinal> ListOfSinais { get; }
    }
}
