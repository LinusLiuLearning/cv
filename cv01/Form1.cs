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
                    // �إ�FileStream����fs�}�ҹ���
                    FileStream fs = File.Open(imagePath, FileMode.Open);

                    // �إ߹q����ı���Ѫ���A �P�ɫ��w�q����ı���Ѫ����ݪA��Key
                    ComputerVisionClient visionClient = new ComputerVisionClient(new ApiKeyServiceClientCredentials(cvApiKey),
                        new System.Net.Http.DelegatingHandler[] { });

                    // �q����ı���Ѫ�����w���ݪA��Api��}
                    visionClient.Endpoint = cvApiUrl;

                    // �ϥ� DescribeImageInStreamAsync()��k�Ǧ^���Ѥ��R���G res
                    ImageDescription res = await visionClient.DescribeImageInStreamAsync(fs);

                    // �Y���ѥ��ѫh�Ǧ^null
                    if (res == null)
                    {
                        richTextBox1.Text = "���ѥ��ѡA�Э��s���w����";
                        return;
                    }

                    // �Ϥ������Ѥ��e��ܩ� richTexBox1
                    richTextBox1.Text = $"�y�z:{res.Captions[0].Text}\n" +
                        $"�H��:{res.Captions[0].Confidence}";

                    string tags = "\n����:\n";
                    for (int i = 0; i < res.Tags.Count; i++)
                    {
                        tags += $"\t{res.Tags[i]}\n";
                    }

                    richTextBox1.Text += tags;
                    // pictureBox1 ��ܫ��w���Ϥ�
                    pictureBox1.Image = new Bitmap(imagePath);
                    // ����v����y�귽
                    fs.Close();
                    fs.Dispose();
                    GC.Collect();

                }
            }
            catch (Exception ex)
            {
                richTextBox1.Text = $"���~�T��:{ex.Message}";
            }

        }
    }
}