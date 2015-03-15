using System;
using Machine.Specifications;

namespace LanguageModel.Tests.ChineseCharacter
{
    [Subject(typeof(LanguageModel.ChineseCharacter))]
    public class When_creating_a_new_character
    {
        Establish Context = () => Character = new LanguageModel.ChineseCharacter('a', 'b', "abc1");

        It Should_have_the_traditional = () => Character.Traditional.ShouldEqual('a');

        It Should_have_the_simplified = () => Character.Simplified.ShouldEqual('b');

        It Should_have_the_pinyin_letters = () => Character.Mandarin.Letters.ShouldEqual("abc");

        It Should_have_the_pinyin_tone = () => Character.Mandarin.Tone.ShouldEqual(Tone.Flat);

        static LanguageModel.ChineseCharacter Character;
    }
}
