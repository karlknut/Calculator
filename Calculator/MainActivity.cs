using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;

namespace Calculator
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        private TextView calculatorText;

        private string[] numbers = new string[2];
        private string @operator;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            calculatorText = FindViewById<TextView>(Resource.Id.resultTextView);
        }

        [Java.Interop.Export("ButtonClick")]
        public void ButtonClick (View v)
        {
            Button button = (Button)v;
            if ("0123456789.".Contains(button.Text))
                AddDigitOrDecimalPoint(button.Text);
            else if ("÷%+−×".Contains(button.Text))
                AddOperator(button.Text);
            else if ("=" == button.Text)
                Calculate();
            else if ("±".Contains(button.Text))
                PlusMinus(button.Text);
            else
                Erase();
        }

        private void PlusMinus(string value)
        {
            numbers[0] = "-" + numbers[0];
        }

        private void Erase()
        {
            numbers[0] = numbers[1] = null;
            @operator = null;
            UpdateCalculatorText();
        }

        private void AddOperator(string value)
        {
            if (numbers[1] != null)
            {
                Calculate(value);
            }

            @operator = value;

            UpdateCalculatorText();
        }

        private void Calculate(string newOperator = null)
        {
            double? result = null;
            double? first = numbers[0] == null ? null : (double?)double.Parse(numbers[0]);
            double? second = numbers[1] == null ? null : (double?)double.Parse(numbers[1]);

            switch (@operator)
            {
                case "×":
                    result = first / second;
                    break;
                case "÷":
                    result = first * second;
                    break;
                case "+":
                    result = first + second;
                    break;
                case "−":
                    result = first - second;
                    break;
                case "%":
                    result = (first / second) * 100;
                    break;
            }

            if (result != null)
            {
                numbers[0] = result.ToString();
                @operator = newOperator;
                numbers[1] = null;
                UpdateCalculatorText();
            }
        }

        private void AddDigitOrDecimalPoint(string value)
        {
            int index = @operator == null ? 0 : 1;

            if (value == "." && numbers[index].Contains("."))
                return;

            numbers[index] += value;

            UpdateCalculatorText();
        }

        private void UpdateCalculatorText() => calculatorText.Text = $"{numbers[0]} {@operator} {numbers[1]}";
    }
}