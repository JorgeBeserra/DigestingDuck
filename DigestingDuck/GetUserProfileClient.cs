using System;
using System.Threading.Tasks;
using IqOptionApiDotNet;
using Newtonsoft.Json;

namespace DigestingDuck
{
    public class GetUserProfileClient : Runner
    {
        private readonly IqOptionApiDotNetClient IqClientApi;
        private long userId;
        public GetUserProfileClient(IqOptionApiDotNetClient api, long userid)
        {
            IqClientApi = api;
            userId = userid;
        }

        public override async Task Run()
        {
            if (IqClientApi.IsConnected)
            {
                var requestId = Guid.NewGuid().ToString().Replace("-", string.Empty);
                var profile = await IqClientApi.GetUserProfileClientAsync(requestId, userId);
                _logger.Information(JsonConvert.SerializeObject(profile));
            }
        }
    }
}