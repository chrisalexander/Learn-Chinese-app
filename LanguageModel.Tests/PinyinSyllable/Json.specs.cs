using Machine.Specifications;
using Newtonsoft.Json;

namespace LanguageModel.Tests.PinyinSyllable
{
    [Subject(typeof(LanguageModel.PinyinSyllable))]
    public class When_deserializing_from_json
    {
        Because Of = () => Syllable = JsonConvert.DeserializeObject<LanguageModel.PinyinSyllable>(Json);

        It Should_have_the_tone = () => Syllable.Tone.ShouldEqual(Tone.Flat);

        It Should_have_the_letters = () => Syllable.Letters.ShouldEqual("abc");

        static LanguageModel.PinyinSyllable Syllable;

        static string Json = "{\"letters\": \"abc\", \"tone\": \"flat\"}";
    }
}
