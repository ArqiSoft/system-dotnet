using MassTransit;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Sds.MassTransit.Observers
{
    public class BusObserver : IBusObserver
    {
        public async Task CreateFaulted(Exception exception)
        {
            Log.Error($"[BusObserver] Bus exception: {exception.Message}");
            await Task.CompletedTask;
        }

        public async Task PostCreate(IBus bus)
        {
            Log.Information($"[BusObserver] Bus has been created with address {bus.Address}");
            await Task.CompletedTask;
        }

        public async Task PostStart(IBus bus, Task<BusReady> busReady)
        {
            Log.Information($"[BusObserver] Bus has been started with address {bus.Address}");
            await busReady;
        }

        public async Task PostStop(IBus bus)
        {
            Log.Information($"[BusObserver] Bus has been stopped with address {bus.Address}");
            await Task.CompletedTask;
        }

        public async Task PreStart(IBus bus)
        {
            Log.Information($"[BusObserver] Bus is about to start with address {bus.Address}");
            await Task.CompletedTask;
        }

        public async Task PreStop(IBus bus)
        {
            Log.Information($"[BusObserver] Bus is about to stop with address {bus.Address}");
            await Task.CompletedTask;
        }

        public async Task StartFaulted(IBus bus, Exception exception)
        {
            Log.Error($"[BusObserver] Bus exception at start-up: {exception.Message} for bus {bus.Address}");
            await Task.CompletedTask;
        }

        public async Task StopFaulted(IBus bus, Exception exception)
        {
            Log.Error($"[BusObserver] Bus exception at shut-down: {exception.Message} for bus {bus.Address}");
            await Task.CompletedTask;
        }
    }
}
