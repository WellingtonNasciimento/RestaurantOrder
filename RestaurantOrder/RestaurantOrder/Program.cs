using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantOrder
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine();
                Constantes.ExibeMensagem("> GFT - Restaurant Order App", ConsoleColor.Cyan, ConsoleColor.Gray);
                Console.WriteLine();

                #region TRATA O PERIODO
                Console.Write("> Enter time of day as “morning” or “night” ".PadRight(50, '.') + " : ");

                var timeDay = Console.ReadLine().ToLower();

                if (!timeDay.Equals("morning") && !timeDay.Equals("night"))
                {
                    RegistrarLog(string.Format(Constantes.Mensagens.InvalidTime, timeDay), Constantes.StatusLog.ERRO);
                    Constantes.ExibeMensagem(Constantes.Mensagens.InvalidTimeRodape, ConsoleColor.Red, ConsoleColor.Gray);
                    return;
                }
                else
                {
                    RegistrarLog(string.Format(Constantes.Mensagens.ValidTime, timeDay), Constantes.StatusLog.OK);
                }
                #endregion

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(" MENU ".PadLeft(32, '-').PadRight(70, '-'));
                MontaMenu("1", ConsoleColor.DarkYellow, "entrée", ConsoleColor.Yellow);
                MontaMenu("2", ConsoleColor.DarkYellow, "side", ConsoleColor.Yellow);
                MontaMenu("3", ConsoleColor.DarkYellow, "drink", ConsoleColor.Yellow);
                MontaMenu("4", ConsoleColor.DarkYellow, "dessert", ConsoleColor.Yellow);
                Console.WriteLine("-".PadRight(70, '-'));

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("Please, enter your desired options by separating with comma: ");

                var options = Console.ReadLine();
                List<string> listaOptions = options.Trim().Split(',').OrderBy(q => q).ToList();

                if (!listaOptions.FirstOrDefault().Equals(""))
                {
                    Order o = new Order(timeDay, listaOptions);
                }
                else
                {
                    Constantes.ExibeMensagem(Constantes.Mensagens.NoItemInformed, ConsoleColor.Red, ConsoleColor.Gray);
                    Constantes.ExibeMensagem(Constantes.Mensagens.InvalidSelection, ConsoleColor.Cyan, ConsoleColor.Gray);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadKey();
            }
        }

        static void RegistrarLog(string msg, Constantes.StatusLog status)
        {
            string statusMsg = Enum.GetName(typeof(Constantes.StatusLog), status).PadRight(Enum.GetName(typeof(Constantes.StatusLog), status).Length, ' ');

            Console.WriteLine();
            Console.Write("> ");

            if ((int)status == 1)
                Console.ForegroundColor = ConsoleColor.Green;
            else
                Console.ForegroundColor = ConsoleColor.Red;

            Console.Write(statusMsg);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(string.Format(" -> {0}", msg));
            Console.WriteLine();
        }

        static void MontaMenu(string item, ConsoleColor corDoItem, string descricao, ConsoleColor corDaDescricao)
        {
            Console.ForegroundColor = corDoItem;
            Console.Write(string.Format("> [{0}] ", item));
            Console.ForegroundColor = corDaDescricao;
            Console.WriteLine(descricao);
        }
    }
}