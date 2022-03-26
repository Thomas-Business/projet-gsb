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
using LiveCharts;
using LiveCharts.Wpf;
using LibraryOfClass;
using System.Linq;

namespace GSB_grp.Vues
{
    /// <summary>
    /// Logique d'interaction pour PagesStatistique.xaml
    /// </summary>
    public partial class PagesStatistique : Page
    {
        GstBDD gstBDD;
        GstGlobale gstGlobale;
        public PagesStatistique()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            gstBDD = new GstBDD();
            gstGlobale = new GstGlobale();

            cboGraph.ItemsSource = gstBDD.getAllMedicament();


            //Debut des actions sur le GraphMedoParFam :
            PieSeries ps;
            ChartValues<int> line;
            Dictionary<string, int> lesDatasGraphTypeAbo = new Dictionary<string, int>();

            lesDatasGraphTypeAbo = gstBDD.getNbMedocByFam();
            foreach (string cle in lesDatasGraphTypeAbo.Keys)
            {
                ps = new PieSeries();
                line = new ChartValues<int>();
                line.Add(lesDatasGraphTypeAbo[cle]);
                ps.Values = line;
                ps.Title = cle;
                ps.DataLabels = true;
                GraphMedoParFam.Series.Add(ps);
            }
            GraphMedoParFam.LegendLocation = LegendLocation.Bottom;
            //Fin des actions sur le GraphMedoParFam

            //Nouveau graphique
            ColumnSeries cs = new ColumnSeries();
            cs.Fill = Brushes.Red;
            ChartValues<double> line2 = new ChartValues<double>();

            Dictionary<string, double> lesDatas2 = new Dictionary<string, double>();

            lesDatas2 = gstBDD.getMedPrixEleve();

            foreach (string cle in lesDatas2.Keys)
            {
                line2.Add(lesDatas2[cle]);
            }
            cs.Values = line2;

            grapheMedPresc.Series.Clear();
            grapheMedPresc.AxisX.Clear();
            Axis axe = new Axis();
            axe.Labels = lesDatas2.Keys.ToList();
            grapheMedPresc.AxisX.Add(axe);
            grapheMedPresc.Series.Add(cs);
            cs.Title = "Les 10 médicamements les plus chère";
            cs.DataLabels = true;

            grapheMedPresc.LegendLocation = LegendLocation.Top;
            //Fin du nouveau graphique





        }
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }



        private void cboGraph_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cboGraph.SelectedItem != null)
            {
                Dictionary<string, int> lesDatas = gstGlobale.getNbMedPertnonPert((cboGraph.SelectedItem as Medicament).Med_depotlegal);

                int nbNonPert = lesDatas["NonPerturbateur"];
                int nbPert = lesDatas["Perturbateurs"];

                SeriesCollection = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Vue Globale",
                        Values = new ChartValues<int> { nbNonPert, nbPert }
                    }
                };

                Labels = new[] { "Non Perturbateurs", "Perturbateurs" };
                Formatter = value => value.ToString("N");
                DataContext = this;

            }
        }
    }
}
