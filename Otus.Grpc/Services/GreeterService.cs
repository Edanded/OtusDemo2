using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Otus.Grpc
{
    public class GreeterService : CarProvider.CarProviderBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<CarReply> GetCar(CarRequest request, Grpc.Core.ServerCallContext context)
        {
            var r=new CarReply();
            var f=r.Documents;
        
            return base.GetCar(request, context);
        }


        // public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        // {
        //     return Task.FromResult(new HelloReply
        //     {
        //         Message = "Hello " + request.Name
        //     });
        // }
    }
}
