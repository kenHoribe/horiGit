using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

namespace MultiLanguageSample {
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
			DataContext = new MainWindowModel() { LanguageIndex = 0 };
		}

		private string[] CultureList = { "ja-JP", "en-US", "zh-CN" };

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			var vm = DataContext as MainWindowModel;
			ResourceService.Current.ChangeCulture(CultureList[vm.LanguageIndex]);

            vm.RaisePropertyChange("MyImage");
        }

		class MainWindowModel: INotifyPropertyChanged
        {

            public int LanguageIndex { get; set; }

            public string MyImage
            {
                get
                {
                    return Properties.Resources.pikminImg; 
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            private void NotifyPropertyChanged(String info)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(info));
                }
            }

            public void RaisePropertyChange(string propertyname)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
                }
            }

        }
	}
}
