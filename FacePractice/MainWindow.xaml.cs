using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace FacePractice
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            picGrid.Visibility = Visibility.Hidden;
            create_person.Visibility = Visibility.Hidden;
            souce_box.ItemsSource = TraningSource.SouceBoxItems;
            deleteAllPersonAndFace();
        }



        private void BtnCreate(object sender, RoutedEventArgs e)
        {
            if (souce_box.SelectedItem != null && !personName.Text.Equals(""))
            {
                string name = personName.Text;
                APIAction apiAct = new APIAction();
                Task<bool> t = Task<bool>.Run(() => apiAct.CreatePerson(name));
                if (t.Result)
                {
                    string personID = "";
                    Task<List<PersonObj>> tForList = Task<List<PersonObj>>.Run(() => apiAct.ListAllPersons());
                    List<PersonObj> list = tForList.Result;
                    foreach (PersonObj obj in list)
                    {
                        if (obj.Name.Equals(name))
                        {
                            personID = obj.PersonId;
                            break;
                        }
                    }
                    switch (souce_box.SelectedIndex)
                    {
                        case 0:
                            foreach (string url in TraningSource.jinping)
                            {
                                Task<bool>.Run(() => apiAct.AddFace(personID, url));
                                Thread.Sleep(5000);
                            }
                            break;
                        

                    }
                }
                System.Windows.MessageBox.Show("Creating and traning finish.");
                create_person.Visibility = Visibility.Hidden;
            }
            else
            {
                if (personName.Text.Equals(""))
                {
                    System.Windows.MessageBox.Show("Enter name.");
                }
                else if (souce_box.SelectedItem == null)
                {
                    System.Windows.MessageBox.Show("Select souce.");
                }
            }

        }

        private void BtnPreviewUrl(object sender, RoutedEventArgs e)
        {
            if (IdentifyUrl.Text != null && !IdentifyUrl.Text.Equals(""))
            {
                IdentifyView.ItemsSource = new List<string>();
                string imgUrl = IdentifyUrl.Text;
                pic_preview.Source = new BitmapImage(new Uri(imgUrl));
                APIAction apiAct = new APIAction();
                Task<string> t = Task<string>.Run(() => apiAct.GetFaceGenderAndAge(imgUrl));
                string[] FaceGenderAndAge = t.Result.Split(',');
                preview_gender.Text = FaceGenderAndAge[0];
                preview_age.Text = FaceGenderAndAge[1];
            }
        }
        private void BtnIdentify(object sender, RoutedEventArgs e)
        {
            if (IdentifyUrl.Text != null && !IdentifyUrl.Text.Equals(""))
            {
                string imgUrl = IdentifyUrl.Text;
                BtnPreviewUrl(sender, e);
                APIAction apiAct = new APIAction();
                Task<List<PersonObj>> t = Task<List<PersonObj>>.Run(() => apiAct.IdentifyFace(imgUrl));
                List<PersonObj> itemsSource = t.Result;
                IdentifyView.ItemsSource = itemsSource;
            }
        }


        private void BtnCreatePerson(object sender, RoutedEventArgs e)
        {
            btn_create.IsEnabled = true;
            btn_create.IsEnabled = true;
            create_person.Visibility = Visibility.Visible;
        }
        private void CancelCreate(object sender, RoutedEventArgs e)
        {
            create_person.Visibility = Visibility.Hidden;
        }

        private void BtnShow(object sender, RoutedEventArgs e)
        {


            picGrid.Visibility = Visibility.Visible;
            APIAction apiAct = new APIAction();
            Task<List<PersonObj>> tForListAllPersons = Task<List<PersonObj>>.Run(() => apiAct.ListAllPersons());
            List<string> trainedPicUrlList = new List<string>();
            foreach (PersonObj person in tForListAllPersons.Result)
            {
                string personId = person.PersonId;
                foreach (string faceid in person.FaceIds)
                {
                    Task<string> tForGetFaceUrlByID = Task<string>.Run(() => apiAct.GetFaceUrlByID(personId, faceid));
                    trainedPicUrlList.Add(tForGetFaceUrlByID.Result);
                }
            }

            List<PicListData> datas = new List<PicListData>();

            foreach (string url in trainedPicUrlList)
            {
                datas.Add(new PicListData(new BitmapImage(new Uri(url))));
            }
            pic_view.ItemsSource = datas;


        }

      
        private void BtnBack(object sender, RoutedEventArgs e)
        {
            picGrid.Visibility = Visibility.Hidden;
        }

        private void deleteAllPersonAndFace()
        {
            APIAction apiAct = new APIAction();
            Task<List<PersonObj>> tForList = Task<List<PersonObj>>.Run(() => apiAct.ListAllPersons());
            List<PersonObj> list = tForList.Result;
            foreach (PersonObj obj in list)
            {
                foreach (string id in obj.FaceIds)
                {
                    Task<bool> t = Task<bool>.Run(() => apiAct.DeletePersonsFace(obj.PersonId, id));
                    Console.WriteLine(t.Result);
                    Thread.Sleep(2000);
                }
                Task<bool> t0 = Task<bool>.Run(() => apiAct.DeletePerson(obj.PersonId));
                Console.WriteLine(t0.Result);
                Thread.Sleep(2000);
            }
        }

        private void souce_box_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
