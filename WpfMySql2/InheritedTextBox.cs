namespace UserInterface.Controls
{
    using System.Windows;
    using System.Windows.Controls;


    public class WatermarkedTextBox : TextBox
    {

        private const string _defaultWatermark = "Kunde";

        public static readonly DependencyProperty WatermarkTextProperty = DependencyProperty.Register("WatermarkText", typeof(string), typeof(WatermarkedTextBox), new UIPropertyMetadata(string.Empty, OnWatermarkTextChanged));


        /// <summary>
        /// Initializes a new instance of the <see cref="WatermarkedTextBox"/> class with default watermark text.
        /// </summary>
        public WatermarkedTextBox()
            : this(_defaultWatermark)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WatermarkedTextBox"/> class.
        /// </summary>
        /// <param name="watermark">The watermark to show when value is <c>null</c> or empty.</param>
        public WatermarkedTextBox(string watermark)
        {
            WatermarkText = watermark;
        }


        public string WatermarkText
        {
            get { return (string)GetValue(WatermarkTextProperty); }
            set { SetValue(WatermarkTextProperty, value); }
        }

        public static void OnWatermarkTextChanged(DependencyObject box, DependencyPropertyChangedEventArgs e)
        {
            //Add changed functionality here
        }
    }
}