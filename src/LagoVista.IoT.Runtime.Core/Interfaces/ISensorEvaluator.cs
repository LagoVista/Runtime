// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 0d26a0c7b5c2c4a5037c7440f8dd0d69142df04e704d6c8bee0dea560b2ef024
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.IoT.DeviceManagement.Core.Models;
using System.Threading.Tasks;

namespace LagoVista.IoT.Runtime.Core.Interfaces
{
    public interface ISensorEvaluator
    {
        Task EvaluateAsync(Device device);
    }
}
