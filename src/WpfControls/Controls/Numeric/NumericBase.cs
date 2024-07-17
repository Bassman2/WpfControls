using System.Numerics;

namespace WpfControls.Controls;

public abstract class NumericBase<T> : Control where T : IFormattable
{
    private RepeatButton? incRepeatButton;
    private RepeatButton? decRepeatButton;

    public NumericBase()
    {
        this.Text = ValueToString();
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        //if (Spinner != null)
        //    Spinner.Spin -= OnSpinnerSpin;

        this.incRepeatButton = GetTemplateChild("incRepeatButton") as RepeatButton;
        this.decRepeatButton = GetTemplateChild("decRepeatButton") as RepeatButton;

        if (this.incRepeatButton != null)
        {
            this.incRepeatButton.Click += OnIncRepeatButtonClick;
        }
        if (this.decRepeatButton != null)
        {
            this.decRepeatButton.Click += OnDecRepeatButtonClick;
        }
        //if (Spinner != null)
        //    Spinner.Spin += OnSpinnerSpin;
        //Slider
    }

    protected void OnIncRepeatButtonClick(object sender, RoutedEventArgs e)
    {
        IncrementValue();
    }

    protected void OnDecRepeatButtonClick(object sender, RoutedEventArgs e)
    {
        DecrementValue();
    }

    protected void IncrementValue()
    {
        this.Value += (dynamic)this.Increment;
    }

    protected void DecrementValue()
    {
        this.Value -= (dynamic)this.Increment;
    }

    #region CultureInfo

    public static readonly DependencyProperty CultureInfoProperty =
        DependencyProperty.Register("CultureInfo", typeof(CultureInfo), typeof(NumericBase<T>), new UIPropertyMetadata(CultureInfo.CurrentCulture, OnCultureInfoChanged));

    public CultureInfo CultureInfo
    {
        get => (CultureInfo)GetValue(CultureInfoProperty);
        set => SetValue(CultureInfoProperty, value);
    }

    private static void OnCultureInfoChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        if (o is NumericBase<T> numericSlider)
        {
            numericSlider.OnCultureInfoChanged((CultureInfo)e.OldValue, (CultureInfo)e.NewValue);
        }
    }

    protected virtual void OnCultureInfoChanged(CultureInfo oldValue, CultureInfo newValue)
    {

    }

    #endregion //CultureInfo

    #region StringFormat

    public static readonly DependencyProperty StringFormatProperty = DependencyProperty.Register("StringFormat", typeof(string), typeof(NumericBase<T>), 
        new UIPropertyMetadata(String.Empty, OnStringFormatChanged));

    public string StringFormat
    {
        get =>  (string)GetValue(StringFormatProperty);
        set => SetValue(StringFormatProperty, value);
    }

    private static void OnStringFormatChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) => ((NumericBase<T>)o).OnStringFormatChanged((string)e.OldValue, (string)e.NewValue); 

#pragma warning disable IDE0060 // Remove unused parameter
    private void OnStringFormatChanged(string oldValue, string newValue)
