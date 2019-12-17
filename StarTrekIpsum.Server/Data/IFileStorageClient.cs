using System.Threading.Tasks;

namespace StarTrekIpsum.Server.Data
{
    public interface IBlobStorageClient
    {
        Task<string> GetStarTrekText(StarTrekCaptain captain);
    }
}