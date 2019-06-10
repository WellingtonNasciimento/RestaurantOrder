using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

namespace RestaurantOrder
{
    public class Order
    {
        string timeDay;
        List<string> listaDeCodigos;
        string codigosIcluidos;

        public string CodigosIcluidos
        {
            get { return codigosIcluidos; }
        }

        public Order()
        {

        }

        public Order(string timeOfDay, List<string> itensDoPedido)
        {
            timeDay = timeOfDay;
            listaDeCodigos = itensDoPedido;

           codigosIcluidos = ControlaPedido(timeDay, listaDeCodigos);
        }


        string ControlaPedido(string timeDay, List<string> listaCodigos)
        {
            int count = 0;
            int countError = 0;
            int countErrorDuplicateItem = 0;
            string mensagem = string.Empty;
            string itensDoPedido = string.Empty; ;
            string itemAnterior = string.Empty;
            string itemAtual = string.Empty;

            //Carrega os dishes para pesquisas via linq
            DishesTypes listaDishes = JsonConvert.DeserializeObject<DishesTypes>(File.ReadAllText("dish.json"));

            #region ITENS DO PEDIDO
            foreach (var item in listaCodigos)
            {
                count++;
                itemAtual = item;
                //Verifica se o codigo do item faz parte do menu
                if (int.Parse(item) >= 1 && int.Parse(item) <= 4)
                {
                    if (itemAtual != itemAnterior)
                    {
                        if (item.Equals("3") && timeDay.Equals("morning") && listaCodigos.Count(q => q == item) > 1)
                        {
                            //Permite multiplos pedidos de cafe pela manha
                            if (!itensDoPedido.Contains("error"))
                                itensDoPedido = itensDoPedido + string.Format("{0}(x{1})", listaDishes.type.Where(q => q.code == item).First().morning, listaCodigos.Count(q => q == item));
                        }
                        else if (item.Equals("4") && timeDay.Equals("morning"))
                        {
                            //Nao permite pedir sobremesa pela manha
                            if (itensDoPedido.Length > 0)
                            {
                                if (!itensDoPedido.Contains("error"))
                                    itensDoPedido = string.Format("{0}{1}", itensDoPedido, ", error");
                            }
                            else
                                itensDoPedido = "error";

                            mensagem = Constantes.Mensagens.Dessert;
                            countError++;
                        }
                        else if (item.Equals("2") && timeDay.Equals("night") && listaCodigos.Count(q => q == item) > 1)
                        {
                            //Permite multiplos pedidos de batata a noite
                            if (!itensDoPedido.Contains("error"))
                                itensDoPedido = itensDoPedido + string.Format("{0}(x{1}), ", listaDishes.type.Where(q => q.code == item).First().night, listaCodigos.Count(q => q == item));
                        }
                        else
                        {
                            //Nao inclui o item na lista apos erro encontrado
                            if (!itensDoPedido.Contains("error"))
                            {
                                if (timeDay.Equals("morning"))
                                    itensDoPedido = string.Format("{0}{1}", itensDoPedido, listaDishes.type.Where(q => q.code == item).First().morning);
                                else
                                    itensDoPedido = string.Format("{0}{1}", itensDoPedido, listaDishes.type.Where(q => q.code == item).First().night);

                                if (count < listaCodigos.Count())
                                    if (!(count == 3 && timeDay.Equals("morning")))
                                        itensDoPedido = string.Format("{0}{1}", itensDoPedido, ", ");
                            }
                        }
                    }
                    else
                    {
                        //Verifica eventuais duplicidades que fogem da regra de negocio
                        //Duplicidade de cafe pela manha ou batata a noite'e permitida
                        if (!(item.Equals("2") && timeDay.Equals("night") || item.Equals("3") && timeDay.Equals("morning") || item.Equals("4") && timeDay.Equals("morning"))) 
                        {
                            if (!itensDoPedido.Contains("error"))
                                itensDoPedido = string.Format("{0}{1}", itensDoPedido, "error");

                            if (timeDay.Equals("morning"))
                            {
                                if (!mensagem.Contains(string.Format("{0}", string.Format(Constantes.Mensagens.DuplicateItem, item, listaDishes.type.Where(q => q.code == item).First().morning))))
                                    mensagem = string.Format("{0}\n{1}", mensagem, string.Format(Constantes.Mensagens.DuplicateItem, item, listaDishes.type.Where(q => q.code == item).First().morning));
                            }
                            else
                                if (!mensagem.Contains(string.Format("{0}", string.Format(Constantes.Mensagens.DuplicateItem, item, listaDishes.type.Where(q => q.code == item).First().night))))
                                mensagem = string.Format("{0}\n{1}", mensagem, string.Format(Constantes.Mensagens.DuplicateItem, item, listaDishes.type.Where(q => q.code == item).First().night));

                            countError++;
                            countErrorDuplicateItem++;
                        }
                    }
                }
                else
                {
                    //Erro se o usuario informar um item que nao esteja no menu
                    if (itensDoPedido.Length > 0)
                    {
                        if (!itensDoPedido.Contains("error"))
                            itensDoPedido = string.Format("{0}{1}", itensDoPedido, "error");
                    }
                    else
                        itensDoPedido = "error";

                    if (!mensagem.Contains(string.Format(Constantes.Mensagens.InvalidItem, item)))
                        mensagem = string.Format("{0}\n{1}", mensagem, string.Format(Constantes.Mensagens.InvalidItem, item));

                    countError++;
                }

                itemAnterior = item;
            }

            FinalizaPedido(timeDay, mensagem, itensDoPedido, countErrorDuplicateItem, countError);
            #endregion

            return itensDoPedido;
        }

        //Exibe a lista de itens e eventuais mensagens de erro
        static void FinalizaPedido(string timeDay, string mensagem, string itensDoPedido, int countErrorDuplicateItem, int countError)
        {
            if (itensDoPedido.Length > 0)
            {
                //Se houver itens no pedido cria a lista
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Items included in order:");
                Console.WriteLine("-".PadRight(24, '-'));
                Console.Write(itensDoPedido);
                Console.WriteLine();
                Console.WriteLine("-".PadRight(70, '-'));
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            //Exibe eventuais mensagens de erro no pedido
            if (countErrorDuplicateItem > 0)
                if (timeDay.Equals("morning"))
                    mensagem = string.Format("{0}\n{1}", mensagem, Constantes.Mensagens.DuplicateItemRodapeManha);
                else
                    mensagem = string.Format("{0}\n{1}", mensagem, Constantes.Mensagens.DuplicateItemRodapeNoite);

            if (countError > 0)
                mensagem = string.Format("{0}\n{1}", mensagem, Constantes.Mensagens.InvalidSelection);
            else
                mensagem = Constantes.Mensagens.Success;            

            Constantes.ExibeMensagem(mensagem, ConsoleColor.Cyan, ConsoleColor.Gray);
        }
    }
}