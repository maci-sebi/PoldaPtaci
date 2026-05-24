using PoldaPtaci.Soubory;
using System;
using System.Collections.Generic;
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
        private AktivniDialog beziciDialog = null; 

        public MainWindow()
        {
            InitializeComponent();

            TxtDialog.PreviewMouseLeftButtonDown += TxtDialog_Click;

            stav.CurrentScene = "Menu";
            AktualizujScenu();
        }

        private void AktualizujScenu()
        {
            GameCanvas.Children.Clear();

            // menu
            if (stav.CurrentScene == "Menu")
            {
                Image bgMenu = VytvorObrazek("Menu.png", 1920, 1080);
                Canvas.SetLeft(bgMenu, 0);
                Canvas.SetTop(bgMenu, 0);
                GameCanvas.Children.Add(bgMenu);

                TextBlock txtTitul = new TextBlock
                {
                    Text = "POLDA PTÁCI",
                    Foreground = System.Windows.Media.Brushes.Black,
                    FontSize = 120,
                    FontFamily = new System.Windows.Media.FontFamily("Comic Sans MS"),
                    FontWeight = FontWeights.Bold,
                    TextAlignment = TextAlignment.Center,
                    Width = 1920
                };
                Canvas.SetTop(txtTitul, 120);
                GameCanvas.Children.Add(txtTitul);
                //spustit btn
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
                    stav.CurrentScene = "Park";
                    TxtDialog.Text = "Detektiv: Musím najít vajíčka paní Kosi. Podívám se po parku...";
                    AktualizujScenu();
                };
                Canvas.SetLeft(btnStart, 760);
                Canvas.SetTop(btnStart, 380);
                GameCanvas.Children.Add(btnStart);
                //ukoncit btn
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
                Canvas.SetTop(btnKonec, 510);
                GameCanvas.Children.Add(btnKonec);
                //popis hry
                TextBlock txtPopisHry = new TextBlock
                {
                    Text = "Humorná point-and-click adventura. Paní Kosi se ztratila vajíčka a v ptačím světě je plno podezřelých! Dokážeš jako elitní ptačí detektiv vyřešit tento případ, prohledat park i les a zachránit ptačí rodinu?",
                    Foreground = System.Windows.Media.Brushes.Black,
                    FontSize = 22,
                    FontFamily = new System.Windows.Media.FontFamily("Comic Sans MS"),
                    TextAlignment = TextAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                    Width = 1200
                };
                Canvas.SetLeft(txtPopisHry, 360);
                Canvas.SetTop(txtPopisHry, 700);
                GameCanvas.Children.Add(txtPopisHry);

                InventoryPanel.Visibility = Visibility.Collapsed;
                TxtDialog.Visibility = Visibility.Collapsed;
                return;
            }

            InventoryPanel.Visibility = Visibility.Visible;
            TxtDialog.Visibility = Visibility.Visible;

            // konec hry
            if (stav.GameFinished)
            {
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
                    return;
                }
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
                    return;
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
                    VytvorPrechod("Do Lesa", "Les", 60, 650, 150, 100);
                    VytvorPrechod("Do Města", "Mesto", 1400, 400, 150, 100);
                }
                if (stav.GlassOnGround)
                    VytvorPredmetNaZemi("Glass.png", "Sklo", 885, 650, 20, 20, "Našel jsi ostré sklo.");
                if (stav.FeatherOnGround)
                    VytvorPredmetNaZemi("Feather.png", "Pírko", 1790, 900, 80, 120, "Aha, ptačí pírko!");

                VytvorInterakci("Kosi.png", 1200, 600, 340, 290, (s, e) => {
                    if (beziciDialog == null)
                    {
                        var repliky = CharacterManager.GetDialogKosi(stav);
                        beziciDialog = new AktivniDialog(repliky, () => {
                            if (stav.HasNestWithEggs)
                            {
                                stav.GameFinished = true;
                            }
                            AktualizujScenu();
                        });
                        TxtDialog.Text = beziciDialog.ZobrazAktualni();
                    }
                });
            }
            else if (stav.CurrentScene == "Mesto")
            {
                VytvorPrechod("´Do Parku", "Park", 50, 900, 150, 100);

                VytvorInterakci("Noir.png", 1450, 450, 350, 550, (s, e) => {
                    if (beziciDialog == null)
                    {
                        string pouzityPredmet = vybranyPredmetVInventari;

                        var repliky = CharacterManager.GetDialogNoira(stav, ref vybranyPredmetVInventari);
                        beziciDialog = new AktivniDialog(repliky, () => {
                            
                            if (pouzityPredmet == "Bobule")
                            {
                                stav.Inventory.Remove("Bobule");

                                if (stav.Inventory.Contains("Hnízdo"))
                                {
                                    stav.Inventory.Remove("Hnízdo"); 
                                    stav.Inventory.Add("Hnízdo s vajíčky"); 
                                    stav.NoirGaveEggs = true;
                                    stav.HasNestWithEggs = true; 
                                }
                                else
                                {
                                    stav.NoirGaveEggs = true;
                                    stav.HasNestWithEggs = false; 
                                    stav.GameFinished = true; 
                                }

                                vybranyPredmetVInventari = null;
                            }
                            
                            AktualizujScenu(); 
                        });
                        TxtDialog.Text = beziciDialog.ZobrazAktualni();
                    }
                });
            }
            else if (stav.CurrentScene == "Les")
            {
                VytvorPrechod("Do Parku", "Park", 870, 970, 150, 100);
                if (stav.CarolPassed) VytvorPrechod("K Potoku", "Potok", 855, 560, 150, 100);

                if (stav.BranchesOnGround) VytvorPredmetNaZemi("Branches.png", "Větvičky", 270, 950, 150, 100, "Nasbíral jsi suché větvičky.");
                if (stav.LogOnGround) VytvorPredmetNaZemi("Log.png", "Kláda", 1330, 880, 220, 150, "Těžká kláda. To se může hodit.");
                if (stav.MossOnGround) VytvorPredmetNaZemi("Moss.png", "Mech", 1700, 670, 120, 70, "Trocha měkkého mechu.");

                VytvorInterakci("Carol.png", 1200, 270, 300, 400, (s, e) => {
                    if (beziciDialog == null)
                    {
                        var repliky = CharacterManager.GetDialogCarola(stav, ref vybranyPredmetVInventari);
                        beziciDialog = new AktivniDialog(repliky, () => {
                            AktualizujScenu();
                        });
                        TxtDialog.Text = beziciDialog.ZobrazAktualni();
                    }
                });
            }
            else if (stav.CurrentScene == "Potok")
            {
                VytvorPrechod("Do Lesa", "Les", 50, 900, 150, 100);
                if (stav.JohnPassed) VytvorPrechod("K Buku", "Buk", 600, 300, 150, 100);

                VytvorInterakci("John.png", 1250, 500, 450, 400, (s, e) => {
                    if (beziciDialog == null)
                    {
                        var repliky = CharacterManager.GetDialogJohna(stav, ref vybranyPredmetVInventari);
                        beziciDialog = new AktivniDialog(repliky, () => {
                            AktualizujScenu();
                        });
                        TxtDialog.Text = beziciDialog.ZobrazAktualni();
                    }
                });
            }
            else if (stav.CurrentScene == "Buk")
            {
                VytvorPrechod("K Potoku", "Potok", 430, 1000, 200, 80);

                VytvorInterakci("Owl.png", 1175, 375, 150, 150, (s, e) => {
                    if (beziciDialog == null)
                    {
                        var repliky = CharacterManager.GetDialogSovy(stav);
                        beziciDialog = new AktivniDialog(repliky, () => {
                            if (!stav.OwlGaveBerries)
                            {
                                stav.OwlGaveBerries = true;
                                if (!stav.Inventory.Contains("Bobule") && !stav.NoirGaveEggs)
                                {
                                    stav.Inventory.Add("Bobule");
                                }
                            }
                            AktualizujScenu();
                        });
                        TxtDialog.Text = beziciDialog.ZobrazAktualni();
                    }
                });
            }

            AktualizujInventar();
        }

        //preklikavani dialogu
        private void TxtDialog_Click(object sender, MouseButtonEventArgs e)
        {
            if (beziciDialog != null)
            {
                if (beziciDialog.DalsiReplika())
                {
                    TxtDialog.Text = beziciDialog.ZobrazAktualni();
                }
                else
                {
                    beziciDialog = null;
                }
            }
        }

        private void RestartujHru()
        {
            stav = new GameState();
            stav.CurrentScene = "Menu";
            vybranyPredmetVInventari = null;
            beziciDialog = null;
            TxtDialog.Text = "";
            AktualizujScenu();
        }

        private void VytvorTlacitkoRestart(double topPozice)
        {
            Button btnRestart = new Button
            {
                Content = "Hlavní menu",
                Width = 300,
                Height = 80,
                Background = System.Windows.Media.Brushes.DarkSlateGray,
                Foreground = System.Windows.Media.Brushes.White,
                FontFamily = new System.Windows.Media.FontFamily("Comic Sans MS"),
                FontSize = 24,
                FontWeight = FontWeights.Bold
            };
            btnRestart.Click += (s, e) => RestartujHru();
            Canvas.SetLeft(btnRestart, 810);
            Canvas.SetTop(btnRestart, topPozice);
            GameCanvas.Children.Add(btnRestart);
        }

        private void VytvorPrechod(string text, string cilovaScena, double x, double y, double sirka, double vyska)
        {
            TextBlock txtPrechod = new TextBlock
            {
                Text = text,
                Width = sirka,
                Height = vyska,
                FontSize = 28,
                Foreground = System.Windows.Media.Brushes.Yellow,
                FontFamily = new System.Windows.Media.FontFamily("Comic Sans MS"),
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
                Cursor = System.Windows.Input.Cursors.Hand
            };

            txtPrechod.MouseLeftButtonDown += (s, e) => {
                stav.CurrentScene = cilovaScena;
                TxtDialog.Text = $"Přesunul jsi se: {cilovaScena}";
                beziciDialog = null;
                AktualizujScenu();
            };

            Canvas.SetLeft(txtPrechod, x);
            Canvas.SetTop(txtPrechod, y);
            GameCanvas.Children.Add(txtPrechod);
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
}