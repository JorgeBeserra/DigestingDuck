using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigestingDuck.models
{
    public class UsuariosSaldos : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }
        public Usuarios Usuario { get; set; }
        public DateTimeOffset Data { get; set; }
        public double SaldoTreinamento { get; set; }
        public double SaldoReal { get; set; }
    }
}
