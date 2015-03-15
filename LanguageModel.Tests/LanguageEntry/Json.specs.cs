using System.Linq;
using Machine.Specifications;
using Newtonsoft.Json;

namespace LanguageModel.Tests.LanguageEntry
{
    [Subject(typeof(LanguageModel.LanguageEntry))]
    public class When_deserializing_from_json
    {
        Because Of = () => Entry = JsonConvert.DeserializeObject<LanguageModel.LanguageEntry>(Json);

        It Should_have_the_first_traditional = () => Entry.Chinese.Characters.First().Traditional.ShouldEqual('a');

        It Should_have_the_first_simplified = () => Entry.Chinese.Characters.First().Simplified.ShouldEqual('b');

        It Should_have_the_first_pinyin_letters = () => Entry.Chinese.Characters.First().Mandarin.Letters.ShouldEqual("abc");

        It Should_have_the_first_pinyin_tone = () => Entry.Chinese.Characters.First().Mandarin.Tone.ShouldEqual(Tone.Flat);

        It Should_have_the_second_traditional = () => Entry.Chinese.Characters.ToList()[1].Traditional.ShouldEqual('c');

        It Should_have_the_second_simplified = () => Entry.Chinese.Characters.ToList()[1].Simplified.ShouldEqual('d');

        It Should_have_the_second_pinyin_letters = () => Entry.Chinese.Characters.ToList()[1].Mandarin.Letters.ShouldEqual("def");

        It Should_have_the_second_pinyin_tone = () => Entry.Chinese.Characters.ToList()[1].Mandarin.Tone.ShouldEqual(Tone.Up);

        It Should_have_the_first_english = () => Entry.English.First().ShouldEqual("test");

        It Should_have_the_second_english = () => Entry.English.ToList()[1].ShouldEqual("test2");

        static LanguageModel.LanguageEntry Entry;

        static string Json = "{\"english\": [\"test\", \"test2\"], \"chinese\": {\"characters\": [{\"traditional\": \"a\", \"simplified\": \"b\", \"mandarin\": {\"letters\": \"abc\", \"tone\": \"flat\"} }, {\"traditional\": \"c\", \"simplified\": \"d\", \"mandarin\": {\"letters\": \"def\", \"tone\": \"up\"} }] } }";
    }
}
