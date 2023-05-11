using Godot;

namespace GalaxyLib
{
    public class Star
    {
        public Vector3 Position { get; internal set; }

        public float Size { get; private set; }
        public string Name { get; private set; }
        public float Temperature { get; internal set; }
        public Color Color { get; internal set; }

        public Star(Vector3 position, string name, float size, float temp = 0)
        {
            Name = name;
            Position = position;
            Size = size;
            Temperature = temp;
            Color = CalculateRGBFromTempKelvin(Temperature);
        }

        private Color CalculateRGBFromTempKelvin(float temperature)
        {
            double tempCalc;
            int tempKelvin = Mathf.Clamp((int) Temperature, 1000, 40000) / 100;

            int r;
            int g;
            int b;

            double tempR;
            double tempG;
            double tempB;

            // RED
            if (tempKelvin <= 66) {
                r = 255;
                tempR = 255;
            } else {
                tempCalc = tempKelvin - 60;
                tempCalc = 329.698727446 * Mathf.Pow(tempCalc, -0.1332047592);
                r = Mathf.Clamp((int) tempCalc, 0, 255);
                tempR = tempCalc;
            }

            // GREEN
            if (tempKelvin <= 66) {
                tempCalc = tempKelvin;
                tempCalc = 99.4708025861 * Mathf.Log(tempCalc) - 161.1195681661;
                g = Mathf.Clamp((int) tempCalc, 0, 255);
                tempG = tempCalc;
            } else {
                tempCalc = tempKelvin - 60;
                tempCalc = 288.1221695283 * Mathf.Pow(tempCalc, -0.0755148492);
                g = Mathf.Clamp((int) tempCalc, 0, 255);
                tempG = tempCalc;
            }

            // BLUE
            if (tempKelvin >= 66) {
                b = 255;
                tempB = 255;
            } else if (tempKelvin <= 19) {
                b = 0;
                tempB = 0;
            } else {
                tempCalc = tempKelvin - 10;
                tempCalc = 138.5177312231 * Mathf.Log(tempCalc) - 305.0447927307;
                b = Mathf.Clamp((int) tempCalc, 0, 255);
                tempB = tempCalc;
            }

            double red = tempR / 255;
            double green = tempG / 255;
            double blue = tempB / 255;

            Color color = new Color((float) red, (float) green, (float) blue, 1);
            return color;
        }
    }
}
