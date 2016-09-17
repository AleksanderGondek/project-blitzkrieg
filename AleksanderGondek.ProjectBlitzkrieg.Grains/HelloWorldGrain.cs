using System.Threading.Tasks;
using AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces;
using Orleans;

namespace AleksanderGondek.ProjectBlitzkrieg.Grains
{
    public class HelloWorldGrain : Grain, IHelloWorldGrain
    {
        public Task<string> SayHello()
        {
            return Task.FromResult("Hello world!");
        }
    }
}
