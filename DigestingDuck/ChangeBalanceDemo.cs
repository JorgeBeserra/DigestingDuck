using System;
using System.Linq;
using System.Threading.Tasks;
using IqOptionApiDotNet;
using IqOptionApiDotNet.Models;

namespace DigestingDuck
{
    public class ChangeBalanceDemo : Runner
    {
        private IqOptionApiDotNetClient IqClientApi;

        public ChangeBalanceDemo(IqOptionApiDotNetClient api)
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
                var demo = profile.Balances.FirstOrDefault(x => x.Type == BalanceType.Practice);
                await IqClientApi.ChangeBalanceAsync(requestId, demo.Id);
            }
        }
    }
}