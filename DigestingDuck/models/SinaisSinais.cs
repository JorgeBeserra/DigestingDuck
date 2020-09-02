using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigestingDuck.models
{
    public class SinaisSinais : RealmObject
    {
        [PrimaryKey]
        public int Id { get; set; }
        public int SinaisListasId { get; set; }
        public DateTimeOffset DataHora { get; set; }
        public AtivosStatus Ativo { get; set; }
        public string InstrumentDir { get; set; }
        public string ExpirationType { get; set; }
        public string Resultado { get; set; }
        public double LucroReal { get; set; }
        public double LucroTreinamento { get; set; }

    }
}
