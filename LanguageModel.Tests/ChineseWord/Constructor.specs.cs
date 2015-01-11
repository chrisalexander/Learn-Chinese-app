using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageModel.Tests.ChineseWord
{
    [Subject(typeof(LanguageModel.ChineseWord))]
    public class When_constructed_with_same_lengths
    {
        Because of = () => Word = new LanguageModel.ChineseWord(Traditional, Simplified, Pinyin);

        It Should_have_the_right_number_of_characters = () => Word.Characters.Count().ShouldEqual(2);

        static LanguageModel.ChineseWord Word;

        static string Traditional = "中國";

        static string Simplified = "中国";

        static string Pinyin = "Zhong1 guo2";
    }

    [Subject(typeof(LanguageModel.ChineseWord))]
    public class When_constructed_with_different_lengths
    {
        Because of = () => Exception = Catch.Exception(() => new LanguageModel.ChineseWord(Traditional, Simplified, Pinyin));

        It Should_have_thrown = () => Exception.ShouldBeOfType<ArgumentException>();

        static Exception Exception;

        static string Traditional = "中國国";

        static string Simplified = "中";

        static string Pinyin = "Zhong1";
    }
}
