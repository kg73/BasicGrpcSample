﻿using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Threading.Tasks;

namespace BasicGrpcSample.ConsoleClient
{
	class Program
	{
		static async Task Main(string[] args)
		{
			// The port number(5001) must match the port of the gRPC server.
			using var channel = GrpcChannel.ForAddress("http://localhost:5001", new GrpcChannelOptions() 
			{
				Credentials = ChannelCredentials.Insecure
			});
			var client = new Server.Greeter.GreeterClient(channel);
			var reply = await client.SayHelloAsync(
							  new Server.HelloRequest { Name = "GreeterClient" });
			Console.WriteLine("Greeting: " + reply.Message);
			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}
	}
}
