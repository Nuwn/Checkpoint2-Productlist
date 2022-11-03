using System;

namespace Checkpoint2_Productlist.App
{
    public static class ViewController
    {
        public static void StartView(InputController inputController)
        {
            Console.Clear();
            Console.WriteLine("Welcome!");
            Console.WriteLine();
            InputController.AwaitTextCommand(ConsoleColor.Magenta,
                ("New category - \"C\"", "C", () => {
                    inputController.ChangeView(View.CategoryCreation);
                }),
                ("New product - \"P\"", "P", () => { 
                    inputController.ChangeView(View.ProductCreation);
                }),
                ("List all products - \"L\"", "L", () => {
                    inputController.ChangeView(View.ProductDisplay);
                }),
                ("Quit - \"Q\"", "Q", () => {
                    inputController.ChangeView(View.End);
                })
            ); 
        }

        public static void CategoryCreationView(List<Category> categories, InputController inputController)
        {
            InputController.AwaitTextInput("Enter a new category name: ",(text) =>
            {
                if (text == null || text == string.Empty)
                {
                    ConsoleEx.WriteColoredLine("Unable to add category, try again.", ConsoleColor.Red);
                }
                else
                {
                    categories.Add(new Category(text));
                    ConsoleEx.WriteColoredLine("Category successfully Added.", ConsoleColor.Green);
                }
            }, ConsoleColor.DarkYellow);

            Console.WriteLine();
            InputController.AwaitTextCommand(ConsoleColor.Magenta,
                ("New category - \"C\"", "C", () => {
                    inputController.ChangeView(View.CategoryCreation);
                }),
                ("New product - \"P\"", "P", () => {
                    inputController.ChangeView(View.ProductCreation);
                }),
                ("Return - \"R\"", "R", () => {
                    inputController.ChangeView(View.Start);
                }),
                ("Quit - \"Q\"", "Q", () => {
                    inputController.ChangeView(View.End);
                })
            );
        }

        public static void ProducCreationView(List<Product> products, List<Category> categories, InputController inputController)
        {
            if(categories.Count == 0)
            {
                Console.WriteLine("No categories added, please add categories before adding products,");
                inputController.ChangeView(View.CategoryCreation);
                return;
            }

            bool cancelled = false;
            string name = "";
            double price = default;
            Category category = default;

            ConsoleEx.WriteColoredLine("Create new product...(type \"cancel\" to cancel)", ConsoleColor.DarkYellow);
            void TryAddProductName() => InputController.AwaitTextInput("Enter a product title: ", (text) =>
            {
                if (cancelled) return;
                if (text != null && text != string.Empty)
                {
                    if (text == "cancel")
                    {
                        inputController.ChangeView(View.Start);
                        cancelled = true;
                        return;
                    }
                    name = text;
                    return;
                }

                Console.WriteLine("Incorrect input, try again...");
                TryAddProductName();
            });

            void TryAddProductPrice() => InputController.AwaitTextInput("Enter a product price: (e.g 199,25) ", (text) =>
            {
                if (cancelled) return;
                if (text != null && text != string.Empty)
                {
                    if (text == "cancel")
                    {
                        inputController.ChangeView(View.Start);
                        cancelled = true;
                        return;
                    }

                    if (double.TryParse(text, out double n))
                    {
                        price = n;
                        return;
                    }
                }
                Console.WriteLine("Incorrect input, try again...");
                TryAddProductPrice();
            });

            void TryAddProductCategory() => InputController.AwaitTextInput("Enter a category: ", (text) => 
            {
                if (cancelled) return;
                
                if (text != null && text != string.Empty)
                {
                    if (text == "cancel")
                    {
                        inputController.ChangeView(View.Start);
                        cancelled = true;
                        return;
                    }

                    if (int.TryParse(text, out int n))
                    {
                        if(n >= categories.Count)
                        {
                            Console.WriteLine("Incorrect input, try again...");
                            TryAddProductCategory();
                            return;
                        }

                        category = categories[n];
                        return;
                    }
                }

                Console.WriteLine("Incorrect input, try again...");
                TryAddProductCategory();
            });

            if (!cancelled) TryAddProductName();
            if (!cancelled) TryAddProductPrice();
            if (!cancelled)
            {
                Console.WriteLine();
                ConsoleEx.WriteColoredLine("Available categories (Enter number):", ConsoleColor.DarkYellow);
                int i = 0;
                categories.ForEach(x => {
                    Console.WriteLine($"{i}. {x.name}");
                    i++;
                });
                TryAddProductCategory();
            }
            if (!cancelled)
            {
                products.Add(new Product(name, price, category));
                ConsoleEx.WriteColoredLine("Product successfully added.", ConsoleColor.Green);

                Console.WriteLine();
                InputController.AwaitTextCommand(ConsoleColor.Magenta,
                    ("New product - \"P\"", "P", () => {
                        inputController.ChangeView(View.ProductCreation);
                    }),
                    ("List all products - \"L\"", "L", () => {
                        inputController.ChangeView(View.ProductDisplay);
                    }),
                    ("Return - \"R\"", "R", () => {
                        inputController.ChangeView(View.Start);
                    }),
                    ("Quit - \"Q\"", "Q", () => {
                        inputController.ChangeView(View.End);
                    })
                );
            } 
        }

        public static void ProductDisplayView(List<Product> products, InputController inputController)
        {
            Console.Clear();
            Console.WriteLine((products.Count == 0) ? "No products...": "Listing products...");
            Console.WriteLine();
            Console.Write("Category".PadRight(15));
            Console.Write("Title".PadRight(25));
            Console.Write("Price");
            Console.WriteLine("\n".PadRight(50, '-'));


            int i = 0;
            products.OrderBy(x => x.Price).ToList().ForEach(x => {
                if(i != 0) Console.WriteLine();
                Console.Write($"{FormatText(x.Category.name, 10)}".PadRight(15));
                Console.Write($"{FormatText(x.Title, 20)}".PadRight(25));
                Console.Write($"{FormatText(x.Price.ToString(), 10)};-");
                i++;
            });
            Console.WriteLine();

            double totalPrice = products.Sum(x => x.Price);
            Console.WriteLine(String.Format("{0, 15}{1, -25}{2, 0};-", " ", "Total price:", totalPrice));

            Console.WriteLine();
            Console.WriteLine();
            InputController.AwaitTextCommand(ConsoleColor.Magenta,
                ("Search - \"S\"", "S", () => {
                    SearchDisplayView(products, inputController);
                }),
                ("Return - \"R\"", "R", () => {
                    inputController.ChangeView(View.Start);
                }),
                ("Quit - \"Q\"", "Q", () => {
                    inputController.ChangeView(View.End);
                })
            );
        }


        public static void SearchDisplayView(List<Product> products, InputController inputController)
        {
            ConsoleEx.WriteColored("Search for title: ", ConsoleColor.DarkCyan);
            string? searchQuery = Console.ReadLine();

            List<Product> results = products.FindAll((x) => x.Title == searchQuery);

            ProductDisplayView(results, inputController);
        }

        public static void EndView()
        {
            Console.WriteLine("Bye!");
        }
        private static string FormatText(string text, int maxLength) => (text.Length > maxLength) ? string.Concat(text.AsSpan(0, maxLength - 3), "...") : text;

    }
}