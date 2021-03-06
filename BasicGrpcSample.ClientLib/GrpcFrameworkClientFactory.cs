﻿#if NET48
using System;
using ProtoBuf.Grpc.Client;
using Grpc.Core;

namespace BasicGrpcSample.ClientLib
{
    public class GrpcFrameworkClientFactory : IGrpcClientFactory, IDisposable
    {
        private readonly Channel _channel;

        public GrpcFrameworkClientFactory(string grpcServerAddress)
        {
            _channel = new Channel(
                grpcServerAddress,
                ChannelCredentials.Insecure
                );
        }

        public T GetClient<T>() where T : class
        {
            var isAutogenerated = typeof(T).IsSubclassOf(typeof(Grpc.Core.ClientBase));
            if (isAutogenerated)
            {
                return (T)Activator.CreateInstance(typeof(T), _channel);
            }

            var svc = _channel.CreateGrpcService<T>();
            return svc;
        }

        public void Dispose()
        {
            _channel.ShutdownAsync().Wait();
        }
    }
}
#endif
