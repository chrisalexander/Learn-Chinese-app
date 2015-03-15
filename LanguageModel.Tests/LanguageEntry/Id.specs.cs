using Machine.Specifications;

namespace LanguageModel.Tests.LanguageEntry
{
    [Subject(typeof(LanguageModel.LanguageEntry))]
    public class When_generating_id_identical_entries
    {
        Because Of = () =>
        {
            FirstEntry = new LanguageModel.LanguageEntry("中", "中", "Zhong1", new[] { "China" });
            SecondEntry = new LanguageModel.LanguageEntry("中", "中", "Zhong1", new[] { "China" });
        };

        It Should_have_different_ids = () => FirstEntry.Id.ShouldEqual(SecondEntry.Id);

        static ILanguageEntry FirstEntry;

        static ILanguageEntry SecondEntry;
    }

    [Subject(typeof(LanguageModel.LanguageEntry))]
    public class When_generating_id_different_entries
    {
        Because Of = () =>
        {
            FirstEntry = new LanguageModel.LanguageEntry("中", "中", "Zhong1", new[] { "China" });
            SecondEntry = new LanguageModel.LanguageEntry("國", "国", "guo2", new[] { "Middle Kingdom" });
        };

        It Should_have_different_ids = () => FirstEntry.Id.ShouldNotEqual(SecondEntry.Id);

        static ILanguageEntry FirstEntry;

        static ILanguageEntry SecondEntry;
    }

    [Subject(typeof(LanguageModel.LanguageEntry))]
    public class When_generating_id_different_traditional
    {
        Because Of = () =>
        {
            FirstEntry = new LanguageModel.LanguageEntry("中", "中", "Zhong1", new[] { "China" });
            SecondEntry = new LanguageModel.LanguageEntry("國", "中", "Zhong1", new[] { "China" });
        };

        It Should_have_different_ids = () => FirstEntry.Id.ShouldNotEqual(SecondEntry.Id);

        static ILanguageEntry FirstEntry;

        static ILanguageEntry SecondEntry;
    }

    [Subject(typeof(LanguageModel.LanguageEntry))]
    public class When_generating_id_different_simplified
    {
        Because Of = () =>
        {
            FirstEntry = new LanguageModel.LanguageEntry("中", "中", "Zhong1", new[] { "China" });
            SecondEntry = new LanguageModel.LanguageEntry("中", "国", "Zhong1", new[] { "China" });
        };

        It Should_have_different_ids = () => FirstEntry.Id.ShouldNotEqual(SecondEntry.Id);

        static ILanguageEntry FirstEntry;

        static ILanguageEntry SecondEntry;
    }
}
