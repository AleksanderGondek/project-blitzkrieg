using System;
using AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Brokers;
using AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Contracts;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameStates.Examples.Chess;
using Orleans;
using Orleans.Runtime.Configuration;

namespace AleksanderGondek.ProjectBlitzkrieg.LocalRunners.Orleans
{
    public static class OrleansSilo
    {
        private static OrleansHostWrapper _hostWrapper;
 
        public static void Start()
        {
            // The Orleans silo environment is initialized in its own app domain in order to more
            // closely emulate the distributed situation, when the client and the server cannot
            // pass data via shared memory.
            var hostDomain = AppDomain.CreateDomain("OrleansHost", null, new AppDomainSetup
            {
                AppDomainInitializer = InitSilo,
                AppDomainInitializerArguments = new string[] {},
            });

            var config = ClientConfiguration.LocalhostSilo();
            GrainClient.Initialize(config);

            var friend = GrainClient.GrainFactory.GetGrain<AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.IHelloWorldGrain>(Guid.NewGuid());
            Console.WriteLine("\n\n{0}\n\n", friend.SayHello().Result);


            var brokerTest = GrainClient.GrainFactory.GetGrain<IMctsBroker>(Guid.NewGuid());
            var newGameState = new ChessGameState();
            newGameState.Initialize();
            newGameState.IsValid();

            var request = new ProcessingRequest()
            {
                GameState = newGameState.ToJson()
            };

            Console.WriteLine("\n\n{0}\n\n", brokerTest.GetNextMove(request).Result);

            // TODO: once the previous call returns, the silo is up and running.
            //       This is the place your custom logic, for example calling client logic
            //       or initializing an HTTP front end for accepting incoming requests.

            Console.WriteLine("Orleans Silo is running.\nPress Enter to terminate...");
            Console.ReadLine();

            hostDomain.DoCallBack(ShutdownSilo);
        }

        private static void InitSilo(string[] args)
        {
            _hostWrapper = new OrleansHostWrapper(args);

            if (!_hostWrapper.Run())
            {
                Console.Error.WriteLine("Failed to initialize Orleans silo");
            }
        }

        private static void ShutdownSilo()
        {
            if (_hostWrapper != null)
            {
                _hostWrapper.Dispose();
                GC.SuppressFinalize(_hostWrapper);
            }
        }
    }
}
