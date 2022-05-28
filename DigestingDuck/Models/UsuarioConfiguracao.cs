using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigestingDuck.Models
{
    public class UsuarioConfiguracao : RealmObject
    {
        [PrimaryKey]
        public int Id { get; set; }
        public Usuario Usuario { get; set; }
        public double ValorEntrada { get; set; }
        public string ValorEntradaTipo { get; set; }
        public int SorosNivel { get; set; }
        public string StopWinRegra { get; set; }
        public double StopWin { get; set; }
        public string StopLossRegra { get; set; }
        public double StopLoss { get; set; }  
        public int FiltroTopTraders { get; set; }
        public double ValorMinimo { get; set; }
        public int DelaySinal { get; set; }
        public int PayoyutMinimo { get; set; }
        public double ReduzirRiscoTendencia { get; set; }
        public bool ReenviarOrdemRecusada { get; set; }
        public string HorarioInicio { get; set; }
        public string HorarioFim { get; set; }
        public string Ambiente { get; set; }
        public string MtgRegra { get; set; }
        public double MtgFator { get; set; }
        public int MtgNivel { get; set; }
        public double MtgFixo01 { get; set; }
        public double MtgFixo02 { get; set; }
        public double MtgFixo03 { get; set; }
        public double MtgFixo04 { get; set; }
        public double MtgFixo05 { get; set; }
        public double MtgFixo06 { get; set; }
        public double MtgFixo07 { get; set; }
        public double MtgFixo08 { get; set; }
        public double MtgFixo09 { get; set; }
        public double MtgFixo10 { get; set; }
    }
}
