using Machine.Specifications;
using Newtonsoft.Json;

namespace LanguageModel.Tests.ChineseCharacter
{
    [Subject(typeof(LanguageModel.PinyinSyllable))]
    public class When_deserializing_from_json
    {
        Because Of = () => Character = JsonConvert.DeserializeObject<LanguageModel.ChineseCharacter>(Json);

        It Should_have_the_traditional = () => Character.Traditional.ShouldEqual('a');

        It Should_have_the_simplified = () => Character.Simplified.ShouldEqual('b');

        It Should_have_the_pinyin_letters = () => Character.Mandarin.Letters.ShouldEqual("abc");

        It Should_have_the_pinyin_tone = () => Character.Mandarin.Tone.ShouldEqual(Tone.Flat);

        static LanguageModel.ChineseCharacter Character;

        static string Json = "{\"traditional\": \"a\", \"simplified\": \"b\", \"mandarin\": {\"letters\": \"abc\", \"tone\": \"flat\"} }";
    }
}
