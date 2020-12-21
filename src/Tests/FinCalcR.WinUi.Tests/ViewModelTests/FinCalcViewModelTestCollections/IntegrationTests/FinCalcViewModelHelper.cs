using System;
using System.Globalization;
using FluentAssertions.Numeric;
using StEn.FinCalcR.Calculations.Calculator.Commands;
using StEn.FinCalcR.WinUi.ViewModels;

namespace FinCalcR.WinUi.Tests.ViewModelTests.FinCalcViewModelTestCollections.IntegrationTests
{
    public static class FinCalcViewModelHelper
    {
        public static void SetFinancialValue(FinCalcViewModel2 vm, double valueToAssign, CommandWord variableToAssignValueTo)
        {
            InputNumberWithCommands(vm, valueToAssign);
            switch (variableToAssignValueTo)
            {
                case CommandWord.None:
                    break;
                case CommandWord.DecimalSeparator:
                    break;
                case CommandWord.SetYears:
                    vm.YearsPressedCommand.Execute(false);
                    break;
                case CommandWord.Interest:
                    vm.InterestPressedCommand.Execute(false);
                    break;
                case CommandWord.NominalInterestRate:
                    vm.OperatorPressedCommand.Execute("*");
                    vm.InterestPressedCommand.Execute(false);
                    break;
                case CommandWord.Start:
                    vm.StartPressedCommand.Execute(false);
                    break;
                case CommandWord.SetRate:
                    vm.RatePressedCommand.Execute(false);
                    break;
                case CommandWord.SetEnd:
                    vm.EndPressedCommand.Execute(false);
                    break;
                case CommandWord.SetRatesPerAnnum:
                    vm.OperatorPressedCommand.Execute("*");
                    vm.YearsPressedCommand.Execute(false);
                    break;
                case CommandWord.PercentCalculation:
                    break;
                case CommandWord.Clear:
                    break;
                case CommandWord.Operator:
                    break;
                case CommandWord.Digit:
                    break;
                case CommandWord.AlgebSign:
                    break;
                case CommandWord.Calculate:
                    break;
                case CommandWord.LoadMemoryValue:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(variableToAssignValueTo), variableToAssignValueTo, null);
            }
        }

        public static void ExecuteDummyOperations(FinCalcViewModel2 vm, Ca[] operations)
        {
            foreach (Ca operation in operations)
            {
                ExecuteDummyOperation(vm, operation);
            }
        }

