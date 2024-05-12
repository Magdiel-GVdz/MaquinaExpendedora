using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MaquinaExpendedora
{
    /*
    Simulación del funcionamiento de una máquina expendedora creando una
    operación donde reciba dinero (array) y un número donde indique la selección del
    producto y el monto que lleva
    Criterios: 
    El programa debe regresar el nombre de un producto y el cambio (dinero).
    Si falta dinero o el producto no existe, se deberá notificar y retornar todas las monedas.
    Si no hay dinero cambio, se notificará.
    El programa solo aceptará los dígitos de 5, 10, 50, 100 y 200, de lo contrario se notificará.
     */

    /// <summary>
    /// Representa un producto con un nombre y un precio.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// El nombre del producto.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// El precio del producto.
        /// </summary>
        public int price { get; set; }

        /// <summary>
        /// Constructor de la clase Product.
        /// </summary>
        /// <param name="name">El nombre del producto.</param>
        /// <param name="price">El precio del producto.</param>
        public Product(string name, int price)
        {
            this.name = name;
            this.price = price;
        }
    }

    public class VendingMachine
    {
        /// <summary>
        /// Diccionario que contiene los productos disponibles en la máquina expendedora.
        /// </summary>
        public Dictionary<int, Product> products { get; set; }
        /// <summary>
        /// Diccionario que contiene las monedas disponibles en la máquina expendedora.
        /// </summary>
        public Dictionary<int, int> coins { get; set; }
        /// <summary>
        /// Constructor de la clase VendingMachine.
        /// </summary>
        /// <param name="products">Diccionario con los productos disponibles en la máquina expendedora.</param>
        public VendingMachine(Dictionary<int, Product> products) 
        {
            this.products = products;
            coins = new Dictionary<int, int>()
            {
                { 5, 100 }, 
                { 10, 100 }, 
                { 50, 50 }, 
                { 100, 10 }, 
                { 200, 10 }
            };
        }

        /// <summary>
        /// Verifica si el producto existe en la máquina expendedora.
        /// </summary>
        /// <param name="id">Identificador del producto.</param>
        /// <returns>True si el producto existe, false de lo contrario.</returns>
        private bool productExist(int id)
        {
            if (!products.ContainsKey(id)) 
            {
                Console.WriteLine("Product not found\n");
                return false;
            } 
            return true;

        }

        /// <summary>
        /// Verifica si las monedas ingresadas son válidas.
        /// </summary>
        /// <param name="money">Arreglo con las monedas ingresadas.</param>
        /// <returns>True si las monedas son válidas, false de lo contrario.</returns>
        private bool isValidCoin(int[] money)
        {
            foreach (var coin in money)
            {
                if (!this.coins.ContainsKey(coin)) 
                {
                    Console.WriteLine("Invalid coin: " + coin + "\n Only 5, 10, 50, 100 and 200 are accepted\n");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Verifica si hay suficiente dinero para comprar el producto.
        /// </summary>
        /// <param name="money">Arreglo con las monedas ingresadas.</param>
        /// <param name="selectedProduct">Identificador del producto seleccionado.</param>
        /// <returns>True si hay suficiente dinero, false de lo contrario.</returns>
        private bool sufficientMoney(int[] money, int selectedProduct)
        {
            if (!(money.Sum() >= products[selectedProduct].price))
            {
                Console.WriteLine("Insufficient money\n");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Verifica si hay suficiente cambio para la compra.
        /// </summary>
        /// <param name="money">Arreglo con las monedas ingresadas.</param>
        /// <param name="selectedProduct">Identificador del producto seleccionado.</param>
        /// <returns>True si hay suficiente cambio, false de lo contrario.</returns>
        private bool sufficientChange(int[] money, int selectedProduct)
        {
            int moneyToPay = money.Sum();
            int productPrice = products[selectedProduct].price;
            int change = moneyToPay - productPrice;
            Console.WriteLine("Money to pay: " + moneyToPay);
            Console.WriteLine("Product price: " + productPrice);
            Console.WriteLine("Change: " + change + "\n");

            var copyCoins = new Dictionary<int, int>(coins);

            do
            {
                if (change >= 200 && copyCoins[200] > 0)
                {
                    change -= 200;
                    copyCoins[200]--;
                }
                else if (change >= 100 && copyCoins[100] > 0)
                {
                    change -= 100;
                    copyCoins[100]--;
                }
                else if (change >= 50 && copyCoins[50] > 0)
                {
                    change -= 50;
                    copyCoins[50]--;
                }
                else if (change >= 10 && copyCoins[10] > 0)
                {
                    change -= 10;
                    copyCoins[10]--;
                }
                else if (change >= 5 && copyCoins[5] > 0)
                {
                    change -= 5;
                    copyCoins[5]--;
                }
                else if (change == 0)
                {
                    Console.WriteLine("Change generated \n");
                    return true;
                }
                else
                {
                    Console.WriteLine("Insufficient change\n");
                    return false;
                }


            } while (change >= 0);
            Console.WriteLine("Change: " + change + "\n");
            return false;
        }

        /// <summary>
        /// Agrega las monedas ingresadas a la máquina expendedora.
        /// </summary>
        /// <param name="coins">Arreglo con las monedas ingresadas.</param>
        public void addCoins(int[] coins)
        {
            foreach (var coin in coins)
            {
                if (this.coins.ContainsKey(coin))
                {
                    this.coins[coin]++;
                }
            }
        }

        /// <summary>
        /// Compra un producto de la máquina expendedora.
        /// </summary>
        /// <param name="selectedProduct">Identificador del producto seleccionado.</param>
        /// <param name="money">Arreglo con las monedas ingresadas.</param>
        /// <returns>Diccionario con el cambio generado, o null si no se puede comprar.</returns>
        public Dictionary<int, int> buyProduct(int selectedProduct, int[] money)
        {
            if (isValidCoin(money) && productExist(selectedProduct) && sufficientMoney(money, selectedProduct) && sufficientChange(money, selectedProduct))
            {
                int moneyToPay = money.Sum();
                int productPrice = products[selectedProduct].price;
                int change = moneyToPay - productPrice;

                var copyCoins = new Dictionary<int, int>(coins);
                Console.WriteLine("Coins in machine before purchase: ");
                foreach (var coin in copyCoins.Keys)
                {
                    Console.WriteLine(coin + ": " + copyCoins[coin]);
                }
                Console.WriteLine("\n");

                addCoins(money);

                Dictionary<int, int> coinsChange = new Dictionary<int, int>()
                {
                    { 5, 0 },
                    { 10, 0 },
                    { 50, 0 },
                    { 100, 0 },
                    { 200, 0 }
                };

                do
                {
                    if (change >= 200 && coins[200] > 0)
                    {
                        change -= 200;
                        coins[200]--;
                        coinsChange[200]++;
                    }
                    else if (change >= 100 && coins[100] > 0)
                    {
                        change -= 100;
                        coins[100]--;
                        coinsChange[100]++;
                    }
                    else if (change >= 50 && coins[50] > 0)
                    {
                        change -= 50;
                        coins[50]--;
                        coinsChange[50]++;
                    }
                    else if (change >= 10 && coins[10] > 0)
                    {
                        change -= 10;
                        coins[10]--;
                        coinsChange[10]++;
                    }
                    else if (change >= 5 && coins[5] > 0)
                    {
                        change -= 5;
                        coins[5]--;
                        coinsChange[5]++;
                    }
                    else if (change == 0)
                    {
                        Console.WriteLine("Buy successful\n");
                        return coinsChange;
                    }

                } while (change >= 0);
                return coinsChange;
            }
            else
            {
                Console.WriteLine("Cannot buy\n");
                return null;

            }

        }

    }
    internal class Program
    {
        static void Main(string[] args)
        {
            // Crear productos
            var products = new Dictionary<int, Product>
            {
                { 1, new Product("Coca Cola", 100) },
                { 2, new Product("Pepsi", 75) },
                { 3, new Product("Fanta", 80) },
                { 4, new Product("Sprite", 125) },
                { 5, new Product("Water", 15) },
            };

            // Crear máquina expendedora
            var vendingMachine = new VendingMachine(products);

            int selection = -1;

            do
            {
                Console.WriteLine("########### Welcome to the vending machine ###########\n");
                Console.WriteLine("Select a product id or enter 0 to exit");
                // Imprimir productos con id, nombre y precio
                Console.WriteLine("ID - Name - Price");
                foreach (var product in products)
                {
                    Console.WriteLine(product.Key + " - " + product.Value.name + " - $" + product.Value.price);
                }
                Console.WriteLine("\n");

                // Obtener y validar producto
                string input = Console.ReadLine();
                if (int.TryParse(input, out selection))
                {
                    Console.WriteLine($"The entered number is: {selection}");
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                }

                
                if (selection != 0)
                {
                    // Ingresar monedas
                    Console.WriteLine("Insert coins separated by spaces");
                    Console.WriteLine("Accepted coins: 5, 10, 50, 100, 200\n");
                    int[] coins = Console.ReadLine().Trim().Split(' ').Select(int.Parse).ToArray();
                    Console.WriteLine("Inserted coins: ");
                    foreach (var coin in coins)
                    {
                        Console.Write(coin + " ");
                    }
                    Console.WriteLine("\n");

                    // Comprar producto
                    var change = vendingMachine.buyProduct(selection, coins);
                    if (change != null)
                    {
                        // Mostrar cambio
                        Console.WriteLine("Change: ");
                        foreach (var coin in change)
                        {
                            if (coin.Value > 0)
                            {
                                Console.WriteLine(coin.Value + " coins of " + coin.Key + " ");
                            }
                        };
                        Console.WriteLine("\n");
                    };
                    // Imprimir monedas en la maquina
                    Console.WriteLine("Coins in machine after purchase: ");
                    foreach (var coin in vendingMachine.coins.Keys)
                    {
                        Console.WriteLine(coin + ": " + vendingMachine.coins[coin]);
                    }
                    Console.WriteLine("\n");
                }

            } while (selection != 0);
        }
    }
}
