using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigestingDuck.Models
{
    public class UsuarioSaldo : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }
        public Usuario Usuario { get; set; }
        public DateTimeOffset Data { get; set; }
        public double SaldoTreinamento { get; set; }
        public double SaldoReal { get; set; }
    }
}
