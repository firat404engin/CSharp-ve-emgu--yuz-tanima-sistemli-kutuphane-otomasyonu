
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.IO;
using System.Diagnostics;
using MultiFaceRec.Models;

namespace MultiFaceRec
{
    public partial class FrmPrincipal : Form
    {
        //Tüm değişkenlerin, vektörlerin ve haarcascade'lerin beyanı
        Image<Bgr, Byte> currentFrame;
        Capture grabber;
        HaarCascade face;
        HaarCascade eye;
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
        Image<Gray, byte> result, TrainedFace = null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels= new List<string>();
        List<string> NamePersons = new List<string>();
        int ContTrain, NumLabels, t;
        string name, names = null;

        private void label4_Click(object sender, EventArgs e)
        {
          
        }
        anaform anaform = new anaform();
        int sayac = 0;
        private void button3_Click(object sender, EventArgs e)
        {
            yoneticipanel yoneticipanel = new yoneticipanel();
            if (label4.Text.Contains("firat"))
            {

                yoneticipanel.Show();
                this.Hide();

                // Şu anki formu gizle
            }
            else
            {
                sayac++;
                if (sayac >=3)
                {
                    MessageBox.Show("3 den fazla yatkisiz giriş denemesii !");
                    mailgonderim sm = new mailgonderim();
                    string mesaj = "Merhaba  FIRAT  sisteme yetkisiz giriş uyarısı !!";
                    sm.Microsoft("UYARI", "outlook hesabi", "firat1903", "fengin7373@outlook.com", mesaj);
                }
                else
                {
                    MessageBox.Show("yetkisiz giris kalan deneme sayisi :" + (3 - sayac) );
                }
            }
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {

        }

        public FrmPrincipal()
        {
            InitializeComponent();
            //Yüz algılama için haarcascade'leri yükleyin
            face = new HaarCascade("haarcascade_frontalface_default.xml");
            //eye = new HaarCascade("haarcascade_eye.xml");
            try
            {
                //Her görüntü için önceden eğitilmiş yüzler ve etiketler
                string Labelsinfo = File.ReadAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt");
                string[] Labels = Labelsinfo.Split('%');
                NumLabels = Convert.ToInt16(Labels[0]);
                ContTrain = NumLabels;
                string LoadFaces;

                for (int tf = 1; tf < NumLabels+1; tf++)
                {
                    LoadFaces = "face" + tf + ".bmp";
                    trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/" + LoadFaces));
                    labels.Add(Labels[tf]);
                }
            
            }
            catch(Exception)
            {
                //MessageBox.Show(e.ToString());
                MessageBox.Show("İkili veritabanında hiçbir şey yok, lütfen yüz ekleyin.", "Triained Yüzler yükleniyor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }


        private void button1_Click(object sender, EventArgs e)
        {
            //kamerayı başlat
            grabber = new Capture();
            grabber.QueryFrame();
            //FrameGraber olayını başlat
            Application.Idle += new EventHandler(FrameGrabber);
            button1.Enabled = false;
        }


        private void button2_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    MessageBox.Show("Please input your name to add face");
                }
                else
                {
                    //Trained yüz sayacı
                    ContTrain = ContTrain + 1;

                    //Yakalama cihazından gri bir çerçeve alın
                    gray = grabber.QueryGrayFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                    //Yüz Dedektörü
                    MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                    face,
                    1.2,
                    10,
                    Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                    new Size(20, 20));

                    //Algılanan her öğe için eylem
                    foreach (MCvAvgComp f in facesDetected[0])
                    {
                        TrainedFace = currentFrame.Copy(f.rect).Convert<Gray, byte>();
                        break;
                    }

                    //Yüz ile algılanan görüntüyü yeniden boyutlandırarak aynı boyutu karşılaştırmaya zorlayın.
                    //Kübik enterpolasyon tipi yöntemi ile test görüntüsü
                    TrainedFace = result.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    trainingImages.Add(TrainedFace);
                    labels.Add(textBox1.Text);

                    //Gri tonlamayla eklenen yüzü göster
                    imageBox1.Image = TrainedFace;

                    //Daha fazla yükleme için bir dosya metnine üçlü yüzlerin sayısını yazın
                    File.WriteAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", trainingImages.ToArray().Length.ToString() + "%");

                    //Write the labels of triained faces in a file text for further load
                    for (int i = 1; i < trainingImages.ToArray().Length + 1; i++)
                    {
                        trainingImages.ToArray()[i - 1].Save(Application.StartupPath + "/TrainedFaces/face" + i + ".bmp");
                        File.AppendAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", labels.ToArray()[i - 1] + "%");
                    }

                    MessageBox.Show(textBox1.Text + "´s face detected and added :)", "Training OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show("No face detected. Please check your camera or stand closer.", "Training Fail", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        void FrameGrabber(object sender, EventArgs e)
        {
            // label3.Text = "0";
            //label4.Text = "";
            NamePersons.Add("");


            //Geçerli çerçeve formu yakalama cihazını edinin
            currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

            //Gri tonlamaya dönüştür
            gray = currentFrame.Convert<Gray, Byte>();

                    //Yüz detektörü
                    MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                  face,
                  1.2,
                  10,
                  Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                  new Size(20, 20));

            //Algılanan her öğe için eylem
            foreach (MCvAvgComp f in facesDetected[0])
                    {
                        t = t + 1;
                        result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                //0. (gri) kanalda tespit edilen yüzü mavi renkle çizin
                currentFrame.Draw(f.rect, new Bgr(Color.Green), 2);


                        if (trainingImages.ToArray().Length != 0)
                        {
                    //maxIteration gibi çok sayıda eğitilmiş görüntüyle yüz tanıma için TermCriteria
                    MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);

                        //Eigen face recognizer
                        EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                           trainingImages.ToArray(),
                           labels.ToArray(),
                           3000,
                           ref termCrit);

                        name = recognizer.Recognize(result);

                    //Algılanan ve tanınan her yüz için etiketi çizin
                    currentFrame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.Red));

                        }

                            NamePersons[t-1] = name;
                            NamePersons.Add("");


                //Sahnede algılanan yüzlerin sayısını ayarlayın
                // label3.Text = facesDetected[0].Length.ToString();

                /*
                //Set the region of interest on the faces

                gray.ROI = f.rect;
                MCvAvgComp[][] eyesDetected = gray.DetectHaarCascade(
                   eye,
                   1.1,
                   10,
                   Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                   new Size(20, 20));
                gray.ROI = Rectangle.Empty;

                foreach (MCvAvgComp ey in eyesDetected[0])
                {
                    Rectangle eyeRect = ey.rect;
                    eyeRect.Offset(f.rect.X, f.rect.Y);
                    currentFrame.Draw(eyeRect, new Bgr(Color.Blue), 2);
                }
                 */

            }
            t = 0;

                        //Names concatenation of persons recognized
                    for (int nnn = 0; nnn < facesDetected[0].Length; nnn++)
                    {
                        names = names + NamePersons[nnn] + ", ";
                    }
            //İşlenen ve tanınan yüzleri göster
            imageBoxFrameGrabber.Image = currentFrame;
                    label4.Text = names;
                    names = "";
            //İsim listesini (vektör) temizle
            NamePersons.Clear();
          
        }
    }
}
