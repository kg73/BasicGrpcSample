﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicGrpcSample.Server.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProtoBuf.Grpc.Server;

namespace BasicGrpcSample.Server
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddGrpc();
			services.AddCodeFirstGrpc();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseMiddleware<CustomMiddleware>();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapGrpcService<GreeterService>();
				endpoints.MapGrpcService<CalculatorService>();
				endpoints.MapGrpcService<OrderService>();
				endpoints.MapGrpcService<SyncOrderService>();

				endpoints.MapGet("/", async context =>
				{
					await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
				});
			});

			// Some middleware
		}
	}

	public class CustomMiddleware
	{
		private readonly RequestDelegate _next;

		public CustomMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext httpContext)
		{
			await _next(httpContext);
		}
	}
}
