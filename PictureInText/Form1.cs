using System;
using System.Drawing;
using System.Windows.Forms;
using IronOcr;
using asprise_ocr_api;
using System.IO;
using Vintasoft;

namespace PictureInText
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    static Bitmap ScaleImage(Image source, int width, int height)
    {

      Bitmap dest = new Bitmap(width, height);
      using (Graphics gr = Graphics.FromImage(dest))
      {
        gr.FillRectangle(Brushes.White, 0, 0, width, height);
        gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

        float srcwidth = source.Width;
        float srcheight = source.Height;
        float dstwidth = width;
        float dstheight = height;

        if (srcwidth <= dstwidth && srcheight <= dstheight)
        {
          int left = (width - source.Width) / 2;
          int top = (height - source.Height) / 2;
          gr.DrawImage(source, left, top, source.Width, source.Height);
        }
        else if (srcwidth / srcheight > dstwidth / dstheight)
        {
          float cy = srcheight / srcwidth * dstwidth;
          float top = ((float)dstheight - cy) / 2.0f;
          if (top < 1.0f) top = 0;
          gr.DrawImage(source, 0, top, dstwidth, cy);
        }
        else
        {
          float cx = srcwidth / srcheight * dstheight;
          float left = ((float)dstwidth - cx) / 2.0f;
          if (left < 1.0f) left = 0;
          gr.DrawImage(source, left, 0, cx, dstheight);
        }

        return dest;
      }
    }

    static void ConverPicture(Bitmap newBitmap)
    {
      StreamWriter file = new StreamWriter("Picture.txt", true);
      var backGroundColor = (1.0 - (newBitmap.GetPixel(0, 0).R / 255));
      if (backGroundColor == 0)
      {
        for (int i = 0; i < newBitmap.Height; i++)
        {
          for (int j = 0; j < newBitmap.Width; j++)
          {
            var gray = (1.0 - (newBitmap.GetPixel(j, i).R / 255));
            file.Write(gray);
            file.Write(" ");
          }
        }
        file.Close();
      }
      if (backGroundColor == 1)
      {
        for (int i = 0; i < newBitmap.Height; i++)
        {
          for (int j = 0; j < newBitmap.Width; j++)
          {
            var gray = (1.0 - (newBitmap.GetPixel(j, i).R / 255));
            if (gray == 1)
              gray = 0;
            else
              gray = 1;
            file.Write(gray);
            file.Write(" ");
          }
        }
        file.Close();
      }
    }

    public class NeuralNetwork
    {

      int[] layer; //layer information
      Layer[] layers; //layers in the network


      public float getAverageError()
      {
        return layers[layers.Length - 1].getAverageError();
      }

      public void saveWeights()
      {

        /*for (int i = layers.Length - 1; i > 0; i--)
        {
          layers[i].saveWeights(layer[i], layer[i + 1]);
        }*/
        for (int i = 0; i < layers.Length; i++)
        {
          layers[i].saveWeights(layer[i], layer[i + 1]);
        }
      }
      public NeuralNetwork(int[] layer)
      {
        //deep copy layers
        this.layer = new int[layer.Length];
        for (int i = 0; i < layer.Length; i++)
          this.layer[i] = layer[i];

        //creates neural layers
        layers = new Layer[layer.Length - 1];

        for (int i = 0; i < layers.Length; i++)
        {
          layers[i] = new Layer(layer[i], layer[i + 1]);
        }
      }

      public float[] FeedForward(float[] inputs)
      {
        //feed forward
        layers[0].FeedForward(inputs);
        for (int i = 1; i < layers.Length; i++)
        {
          layers[i].FeedForward(layers[i - 1].outputs);
        }

        return layers[layers.Length - 1].outputs; //return output of last layer
      }

      public void BackProp(float[] expected)
      {
        // run over all layers backwards
        for (int i = layers.Length - 1; i >= 0; i--)
        {
          if (i == layers.Length - 1)
          {
            layers[i].BackPropOutput(expected); //back prop output
          }
          else
          {
            layers[i].BackPropHidden(layers[i + 1].gamma, layers[i + 1].weights); //back prop hidden
          }
        }

        //Update weights
        for (int i = 0; i < layers.Length; i++)
        {
          layers[i].UpdateWeights();
        }
      }

      public class Layer
      {
        int numberOfInputs; //# of neurons in the previous layer
        int numberOfOuputs; //# of neurons in the current layer


        public float[] outputs; //outputs of this layer
        public float[] inputs; //inputs in into this layer
        public float[,] weights; //weights of this layer
        public float[,] weightsDelta; //deltas of this layer
        public float[] gamma; //gamma of this layer
        public float[] error; //error of the output layer
        public float averageError;

        public static Random random = new Random(); //Static random class variable


        public void saveWeights(int numberOfInputs, int numberOfOuputs)
        {
          StreamWriter someWeights = new StreamWriter("Weights.txt", true);
          for (int i = 0; i < numberOfOuputs; i++)
          {
            for (int j = 0; j < numberOfInputs; j++)
            {
              someWeights.WriteLine(weights[i, j]);
            }
          }
          someWeights.Close();
        }
        public float getAverageError()
        {
          return averageError;
        }
        public Layer(int numberOfInputs, int numberOfOuputs)
        {
          this.numberOfInputs = numberOfInputs;
          this.numberOfOuputs = numberOfOuputs;

          //initilize datastructures
          outputs = new float[numberOfOuputs];
          inputs = new float[numberOfInputs];
          weights = new float[numberOfOuputs, numberOfInputs];
          weightsDelta = new float[numberOfOuputs, numberOfInputs];
          gamma = new float[numberOfOuputs];
          error = new float[numberOfOuputs];
          if (numberOfInputs == 1024)
          {
            InitilizeWeights0(false); //initilize weights
          }
          else
          {
            InitilizeWeights1(false);
          }
        }

        public void InitilizeWeights0(bool randomWeigths)
        {
          if (randomWeigths)
          {
            for (int i = 0; i < numberOfOuputs; i++)
            {
              for (int j = 0; j < numberOfInputs; j++)
              {
                weights[i, j] = (float)random.NextDouble() - 0.5f;
              }
            }
          }
          else
          {
            StreamReader someWeights = new StreamReader("Weights.txt", true);
            for (int i = 0; i < numberOfOuputs; i++)
            {
              for (int j = 0; j < numberOfInputs; j++)
              {
                weights[i, j] = float.Parse(someWeights.ReadLine());
              }
            }
            someWeights.Close();
          }
        }

        public void InitilizeWeights1(bool randomWeigths)
        {
          if (randomWeigths)
          {
            for (int i = 0; i < numberOfOuputs; i++)
            {
              for (int j = 0; j < numberOfInputs; j++)
              {
                weights[i, j] = (float)random.NextDouble() - 0.5f;
              }
            }
          }
          else
          {
            StreamReader someWeights = new StreamReader("Weights.txt", true);
            for (int i = 0; i < 1024; i++)
            {
              for (int j = 0; j < 500; j++)
              {
                //weights[i, j] = float.Parse(someWeights.ReadLine());
                someWeights.ReadLine();
              }
            }
            for (int i = 0; i < numberOfOuputs; i++)
            {
              for (int j = 0; j < numberOfInputs; j++)
              {
                weights[i, j] = float.Parse(someWeights.ReadLine());
              }
            }
            someWeights.Close();
          }
        }

        public float[] FeedForward(float[] inputs)
        {
          this.inputs = inputs;// keep shallow copy which can be used for back propagation

          //feed forwards
          for (int i = 0; i < numberOfOuputs; i++)
          {
            outputs[i] = 0;
            for (int j = 0; j < numberOfInputs; j++)
            {
              outputs[i] += inputs[j] * weights[i, j];
            }

            outputs[i] = (float)Math.Tanh(outputs[i]);
          }

          return outputs;
        }

        public float TanHDer(float value)
        {
          return 1 - (value * value);
        }

        public void BackPropOutput(float[] expected)
        {
          //Error dervative of the cost function
          for (int i = 0; i < numberOfOuputs; i++)
          {
            error[i] = outputs[i] - expected[i];
            averageError += (float)Math.Pow(outputs[i] - expected[i], 2);
          }
          averageError = averageError / (float)numberOfOuputs;

          //Gamma calculation
          for (int i = 0; i < numberOfOuputs; i++)
            gamma[i] = error[i] * TanHDer(outputs[i]);

          //Caluclating detla weights
          for (int i = 0; i < numberOfOuputs; i++)
          {
            for (int j = 0; j < numberOfInputs; j++)
            {
              weightsDelta[i, j] = gamma[i] * inputs[j];
            }
          }
        }

        public void BackPropHidden(float[] gammaForward, float[,] weightsFoward)
        {
          //Caluclate new gamma using gamma sums of the forward layer
          for (int i = 0; i < numberOfOuputs; i++)
          {
            gamma[i] = 0;

            for (int j = 0; j < gammaForward.Length; j++)
            {
              gamma[i] += gammaForward[j] * weightsFoward[j, i];
            }

            gamma[i] *= TanHDer(outputs[i]);
          }

          //Caluclating detla weights
          for (int i = 0; i < numberOfOuputs; i++)
          {
            for (int j = 0; j < numberOfInputs; j++)
            {
              weightsDelta[i, j] = gamma[i] * inputs[j];
            }
          }
        }
        public void UpdateWeights()
        {
          for (int i = 0; i < numberOfOuputs; i++)
          {
            for (int j = 0; j < numberOfInputs; j++)
            {
              weights[i, j] -= weightsDelta[i, j] * 0.1f;
            }
          }
        }
      }
    }

    bool imageSetuped = false;
    OpenFileDialog file = new OpenFileDialog();
    private void button1_Click(object sender, EventArgs e)
    {
      if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        var bitmap = new Bitmap(file.FileName);
        if (bitmap.Width > 1920 && bitmap.Height > 1080)
        {
          MessageBox.Show("Sorry, image is too big");
          imageSetuped = false;
        }
        else
        {
          pictureBox1.Image = bitmap;
          imageSetuped = true;
        }
      }
    }

    int whichLibrary = 0;
    bool libraryChoosed = false;
    private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      libraryChoosed = false;
      for (int i = 0; i < checkedListBox1.Items.Count; i++)
      {
        if (checkedListBox1.GetItemCheckState(i).ToString() == "Checked")
        {
          libraryChoosed = true;
          whichLibrary = i + 1;
        }
      }
    }

    private void button3_Click(object sender, EventArgs e)
    {
      if (imageSetuped)
      {
        if (whichLibrary <= 3 && whichLibrary >= 1 && libraryChoosed)
        {
          if (whichLibrary == 1)
          {
            try
            {
              var Ocr = new IronTesseract();
              using (var Input = new OcrInput(file.FileName))
              {
                var Result = Ocr.Read(Input);
                textBox1.Text = Result.Text;
              }
            }
            catch
            {
              MessageBox.Show("Sorry, something went wrong");
            }
          }
          if (whichLibrary == 2)
          {
            try
            {
              AspriseOCR.SetUp();
              AspriseOCR ocr = new AspriseOCR();
              ocr.StartEngine("eng", AspriseOCR.SPEED_FASTEST);
              string path = file.FileName;
              string Result = ocr.Recognize(path, -1, -1, -1, -1, -1, AspriseOCR.RECOGNIZE_TYPE_ALL, AspriseOCR.OUTPUT_FORMAT_PLAINTEXT);
              textBox1.Text = Result;
              ocr.StopEngine();
            }
            catch
            {
              MessageBox.Show("Sorry, something went wrong");
            }
          }
          if (whichLibrary == 3)
          {
            try
            {
              using (Vintasoft.Imaging.ImageCollection images = new Vintasoft.Imaging.ImageCollection())
              {
                images.Add(file.FileName);
                using (Vintasoft.Imaging.Ocr.Tesseract.TesseractOcr tesseractOcr = new Vintasoft.Imaging.Ocr.Tesseract.TesseractOcr())
                {
                  tesseractOcr.Init(new Vintasoft.Imaging.Ocr.OcrEngineSettings(Vintasoft.Imaging.Ocr.OcrLanguage.English));
                  foreach (Vintasoft.Imaging.VintasoftImage image in images)
                  {
                    Vintasoft.Imaging.Ocr.Results.OcrPage ocrResult = tesseractOcr.Recognize(image);
                    textBox1.Text = ocrResult.GetText();
                  }
                }
              }
            }
            catch
            {
              MessageBox.Show("Sorry, something went wrong");
            }
          }
        }
        else
        {
          NeuralNetwork net = new NeuralNetwork(new int[] { 1024, 500, 10 });
          var bitmap = new Bitmap(file.FileName);
          var NewBitmap = ScaleImage(bitmap, 32, 32);
          ConverPicture(NewBitmap);
          float[] results = new float[2];
          var data = new StreamReader("Picture.txt");
          int sum = 1;
          float[][] OnesAndZerosInUserTest = new float[sum][];
          // Convert to array of boll-type symbols
          for (int i = 0; i < sum; i++)
          {
            OnesAndZerosInUserTest[i] = new float[1024];
            for (int j = 0; j < 1024; j++)
            {
              var character = data.Read();
              if (character != 32)
                OnesAndZerosInUserTest[i][j] = character - '0';
              else
                j--;
            }
          }
          data.Close();
          //Work of neural network
          for (int i = 0; i < sum; i++)
          {
            results = net.FeedForward(OnesAndZerosInUserTest[i]);
            float maxOutPut = results[0];
            int index = 0;
            for (int j = 0; j < 10; j++)
            {
              if (results[j] > maxOutPut)
              {
                index = j;
                maxOutPut = results[j];
              }

            }
            textBox1.Text = index.ToString();
          }
        }
      }
      else
      {
        MessageBox.Show("Image is not seted");
      }
    }

    private void button2_Click(object sender, EventArgs e)
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.Filter = "Текстовый документ (*.txt)|*.txt|Все файлы (*.*)|*.*";

      if (saveFileDialog.ShowDialog() == DialogResult.OK)
      {
        StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName);
        streamWriter.WriteLine(textBox1.Text);
        streamWriter.Close();
      }
    }
  }
}
