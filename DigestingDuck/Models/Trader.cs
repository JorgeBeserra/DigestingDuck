using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigestingDuck.Models
{
    public class Trader : RealmObject
    {
        [PrimaryKey]
        public long UserId { get; set; }
        public DateTimeOffset DataCadastro { get; set; }
        public DateTimeOffset DataAtualizacao { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public string Flag { get; set; }
        public int Win { get; set; }
        public int Loss { get; set; }
        public bool Alvo { get; set; }
        public double RankingValor { get; set; }
        public int RankingPosicao { get; set; }

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

        [Ignored]
        public string FlagImage
        {
            get
            {
                return "../images/flags/" + this.Flag + ".png";
            }
        }
    }
}
