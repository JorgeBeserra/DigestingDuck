using System;
using System.Threading.Tasks;
using Serilog;

namespace DigestingDuck
{
    public interface IRunner
    {
        Task Run();
    }

    public abstract class Runner : IRunner
    {
        protected readonly ILogger _logger = LogHelper.Log;
        public abstract Task Run();
    }
}