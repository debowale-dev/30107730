using System;
using System.IO;
using System.Runtime.InteropServices;
using Keras.Models;
using Numpy;
using OpenCvSharp;

namespace FoodApi
{
    public class Dt
    {
        private readonly BaseModel _model;

        public Dt()
        {
            // Load the model from the specified path
            _model = BaseModel.LoadModel("model.h5");
        }

        public int Predict(Stream imageStream)
        {
            // Convert the stream to an image using OpenCvSharp
            using var mat = Mat.FromStream(imageStream, ImreadModes.Color);

            // Check if the image was correctly loaded
            if (mat.Empty())
            {
                throw new InvalidOperationException("Failed to load image from stream.");
            }

            // Resize the image to the size expected by your model (e.g., 128x128)
            var resizedImage = mat.Resize(new OpenCvSharp.Size(128, 128)); // Change from 224x224 to 128x128

            // Ensure the image has 3 channels (RGB)
            if (resizedImage.Channels() != 3)
            {
                throw new InvalidOperationException("The image must have 3 channels (RGB).");
            }

            // Extract raw pixel data directly
            int expectedSize = resizedImage.Height * resizedImage.Width * resizedImage.Channels();
            var imageArray = new byte[expectedSize];
            Marshal.Copy(resizedImage.Data, imageArray, 0, expectedSize);

            // Reshape the byte array to match the image dimensions
            var reshapedImageArray = np.array(imageArray).reshape(128, 128, 3); // Adjust to 128x128

            // Normalize the image data by converting to float32 and dividing by 255.0
            var normalizedImageArray = reshapedImageArray.astype(np.float32) / 255.0;

            // Add batch dimension (assuming model expects input shape [batch_size, height, width, channels])
            var inputTensor = np.expand_dims(normalizedImageArray, 0);

            // Perform prediction
            var prediction = _model.Predict(inputTensor);

            // Convert the prediction to a label (assuming the model returns probabilities for classes)
            int predictedLabel = np.argmax(prediction).item<int>(); // Replace asscalar with item


            return predictedLabel;
        }





    }
}
