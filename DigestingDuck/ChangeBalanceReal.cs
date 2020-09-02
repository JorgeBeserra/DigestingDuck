using System;
using System.Linq;
using System.Threading.Tasks;
using IqOptionApiDotNet;
using IqOptionApiDotNet.Models;

namespace DigestingDuck
{
    public class ChangeBalanceReal : Runner
    {
        private readonly IqOptionApiDotNetClient IqClientApi;

        public ChangeBalanceReal(IqOptionApiDotNetClient api)
        {
            IqClientApi = api;
        }

        public override async Task Run()
        {
            if (IqClientApi.IsConnected)
            {
                var requestId = Guid.NewGuid().ToString().Replace("-", string.Empty);
                var profile = await IqClientApi.GetProfileAsync(requestId);

                requestId = Guid.NewGuid().ToString().Replace("-", string.Empty);
                var real = profile.Balances.FirstOrDefault(x => x.Type == BalanceType.Real);
                await IqClientApi.ChangeBalanceAsync(requestId, real.Id);
            }
        }
    }
}