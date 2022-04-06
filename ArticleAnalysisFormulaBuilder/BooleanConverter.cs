﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace ArticleAnalysisFormulaBuilder;

internal class BooleanConverter : MarkupExtension, IValueConverter
{
    private BooleanConverter? _instance;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool) value ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return _instance ??= new BooleanConverter();
    }
}