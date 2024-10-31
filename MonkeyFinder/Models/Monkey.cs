namespace MonkeyFinder.Models
{
    public class Monkey
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
        // url https://github.com/jamesmontemagno/app-monkeys/blob/master/MonkeysApp/monkeydata.json
        // https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/MonkeysApp/monkeydata.json

        public string Name { get; set; }
        public string Location { get; set; }
        public string Details { get; set; }
        public string Image { get; set; }
        public int Population { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
