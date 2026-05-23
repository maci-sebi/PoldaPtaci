using PoldaPtaci.Soubory;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PoldaPtaci
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameState stav = new GameState();
        private string vybranyPredmetVInventari = null;

        public MainWindow()
        {
            InitializeComponent();

            stav.CurrentScene = "Menu";
            AktualizujScenu();
        }


        private void AktualizujScenu()
        {
            GameCanvas.Children.Clear();
            //menu
            if (stav.CurrentScene == "Menu")
            {

                Image bgMenu = VytvorObrazek("Menu.png", 1920, 1080);
                Canvas.SetLeft(bgMenu, 0);
                Canvas.SetTop(bgMenu, 0);
                GameCanvas.Children.Add(bgMenu);

                // ńazev
                TextBlock txtTitul = new TextBlock
                {
                    Text = "PAČÍ POLDA",
                    Foreground = System.Windows.Media.Brushes.Black,
                    FontSize = 110,
                    FontFamily = new System.Windows.Media.FontFamily("Comic Sans MS"),
                    FontWeight = FontWeights.Bold,
                    TextAlignment = TextAlignment.Center,
                    Width = 1920
                };
                Canvas.SetTop(txtTitul, 150);
                GameCanvas.Children.Add(txtTitul);

                // spusteni hry
                Button btnStart = new Button
                {
                    Content = "SPUSTIT HRU",
                    Width = 400,
                    Height = 100,
                    FontSize = 36,
                    FontFamily = new System.Windows.Media.FontFamily("Comic Sans MS"),
                    FontWeight = FontWeights.Bold,
                    Background = System.Windows.Media.Brushes.Gold,
                    BorderBrush = System.Windows.Media.Brushes.Black,
                    BorderThickness = new Thickness(5),
                    Cursor = System.Windows.Input.Cursors.Hand
                };
                btnStart.Click += (s, e) => {
                    stav.CurrentScene = "Park"; // Přesun do první herní lokace
                    TxtDialog.Text = "Detektiv: Musím najít vajíčka paní Kosi. Podívám se po parku...";
                    AktualizujScenu();
                };
                Canvas.SetLeft(btnStart, 760);
                Canvas.SetTop(btnStart, 450);
                GameCanvas.Children.Add(btnStart);

                // ukonceni aplikace
                Button btnKonec = new Button
                {
                    Content = "ODEJÍT",
                    Width = 300,
                    Height = 80,
                    FontSize = 24,
                    FontFamily = new System.Windows.Media.FontFamily("Comic Sans MS"),
                    Background = System.Windows.Media.Brushes.LightCoral,
                    BorderBrush = System.Windows.Media.Brushes.Black,
                    BorderThickness = new Thickness(3),
                    Cursor = System.Windows.Input.Cursors.Hand
                };
                btnKonec.Click += (s, e) => Application.Current.Shutdown();
                Canvas.SetLeft(btnKonec, 810);
                Canvas.SetTop(btnKonec, 600);
                GameCanvas.Children.Add(btnKonec);

                InventoryPanel.Visibility = Visibility.Collapsed;
                TxtDialog.Visibility = Visibility.Collapsed;
                return; 
            }

            InventoryPanel.Visibility = Visibility.Visible;
            TxtDialog.Visibility = Visibility.Visible;

            // konec hry
            if (stav.GameFinished)
            {
                // prohra
                if (!stav.HasNestWithEggs)
                {
                    TextBlock txtKonec = new TextBlock
                    {
                        Text = "GAME OVER\n(Vajíčka byla rozbita)",
                        Foreground = System.Windows.Media.Brushes.Red,
                        FontSize = 60,
                        FontFamily = new System.Windows.Media.FontFamily("Comic Sans MS"),
                        FontWeight = FontWeights.Bold,
                        TextAlignment = TextAlignment.Center,
                        Width = 1920
                    };
                    Canvas.SetTop(txtKonec, 350);
                    GameCanvas.Children.Add(txtKonec);

                    VytvorTlacitkoRestart(600);
                    InventoryPanel.Children.Clear();
                    return; // zastavit vykreslovani
                }
                // vyhra
                else
                {
                    TextBlock txtVyhra = new TextBlock
                    {
                        Text = "VÍTĚZSTVÍ!\nZachránil jsi ptačí rodinu!",
                        Foreground = System.Windows.Media.Brushes.LimeGreen,
                        FontSize = 60,
                        FontFamily = new System.Windows.Media.FontFamily("Comic Sans MS"),
                        FontWeight = FontWeights.Bold,
                        TextAlignment = TextAlignment.Center,
                        Width = 1920
                    };
                    Canvas.SetTop(txtVyhra, 350);
                    GameCanvas.Children.Add(txtVyhra);

                    VytvorTlacitkoRestart(600);
                    InventoryPanel.Children.Clear();
                    return; // zastavit vykreslovani
                }
            }

            // pozadi
            string bgName = SceneManager.GetSceneBackground(stav.CurrentScene);
            Image bg = VytvorObrazek(bgName, 1920, 1080);
            Canvas.SetLeft(bg, 0);
            Canvas.SetTop(bg, 0);
            GameCanvas.Children.Add(bg);

            // vykreslovani podle aktualni sceny
            if (stav.CurrentScene == "Park")
            {
                if (stav.HasCompass)
                {
                    VytvorPrechod("Do Lesa", "Les", 0, 350, 350, 550);
                    VytvorPrechod("Do Města", "Mesto", 1200, 0, 700, 465);
                }
                if (stav.GlassOnGround)
                    VytvorPredmetNaZemi("Glass.png", "Sklo", 885, 650, 20, 20, "Našel jsi ostré sklo.");
                if (stav.FeatherOnGround)
                    VytvorPredmetNaZemi("Feather.png", "Pírko", 1790, 900, 80, 120, "Aha, ptačí pírko!");

                VytvorInterakci("Kosi.png", 1200, 600, 340, 290, (s, e) => {
                    TxtDialog.Text = CharacterManager.KlikNaKosi(stav);
                    AktualizujScenu(); // prekresli scenu popripade vyhra
                });
            }
            else if (stav.CurrentScene == "Mesto")
            {
                VytvorPrechod("Zpět do Parku", "Park", 50, 900, 150, 100);

                VytvorInterakci("Noir.png", 1450, 450, 350, 550, (s, e) => {
                    TxtDialog.Text = CharacterManager.KlikNaNoira(stav, ref vybranyPredmetVInventari);
                    AktualizujScenu(); // prekresli scenu popripade game over
                });
            }
            else if (stav.CurrentScene == "Les")
            {
                VytvorPrechod("Zpět do Parku", "Park", 620, 870, 650, 250);
                if (stav.CarolPassed) VytvorPrechod("K Potoku", "Potok", 570, 450, 450, 180);

                if (stav.BranchesOnGround) VytvorPredmetNaZemi("Branches.png", "Větvičky", 270, 950, 150, 100, "Nasbíral jsi suché větvičky.");
                if (stav.LogOnGround) VytvorPredmetNaZemi("Log.png", "Kláda", 1330, 880, 220, 150, "Těžká kláda. To se může hodit.");
                if (stav.MossOnGround) VytvorPredmetNaZemi("Moss.png", "Mech", 1700, 670, 120, 70, "Trocha měkkého mechu.");

                VytvorInterakci("Carol.png", 1200, 270, 300, 400, (s, e) => {
                    TxtDialog.Text = CharacterManager.KlikNaCarola(stav, ref vybranyPredmetVInventari);
                    AktualizujScenu();
                });
            }
            else if (stav.CurrentScene == "Potok")
            {
                VytvorPrechod("Zpět do Lesa", "Les", 0, 900, 450, 250);
                if (stav.JohnPassed) VytvorPrechod("K Buku", "Buk", 620, 300, 620, 120);

                VytvorInterakci("John.png", 1250, 500, 450, 400, (s, e) => {
                    TxtDialog.Text = CharacterManager.KlikNaJohna(stav, ref vybranyPredmetVInventari);
                    AktualizujScenu();
                });
            }
            else if (stav.CurrentScene == "Buk")
            {
                VytvorPrechod("Zpět k Potoku", "Potok", 180, 1000, 650, 140);

                VytvorInterakci("Owl.png", 1175, 375, 150, 150, (s, e) => {
                    if (!stav.OwlGaveBerries)
                    {
                        TxtDialog.Text = CharacterManagerExtension.KlikNaLovuScena(stav);
                    }
                    else
                    {
                        TxtDialog.Text = "Sova: Více ti už poradit nedokážu. Použij bobule správně.";
                    }
                    if (stav.OwlGaveBerries && !stav.Inventory.Contains("Bobule") && !stav.NoirGaveEggs)
                    {
                        stav.Inventory.Add("Bobule");
                    }
                    AktualizujInventar();
                });
            }

            AktualizujInventar();
        }

        // reset hry
        private void RestartujHru()
        {
            stav = new GameState(); // zacatek
            stav.CurrentScene = "Menu"; // Po restartu nebo po kliknutí na tlačítko skočíme zpět do Hlavního menu
            vybranyPredmetVInventari = null;
            TxtDialog.Text = "";
            AktualizujScenu();
        }

        //pomocne funkce

        private void VytvorTlacitkoRestart(double topPozice)
        {
            Button btnRestart = new Button
            {
                Content = "Hlavní menu", // Změněno z "Zkusit znovu", aby se hráč vrátil do úvodní obrazovky
                Width = 300,
                Height = 80,
                Background = System.Windows.Media.Brushes.DarkSlateGray,
                Foreground = System.Windows.Media.Brushes.White,
                FontFamily = new System.Windows.Media.FontFamily("Comic Sans MS"),
                FontSize = 24,
                FontWeight = FontWeights.Bold
            };
            btnRestart.Click += (s, e) => RestartujHru();
            Canvas.SetLeft(btnRestart, 810); // vycentrovano na 1920
            Canvas.SetTop(btnRestart, topPozice);
            GameCanvas.Children.Add(btnRestart);
        }

        private void VytvorPrechod(string text, string cilovaScena, double x, double y, double sirka, double vyska)
        {
            Button btn = new Button
            {
                Content = text,
                Width = sirka,
                Height = vyska,
                Opacity = 0.7,
                Cursor = System.Windows.Input.Cursors.Arrow,
                FontFamily = new System.Windows.Media.FontFamily("Comic Sans MS"),
                FontWeight = FontWeights.Bold
            };
            btn.Click += (s, e) => {
                stav.CurrentScene = cilovaScena;
                TxtDialog.Text = $"Přesunul jsi se: {cilovaScena}";
                AktualizujScenu();
            };
            Canvas.SetLeft(btn, x);
            Canvas.SetTop(btn, y);
            GameCanvas.Children.Add(btn);
        }

        private void VytvorPredmetNaZemi(string soubor, string nazevPredmetu, double x, double y, double sirka, double vyska, string hlaska)
        {
            Image img = VytvorObrazek(soubor, sirka, vyska);
            img.Cursor = System.Windows.Input.Cursors.Arrow;
            img.MouseLeftButtonDown += (s, e) => {
                stav.Inventory.Add(nazevPredmetu);
                if (nazevPredmetu == "Sklo") stav.GlassOnGround = false;
                if (nazevPredmetu == "Pírko") stav.FeatherOnGround = false;
                if (nazevPredmetu == "Větvičky") stav.BranchesOnGround = false;
                if (nazevPredmetu == "Kláda") stav.LogOnGround = false;
                if (nazevPredmetu == "Mech") stav.MossOnGround = false;
                TxtDialog.Text = hlaska;
                AktualizujScenu();
            };
            Canvas.SetLeft(img, x);
            Canvas.SetTop(img, y);
            GameCanvas.Children.Add(img);
        }

        private void VytvorInterakci(string soubor, double x, double y, double sirka, double vyska, RoutedEventHandler akce)
        {
            Image img = VytvorObrazek(soubor, sirka, vyska);
            img.Cursor = System.Windows.Input.Cursors.Hand;
            img.MouseLeftButtonDown += (s, e) => akce(img, e);
            Canvas.SetLeft(img, x);
            Canvas.SetTop(img, y);
            GameCanvas.Children.Add(img);
        }

        private Image VytvorObrazek(string nazevSouboru, double sirka, double vyska)
        {
            Image img = new Image();
            try
            {
                img.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/{nazevSouboru}"));
            }
            catch { }
            img.Width = sirka;
            img.Height = vyska;
            return img;
        }

        // kombinovani a inventar

        private void AktualizujInventar()
        {
            InventoryPanel.Children.Clear();
            foreach (string predmet in stav.Inventory)
            {
                string souborIkonky = ItemManager.GetImageForExtension(predmet);
                Border policko = new Border
                {
                    BorderBrush = (vybranyPredmetVInventari == predmet) ? System.Windows.Media.Brushes.Yellow : System.Windows.Media.Brushes.Gray,
                    BorderThickness = new Thickness(2),
                    Margin = new Thickness(5),
                    Width = 80,
                    Height = 80,
                    ToolTip = predmet
                };
                Image img = VytvorObrazek(souborIkonky, 70, 70);
                policko.Child = img;
                policko.MouseLeftButtonDown += (s, e) => { KlikNaPredmetVInventari(predmet); };
                InventoryPanel.Children.Add(policko);
            }
        }

        private void KlikNaPredmetVInventari(string kliknutyPredmet)
        {
            if (vybranyPredmetVInventari == null)
            {
                vybranyPredmetVInventari = kliknutyPredmet;
                TxtDialog.Text = $"Vybral jsi: {kliknutyPredmet}";
            }
            else if (vybranyPredmetVInventari == kliknutyPredmet)
            {
                vybranyPredmetVInventari = null;
                TxtDialog.Text = "Zrušil jsi výběr.";
            }
            else
            {
                ZkusKombinovat(vybranyPredmetVInventari, kliknutyPredmet);
            }
            AktualizujInventar();
        }

        private void ZkusKombinovat(string p1, string p2)
        {
            if ((p1 == "Sklo" && p2 == "Pírko") || (p1 == "Pírko" && p2 == "Sklo"))
            {
                stav.Inventory.Remove("Sklo");
                stav.Inventory.Remove("Pírko");
                stav.Inventory.Add("Ptačí kompas");
                stav.HasCompass = true;
                TxtDialog.Text = "Vznikl Ptačí kompas!";
                vybranyPredmetVInventari = null;
                AktualizujScenu();
            }
            else if ((p1 == "Větvičky" && p2 == "Mech") || (p1 == "Mech" && p2 == "Větvičky"))
            {
                stav.Inventory.Remove("Větvičky");
                stav.Inventory.Remove("Mech");
                stav.Inventory.Add("Hnízdo");
                TxtDialog.Text = "Máš parádní hnízdo!";
                vybranyPredmetVInventari = null;
                AktualizujInventar();
            }
            else
            {
                TxtDialog.Text = "To nejde zkombinovat.";
                vybranyPredmetVInventari = null;
            }
        }
    }
    public static class CharacterManagerExtension
    {
        public static string KlikNaLovuScena(GameState stav)
        {
            stav.OwlGaveBerries = true;
            return "Sova: Vím, co hledáš, detektive. Orel Noir má slabost pro sladké lesní bobule. Tady máš jedny, zkus mu je nabídnout.";
        }
    }
}