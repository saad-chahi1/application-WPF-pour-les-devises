using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestExercice
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<string, string> source = new Dictionary<string, string>();
        private Dictionary<string, string> destination = new Dictionary<string, string>();

        public MainWindow()
        {
            InitializeComponent();

            //Remplir notre premier dictionnaire
            source.Add("Séléctionner la devise source","");
            source.Add("Bitcoin", "btc-bitcoin");
            source.Add("Euro", "eur-euro-token");
            source.Add("Neurochain", "ncc-neurochain");

            //Remplir notre deuxiéme dictionnaire
            destination.Add("Séléctionner la devise destination", "");
            destination.Add("USD", "usd-us-dollars");
            destination.Add("Ethereum", "eth-ethereum");
            destination.Add("XRP", "xrp-xrp");

            //charger la 1ère combobox par les devises (source)
            var valSor = source.Keys.ToList();
            DeviseSrc.ItemsSource = valSor;

            //charger la 2ème combobox par les devises (destination)
            var valDes = destination.Keys.ToList();
            DeviseDest.ItemsSource = valDes;
        }

        public void Conver(object sender, RoutedEventArgs e)
        {
                
            if(montant.Text != "")
            {
                if(DeviseSrc.SelectedValue.ToString() != "Séléctionner la devise source")
                {
                    if (DeviseDest.SelectedValue.ToString() != "Séléctionner la devise destination")
                    {
                        int monta = Int32.Parse(montant.Text);
                        string sour = this.source[DeviseSrc.SelectedValue.ToString()];

                        string dest = this.destination[DeviseDest.SelectedValue.ToString()];

                        String URLapi = String.Format("https://api.coinpaprika.com/v1/price-converter?base_currency_id={0}&quote_currency_id={1}&amount={2}", sour, dest, monta);
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                        WebRequest request = WebRequest.Create(URLapi);
                        try
                        {
                            WebResponse response = (WebResponse)request.GetResponse();
                            using (Stream stream = response.GetResponseStream())
                            {
                                StreamReader streamReader = new StreamReader(stream);
                                var responseValues = JObject.Parse(streamReader.ReadToEnd());
                                Affichage.Content = responseValues["price"];
                            }

                        }
                        catch (WebException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("séléctionnez votre devise destination");
                    }
                }
                else
                {
                    MessageBox.Show("séléctionnez votre devise source");
                }       
            }
            else
            {
                MessageBox.Show("s'il vous plait saisissez votre montant");
            }
        }

    }
}
