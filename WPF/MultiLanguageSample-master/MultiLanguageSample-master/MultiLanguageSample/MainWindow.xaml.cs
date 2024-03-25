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
			var bindData = DataContext as MainWindowModel;
			ResourceService.Current.ChangeCulture(CultureList[bindData.LanguageIndex]);

            bindData.MyImage = Properties.Resources.pikminImg;
        }

		class MainWindowModel: INotifyPropertyChanged {

            private string _MyImage = (string)Application.Current.Resources["pikminImg"];

            public int LanguageIndex { get; set; }

            public string MyImage
            {
                get
                {
                    return _MyImage; 
                }
                set
                {
                    _MyImage = value;
                    NotifyPropertyChanged("MyImage");
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

        }
	}
}
