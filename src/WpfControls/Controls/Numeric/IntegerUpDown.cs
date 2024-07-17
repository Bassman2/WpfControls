namespace WpfControls.Controls;

public class IntegerUpDown : NumericUpDown<int>
{
    static IntegerUpDown()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(IntegerUpDown), new FrameworkPropertyMetadata(typeof(IntegerUpDown)));
    }
}