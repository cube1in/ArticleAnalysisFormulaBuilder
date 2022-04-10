using System;
using System.Globalization;
using System.Windows;

// ReSharper disable once CheckNamespace
namespace ArticleAnalysisFormulaBuilder;

internal class BooleanConverter : BaseValueConverter<BooleanConverter>
{
    public override object? Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        if (value == null) return null;
        return (bool) value ? Visibility.Visible : Visibility.Collapsed;
    }

    public override object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}