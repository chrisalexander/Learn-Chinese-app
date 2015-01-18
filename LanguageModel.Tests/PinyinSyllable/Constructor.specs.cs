using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageModel;

namespace LanguageModel.Tests.PinyinSyllable
{
    [Subject(typeof(LanguageModel.PinyinSyllable))]
    public class When_constructing_with_complete_character
    {
        Because of = () => Syllable = new LanguageModel.PinyinSyllable(Pinyin);

        It Should_have_the_right_letters = () => Syllable.Letters.ShouldEqual("kou");

        It Should_have_the_right_tone = () => Syllable.Tone.ShouldEqual(Tone.Curve);

        It Should_create_a_string = () => Syllable.ToString().ShouldEqual(Pinyin);

        static LanguageModel.PinyinSyllable Syllable;

        static string Pinyin = "kou3";
    }

    [Subject(typeof(LanguageModel.PinyinSyllable))]
    public class When_constructing_with_incomplete_character
    {
        Because of = () => Syllable = new LanguageModel.PinyinSyllable(Pinyin);

        It Should_have_the_right_letters = () => Syllable.Letters.ShouldEqual("kou");

        It Should_have_the_right_tone = () => Syllable.Tone.ShouldEqual(Tone.Short);

        static LanguageModel.PinyinSyllable Syllable;

        static string Pinyin = "kou";
    }

    [Subject(typeof(LanguageModel.PinyinSyllable))]
    public class When_constructing_with_invalid_input
    {
        Because of = () => Exception = Catch.Exception(() => new LanguageModel.PinyinSyllable(string.Empty));

        It Should_have_thrown_the_right_exception = () => Exception.ShouldBeOfType<ArgumentOutOfRangeException>();

        static Exception Exception;
    }
}