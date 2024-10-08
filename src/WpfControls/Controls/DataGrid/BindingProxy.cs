﻿namespace WpfControls.Controls;

public class BindingProxy : Freezable
{
    protected override Freezable CreateInstanceCore()
    {
        return new BindingProxy();
    }

    public static readonly DependencyProperty DataProperty = 
        DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), 
            new UIPropertyMetadata(null));

    public object Data
    {
        get => (object)GetValue(DataProperty); 
        set => SetValue(DataProperty, value); 
    }
}
