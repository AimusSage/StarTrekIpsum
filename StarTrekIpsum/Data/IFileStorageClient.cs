using System.Threading.Tasks;

namespace StarTrekIpsum.Data
{
    public interface IBlobStorageClient
    {
        Task<string> GetStarTrekText(StarTrekCaptain captain);
    }
}