using System;

namespace RestaurantOrder
{
    public static class Constantes
    {
        public enum StatusLog
        {
            OK = 1,
            ERRO = 0
        }

        public static class Mensagens
        {
            public const string InvalidSelection = "Invalid selections.\nPress ENTER to re-order.";
            public const string DuplicateItem = "Duplicate item: [{0}] - {1}.\n";
            public const string DuplicateItemRodapeManha = "In the morning, you can only order multiple cups of coffee.\n";
            public const string DuplicateItemRodapeNoite = "At night, you can only multiple orders of potatoes.\n";
            public const string Dessert = "Invalid item: [4] - dessert.\nDessert is only available at night.\n";
            public const string Success = "Order included successfully!!!\nPress ENTER to continue.";
            public const string InvalidItem = "Invalid item: [{0}] - Not Found.\n";
            public const string ValidTime = "Type[{0}] valid!";
            public const string InvalidTime = "Invalid value [{0}] typed!";
            public const string InvalidTimeRodape = "Invalid time of day!!!\nPress ENTER to continue.";
            public const string NoItemInformed = "No item informed or Invalid format.";
        }

        public static void ExibeMensagem(string mensagem, ConsoleColor corDaFonteInicio, ConsoleColor corDaFonteFinal)
        {
            Console.WriteLine();
            Console.ForegroundColor = corDaFonteInicio;
            Console.WriteLine("-".PadRight(70, '-'));
            Console.WriteLine(mensagem);
            Console.WriteLine("-".PadRight(70, '-'));
            Console.ForegroundColor = corDaFonteFinal;
        }
    }
}