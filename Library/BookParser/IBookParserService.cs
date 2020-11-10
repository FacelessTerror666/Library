using System.Threading.Tasks;

namespace Library.Parser
{
    public interface IBookParserService
    {
        Task Pars(int parsCount);
    }
}
