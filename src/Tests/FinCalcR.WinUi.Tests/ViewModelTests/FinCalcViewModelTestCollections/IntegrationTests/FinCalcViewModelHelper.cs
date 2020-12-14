using System;
using StEn.FinCalcR.WinUi.ViewModels;

namespace FinCalcR.WinUi.Tests.ViewModelTests.FinCalcViewModelTestCollections.IntegrationTests
{
    public static class FinCalcViewModelHelper
    {
        public static void ExecuteOperations(FinCalcViewModel2 vm, Ca[] operations)
        {
            foreach (Ca operation in operations)
            {
                ExecuteOperation(vm, operation);
            }
        }

        private static void ExecuteOperation(FinCalcViewModel2 vm, Ca operation)
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
    }
}
