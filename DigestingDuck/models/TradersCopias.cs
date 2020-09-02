using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigestingDuck.models
{
    class TradersCopias : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }
        public Traders Trader { get; set; }
        public DateTimeOffset DataHora { get; set; }
        public AtivosStatus Ativo { get; set; }
        public long PosicaoIdOrigem { get; set; }
        public long PosicaoIdNovo { get; set; }
        public string InstrumentDir { get; set; }
        public double AmountEnrolled { get; set; }
        public string ExpirationType { get; set; }
        public string Resultado { get; set; }
        public double LucroReal { get; set; }
        public double LucroTreinamento { get; set; }
    }
}
