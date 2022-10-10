using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;

namespace cv01
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {

                    string cvApiUrl = "https://cvimageserviceforlearning.cognitiveservices.azure.com/";
                    string cvApiKey = "5949389750fd437eab5b3799157e1393";
                    string imagePath = openFileDialog1.FileName;
                    // 建立FileStream物件fs開啟圖檔
                    FileStream fs = File.Open(imagePath, FileMode.Open);

                    // 建立電腦視覺辨識物件， 同時指定電腦視覺辨識的雲端服務Key
                    ComputerVisionClient visionClient = new ComputerVisionClient(new ApiKeyServiceClientCredentials(cvApiKey),
                        new System.Net.Http.DelegatingHandler[] { });

                    // 電腦視覺辨識物件指定雲端服務Api位址
                    visionClient.Endpoint = cvApiUrl;

                    // 使用 DescribeImageInStreamAsync()方法傳回辨識分析結果 res
                    ImageDescription res = await visionClient.DescribeImageInStreamAsync(fs);

                    // 若辨識失敗則傳回null
                    if (res == null)
                    {
                        richTextBox1.Text = "辨識失敗，請重新指定圖檔";
                        return;
                    }

                    // 圖片的辨識內容顯示於 richTexBox1
                    richTextBox1.Text = $"描述:{res.Captions[0].Text}\n" +
                        $"信度:{res.Captions[0].Confidence}";

                    string tags = "\n標籤:\n";
                    for (int i = 0; i < res.Tags.Count; i++)
                    {
                        tags += $"\t{res.Tags[i]}\n";
                    }

                    richTextBox1.Text += tags;
                    // pictureBox1 顯示指定的圖片
                    pictureBox1.Image = new Bitmap(imagePath);
                    // 釋放影像串流資源
                    fs.Close();
                    fs.Dispose();
                    GC.Collect();

                }
            }
            catch (Exception ex)
            {
                richTextBox1.Text = $"錯誤訊息:{ex.Message}";
            }

        }
    }
}