using System.Threading.Tasks;

namespace StarTrekIpsum.Server.Ipsum
{
    public interface IStarTrekIpsumGenerator
    {
        Task<string> ParagraphGenerator();
        
        Task<string> ParagraphGenerator(StarTrekCaptain captain);
        
        Task<string> ParagraphGenerator(int sentencesCount, StarTrekCaptain captain);
        
        Task<string[]> MultiParagraphGenerator(int paragraphCount);

        Task<string[]> MultiParagraphGenerator(int paragraphCount, StarTrekCaptain captain);
    }
}
