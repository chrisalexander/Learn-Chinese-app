using System.Threading.Tasks;
using LanguageModel;
using SimpleInMemorySearch;

namespace LangDB
{
    /// <summary>
    /// Interface for a service to search language entries.
    /// </summary>
    public interface ILanguageSearchService : ISearchService<ILanguageEntry, LanguageEntryId>
    {
        /// <summary>
        /// Index an entire database.
        /// </summary>
        /// <param name="database">The database to index.</param>
        /// <returns>When complete.</returns>
        Task Index(ILanguageDatabase database);
    }
}
