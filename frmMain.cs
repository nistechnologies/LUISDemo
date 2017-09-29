using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using Demo.Models;


namespace Demo
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            tbSearch.Text = "find a mustang";            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var result =  LUISParse(tbSearch.Text);
        }

        private async Task<string> LUISParse(string query)
        {
            string topIntent = string.Empty;
            string topEntity = string.Empty;

            using (var client = new HttpClient())
            {
                string uri = "https://eastus2.api.cognitive.microsoft.com/luis/v2.0/apps/04bf5081-2934-4bcb-ad4e-69f801a91bfc?subscription-key=fcc211a43a2d4a78a90e83f23ecebc6d&verbose=true&timezoneOffset=0&q=" + query;

                HttpResponseMessage msg = await client.GetAsync(uri);

                if (msg.IsSuccessStatusCode)
                {
                    var jsonResponse = await msg.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<LUISResponse>(jsonResponse);
                    topIntent = response.intents[0].intent;
                    topEntity = response.entities[0].entity;                    
                }
            }

            switch (topIntent)
            {
                case ("FindCar"):
                    Car car = new Car();
                    tbOutput.Text = car.Get(topEntity);
                    break;

                default:
                    break;
            }

            return topEntity;
        }


    }
}
