using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigestingDuck.models
{
    public class AtivosStatus : RealmObject
    {
        [PrimaryKey]
        public int Id { get; set; }
        public AtivosCategorias AtivoCategoria { get; set; }
        public Ativos Ativo { get; set; }
        public string Descricao { get; set; }
        public bool Aberto { get; set; }
        public bool Alvo { get; set; }
        public double Payout { get; set; }

        [Ignored]
        public string AbertoImage
        {
            get
            {
                if (Aberto)
                    return "../images/check.png";
                return "../images/uncheck.png";
            }
        }

        [Ignored]
        public string AlvoImage
        {
            get
            {
                if (Alvo)
                    return "../images/check.png";
                return "../images/uncheck.png";
            }
        }
    }
}
