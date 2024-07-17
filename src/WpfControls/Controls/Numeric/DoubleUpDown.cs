namespace WpfControls.Controls;

public class DoubleUpDown : NumericUpDown<double>
{
    static DoubleUpDown()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DoubleUpDown), new FrameworkPropertyMetadata(typeof(DoubleUpDown)));
    }
}

