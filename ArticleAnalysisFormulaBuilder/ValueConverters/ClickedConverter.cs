using System;
using System.Globalization;

// ReSharper disable once CheckNamespace
namespace ArticleAnalysisFormulaBuilder;

internal class ClickedConverter : BaseValueConverter<ClickedConverter>
{
    public override object? Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        return value is true ? false : value;
    }

    public override object? ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}