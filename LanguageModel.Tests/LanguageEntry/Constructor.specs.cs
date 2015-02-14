using Machine.Specifications;
using System.Collections.Generic;
using System.Linq;

namespace LanguageModel.Tests.LanguageEntry
{
    [Subject(typeof(LanguageModel.LanguageEntry))]
    public class When_constructed
    {
        Because of = () => Entry = new LanguageModel.LanguageEntry(Traditional, Simplified, Pinyin, English);

        It Should_have_the_english = () =>
        {
            Entry.English.First().ShouldEqual("China");
            Entry.English.Last().ShouldEqual("Middle Kingdom");
        };

        It Should_have_the_first_character = () =>
        {
            Entry.Chinese.Characters.First().Mandarin.Letters.ShouldEqual("Zhong");
            Entry.Chinese.Characters.First().Mandarin.Tone.ShouldEqual(Tone.Flat);
            Entry.Chinese.Characters.First().Traditional.ShouldEqual('中');
            Entry.Chinese.Characters.First().Simplified.ShouldEqual('中');
        };

        It Should_have_the_second_character = () =>
        {
            Entry.Chinese.Characters.Last().Mandarin.Letters.ShouldEqual("guo");
            Entry.Chinese.Characters.Last().Mandarin.Tone.ShouldEqual(Tone.Up);
            Entry.Chinese.Characters.Last().Traditional.ShouldEqual('國');
            Entry.Chinese.Characters.Last().Simplified.ShouldEqual('国');
        };

        static LanguageModel.LanguageEntry Entry;

        static string Traditional = "中國";

        static string Simplified = "中国";

        static string Pinyin = "Zhong1 guo2";

        static IEnumerable<string> English = new[] { "China", "Middle Kingdom" };
    }
}
