using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace DigestingDuck.Models
{
    public class SinaisSinal : RealmObject
    {
        [PrimaryKey]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public bool Status { get; set; }
        public DateTimeOffset DataHora { get; set; }
        public string InstrumentDir { get; set; }
        public string ExpirationType { get; set; }
        public string Resultado { get; set; }
        public double LucroReal { get; set; }
        public double LucroTreinamento { get; set; }

        [Backlink(nameof(Ativo.ListOfSinais))]
        public IQueryable<Ativo> Ativos { get; }

        [Backlink(nameof(SinaisLista.ListOfSinais))]
        public IQueryable<SinaisLista> SinaisListas { get; }

        [Ignored]
        public string StatusImage
        {
            get
            {
                if (Status)
                    return "../Images/check.png";
                return "../Images/uncheck.png";
            }
        }

    }
}
