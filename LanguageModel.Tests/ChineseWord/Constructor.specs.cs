﻿using Machine.Specifications;
using System;
using System.Linq;

namespace LanguageModel.Tests.ChineseWord
{
    [Subject(typeof(LanguageModel.ChineseWord))]
    public class When_constructed_with_same_lengths
    {
        Because Of = () => Word = new LanguageModel.ChineseWord(Traditional, Simplified, Pinyin);

        It Should_have_the_right_number_of_characters = () => Word.Characters.Count().ShouldEqual(2);

        static LanguageModel.ChineseWord Word;

        static string Traditional = "中國";

        static string Simplified = "中国";

        static string Pinyin = "Zhong1 guo2";
    }

    [Subject(typeof(LanguageModel.ChineseWord))]
    public class When_constructed_with_different_lengths
    {
        Because Of = () => Exception = Catch.Exception(() => new LanguageModel.ChineseWord(Traditional, Simplified, Pinyin));

        It Should_have_thrown = () => Exception.ShouldBeOfExactType<ArgumentException>();

        static Exception Exception;

        static string Traditional = "中國国";

        static string Simplified = "中";

        static string Pinyin = "Zhong1";
    }
}