#pragma warning restore IDE0060 // Remove unused parameter
    {
        this.Text = ValueToString();
    }

    #endregion

    #region Text

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(NumericBase<T>), 
            new FrameworkPropertyMetadata(default(String), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextChanged, null, false, UpdateSourceTrigger.LostFocus));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    private static void OnTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) => ((NumericBase<T>)o).OnTextChanged((string)e.OldValue, (string)e.NewValue);

    protected virtual void OnTextChanged(string oldValue, string newValue)
    {

    }

    #endregion //Text

    #region Increment

    public static readonly DependencyProperty IncrementProperty =
        DependencyProperty.Register("Increment", typeof(T), typeof(NumericBase<T>), new PropertyMetadata(default(T), OnIncrementChanged, OnCoerceIncrement));

    public T Increment
    {
        get => (T)GetValue(IncrementProperty);
        set => SetValue(IncrementProperty, value);
    }

    private static void OnIncrementChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) => ((NumericBase<T>)o).OnIncrementChanged((T)e.OldValue, (T)e.NewValue);
    

    protected virtual void OnIncrementChanged(T oldValue, T newValue)
    {
        if (this.IsInitialized)
        {
            //SetValidSpinDirection();
        }
    }

    private static object? OnCoerceIncrement(DependencyObject d, object baseValue)
    {
        if (d is NumericBase<T> numericUpDown)
        {
            return numericUpDown!.OnCoerceIncrement((T)baseValue);
        }
        return baseValue;
    }

    protected virtual T OnCoerceIncrement(T baseValue)
    {
        return baseValue;
    }

    #endregion

    #region Maximum

    public static readonly DependencyProperty MaximumProperty =
        DependencyProperty.Register("Maximum", typeof(T), typeof(NumericBase<T>), new UIPropertyMetadata(default(T), OnMaximumChanged, OnCoerceMaximum));

    public T Maximum
    {
        get => (T)GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    private static void OnMaximumChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) => ((NumericBase<T>)o).OnMaximumChanged((T)e.OldValue, (T)e.NewValue);
    

    protected virtual void OnMaximumChanged(T oldValue, T newValue)
    {
        if (this.IsInitialized)
        {
            //SetValidSpinDirection();
        }
    }

    private static object? OnCoerceMaximum(DependencyObject d, object baseValue)
    {
        if (d is NumericBase<T> upDown)
        {
            return upDown.OnCoerceMaximum((T)baseValue);
        }
        return baseValue;
    }

    protected virtual T OnCoerceMaximum(T baseValue)
    {
        return baseValue;
    }

    #endregion

    #region Minimum

    public static readonly DependencyProperty MinimumProperty =
        DependencyProperty.Register("Minimum", typeof(T), typeof(NumericBase<T>), new UIPropertyMetadata(default(T), OnMinimumChanged, OnCoerceMinimum));

    public T Minimum
    {
        get => (T)GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    private static void OnMinimumChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) => ((NumericBase<T>)o).OnMinimumChanged((T)e.OldValue, (T)e.NewValue);
    
    protected virtual void OnMinimumChanged(T oldValue, T newValue)
    {
        if (this.IsInitialized)
        {
            //SetValidSpinDirection();
        }
    }

    private static object OnCoerceMinimum(DependencyObject d, object baseValue)
    {
        if (d is NumericBase<T> upDown)
            return upDown!.OnCoerceMinimum((T)baseValue)!;

        return baseValue;
    }

    protected virtual T OnCoerceMinimum(T baseValue)
    {
        return baseValue;
    }

    #endregion

    #region Value

    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(T?), typeof(NumericBase<T>), 
            new FrameworkPropertyMetadata(default(T?), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged, OnCoerceValue, false, UpdateSourceTrigger.PropertyChanged));

    public T? Value
    {
        get => (T?)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    private static object OnCoerceValue(DependencyObject o, object basevalue) => ((NumericBase<T>)o).OnCoerceValue(basevalue);
    
    protected virtual object OnCoerceValue(object newValue)
    {
        //return newValue;

        dynamic value = newValue;
        value = Math.Max(value, (dynamic)this.Minimum);
        value = Math.Min(value, (dynamic)this.Maximum);
        value = Math.Round(value / (dynamic)this.Increment) * this.Increment;
        return value;
    }

    private static void OnValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) => ((NumericBase<T>)o).OnValueChanged((T?)e.OldValue, (T?)e.NewValue);
    
    protected virtual void OnValueChanged(T? oldValue, T? newValue)
    {
        this.Text = ValueToString();
    }

    protected virtual string ValueToString()
    {
        return this.Value!.ToString(this.StringFormat, this.CultureInfo);
    }

    #endregion

    public static readonly DependencyProperty TickFrequencyProperty =
        DependencyProperty.Register("TickFrequency", typeof(T), typeof(NumericBase<T>), new UIPropertyMetadata(default(T)));

    public T TickFrequency
    {
        get
        {
            return (T)GetValue(TickFrequencyProperty);
        }
        set
        {
            SetValue(TickFrequencyProperty, value);
        }
    }

    public static readonly DependencyProperty TickPlacementProperty =
        DependencyProperty.Register("TickPlacement", typeof(TickPlacement), typeof(NumericBase<T>), new UIPropertyMetadata(TickPlacement.None));

    public TickPlacement TickPlacement
    {
        get
        {
            return (TickPlacement)GetValue(TickPlacementProperty);
        }
        set
        {
            SetValue(TickPlacementProperty, value);
        }
    }

    public static readonly DependencyProperty SliderWidthProperty =
        DependencyProperty.Register("SliderWidth", typeof(double), typeof(NumericBase<T>), new UIPropertyMetadata(160.0));

    public double SliderWidth
    {
        get
        {
            return (double)GetValue(SliderWidthProperty);
        }
        set
        {
            SetValue(SliderWidthProperty, value);
        }
    }

    public static readonly DependencyProperty TextWidthProperty =
        DependencyProperty.Register("TextWidth", typeof(double), typeof(NumericBase<T>), new UIPropertyMetadata(50.0));

    public double TextWidth
    {
        get
        {
            return (double)GetValue(TextWidthProperty);
        }
        set
        {
            SetValue(TextWidthProperty, value);
        }
    }
}