        public static void ExecuteDummyOperation(FinCalcViewModel2 vm, Ca operation)
        {
            switch (operation)
            {
                case Ca.Nr1:
                    vm.DigitPressedCommand.Execute(1);
                    break;
                case Ca.Nr2:
                    vm.DigitPressedCommand.Execute(2);
                    break;
                case Ca.Nr34:
                    vm.DigitPressedCommand.Execute(3);
                    vm.DigitPressedCommand.Execute(4);
                    break;
                case Ca.Dec:
                    vm.DecimalSeparatorPressedCommand.Execute(null);
                    break;
                case Ca.SetYears:
                    vm.YearsPressedCommand.Execute(false);
                    break;
                case Ca.GetYears:
                    vm.YearsPressedCommand.Execute(true);
                    break;
                case Ca.Alg:
                    vm.AlgebSignCommand.Execute(null);
                    break;
                case Ca.OpA:
                    vm.OperatorPressedCommand.Execute("+");
                    break;
                case Ca.OpS:
                    vm.OperatorPressedCommand.Execute("-");
                    break;
                case Ca.OpM:
                    vm.OperatorPressedCommand.Execute("*");
                    break;
                case Ca.OpD:
                    vm.OperatorPressedCommand.Execute("/");
                    break;
                case Ca.Calc:
                    vm.CalculatePressedCommand.Execute(null);
                    break;
                case Ca.ClearP:
                    vm.ClearPressedCommand.Execute(false);
                    break;
                case Ca.ClearF:
                    vm.ClearPressedCommand.Execute(true);
                    break;
                case Ca.SetInt:
                    vm.InterestPressedCommand.Execute(false);
                    break;
                case Ca.GetInt:
                    vm.InterestPressedCommand.Execute(true);
                    break;
                case Ca.SetStart:
                    vm.StartPressedCommand.Execute(false);
                    break;
                case Ca.GetStart:
                    vm.StartPressedCommand.Execute(true);
                    break;
                case Ca.SetRate:
                    vm.RatePressedCommand.Execute(false);
                    break;
                case Ca.GetRate:
                    vm.RatePressedCommand.Execute(true);
                    break;
                case Ca.SetEnd:
                    vm.EndPressedCommand.Execute(false);
                    break;
                case Ca.GetEnd:
                    vm.EndPressedCommand.Execute(true);
                    break;
                case Ca.SetNom:
                    vm.OperatorPressedCommand.Execute("*");
                    vm.InterestPressedCommand.Execute(false);
                    break;
                case Ca.GetNom:
                    vm.OperatorPressedCommand.Execute("*");
                    vm.InterestPressedCommand.Execute(true);
                    break;
                case Ca.SetRpa:
                    vm.OperatorPressedCommand.Execute("*");
                    vm.YearsPressedCommand.Execute(false);
                    break;
                case Ca.GetRpa:
                    vm.OperatorPressedCommand.Execute("*");
                    vm.YearsPressedCommand.Execute(true);
                    break;
                case Ca.ToggleAdv:
                    vm.OperatorPressedCommand.Execute("*");
                    vm.StartPressedCommand.Execute(false);
                    break;
                case Ca.SetRep:
                    vm.OperatorPressedCommand.Execute("*");
                    vm.RatePressedCommand.Execute(false);
                    break;
                case Ca.GetRep:
                    vm.OperatorPressedCommand.Execute("*");
                    vm.RatePressedCommand.Execute(true);
                    break;
                case Ca.PerA:
                    vm.DigitPressedCommand.Execute(2);
                    vm.DigitPressedCommand.Execute(0);
                    vm.DigitPressedCommand.Execute(0);
                    vm.OperatorPressedCommand.Execute("+");
                    vm.DigitPressedCommand.Execute(5);
                    vm.EndPressedCommand.Execute(false);
                    break;
                case Ca.PerS:
                    vm.DigitPressedCommand.Execute(2);
                    vm.DigitPressedCommand.Execute(0);
                    vm.DigitPressedCommand.Execute(0);
                    vm.OperatorPressedCommand.Execute("-");
                    vm.DigitPressedCommand.Execute(5);
                    vm.EndPressedCommand.Execute(false);
                    break;
                case Ca.PerM:
                    vm.DigitPressedCommand.Execute(2);
                    vm.DigitPressedCommand.Execute(0);
                    vm.DigitPressedCommand.Execute(0);
                    vm.OperatorPressedCommand.Execute("*");
                    vm.DigitPressedCommand.Execute(5);
                    vm.EndPressedCommand.Execute(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
            }
        }

        private static void InputNumberWithCommands(FinCalcViewModel2 vm, double number)
        {
            var numberAsText = number.ToString(CultureInfo.CurrentUICulture);
            var inputs = numberAsText.ToCharArray();

            var needsAlgebSign = false;
            foreach (var input in inputs)
            {
                if (char.IsDigit(input))
                {
                    vm.DigitPressedCommand.Execute(int.Parse(input.ToString()));
                }
                else
                {
                    switch (input)
                    {
                        case '-':
                            needsAlgebSign = true;
                            break;
                        case ',':
                            vm.DecimalSeparatorPressedCommand.Execute(false);
                            break;
                        case '.':
                            vm.DecimalSeparatorPressedCommand.Execute(false);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }

            // Algeb sign needs to be pressed not as the first command after a special function to avoid conflicts (see #4)
            if (needsAlgebSign)
            {
                vm.AlgebSignCommand.Execute(false);
            }
        }
    }
}
