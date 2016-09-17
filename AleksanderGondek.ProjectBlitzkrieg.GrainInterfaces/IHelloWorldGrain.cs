using System.Threading.Tasks;
using Orleans;

namespace AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces
{
	public interface IHelloWorldGrain : IGrainWithGuidKey
    {
        Task<string> SayHello();
    }
}
