using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
using T2_FP_PROGRAMADORESPRO.Structs;

namespace T2_FP_PROGRAMADORESPRO
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Medicamento> p_medicines { get; set; } = new ObservableCollection<Medicamento>();
        public MainWindow()
        {
            InitializeComponent();
            MouseLeftButtonDown += (s, e) => { if (e.LeftButton == MouseButtonState.Pressed) this.DragMove(); };
            p_btn_minimize.Click += (s, e) => { this.WindowState = WindowState.Minimized; };
            p_btn_shutdown.Click += (s, e) => { Application.Current.Shutdown(); };
            p_btn_register.Click += RegistrarMedicamento;
            p_ListView_medicine.PreviewMouseLeftButtonDown += SelectedItem;
            p_btn_elementExists.Click += ElementFound;
            p_btn_eliminarmedicamento.Click += DeleteMedicine;
        }
        private void RegistrarMedicamento(object sender, EventArgs e)
        {
            try
            {
                Medicamento p_medicamento = new Medicamento(p_txtbox_name.Text, Convert.ToInt32(p_txtbox_count.Text), float.Parse(p_txtbox_price.Text));
                p_medicines.Add(p_medicamento);
                if(p_medicines.Count >= 2)
                {
                    MergedSort(p_medicines);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private void SelectedItem(object sender, EventArgs e)
        {
            if (p_ListView_medicine.SelectedItem == null) return;
            Medicamento temp = (Medicamento) p_ListView_medicine.SelectedItem;
            p_txt_visualize_name.Text = temp.P_name;
            p_txt_visualize_code.Text = temp.P_code;
            p_txt_visualize_unitprice.Text = temp.P_unitPrice.ToString();
        }
        private void MergedSort(ObservableCollection<Medicamento> collection)
        {
            //condición para terminar la recursividad
            if (collection.Count <= 1) return;
            //crear ObservableCollection left y rigth
            ObservableCollection<Medicamento> leftcollection = new ObservableCollection<Medicamento>();
            ObservableCollection<Medicamento> rightcollection = new ObservableCollection<Medicamento>();
            //llenar Left y right Observable Collection
            int j = collection.Count / 2;
            for (int i = 0; i < collection.Count; i++)
            {
                if (i < j)
                {
                    leftcollection.Add(collection[i]);
                }
                else
                {
                    rightcollection.Add(collection[i]);
                }
            }
            //recursividad
            MergedSort(leftcollection);
            MergedSort(rightcollection);
            //merged
            Merged(collection, leftcollection, rightcollection);
        }
        private void Merged(ObservableCollection<Medicamento> collection, ObservableCollection<Medicamento> leftCollection, ObservableCollection<Medicamento> rightCollection)
        {
            //tamaño de las collecciones
            int leftsize = leftCollection.Count;
            int rightsize = rightCollection.Count;
            collection.Clear();
            //iteradores
            int i = 0; int j = 0;
            while (i < leftsize && j < rightsize)
            {
                if (string.Compare(leftCollection[i].P_name, rightCollection[i].P_name, StringComparison.InvariantCultureIgnoreCase) == -1 || 
                    string.Compare(leftCollection[i].P_name, rightCollection[i].P_name, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    collection.Add(leftCollection[i]);
                    i++;
                }
                else
                {
                    collection.Add(rightCollection[j]);
                    j++;
                }
            }
            while (i < leftsize)
            {
                collection.Add(leftCollection[i]);
                i++;
            }
            while (j < rightsize)
            {
                collection.Add(rightCollection[j]);
                j++;
            }
        }
        private void ElementFound(object sender, EventArgs e)
        {
            if (p_txtbox_elementexists.Text == "")
            {
                p_txt_elementFound.Text = "Cadena vacía";
                return;
            }
            int count = 0;
            for(int i = 0; i < p_medicines.Count; i++)
            {
                if (p_medicines[i].P_name.Equals(p_txtbox_elementexists.Text))
                {
                    count++;
                }
            }
            p_txt_elementFound.Text = $"medicamentos coincidentes: {count}";
        }
        private void DeleteMedicine(object sender, EventArgs e)
        {
            if (p_txtbox_eliminarMedicamento.Text == "")
            {
                p_txt_elementoEliminadoGlobo.Text = "Cadena vacía.";
                return;
            }
            else if (p_txtbox_eliminarMedicamento.Text.Length < 10)
            {
                p_txt_elementoEliminadoGlobo.Text = "Cadena muy pequeña";
                return;
            }
            for (int i = 0; i < p_medicines.Count; i++)
            {
                if (p_medicines[i].P_code.Equals(p_txtbox_eliminarMedicamento.Text))
                {
                    p_medicines.RemoveAt(i);
                    Medicamento.RemoveCode(p_txtbox_eliminarMedicamento.Text);
                    MergedSort(p_medicines);
                    p_txt_elementoEliminadoGlobo.Text = "medicina eliminada";
                    return;
                }
            }
            p_txt_elementoEliminadoGlobo.Text = "No se halló medicina";
        }
    }
}
