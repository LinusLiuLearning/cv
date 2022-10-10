using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace cv02
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

                    // ���w�n���R���C�|����(��ı�S�x)�A�ñN���R���C�|�s�J visualFeatures
                    VisualFeatureTypes?[] visualFeatures = new VisualFeatureTypes?[]
                    {
                        VisualFeatureTypes.ImageType,
                        VisualFeatureTypes.Color,
                        VisualFeatureTypes.Faces,
                        VisualFeatureTypes.Adult,
                        VisualFeatureTypes.Categories,
                        VisualFeatureTypes.Tags,
                        VisualFeatureTypes.Objects,
                        VisualFeatureTypes.Brands,
                        VisualFeatureTypes.Description
                    };

                    // �ϥ� DescribeImageInStreamAsync()��k�Ǧ^���Ѥ��R���G res
                    ImageAnalysis res = await visionClient.AnalyzeImageInStreamAsync(fs, visualFeatures);

                    // �Y���ѥ��ѫh�Ǧ^null
                    if (res == null)
                    {
                        richTextBox1.Text = "���ѥ��ѡA�Э��s���w����";
                        return;
                    }

                    // �Ϥ������Ѥ��e��ܩ� richTexBox1
                    string str = "";
                    str= $"�y�z:{res.Description.Captions[0].Text}\n" +
                        $"�H��:{res.Description.Captions[0].Confidence}";

                    str += "\n�ʧO�P�~�֡G\n";
                    for (int i = 0; i < res.Faces.Count(); i++)
                    {
                        str += $"\t{res.Faces[i].Gender}\t\t{res.Faces[i].Age}\n";
                    }

                    str += "\n���һP�H�סG\n";

                    for (int i = 0; i < res.Tags.Count(); i++)
                    {
                        str += $"\t{res.Tags[i].Name}\t\t{res.Tags[i].Confidence}\n";
                    }

                    richTextBox1.Text += str;
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