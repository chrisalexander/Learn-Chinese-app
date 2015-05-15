using System.Linq;
using LanguageModel;
using Machine.Specifications;

namespace LangDB.Tests.LanguageSearchService
{
    /// <summary>
    /// It should be said this is more of a high-level functional
    /// test of the search index, the low level tests should be
    /// taken care of in the Simple In Memory Search test project.
    /// </summary>
    public class When_searching_language_with_entry_indexed
    {
        Establish Context = () =>
        {
            Service = new LangDB.LanguageSearchService();
            Entry = new LanguageEntry("柴門", "柴门", "chai2 men2", new[] { "lit. woodcutter's family", "humble background", "poor family background" });
        };

        Because Of = () => Service.Index(Entry);

        It Should_match_when_searching_english = () => Service.Search("wood").Result.ToList()[0].Key.ShouldEqual(Entry.Id);

        It Should_get_relevancy_when_searching_english = () => Service.Search("wood").Result.ToList()[0].Relevancy.ShouldEqual(0);

        It Should_match_when_searching_traditional = () => Service.Search("柴門").Result.ToList()[0].Key.ShouldEqual(Entry.Id);

        It Should_get_relevancy_when_searching_traditional = () => Service.Search("柴門").Result.ToList()[0].Relevancy.ShouldEqual(1);

        It Should_match_when_searching_simplified = () => Service.Search("柴门").Result.ToList()[0].Key.ShouldEqual(Entry.Id);

        It Should_get_relevancy_when_searching_simplified = () => Service.Search("柴门").Result.ToList()[0].Relevancy.ShouldEqual(1);

        It Should_match_when_searching_first_character = () => Service.Search("柴").Result.ToList()[0].Key.ShouldEqual(Entry.Id);

        It Should_get_relevancy_when_searching_first_character = () => Service.Search("柴").Result.ToList()[0].Relevancy.ShouldEqual(1);

        It Should_match_when_searching_pinyin = () => Service.Search("chai").Result.ToList()[0].Key.ShouldEqual(Entry.Id);

        It Should_get_relevancy_when_searching_pinyin = () => Service.Search("chai").Result.ToList()[0].Relevancy.ShouldEqual(2);

        It Should_match_when_searching_pinyin_second = () => Service.Search("men2").Result.ToList()[0].Key.ShouldEqual(Entry.Id);

        It Should_get_relevancy_when_searching_pinyin_second = () => Service.Search("men2").Result.ToList()[0].Relevancy.ShouldEqual(1);

        static ILanguageSearchService Service;

        static ILanguageEntry Entry;
    }
}
