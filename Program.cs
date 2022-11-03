using Checkpoint2_Productlist.App;

View currentView = View.Start;
bool appIsQuitting = false;

List<Category> categories = new();
List<Product> products = new();

InputController inputController = new((view) => currentView = view);

void ProgramLoop()
{
    while (!appIsQuitting)
    {
        switch (currentView)
        {
            case View.Start:
                ViewController.StartView(inputController);
                break;
            case View.CategoryCreation:
                ViewController.CategoryCreationView(categories, inputController);
                break;
            case View.ProductCreation:
                ViewController.ProducCreationView(products, categories, inputController);
                break;
            case View.ProductDisplay:
                ViewController.ProductDisplayView(products, inputController);
                break;
            case View.End:
                ViewController.EndView();
                appIsQuitting = true;
                break;
            default:
                ViewController.StartView(inputController);
                break;
        }
    }
}

ProgramLoop();