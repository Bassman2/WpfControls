﻿namespace WpfControls.Behaviours;

public static class InputBindingsManager
{

    public static readonly DependencyProperty UpdatePropertySourceWhenEnterPressedProperty = DependencyProperty.RegisterAttached(
            "UpdatePropertySourceWhenEnterPressed", typeof(DependencyProperty), typeof(InputBindingsManager), new PropertyMetadata(null, OnUpdatePropertySourceWhenEnterPressedPropertyChanged));

    static InputBindingsManager()
    {

    }

    public static void SetUpdatePropertySourceWhenEnterPressed(DependencyObject dp, DependencyProperty value)
    {
        dp.SetValue(UpdatePropertySourceWhenEnterPressedProperty, value);
    }

    public static DependencyProperty? GetUpdatePropertySourceWhenEnterPressed(DependencyObject? dp)
    {
        return (DependencyProperty?)dp?.GetValue(UpdatePropertySourceWhenEnterPressedProperty) ?? null;
    }

    private static void OnUpdatePropertySourceWhenEnterPressedPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
    {
        if (dp is UIElement element)
        {
            if (e.OldValue != null)
            {
                element.PreviewKeyDown -= HandlePreviewKeyDown;
            }

            if (e.NewValue != null)
            {
                element.PreviewKeyDown += new KeyEventHandler(HandlePreviewKeyDown);
            }
        }
    }

    static void HandlePreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            DoUpdateSource(e.Source);
        }
    }

    static void DoUpdateSource(object source)
    {
        DependencyProperty? property = GetUpdatePropertySourceWhenEnterPressed(source as DependencyObject);

        if (property == null)
        {
            return;
        }


        if (source is UIElement elt)
        {
            BindingExpression? binding = BindingOperations.GetBindingExpression(elt, property);

            binding?.UpdateSource();
        }

        
    }
}
