using System.Linq;
using Machine.Specifications;
using Newtonsoft.Json;

namespace LanguageModel.Tests.ChineseWord
{
    [Subject(typeof(LanguageModel.PinyinSyllable))]
    public class When_deserializing_from_json
    {
        Because Of = () => Word = JsonConvert.DeserializeObject<LanguageModel.ChineseWord>(Json);

        It Should_have_the_first_traditional = () => Word.Characters.First().Traditional.ShouldEqual('a');

        It Should_have_the_first_simplified = () => Word.Characters.First().Simplified.ShouldEqual('b');

        It Should_have_the_first_pinyin_letters = () => Word.Characters.First().Mandarin.Letters.ShouldEqual("abc");

        It Should_have_the_first_pinyin_tone = () => Word.Characters.First().Mandarin.Tone.ShouldEqual(Tone.Flat);

        It Should_have_the_second_traditional = () => Word.Characters.ToList()[1].Traditional.ShouldEqual('c');

        It Should_have_the_second_simplified = () => Word.Characters.ToList()[1].Simplified.ShouldEqual('d');

        It Should_have_the_second_pinyin_letters = () => Word.Characters.ToList()[1].Mandarin.Letters.ShouldEqual("def");

        It Should_have_the_second_pinyin_tone = () => Word.Characters.ToList()[1].Mandarin.Tone.ShouldEqual(Tone.Up);

        static LanguageModel.ChineseWord Word;

        static string Json = "{\"characters\": [{\"traditional\": \"a\", \"simplified\": \"b\", \"mandarin\": {\"letters\": \"abc\", \"tone\": \"flat\"} }, {\"traditional\": \"c\", \"simplified\": \"d\", \"mandarin\": {\"letters\": \"def\", \"tone\": \"up\"} }] }";
    }
}
